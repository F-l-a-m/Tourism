using System.Collections.Generic;
using Tourism.Domain.Entities;

namespace Tourism.Domain.Abstract
{
    public interface ITourRepository
    {
        IEnumerable<Tour> Tours { get; }

        void SaveTour(Tour tour);

        Tour DeleteTour(int tourID);
    }
}