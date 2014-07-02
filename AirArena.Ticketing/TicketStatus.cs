using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AirArena.Ticketing
{
    public class TicketStatus
    {
        public enum Id
        {
            New=0,
            Open=1,
            Closed=2,
            RequireCustomerResponse=3,
            CustomerResponded=4,
            TicketTransferred=5
        }
        private static ArrayList _statii;
        static TicketStatus()
        {
            _statii = new ArrayList(5)  
                         {
                             new TicketStatus(0,"New"), 
                             new TicketStatus(1, "Open"),
                             new TicketStatus(2, "Closed"),
                             new TicketStatus(3, "Require Customer Response"),
                             new TicketStatus(4, "Customer Responded"),
                             new TicketStatus(5, "Ticket Transferred"),
                         };
        }

        public static TicketStatus GetStatus(int id)
        {
            return (TicketStatus)_statii[id];
        }
        private TicketStatus(int id, string name)
        {
            StatusId = id;
            Name = name;
        }
        public static IEnumerable<SelectListItem> List(int selectedStatusId)
        {
            return new SelectList(_statii, "StatusId", "Name", selectedStatusId);
        }
        public int StatusId { get; private set; }
        public string Name { get; private set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
