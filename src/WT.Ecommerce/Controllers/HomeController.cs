using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WT.Ecommerce.Domain.Identity;
using WT.Ecommerce.Models;
using WT.Ecommerce.Services.Customer;

namespace WT.Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private IIdentityContext _identityContext;
        private readonly ICustomerInformationService _customerInformationService;

        public HomeController(ILogger<HomeController> logger,
                              IHttpClientFactory httpClientFactory,
                              IIdentityContext identityContext,
                              ICustomerInformationService customerInformationService)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _identityContext = identityContext;
            _customerInformationService = customerInformationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var claims = User.Claims.ToList();
            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

            var x = _identityContext.UserCode;

            var customer = await _customerInformationService.CreateCustomerAsync(new Domain.Models.CustomerInformation
            {
                FirstName="farzin",
                LastName="fa",
                PhoneNumber="5145550"
                
            });





            return View();
        }
    }
}
