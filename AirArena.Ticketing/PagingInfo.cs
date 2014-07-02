using System;
using System.Collections.Generic;
using System.Linq;

namespace AirArena.Ticketing
{
    public class PagingInfo
    {
        public string ActionName { get; set; }      // e.g. "AdminTicket" for /Ticket/AdminTicket
        public int CurrentPageNumber { get; set; }  // the current page
        public int PageSize { get; set; }           // the desired number of records per page
        public int TotalRecords { get; set; }

        public int TotalPages     
        {
            get
            {
                int n = (TotalRecords % PageSize) > 0 ? 1:0; 
                return ((TotalRecords / PageSize) + n)  ;
            }
        }
    }
}