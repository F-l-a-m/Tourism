﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tourism.Domain.Abstract;
using Tourism.Domain.Entities;
using Tourism.WebUI.Models;

namespace Tourism.WebUI.Controllers
{
    public class TourController : Controller
    {
        private ITourRepository repository;
        public int PageSize = 4;

        public TourController(ITourRepository tourRepository)
        {
            this.repository = tourRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            ToursListViewModel model = new ToursListViewModel
            {
                Tours = repository.Tours
                .Where(t => category == null || t.Category == category)
                .OrderBy(t => t.TourID)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                        repository.Tours.Count() :
                        repository.Tours.Where(e => e.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }

        public FileContentResult GetImage(int tourId)
        {
            Tour prod = repository.Tours
            .FirstOrDefault(t => t.TourID == tourId);
            if (prod != null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}