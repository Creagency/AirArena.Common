using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AirArena.Ticketing
{
    public class TicketType
    {
        public enum Id
        {
            GeneralInquiry=0,
            TechnicalSupport=1,
            Other = 2,
            CompanyPledge = 3,
        }
        private static ArrayList _typei;
        static TicketType()
        {
            _typei = new ArrayList(3)  
                         {
                             new TicketType(0,"General Inquiry"),
                             new TicketType(1,"Technical Support"),
                             new TicketType(2, "Other"),
                             new TicketType(3, "Company Pledge")
                         };
        }

        public static TicketType GetType(int id)
        {
            return (TicketType)_typei[id];
        }
        private TicketType(int id, string name)
        {
            TypeId = id;
            Name = name;
        }
        public static IEnumerable<SelectListItem> List(int selectedTypeId)
        {
            return new SelectList(_typei, "TypeId", "Name", selectedTypeId);
        }
        public int TypeId { get; private set; }
        public string Name { get; private set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
