using Application.Features.Distributors.Commands;
using Application.Features.Distributors.Queries;
using Application.Features.Distributors.Requests;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class DistributorsController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Distributor)]
        public async Task<IActionResult> CreateDistributorAsync([FromBody] DistributorRequest createDistributor)
        {
            var response = await Sender.Send(new CreateDistributorCommand { DistributorRequest = createDistributor });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Distributor)]
        public async Task<IActionResult> UpdateDistributorAsync([FromBody] DistributorRequest updateDistributor)
        {
            var response = await Sender.Send(new UpdateDistributorCommand { DistributorRequest = updateDistributor });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{DistributorId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Distributor)]
        public async Task<IActionResult> DeleteDistributorAsync(Guid DistributorId)
        {
            var response = await Sender.Send(new DeleteDistributorCommand { DistributorId = DistributorId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("by-id/{DistributorId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetDistributorByIdAsync(Guid DistributorId)
        {
            var response = await Sender.Send(new GetDistributorByIdQuery { DistributorId = DistributorId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
              
        [HttpGet("all")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetDistributorsAsync()
        {
            var response = await Sender.Send(new GetDistributorsQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
               

        [HttpGet("bycon/{contactId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetDistributorByContactAsync(Guid contactId)
        {
            var response = await Sender.Send(new GetDistributorsByContactQuery { ContactId = contactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // Region
        [HttpPost("Radd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Distributor)]
        public async Task<IActionResult> CreateRegionAsync([FromBody] RegionRequest createRegion)
        {
            var response = await Sender.Send(new CreateRegionCommand { RegionRequest = createRegion });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("Rupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Distributor)]
        public async Task<IActionResult> UpdateRegionAsync([FromBody] RegionRequest updateRegion)
        {
            var response = await Sender.Send(new UpdateRegionCommand { RegionRequest = updateRegion });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("Rdelete/{RegionId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Distributor)]
        public async Task<IActionResult> DeleteRegionAsync(Guid RegionId)
        {
            var response = await Sender.Send(new DeleteRegionCommand { RegionId = RegionId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("Rby-id/{RegionId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Distributor)]
        public async Task<IActionResult> GetRegionByIdAsync(Guid RegionId)
        {
            var response = await Sender.Send(new GetRegionByIdQuery { RegionId = RegionId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        
        [HttpGet("RallbydistId/{DistributorId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetAllRegionbyDistributorAsync(Guid DistributorId)
        {
            var response = await Sender.Send(new GetAllRegionbyDistributorQuery { DistributorId = DistributorId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("Rallassigned")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetAllAssignedRegionsAsync()
        {
            var response = await Sender.Send(new GetAssignedRegionsQuery ());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("Rall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Distributor)]
        public async Task<IActionResult> GetRegionsAsync()
        {
            var response = await Sender.Send(new GetAllRegionQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // Region Contact
        [HttpPost("RCadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Distributor)]
        public async Task<IActionResult> CreateRegionContactAsync([FromBody] RegionContactRequest createRegionContact)
        {
            var response = await Sender.Send(new CreateRegionContactCommand { RegionContactRequest = createRegionContact });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("RCupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Distributor)]
        public async Task<IActionResult> UpdateRegionContactAsync([FromBody] RegionContactRequest updateRegionContact)
        {
            var response = await Sender.Send(new UpdateRegionContactCommand { RegionContactRequest = updateRegionContact });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("RCdelete/{RegionContactId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Distributor)]
        public async Task<IActionResult> DeleteRegionContactAsync(Guid RegionContactId)
        {
            var response = await Sender.Send(new DeleteRegionContactCommand { RegionContactId = RegionContactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("RCby-id/{RegionContactId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Distributor)]
        public async Task<IActionResult> GetRegionContactByIdAsync(Guid RegionContactId)
        {
            var response = await Sender.Send(new GetRegionContactByIdQuery { RegionContactId = RegionContactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("RCall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Distributor)]
        public async Task<IActionResult> GetRegionContactsAsync(Guid RegionId)
        {
            var response = await Sender.Send(new GetAllRegionContactQuery { RegionId = RegionId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("RCbydistid/{DistributorId}/{code}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetRegionContactByDistIdAsync(Guid DistributorId, string code)
        {
            var response = await Sender.Send(new GetRegionContactByDistIdQuery { DistributorId = DistributorId, Code = code});
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("RCbycontactid/{contactId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetRegionContactByContactIdAsync(Guid contactId)
        {
            var response = await Sender.Send(new GetRegionContactByContactIdQuery { ContactId = contactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        /// Misc


    }
}
