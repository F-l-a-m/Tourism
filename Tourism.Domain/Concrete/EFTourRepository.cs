using Tourism.Domain.Abstract;
using Tourism.Domain.Entities;
using System.Collections.Generic;

namespace Tourism.Domain.Concrete
{
    public class EFTourRepository : ITourRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<Tour> Tours
        {
            get { return context.Tours; }
        }
    }
}