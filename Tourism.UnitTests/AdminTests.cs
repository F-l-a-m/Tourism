using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tourism.Domain.Abstract;
using Tourism.Domain.Entities;
using Tourism.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Tourism.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Tours()
        {
            // Arrange - create the mock repository
            Mock<ITourRepository> mock = new Mock<ITourRepository>();
            mock.Setup(m => m.Tours).Returns(new Tour[] {
                new Tour {TourID = 1, Name = "P1"},
                new Tour {TourID = 2, Name = "P2"},
                new Tour {TourID = 3, Name = "P3"},
            });

            // Arrange - create a controller
            AdminController target = new AdminController(mock.Object);

            // Action
            Tour[] result = ((IEnumerable<Tour>)target.Index().ViewData.Model).ToArray();

            // Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Tour()
        {
            // Arrange - create the mock repository
            Mock<ITourRepository> mock = new Mock<ITourRepository>();
            mock.Setup(m => m.Tours).Returns(new Tour[] {
                new Tour {TourID = 1, Name = "P1"},
                new Tour {TourID = 2, Name = "P2"},
                new Tour {TourID = 3, Name = "P3"},
            });

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Act
            Tour p1 = target.Edit(1).ViewData.Model as Tour;
            Tour p2 = target.Edit(2).ViewData.Model as Tour;
            Tour p3 = target.Edit(3).ViewData.Model as Tour;

            // Assert
            Assert.AreEqual(1, p1.TourID);
            Assert.AreEqual(2, p2.TourID);
            Assert.AreEqual(3, p3.TourID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Tour()
        {
            // Arrange - create the mock repository
            Mock<ITourRepository> mock = new Mock<ITourRepository>();
            mock.Setup(m => m.Tours).Returns(new Tour[] {
                new Tour {TourID = 1, Name = "P1"},
                new Tour {TourID = 2, Name = "P2"},
                new Tour {TourID = 3, Name = "P3"},
            });

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Act
            Tour result = (Tour)target.Edit(4).ViewData.Model;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Arrange - create mock repository
            Mock<ITourRepository> mock = new Mock<ITourRepository>();

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Arrange - create a tour
            Tour tour = new Tour { Name = "Test" };

            // Act - try to save the tour
            ActionResult result = target.Edit(tour);

            // Assert - check that the repository was called
            mock.Verify(m => m.SaveTour(tour));

            // Assert - check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange - create mock repository
            Mock<ITourRepository> mock = new Mock<ITourRepository>();

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Arrange - create a tour
            Tour tour = new Tour { Name = "Test" };

            // Arrange - add an error to the model state
            target.ModelState.AddModelError("error", "error");

            // Act - try to save the tour
            ActionResult result = target.Edit(tour);

            // Assert - check that the repository was not called
            mock.Verify(m => m.SaveTour(It.IsAny<Tour>()), Times.Never());

            // Assert - check the method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}