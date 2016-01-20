using System.Collections.Generic;
using Tourism.Domain.Entities;

namespace Tourism.WebUI.Models
{
    public class ToursListViewModel
    {
        public IEnumerable<Tour> Tours { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}