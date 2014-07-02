using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AirArena.Data;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataTable = System.Data.DataTable;
using System.Data;
//using Flashfunders.Models;

namespace AirArena.Ticketing
{
    public class Ticket<TUser>
    {
        internal static string DataConPrefix
        {
            get
            {
                return AirArena.Ticketing.Config.CurrentConfig.DataConPrefix;
            }

        }
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int StatusId { get; set; }
        public Guid AssigneeId { get; set; }

        public DateTime CreateDT { get; set; }

        [StringLength(400)]
        [Required(ErrorMessage = "Please enter the Subject")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [StringLength(1000)]
        [Required(ErrorMessage = "Please describe your issue/comment")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "What this ticket is about?")]
        [Display(Name = "What this ticket is about?")]
        public int TypeId{ get; set; }
        public TicketStatus Status { get { return TicketStatus.GetStatus(StatusId); } }
        public TicketType Type { get { return TicketType.GetType(TypeId); } }
        private TUser assignee;
        public TUser Assignee
        {
            get
            {
                if(assignee == null)
                {
                    Type t = typeof(TUser);
                    user = (TUser)Activator.CreateInstance(t, AssigneeId);
                }
                return assignee;
            }
        }
        private TUser user;
        public TUser User 
        {
            get
            {
                if (user == null)
                {
                    Type t = typeof(TUser);
                    user = (TUser)Activator.CreateInstance(t, UserId);
                }
                return user;
            }
        }

        //public static IEnumerable<SelectListItem> TicketAdminUsers { get { return Users.List("Admin",false,string.Empty).Select(u => new SelectListItem { Text = u.Name, Value = u.UserId.ToString() }).ToList(); } }


        #region Data Commiunication
        private DataConnector DataCon;
        
        public Ticket()
        {
            //The dataconnector prefix would best come from a ticketing config section rather than be hard coded as "Ticketing"
            DataCon = new DataConnector(AirArena.Ticketing.Ticket<TUser>.DataConPrefix);
        }

        public Ticket(int id) :this()
        {
            Load(id);
        }

        public Ticket(DataRow dr)
            : this()
        {
            Load(dr);
        }

        private void Load(int Id)
        {
            var req = new DataRequest("Ticketing.Ticket_Get");
            req.Parameters.Add("@Id", Id);
            System.Data.DataTable dt = DataCon.GetDataTable(req);
            if (dt.Rows.Count > 0)
                Load(dt.Rows[0]);
        }

        private void Load(DataRow dr)
        {
            UserId = Guid.Parse(dr["UserId"].ToString());
            if (dr["Id"] != DBNull.Value)
            {
                Id = int.Parse(dr["Id"].ToString());
                UserId = Guid.Parse(dr["UserId"].ToString());
                Subject = dr["Subject"].ToString();
                Description = dr["Description"].ToString();
                TypeId = int.Parse(dr["TypeId"].ToString());
                StatusId = int.Parse(dr["StatusId"].ToString());
                AssigneeId = Guid.Parse(dr["AssigneeId"].ToString());
                CreateDT = DateTime.Parse(dr["CreateDT"].ToString());
            }
        }
        

        public int AddUpdate()
        {
            var req = new DataRequest("Ticketing.Ticket_AddUpdate");
            if (Id > 0)
                req.Parameters.Add("@Id", Id);
            req.Parameters.Add("@UserId", UserId);
            req.Parameters.Add("@Subject", Subject);
            req.Parameters.Add("@Description", Description);
            req.Parameters.Add("@TypeId", TypeId);
            req.Parameters.Add("@StatusId", StatusId);
            req.Parameters.Add("@AssigneeId", AssigneeId);
            Id = int.Parse(DataCon.ExecuteScalar(req).ToString());
            return Id;
        }

        private TicketMessages<TUser> messages;
        public TicketMessages<TUser> Messages
        {
           get 
           {
               return TicketMessage<TUser>.List(this.Id);
           }
        }


