using System.Collections.Generic;
using System.Linq;

namespace Tourism.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Tour tour, int quantity)
        {
            CartLine line = lineCollection
            .Where(t => t.Tour.TourID == tour.TourID)
            .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Tour = tour,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Tour tour)
        {
            lineCollection.RemoveAll(l => l.Tour.TourID == tour.TourID);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Tour.Price * e.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Tour Tour { get; set; }
        public int Quantity { get; set; }
    }
}