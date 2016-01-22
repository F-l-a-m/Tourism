using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tourism.Domain.Abstract;
using Tourism.Domain.Entities;
using Tourism.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace Tourism.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Arrange - create a Tour with image data
            Tour tour = new Tour
            {
                TourID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Arrange - create the mock repository
            Mock<ITourRepository> mock = new Mock<ITourRepository>();
            mock.Setup(m => m.Tours).Returns(new Tour[] {
                new Tour {TourID = 1, Name = "P1"},
                tour,
                new Tour {TourID = 3, Name = "P3"}
            }.AsQueryable());

            // Arrange - create the controller
            TourController target = new TourController(mock.Object);

            // Act - call the GetImage action method
            ActionResult result = target.GetImage(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(tour.ImageMimeType, ((FileResult)result).ContentType);
        }


        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Arrange - create the mock repository
            Mock<ITourRepository> mock = new Mock<ITourRepository>();
            mock.Setup(m => m.Tours).Returns(new Tour[] {
                new Tour {TourID = 1, Name = "P1"},
                new Tour {TourID = 2, Name = "P2"}
            }.AsQueryable());

            // Arrange - create the controller
            TourController target = new TourController(mock.Object);

            // Act - call the GetImage action method
            ActionResult result = target.GetImage(100);

            // Assert
            Assert.IsNull(result);
        }


    }
}