        public static Tickets<TUser> ListAdminOpenTickets(TUser userType, int pageNo = 1)
        {
            var req = new DataRequest("Ticketing.Ticket_ListAdminOpenTickets");
            DataConnector DataCon = new DataConnector(DataConPrefix);
            //return new Tickets(DataCon.GetDataTable(req));
            return new Tickets<TUser>(DataCon.GetDataTable(req), pageNo);
        }

        public static Tickets<TUser> ListAdminTicketByStatus(int statusid, int pageNo = 1)
        {
            var req = new DataRequest("Ticketing.Ticket_ListAdminTicketByStatus");
            req.Parameters.Add("@Statusid", statusid);
            DataConnector DataCon = new DataConnector(DataConPrefix);
            //return new Tickets(DataCon.GetDataTable(req));
            return new Tickets<TUser>(DataCon.GetDataTable(req), pageNo);
        }

        public static Tickets<TUser> ListAdminTicketByAssignee(int? statusid, Guid assigneeId, int pageNo = 1)
        {
            var req = new DataRequest("Ticketing.Ticket_ListAdminTicketByAssignee");
            if (statusid.HasValue)
                req.Parameters.Add("@Statusid", statusid);

            req.Parameters.Add("@AssigneeId", assigneeId);

            DataConnector DataCon = new DataConnector(DataConPrefix);
            return new Tickets<TUser>(DataCon.GetDataTable(req), pageNo);
        }

        public static Tickets<TUser> List(int? statusid, Guid userId, Guid assigneeId, int pageNo = 1)
        {
            var req = new DataRequest("Ticketing.Ticket_List");
            if (statusid.HasValue)
                req.Parameters.Add("@Statusid", statusid);
            
            if (userId != null && userId!=Guid.Empty)
                req.Parameters.Add("@UserId", userId);

            if (assigneeId != null && assigneeId != Guid.Empty)
                req.Parameters.Add("@AssigneeId", assigneeId);

            DataConnector DataCon = new DataConnector(DataConPrefix);
            //return new Tickets(DataCon.GetDataTable(req));
            return new Tickets<TUser>(DataCon.GetDataTable(req), pageNo);
        }
        #endregion
    }


    public class Tickets<TUser> : List<Ticket<TUser>>
    {
        private DataConnector DataCon;
        public int TicketId { get; private set; }
        public PagingInfo Paging { get; set; }

        private Tickets()
        {            
            DataCon = new DataConnector(Ticket<TUser>.DataConPrefix);
        }

        public Tickets(DataTable dt)
            : this()
        {
            this.Paging = CreatePagingInfo(dt);
            this.Paging.CurrentPageNumber = 1;
            Load(dt);            
        }

        public Tickets(DataTable dt, int pageNo)
            : this()
        {
            IEnumerable<DataRow> rows;
            if (pageNo > 0)
            {
            // initialize paging iformaion
                this.Paging = CreatePagingInfo(dt);
                this.Paging.CurrentPageNumber = pageNo;

                IEnumerable<DataRow> query = from r in dt.AsEnumerable() select r;
                rows = query.Skip((Paging.CurrentPageNumber - 1)*Paging.PageSize).Take(Paging.PageSize);
                int i = rows.Count();
            }
            else
            {
                rows = from r in dt.AsEnumerable() select r;
            }
            foreach (var ticket in from DataRow dr in rows select new Ticket<TUser>(dr))
            {
                Add(ticket);
            }
        }

        private void Load(DataTable dt)
        {
            foreach (var ticket in from DataRow dr in dt.Rows select new Ticket<TUser>(dr))
            {   
                Add(ticket);
            }
        }

        private void LoadPagedData(DataTable dt)
        {
            foreach (var ticket in from DataRow dr in dt.Rows select new Ticket<TUser>(dr))
            {
                Add(ticket);
            }
        }

        private PagingInfo CreatePagingInfo(DataTable dt)
        {
            PagingInfo dataPaging = new PagingInfo();
            //dataPaging.CurrentPageNumber = 1;
            dataPaging.PageSize = 10;  // TODO: make this configurable
            dataPaging.TotalRecords = dt.Rows.Count;
            return dataPaging;
        }

    }
}