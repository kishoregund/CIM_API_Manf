
using Application.Features.Spares.Commands;
using Application.Features.Spares.Queries;
using Application.Features.Spares.Requests;
using Domain.Entities;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class SparepartQuotationsController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> CreateOfferRequestAsync([FromBody] OfferRequestRequest createOfferRequest)
        {
            var response = await Sender.Send(new CreateOfferRequestCommand { OfferRequestRequest = createOfferRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> UpdateOfferRequestAsync([FromBody] OfferRequestRequest updateRequest)
        {
            var response = await Sender.Send(new UpdateOfferRequestCommand { OfferRequestRequest = updateRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpDelete("delete/{offerRequestId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> DeleteOfferRequestAsync(Guid offerRequestId)
        {
            var response = await Sender.Send(new DeleteOfferRequestCommand { OfferRequestId = offerRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("by-id/{offerRequestId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> GetofferRequestByIdAsync(Guid offerRequestId)
        {
            var response = await Sender.Send(new GetOfferRequestByIdQuery { Id = offerRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
              
        [HttpGet("all")]
        [ShouldHavePermission(CimAction.View, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> GetOfferRequestsAsync()
        {
            var response = await Sender.Send(new GetOfferRequestsQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("partno/{instrumentId}/{partNo}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> GetofferRequestByIdAsync(string instrumentId, string partNo)
        {
            var response = await Sender.Send(new GetSparepartsByInstrumentPartNoQuery { InstrumentIds = instrumentId, PartNo = partNo });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }        

        [HttpPost("ORPadd")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> CreateOfferRequestProcessAsync([FromBody] OfferRequestProcessRequest createOfferRequestProcess)
        {
            var response = await Sender.Send(new CreateOfferRequestProcessCommand { OfferRequestProcessRequest = createOfferRequestProcess });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("ORPupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> UpdateOfferRequestProcessAsync([FromBody] OfferRequestProcessRequest updateRequest)
        {
            var response = await Sender.Send(new UpdateOfferRequestProcessCommand { OfferRequestProcessRequest = updateRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("ORPdelete/{OfferRequestProcessId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> DeleteOfferRequestProcessAsync(Guid OfferRequestProcessId)
        {
            var response = await Sender.Send(new DeleteOfferRequestProcessCommand { OfferRequestProcessId = OfferRequestProcessId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("ORPby-id/{OfferRequestProcessId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> GetOfferRequestProcessByIdAsync(Guid OfferRequestProcessId)
        {
            var response = await Sender.Send(new GetOfferRequestProcessByIdQuery { Id = OfferRequestProcessId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("ORPall/{offerRequestId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> GetOfferRequestProcesssAsync(Guid offerRequestId)
        {
            var response = await Sender.Send(new GetOfferRequestProcessesQuery { OfferRequestId = offerRequestId});
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost("SPORadd")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> CreateSparepartOfferRequestAsync([FromBody] List<SparepartOfferRequestRequest> createSparepartOfferRequest)
        {
            var response = await Sender.Send(new CreateSparepartsOfferRequestCommand { SparepartsOfferRequestRequest = createSparepartOfferRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("SPORupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> UpdateSparepartOfferRequestAsync([FromBody] SparepartOfferRequestRequest updateRequest)
        {
            var response = await Sender.Send(new UpdateSparepartsOfferRequestCommand { SparepartsOfferRequestRequest = updateRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpDelete("SPORdelete/{sparepartOfferRequestId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> DeleteSparepartOfferRequestAsync(Guid sparepartOfferRequestId)
        {
            var response = await Sender.Send(new DeleteSparepartsOfferRequestCommand { SparepartsOfferRequestId = sparepartOfferRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("SPORby-id/{sparepartOfferRequestId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> GetSparepartOfferRequestByIdAsync(Guid sparepartOfferRequestId)
        {
            var response = await Sender.Send(new GetSparepartsOfferRequestByIdQuery { Id = sparepartOfferRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SPORall/{offerRequestId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Sparepart_Quotation)]
        public async Task<IActionResult> GetSparepartOfferRequestsAsync(Guid offerRequestId)
        {
            var response = await Sender.Send(new GetSparepartsOfferRequestsQuery { OfferRequestId = offerRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
