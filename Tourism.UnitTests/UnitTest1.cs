using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Tourism.Domain.Abstract;
using Tourism.Domain.Entities;
using Tourism.WebUI.Controllers;
using Tourism.WebUI.Models;
using Tourism.WebUI.HtmlHelpers;

namespace Tourism.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Arrange
            Mock<ITourRepository> mock = new Mock<ITourRepository>();
            mock.Setup(m => m.Tours).Returns(new Tour[] 
            {
                new Tour {TourID = 1, Name = "P1"},
                new Tour {TourID = 2, Name = "P2"},
                new Tour {TourID = 3, Name = "P3"},
                new Tour {TourID = 4, Name = "P4"},
                new Tour {TourID = 5, Name = "P5"}
            });
            TourController controller = new TourController(mock.Object);
            controller.PageSize = 3;

            // Act
            ToursListViewModel result = (ToursListViewModel)controller.List(null, 2).Model;

            // Assert
            Tour[] tourArray = result.Tours.ToArray();
            Assert.IsTrue(tourArray.Length == 2);
            Assert.AreEqual(tourArray[0].Name, "P4");
            Assert.AreEqual(tourArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange - define an HTML helper - we need to do this
            // in order to apply the extension method
            HtmlHelper myHelper = null;
            // Arrange - create PagingInfo data
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            // Arrange - set up the delegate using a lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
            + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
            result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            Mock<ITourRepository> mock = new Mock<ITourRepository>();
            mock.Setup(m => m.Tours).Returns(new Tour[] 
            {
                new Tour {TourID = 1, Name = "P1"},
                new Tour {TourID = 2, Name = "P2"},
                new Tour {TourID = 3, Name = "P3"},
                new Tour {TourID = 4, Name = "P4"},
                new Tour {TourID = 5, Name = "P5"}
            });

            // Arrange
            TourController controller = new TourController(mock.Object);
            controller.PageSize = 3;

            // Act
            ToursListViewModel result = (ToursListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Tours()
        {
            // Arrange
            // - create the mock repository
            Mock<ITourRepository> mock = new Mock<ITourRepository>();
            mock.Setup(m => m.Tours).Returns(new Tour[] 
            {
            new Tour {TourID = 1, Name = "P1", Category = "Cat1"},
            new Tour {TourID = 2, Name = "P2", Category = "Cat2"},
            new Tour {TourID = 3, Name = "P3", Category = "Cat1"},
            new Tour {TourID = 4, Name = "P4", Category = "Cat2"},
            new Tour {TourID = 5, Name = "P5", Category = "Cat3"}
            } );

            // Arrange - create a controller and make the page size 3 items
            TourController controller = new TourController(mock.Object);
            controller.PageSize = 3;

            // Action
            Tour[] result = ((ToursListViewModel)controller.List("Cat2", 1).Model)
            .Tours.ToArray();

            // Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }
    }
}