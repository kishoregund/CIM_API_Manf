using Application.Features.AppBasic.Commands;
using Application.Features.AppBasic.Queries;
using Application.Features.AppBasic.Requests;
using Infrastructure.Identity;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AppBasicsController : BaseApiController
    {

        [HttpGet("GetModalData")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)] /// commented as the logged in user needs this data
        public async Task<IActionResult> GetModalDataAsync()
        {

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

            var response = await Sender.Send(new GetModalDataQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }




        ///  Business Unit
        [HttpPost("BUadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.BusinessUnit)]
        public async Task<IActionResult> CreateBusinessUnitAsync([FromBody] BusinessUnitRequest createBusinessUnit)
        {            
            var response = await Sender.Send(new CreateBusinessUnitCommand { Request = createBusinessUnit });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("BUupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.BusinessUnit)]
        public async Task<IActionResult> UpdateBusinessUnitAsync([FromBody] UpdateBusinessUnitRequest updateBusinessUnit)
        {
            var response = await Sender.Send(new UpdateBusinessUnitCommand { Request = updateBusinessUnit });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("BUdelete/{BusinessUnitId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.BusinessUnit)]
        public async Task<IActionResult> DeleteBusinessUnitAsync(Guid BusinessUnitId)
        {
            var response = await Sender.Send(new DeleteBusinessUnitByIdCommand { Id = BusinessUnitId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("BUby-id/{BusinessUnitId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetBusinessUnitByIdAsync(Guid BusinessUnitId)
        {
            var response = await Sender.Send(new GetBusinessUnitByIdQuery { BusinessUnitId = BusinessUnitId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
              
        [HttpGet("BUall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetBusinessUnitsAsync()
        {
            var response = await Sender.Send(new GetBusinessUnitsQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        //Brand

        [HttpPost("BRadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Brand)]
        public async Task<IActionResult> CreateBrandAsync([FromBody] BrandRequest createBrand)
        {
            var response = await Sender.Send(new CreateBrandCommand { Request = createBrand });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("BRupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Brand)]
        public async Task<IActionResult> UpdateBrandAsync([FromBody] UpdateBrandRequest updateBrand)
        {
            var response = await Sender.Send(new UpdateBrandCommand { Request = updateBrand });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("BRdelete/{BrandId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Brand)]
        public async Task<IActionResult> DeleteBrandAsync(Guid BrandId)
        {
            var response = await Sender.Send(new DeleteBrandByIdCommand { Id = BrandId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("BRby-id/{BrandId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetBrandByIdAsync(Guid BrandId)
        {
            var response = await Sender.Send(new GetBrandByIdQuery { BrandId = BrandId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("BRByBUall/{BusinessUnitId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)] /// commented as the logged in user needs this data
        public async Task<IActionResult> GetBrandsAsync(Guid BusinessUnitId)
        {
            var response = await Sender.Send(new GetBrandsByBusinessUnitIdQuery { BusinessUnitId = BusinessUnitId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("BRByBUs/{businessUnitIds}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetBrandsByBUsAsync(string businessUnitIds)
        {
            var response = await Sender.Send(new GetBrandsByBusinessUnitsQuery { BusinessUnits = businessUnitIds });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpGet("BRall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetBrandsAsync()
        {
            var response = await Sender.Send(new GetBrandsQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        /////////// Manufacturer Business Units

        ///  Business Unit
        [HttpPost("MBUadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.ManfBusinessUnit)]
        public async Task<IActionResult> CreateManfBusinessUnitAsync([FromBody] ManfBusinessUnitRequest createManfBusinessUnit)
        {
            var response = await Sender.Send(new CreateManfBusinessUnitCommand { Request = createManfBusinessUnit });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("MBUupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.ManfBusinessUnit)]
        public async Task<IActionResult> UpdateManfBusinessUnitAsync([FromBody] UpdateManfBusinessUnitRequest updateManfBusinessUnit)
        {
            var response = await Sender.Send(new UpdateManfBusinessUnitCommand { Request = updateManfBusinessUnit });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("MBUdelete/{ManfBusinessUnitId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.ManfBusinessUnit)]
        public async Task<IActionResult> DeleteManfBusinessUnitAsync(Guid ManfBusinessUnitId)
        {
            var response = await Sender.Send(new DeleteManfBusinessUnitByIdCommand { Id = ManfBusinessUnitId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("MBUby-id/{ManfBusinessUnitId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetManfBusinessUnitByIdAsync(Guid ManfBusinessUnitId)
        {
            var response = await Sender.Send(new GetManfBusinessUnitByIdQuery { ManfBusinessUnitId = ManfBusinessUnitId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("MBUall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetManfBusinessUnitsAsync()
        {
            var response = await Sender.Send(new GetManfBusinessUnitsQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
