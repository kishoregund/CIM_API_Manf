
using Application.Features.Spares.Commands;
using Application.Features.Spares.Queries;
using Application.Features.Spares.Requests;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class SparepartController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Spare_Part)]
        public async Task<IActionResult> CreateSparepartAsync([FromBody] SparepartRequest createSparepart)
        {
            var response = await Sender.Send(new CreateSparepartCommand { SparepartRequest = createSparepart });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Spare_Part)]
        public async Task<IActionResult> UpdateSparepartAsync([FromBody] SparepartRequest updateSparepart)
        {
            var response = await Sender.Send(new UpdateSparepartCommand { SparepartRequest = updateSparepart });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{SparepartId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Spare_Part)]
        public async Task<IActionResult> DeleteSparepartAsync(Guid SparepartId)
        {
            var response = await Sender.Send(new DeleteSparePartCommand { SparepartId = SparepartId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("by-id/{SparepartId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Spare_Part)]
        public async Task<IActionResult> GetSparepartByIdAsync(Guid SparepartId)
        {
            var response = await Sender.Send(new GetSparepartQuery { Id = SparepartId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("all")]
        [ShouldHavePermission(CimAction.View, CimFeature.Spare_Part)]
        public async Task<IActionResult> GetSparepartAsync()
        {
            var response = await Sender.Send(new GetSparepartsQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("byconfigs/{configTypeId}")] ///{configValueId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Spare_Part)]
        public async Task<IActionResult> GetConfigSparepartQuery(Guid configTypeId)//, Guid configValueId)
        {
            var response = await Sender.Send(new GetConfigSparepartQuery { configTypeId = configTypeId });//, configValueId = configValueId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("bypartno/{partNo}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Spare_Part)]
        public async Task<IActionResult> GetSparepartByPartNoAsync(string partNo)
        {
            var response = await Sender.Send(new GetSparepartByPartNoQuery { partNo = partNo });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
