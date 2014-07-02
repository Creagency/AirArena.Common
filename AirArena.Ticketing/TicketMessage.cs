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
    public class TicketMessage<TUser>
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public Ticket<TUser> Ticket { get; set; }
        public Guid UserId { get; set; }
        public bool DisplayToUser { get; set; }
        public DateTime CreateDT { get; set; }
        
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

        [StringLength(1000)]
        [Required(ErrorMessage = "Please describe your issue/comment")]
        [Display(Name = "Comment")]
        public string Message { get; set; }
            
        #region Data Commiunication
        private DataConnector DataCon;
        
        public TicketMessage()
        {
            DataCon = new DataConnector(Ticket<TUser>.DataConPrefix);
        }

        public TicketMessage(int id) :this()
        {
            Load(id);
        }

        public TicketMessage(DataRow dr)
            : this()
        {
            Load(dr);
        }


        private void Load(int Id)
        {
            var req = new DataRequest("Ticketing.Message_Get");
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
                TicketId = int.Parse(dr["TicketId"].ToString());
                UserId = Guid.Parse(dr["UserId"].ToString());
                Message = dr["Message"].ToString();
                DisplayToUser = bool.Parse(dr["DisplayToUser"].ToString());
                CreateDT = DateTime.Parse(dr["CreateDT"].ToString());
            }
        }

        public int AddUpdate()
        {
            var req = new DataRequest("Ticketing.Message_Add");
            req.Parameters.Add("@UserId", UserId);
            req.Parameters.Add("@TicketId", TicketId);
            req.Parameters.Add("@Message", Message);
            req.Parameters.Add("@DisplayToUser", DisplayToUser);
            return int.Parse(DataCon.ExecuteScalar(req).ToString());
        }

        public static TicketMessages<TUser> List(int ticketId)
        {
            var req = new DataRequest("Ticketing.TicketMessage_List");
            req.Parameters.Add("@TicketId", ticketId);

            var DataCon  = new DataConnector(Ticket<TUser>.DataConPrefix);
            return new TicketMessages<TUser>(DataCon.GetDataTable(req));
        }

        #endregion

    }

    public class TicketMessages<TUser> : List<TicketMessage<TUser>>
    {
        private DataConnector DataCon;
        public int TicketId { get; private set; }
        private static string DataConPrefix = "Ticketing";
        private TicketMessages()
        {
            DataCon = new DataConnector(Ticket<TUser>.DataConPrefix);
        }

        public TicketMessages(DataTable dt)
            : this()
        {
            Load(dt);
        }

        private void Load(DataTable dt)
        {
            foreach (var ticketmessage in from DataRow dr in dt.Rows select new TicketMessage<TUser>(dr))
            {
                Add(ticketmessage);
            }
        }
    }
}
