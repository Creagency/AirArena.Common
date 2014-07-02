using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirArena.Data;
using System.Data;
namespace AirArena.Audit
{
    public class Event<TUserId>
    {
        internal static string DataConPrefix
        {
            get
            {
                return AirArena.Audit.Config.CurrentConfig.DataConPrefix;
            }

        }
        
        private DataConnector DataCon;
        //public static Event<TUserId> AddEvent(TUserId userId, EventType.EventTypeName eventType, string comments)
        //{
        //    var e = new Event<TUserId> { UserId = userId, EventTypeId = (int)eventType, Comments = comments };
        //    e.Save();
        //    return e;
        //}
        public static Event<TUserId> AddEvent(TUserId userId, int eventTypeId, string comments)
        {
            var e = new Event<TUserId> { UserId = userId, EventTypeId = eventTypeId, Comments = comments };
            e.Save();
            return e;
        }
        public static Event<TUserId> AddEvent(TUserId userId, int eventTypeId)
        {
            return AddEvent(userId, eventTypeId, string.Empty);
        }
        public Event()
        {
            DataCon = new DataConnector(DataConPrefix);
        }
        public Event(int eventId)
            : this()
        {
            Load(eventId);
        }
        private void Load(int eventId)
        {
            var req = new DataRequest("Event.Event_Get");
            req.Parameters.Add("@EventId", eventId);
            System.Data.DataTable dt = DataCon.GetDataTable(req);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                EventId = int.Parse(dr["EventId"].ToString());
                UserId = (TUserId)dr["UserId"];
                EventTypeId = int.Parse(dr["EventTypeId"].ToString());
                CreationDT = DateTime.Parse(dr["CreationDT"].ToString());
                Comments = dr["Comments"].ToString();
            }
        }
        public void Save()
        {
            var req = new DataRequest("Event.Event_Add");
            req.Parameters.Add("@EventId", EventId);
            req.Parameters.Add("@UserId", UserId);
            req.Parameters.Add("@EventTypeId", EventTypeId);
            req.Parameters.Add("@Comments", Comments);
            System.Data.DataTable dt = DataCon.GetDataTable(req);
            if (dt.Rows.Count>0){
                EventId = int.Parse(dt.Rows[0]["EventId"].ToString());
            }
        }


        public int EventId { get; private set; }
        public TUserId UserId { get; set; }
        public int EventTypeId { get; set; }
        public string Comments { get; set; }
        public DateTime CreationDT { get; private set; }

        private EventType eventType;
        public EventType EventType
        {
            get
            {
                if (eventType == null && EventTypeId > 0)
                {
                    eventType = new EventType(EventTypeId);
                }
                return eventType;
            }
        }

    }
}