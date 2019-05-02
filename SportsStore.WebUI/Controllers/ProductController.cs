﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductsRepository repository;
        // GET: Product
        public ProductController(IProductsRepository productsRepository)
        {
            this.repository = productsRepository;
        }

        public ViewResult List()
        {
            return View(repository.Products);
        }
    }
}