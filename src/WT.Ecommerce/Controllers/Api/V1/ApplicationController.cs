using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WT.Ecommerce.Controllers.Api.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Consumes("application/json")]
    [Produces("application/hal+json")]
    [Route("api/v{version:apiVersion}")]
    [ApiExplorerSettings(GroupName = "Applications")]
    
    public class ApplicationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
