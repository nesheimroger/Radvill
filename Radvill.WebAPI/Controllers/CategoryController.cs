using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Radvill.Services.DataFactory;
using Radvill.WebAPI.Models;

namespace Radvill.WebAPI.Controllers
{
    public class CategoryController : ApiController
    {
        private readonly IDataFactory _dataFactory;

        public CategoryController(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }
        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of Categories (Value & Text)</returns>
        public HttpResponseMessage Get()
        {
            var categories = _dataFactory.CategoryRepository.GetAll()
                 .Select(x => new HtmlOptionDTO {Value = x.ID, Text = x.Name});

            return Request.CreateResponse(HttpStatusCode.OK, categories);
        }
    }
}
