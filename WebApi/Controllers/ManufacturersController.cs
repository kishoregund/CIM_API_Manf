using Application.Features.Manufacturers.Commands;
using Application.Features.Manufacturers.Queries;
using Application.Features.Manufacturers.Requests;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ManufacturersController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Manufacturer)]
        public async Task<IActionResult> CreateManufacturerAsync([FromBody] ManufacturerRequest createManufacturer)
        {
            var response = await Sender.Send(new CreateManufacturerCommand { ManufacturerRequest = createManufacturer });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Manufacturer)]
        public async Task<IActionResult> UpdateManufacturerAsync([FromBody] ManufacturerRequest updateManufacturer)
        {
            var response = await Sender.Send(new UpdateManufacturerCommand { ManufacturerRequest = updateManufacturer });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{ManufacturerId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Manufacturer)]
        public async Task<IActionResult> DeleteManufacturerAsync(Guid ManufacturerId)
        {
            var response = await Sender.Send(new DeleteManufacturerCommand { ManufacturerId = ManufacturerId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("by-id/{ManufacturerId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetManufacturerByIdAsync(Guid ManufacturerId)
        {
            var response = await Sender.Send(new GetManufacturerByIdQuery { ManufacturerId = ManufacturerId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
              
        [HttpGet("all")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetManufacturersAsync()
        {
            var response = await Sender.Send(new GetAllManufacturerQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // Sales Region
        [HttpPost("SRadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Manufacturer)]
        public async Task<IActionResult> CreateSalesRegionAsync([FromBody] SalesRegionRequest createSalesRegion)
        {
            var response = await Sender.Send(new CreateSalesRegionCommand { SalesRegionRequest = createSalesRegion });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("SRupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Manufacturer)]
        public async Task<IActionResult> UpdateSalesRegionAsync([FromBody] SalesRegionRequest updateSalesRegion)
        {
            var response = await Sender.Send(new UpdateSalesRegionCommand { SalesRegionRequest = updateSalesRegion });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("SRdelete/{SalesRegionId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Manufacturer)]
        public async Task<IActionResult> DeleteSalesRegionAsync(Guid SalesRegionId)
        {
            var response = await Sender.Send(new DeleteSalesRegionCommand { SalesRegionId = SalesRegionId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("SRby-id/{SalesRegionId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Manufacturer)]
        public async Task<IActionResult> GetSalesRegionByIdAsync(Guid SalesRegionId)
        {
            var response = await Sender.Send(new GetSalesRegionByIdQuery { SalesRegionId = SalesRegionId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SRall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Manufacturer)]
        public async Task<IActionResult> GetSalesRegionsAsync(Guid ManufacturerId)
        {
            var response = await Sender.Send(new GetAllSalesRegionQuery { ManufacturerId = ManufacturerId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        // SalesRegion Contact
        [HttpPost("SRCadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Manufacturer)]
        public async Task<IActionResult> CreateSalesRegionContactAsync([FromBody] SalesRegionContactRequest createSalesRegionContact)
        {
            var response = await Sender.Send(new CreateSalesRegionContactCommand { SalesRegionContactRequest = createSalesRegionContact });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("SRCupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Manufacturer)]
        public async Task<IActionResult> UpdateSalesRegionContactAsync([FromBody] SalesRegionContactRequest updateSalesRegionContact)
        {
            var response = await Sender.Send(new UpdateSalesRegionContactCommand { SalesRegionContactRequest = updateSalesRegionContact });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("SRCdelete/{SalesRegionContactId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Manufacturer)]
        public async Task<IActionResult> DeleteSalesRegionContactAsync(Guid SalesRegionContactId)
        {
            var response = await Sender.Send(new DeleteSalesRegionContactCommand { SalesRegionContactId = SalesRegionContactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("SRCby-id/{SalesRegionContactId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Manufacturer)]
        public async Task<IActionResult> GetSalesRegionContactByIdAsync(Guid SalesRegionContactId)
        {
            var response = await Sender.Send(new GetSalesRegionContactByIdQuery { SalesRegionContactId = SalesRegionContactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SRCall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Manufacturer)]
        public async Task<IActionResult> GetSalesRegionContactsAsync(Guid SalesRegionId)
        {
            var response = await Sender.Send(new GetAllSalesRegionContactQuery { SalesRegionId = SalesRegionId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
