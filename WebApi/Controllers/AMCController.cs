using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;
using Application.Features.AMCS.Requests;
using Application.Features.AMCS.Commands;
using Application.Features.AMCS.Queries;
using System.Security.Claims;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AMCController : BaseApiController
    {
        [HttpPost("AMCadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.AMC)]
        public async Task<IActionResult> CreateAMCAsync([FromBody] AmcRequest createAMC)
        {
            var response = await Sender.Send(new CreateAmcCommand { CreateAmcRequest = createAMC });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("AMCupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.AMC)]
        public async Task<IActionResult> UpdateAMCAsync([FromBody] AmcRequest updateAMC)
        {
            var response = await Sender.Send(new UpdateAmcCommand { Request = updateAMC });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("AMCdelete/{AMCId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.AMC)]
        public async Task<IActionResult> DeleteAMCAsync(Guid AMCId)
        {
            var response = await Sender.Send(new DeleteAmcCommand { Id = AMCId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("AMCby-id/{AMCId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.AMC)]
        public async Task<IActionResult> GetAMCByIdAsync(Guid AMCId)
        {
            var response = await Sender.Send(new GetAmcByIdQuery { Id = AMCId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("AMCall")]
        [ShouldHavePermission(CimAction.View, CimFeature.AMC)]
        public async Task<IActionResult> GetAMCsAsync()
        {
            var response = await Sender.Send(new GetAmcQuery ());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        /// AmcInstrument 
        
        [HttpPost("AIadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.AMC)]
        public async Task<IActionResult> CreateAmcInstrumentAsync([FromBody] List<AmcInstrumentRequest> createAmcInstruments)
        {
            var response = await Sender.Send(new CreateAmcInstrumentCommand { AMCInstruments = createAmcInstruments });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("AIupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.AMC)]
        public async Task<IActionResult> UpdateAmcInstrumentAsync([FromBody] AmcInstrumentRequest updateAmcInstrument)
        {
            var response = await Sender.Send(new UpdateAmcInstrumentCommand { Request = updateAmcInstrument });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpDelete("AIdelete/{AmcInstrumentId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.AMC)]
        public async Task<IActionResult> DeleteAmcInstrumentAsync(Guid AmcInstrumentId)
        {
            var response = await Sender.Send(new DeleteAmcInstrumentCommand { Id = AmcInstrumentId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        //[HttpGet("by-id/{AmcInstrumentId}")]
        //[ShouldHavePermission(CimAction.View, CimFeature.AMC)]
        //public async Task<IActionResult> GetAmcInstrumentByIdAsync(Guid AmcInstrumentId)
        //{
        //    var response = await Sender.Send(new GetAmcInstrumentQuery { AmcInstrumentId = AmcInstrumentId });
        //    if (response.IsSuccessful)
        //    {
        //        return Ok(response);
        //    }
        //    return NotFound(response);
        //}

        [HttpGet("AIall/{AMCId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.AMC)]
        public async Task<IActionResult> GetAmcInstrumentsAsync(Guid AMCId)
        {
            var response = await Sender.Send(new GetAmcInstrumentsQuery { AmcId = AMCId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost("AIexists")]
        [ShouldHavePermission(CimAction.View, CimFeature.AMC)]
        public async Task<IActionResult> ExistsInstrumentInAMC(AmcRequest amc)
        {
            var response = await Sender.Send(new ExistsInstrumentInAMCQuery { AMC = amc });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        // AMC Items

        [HttpPost("AITadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.AMC)]
        public async Task<IActionResult> CreateAmcItemsAsync([FromBody] AmcItemsRequest createAmcItems)
        {
            var response = await Sender.Send(new CreateAmcItemsCommand { Request = createAmcItems });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("AITupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.AMC)]
        public async Task<IActionResult> UpdateAmcItemsAsync([FromBody] AmcItemsRequest updateAmcItems)
        {
            var response = await Sender.Send(new UpdateAmcItemsCommand { Request = updateAmcItems });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpDelete("AITdelete/{AmcItemsId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.AMC)]
        public async Task<IActionResult> DeleteAmcItemsAsync(Guid AmcItemsId)
        {
            var response = await Sender.Send(new DeleteAmcItemsCommand { Id = AmcItemsId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        //[HttpGet("by-id/{AmcItemsId}")]
        //[ShouldHavePermission(CimAction.View, CimFeature.AMC)]
        //public async Task<IActionResult> GetAmcItemsByIdAsync(Guid AmcItemsId)
        //{
        //    var response = await Sender.Send(new GetAmcItemsQuery { AmcItemsId = AmcItemsId });
        //    if (response.IsSuccessful)
        //    {
        //        return Ok(response);
        //    }
        //    return NotFound(response);
        //}

        [HttpGet("AITall/{AMCId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.AMC)]
        public async Task<IActionResult> GetAmcItemsAsync(Guid AMCId)
        {
            var response = await Sender.Send(new GetAmcItemsQuery { AmcId = AMCId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // AMC Stages

        [HttpPost("ASadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.AMC)]
        public async Task<IActionResult> CreateAmcStagesAsync([FromBody] AmcStagesRequest createAmcStages)
        {
            var response = await Sender.Send(new CreateAmcStagesCommand { Request = createAmcStages });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("ASupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.AMC)]
        public async Task<IActionResult> UpdateAmcStagesAsync([FromBody] AmcStagesRequest updateAmcStages)
        {
            var response = await Sender.Send(new UpdateAmcStagesCommand { Request = updateAmcStages });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("ASdelete/{AmcStagesId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.AMC)]
        public async Task<IActionResult> DeleteAmcStagesAsync(Guid AmcStagesId)
        {
            var response = await Sender.Send(new DeleteAmcStagesCommand { Id = AmcStagesId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        //[HttpGet("by-id/{AmcStagesId}")]
        //[ShouldHavePermission(CimAction.View, CimFeature.AMC)]
        //public async Task<IActionResult> GetAmcStagesByIdAsync(Guid AmcStagesId)
        //{
        //    var response = await Sender.Send(new GetAmcStagesQuery { AmcStagesId = AmcStagesId });
        //    if (response.IsSuccessful)
        //    {
        //        return Ok(response);
        //    }
        //    return NotFound(response);
        //}

        [HttpGet("ASall/{AMCId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.AMC)]
        public async Task<IActionResult> GetAmcStagesAsync(Guid AMCId)
        {
            var response = await Sender.Send(new GetAmcStagesQuery { AmcId = AMCId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

       
    }
}
