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

        public void SaveTour(Tour tour)
        {
            if (tour.TourID == 0)
            {
                context.Tours.Add(tour);
            }
            else
            {
                Tour dbEntry = context.Tours.Find(tour.TourID);

                if (dbEntry != null)
                {
                    dbEntry.Name = tour.Name;
                    dbEntry.Description = tour.Description;
                    dbEntry.Price = tour.Price;
                    dbEntry.Category = tour.Category;
                }
            }
            context.SaveChanges();
        }

        public Tour DeleteTour(int tourID)
        {
            Tour dbEntry = context.Tours.Find(tourID);
            if (dbEntry != null)
            {
                context.Tours.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}