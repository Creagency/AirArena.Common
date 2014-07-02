using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirArena.Data;
using System.Data;
namespace AirArena.Audit
{
    public class EventType
    {
        private AirArena.Data.DataConnector DataCon;
        public EventType()
        {
            DataCon = new AirArena.Data.DataConnector(Event<int>.DataConPrefix);
        }

        public EventType(int eventTypeId)
            : this()
        {
            Load(eventTypeId);
        }
        public void Load(int eventTypeId)
        {
            var req = new AirArena.Data.DataRequest("Event.EventType_Get");
            req.Parameters.Add("@EventTypeId", eventTypeId);
            System.Data.DataTable dt = DataCon.GetDataTable(req);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                EventTypeId = int.Parse(dr["EventTypeId"].ToString());
                Name = dr["Name"].ToString();
            }
        }


        public int EventTypeId { get; private set; }
        public string Name { get; private set; }
        public enum EventTypeName
        {
            Register = 101,
            Login = 102,
            Subscribe = 103,
            Unsubscribe = 104
        }
    }
}