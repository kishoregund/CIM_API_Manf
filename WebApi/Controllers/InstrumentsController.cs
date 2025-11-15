using Application.Features.InstrumentAllocations.Commands;
using Application.Features.InstrumentAllocations.Queries;
using Application.Features.Instruments.Commands;
using Application.Features.Instruments.Queries;
using Application.Features.Instruments.Requests;
using Application.Models;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class InstrumentsController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Instrument)]
        public async Task<IActionResult> CreateInstrumentAsync([FromBody] InstrumentRequest createInstrument)
        {
            var response = await Sender.Send(new CreateInstrumentCommand { InstrumentRequest = createInstrument });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Instrument)]
        public async Task<IActionResult> UpdateInstrumentAsync([FromBody] InstrumentRequest updateInstrument)
        {
            var response = await Sender.Send(new UpdateInstrumentCommand { InstrumentRequest = updateInstrument });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpDelete("delete/{InstrumentId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Instrument)]
        public async Task<IActionResult> DeleteInstrumentAsync(Guid InstrumentId)
        {
            var response = await Sender.Send(new DeleteInstrumentCommand { InstrumentId = InstrumentId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("by-id/{InstrumentId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Instrument)]
        public async Task<IActionResult> GetInstrumentByIdAsync(Guid InstrumentId)
        {
            var response = await Sender.Send(new GetInstrumentByIdQuery { InstrumentId = InstrumentId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("byserno/{InsSerialNo}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Instrument)]
        public async Task<IActionResult> GetInstrumentByIdAsync(string InsSerialNo)
        {
            var response = await Sender.Send(new GetInstrumentBySerialNoQuery { serialNo = InsSerialNo });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        

        [HttpPost("all")]
        [ShouldHavePermission(CimAction.View, CimFeature.Instrument)]
        public async Task<IActionResult> GetInstrumentsAsync(BUBrand buBrand)
        {
            var response = await Sender.Send(new GetInstrumentQuery { BUBrandModel = buBrand});
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        //Instrument Accessory

        // SR Action
        [HttpPost("IAadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Instrument)]
        public async Task<IActionResult> CreateInstrumentAccessoryAsync([FromBody] InstrumentAccessoryRequest createInstrumentAccessory)
        {
            var response = await Sender.Send(new CreateInstrumentAccessoryCommand { InstrumentAccessoryRequest = createInstrumentAccessory });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("IAupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Instrument)]
        public async Task<IActionResult> UpdateInstrumentAccessoryAsync([FromBody] InstrumentAccessoryRequest updateInstrumentAccessory)
        {
            var response = await Sender.Send(new UpdateInstrumentAccessoryCommand { InstrumentAccessoryRequest = updateInstrumentAccessory });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("IAdelete/{InstrumentAccessoryId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Instrument)]
        public async Task<IActionResult> DeleteInstrumentAccessoryAsync(Guid InstrumentAccessoryId)
        {
            var response = await Sender.Send(new DeleteInstrumentAccessoryCommand { InstrumentAccessoryId = InstrumentAccessoryId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("IAby-id/{InstrumentAccessoryId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Instrument)]
        public async Task<IActionResult> GetInstrumentAccessoryByIdAsync(Guid InstrumentAccessoryId)
        {
            var response = await Sender.Send(new GetInstrumentAccessoryByIdQuery { InstrumentAccessoryId = InstrumentAccessoryId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("IAall/{InstrumentId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Instrument)]
        public async Task<IActionResult> GetInstrumentAccessoriesAsync(Guid InstrumentId)
        {
            var response = await Sender.Send(new GetInstrumentAccessoryQuery { InstrumentId = InstrumentId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // Instrument Spares

        // SR Action
        [HttpPost("ISadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Instrument)]
        public async Task<IActionResult> CreateInstrumentSparesAsync([FromBody] InstrumentSparesRequest createInstrumentSpares)
        {
            var response = await Sender.Send(new CreateInstrumentSparesCommand { InstrumentSparesRequest = createInstrumentSpares });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("ISupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Instrument)]
        public async Task<IActionResult> UpdateInstrumentSparesAsync([FromBody] InstrumentSparesRequest updateInstrumentSpares)
        {
            var response = await Sender.Send(new UpdateInstrumentSparesCommand { InstrumentSparesRequest = updateInstrumentSpares });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("ISdelete/{InstrumentSparesId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Instrument)]
        public async Task<IActionResult> DeleteInstrumentSparesAsync(Guid InstrumentSparesId)
        {
            var response = await Sender.Send(new DeleteInstrumentSparesCommand { InstrumentSparesId = InstrumentSparesId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("ISby-id/{InstrumentSparesId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Instrument)]
        public async Task<IActionResult> GetInstrumentSparesByIdAsync(Guid InstrumentSparesId)
        {
            var response = await Sender.Send(new GetInstrumentSparesByIdQuery { InstrumentSparesId = InstrumentSparesId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("ISall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Instrument)]
        public async Task<IActionResult> GetInstrumentSparesAsync(Guid InstrumentId)
        {
            var response = await Sender.Send(new GetInstrumentSparesQuery { InstrumentId = InstrumentId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        //// Instrument Allocation
        [HttpPost("InAlladd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Instrument_Allocation)]
        public async Task<IActionResult> CreateInstrumentAllocationAsync([FromBody] InstrumentAllocationRequest createInstrumentAllocation)
        {
            var response = await Sender.Send(new CreateInstrumentAllocationCommand { InstrumentAllocationRequest = createInstrumentAllocation });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("InAllupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Instrument_Allocation)]
        public async Task<IActionResult> UpdateInstrumentAllocationAsync([FromBody] InstrumentAllocationRequest updateInstrumentAllocation)
        {
            var response = await Sender.Send(new UpdateInstrumentAllocationCommand { InstrumentAllocationRequest = updateInstrumentAllocation });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpDelete("InAlldelete/{InstrumentAllocationId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Instrument_Allocation)]
        public async Task<IActionResult> DeleteInstrumentAllocationAsync(Guid InstrumentAllocationId)
        {
            var response = await Sender.Send(new DeleteInstrumentAllocationCommand { InstrumentAllocationId = InstrumentAllocationId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("InAllby-id/{InstrumentAllocationId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Instrument_Allocation)]
        public async Task<IActionResult> GetInstrumentAllocationByIdAsync(Guid InstrumentAllocationId)
        {
            var response = await Sender.Send(new GetInstrumentAllocationByIdQuery { InstrumentAllocationId = InstrumentAllocationId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        
        [HttpGet("InAllby-insid/{InstrumentId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Instrument_Allocation)]
        public async Task<IActionResult> GetInstrumentAllocationByInsIdAsync(Guid InstrumentId)
        {
            var response = await Sender.Send(new GetInstrumentAllocationByInsIdQuery { InstrumentId = InstrumentId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("InAllall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Instrument_Allocation)]
        public async Task<IActionResult> GetInstrumentAllocationsAsync()
        {
            var response = await Sender.Send(new GetInstrumentAllocationQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
