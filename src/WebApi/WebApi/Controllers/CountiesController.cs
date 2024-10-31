using Common.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountiesController : ApiControllerBase
    {
        private ICataloguesService addressService;

        public CountiesController(
            ICataloguesService cataloguesService)
        {
            this.addressService = cataloguesService ?? throw new ArgumentNullException(
                nameof(cataloguesService));
        }

        [HttpGet("{countryCode}")]
        public Task<IActionResult> Get(
            string countryCode) => ExecuteAsync(
                () => addressService.GetCounties(
                    countryCode.ToUpper()));
    }
}
