using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tourism.Domain.Abstract;
using Tourism.Domain.Entities;

namespace Tourism.WebUI.Controllers
{
    public class TourController : Controller
    {
        private ITourRepository repository;
        public TourController(ITourRepository tourRepository)
        {
            this.repository = tourRepository;
        }

        public ViewResult List()
        {
            return View(repository.Tours);
        }
    }
}