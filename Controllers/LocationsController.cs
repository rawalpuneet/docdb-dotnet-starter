using docdb_dotnet_starter.Models;
using docdb_dotnet_starter.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace docdb_dotnet_starter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly LocationsService _locationService;

        public LocationsController(LocationsService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public ActionResult<List<Location>> Get() =>
            _locationService.Get();

        [HttpGet("{id:length(24)}", Name = "GetLocation")]
        public ActionResult<Location> Get(string id)
        {
            var location = _locationService.Get(id);

            if (location == null)
            {
                return NotFound();
            }

            return location;
        }

        [HttpPost]
        public ActionResult<Location> Create(Location location)
        {
            _locationService.Create(location);

            return CreatedAtRoute("GetLocation", new { id = location.Id.ToString() }, location);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Location locationIn)
        {
            var location = _locationService.Get(id);

            if (location == null)
            {
                return NotFound();
            }

            _locationService.Update(id, locationIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var location = _locationService.Get(id);

            if (location == null)
            {
                return NotFound();
            }

            _locationService.Delete(location.Id);

            return NoContent();
        }
    }
}