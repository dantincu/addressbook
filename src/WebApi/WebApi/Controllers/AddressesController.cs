using Common.Entities;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ApiControllerBase
    {
        private IAddressService addressService;

        public AddressesController(
            IAddressService addressService)
        {
            this.addressService = addressService ?? throw new ArgumentNullException(
                nameof(addressService));
        }

        [HttpGet]
        public Task<IActionResult> Get() => ExecuteAsync(
            addressService.GetAllAsync);

        [HttpGet("{id}")]
        public Task<IActionResult> Get(
            int id) => ExecuteAsync((
                ) => addressService.GetAsync(id));

        [HttpPost]
        public Task<IActionResult> Post(
            [FromBody] Address value) => ExecuteAsync((
                ) => addressService.CreateAsync(value));

        /// <summary>
        /// This endpoint will returned data queried from the database based on the properties present in the filter object.
        /// Despite having the http verb POST, it will never persist anything in the database. I changed the http verb from
        /// GET to POST because the GET requsts do not support request body content, and I did not want to put all the
        /// filter property values in the query string because for some requests the query string might become quite large
        /// and this would not be entirely ok either.
        /// </summary>
        /// <param name="filter">The filter propery values.</param>
        /// <returns>A collection containing the address entities that satisfy the filter conditions.</returns>
        [HttpGet]
        [Route("get-filtered")]
        public Task<IActionResult> Post(
            [FromBody] AddressFilter filter) => ExecuteAsync(
                () => addressService.GetFilteredAddressesAsync(filter));

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            [FromBody] Address value)
        {
            value.Id = id;

            var result = await ExecuteAsync(
                () => addressService.UpdateAsync(value));

            return result;
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(
            int id) => ExecuteAsync(
                () => addressService.DeleteAsync(id));
    }
}
