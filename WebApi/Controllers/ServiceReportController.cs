using Application.Features.Customers.Queries;
using Application.Features.ServiceReports;
using Application.Features.ServiceReports.Commands;
using Application.Features.ServiceReports.Queries;
using Application.Features.ServiceReports.Requests;
using Application.Features.ServiceRequests.Commands;
using Application.Features.ServiceRequests.Queries;
using Application.Features.ServiceRequests.Requests;
using Application.Features.SPConsumeds.Commands;
using Application.Models;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ServiceReportsController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Service_Report)]
        public async Task<IActionResult> CreateServiceReportAsync([FromBody] ServiceReportRequest createServiceReport)
        {
            var response = await Sender.Send(new CreateServiceReportCommand { ServiceReportRequest = createServiceReport });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Report)]
        public async Task<IActionResult> UpdateServiceReportAsync([FromBody] ServiceReportRequest updateServiceReport)
        {
            var response = await Sender.Send(new UpdateServiceReportCommand { ServiceReportRequest = updateServiceReport });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{ServiceReportId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Service_Report)]
        public async Task<IActionResult> DeleteServiceReportAsync(Guid ServiceReportId)
        {
            var response = await Sender.Send(new DeleteServiceReportCommand { ServiceReportId = ServiceReportId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("by-id/{ServiceReportId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetServiceReportByIdAsync(Guid ServiceReportId)
        {
            var response = await Sender.Send(new GetServiceReportQuery { ServiceReportId = ServiceReportId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
              
        [HttpGet("all")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetServiceReportsAsync()
        {
            var response = await Sender.Send(new GetServiceReportsQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("CSPInvenbysrp/{serviceReportId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetCustSpInventoryServiceReportsAsync(Guid serviceReportId)
        {
            var response = await Sender.Send(new GetCustSPInventoryBySRPIdQuery { ServiceReportId = serviceReportId});
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpGet("pdf/{ServiceReportId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetServiceReportPDFByIdAsync(Guid ServiceReportId)
        {
            var response = await Sender.Send(new GetServiceReportPDFQuery { ServiceReportId = ServiceReportId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost("uploadreport")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> UploadServiceReportRequestAsync([FromBody] UploadServiceReportRequest serviceReport)
        {
            var response = await Sender.Send(new UploadServiceReportCommand { uploadRequest = serviceReport });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // SRP SP Consumed
        [HttpPost("SPCadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Service_Report)]
        public async Task<IActionResult> CreateSPConsumedAsync([FromBody] SPConsumedRequest createSPConsumed)
        {
            var response = await Sender.Send(new CreateSPConsumedCommand { SPConsumedRequest = createSPConsumed });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("SPCupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Report)]
        public async Task<IActionResult> UpdateSPConsumedAsync([FromBody] SPConsumedRequest updateSPConsumed)
        {
            var response = await Sender.Send(new UpdateSPConsumedCommand { SPConsumedRequest = updateSPConsumed });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpDelete("SPCdelete/{SPConsumedId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Service_Report)]
        public async Task<IActionResult> DeleteSPConsumedAsync(Guid SPConsumedId)
        {
            var response = await Sender.Send(new DeleteSPConsumedCommand { SPConsumedId = SPConsumedId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("SPCby-id/{SPConsumedId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetSPConsumedByIdAsync(Guid SPConsumedId)
        {
            var response = await Sender.Send(new GetSPConsumedQuery { SPConsumedId = SPConsumedId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SPCall/{ServiceReportId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetSPConsumedBySRPIdAsync(Guid ServiceReportId)
        {
            var response = await Sender.Send(new GetSPConsumedBySRPIdQuery { ServiceReportId = ServiceReportId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // SRP SPRecommended
        [HttpPost("SPRadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Service_Report)]
        public async Task<IActionResult> CreateSPRecommendedAsync([FromBody] SPRecommendedRequest createSPRecommended)
        {
            var response = await Sender.Send(new CreateSPRecommendedCommand { SPRecommendedRequest = createSPRecommended });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("SPRupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Report)]
        public async Task<IActionResult> UpdateSPRecommendedAsync([FromBody] SPRecommendedRequest updateSPRecommended)
        {
            var response = await Sender.Send(new UpdateSPRecommendedCommand { SPRecommendedRequest = updateSPRecommended });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("SPRdelete/{SPRecommendedId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Service_Report)]
        public async Task<IActionResult> DeleteSPRecommendedAsync(Guid SPRecommendedId)
        {
            var response = await Sender.Send(new DeleteSPRecommendedCommand { SPRecommendedId = SPRecommendedId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("SPRby-id/{SPRecommendedId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetSPRecommendedByIdAsync(Guid SPRecommendedId)
        {
            var response = await Sender.Send(new GetSPRecommendedQuery { SPRecommendedId = SPRecommendedId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SPRall/{ServiceReportId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetSPRecommendedBySRPIdAsync(Guid ServiceReportId)
        {
            var response = await Sender.Send(new GetSPRecommendedBySRPIdQuery { ServiceReportId = ServiceReportId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SPRbyserreq/{ServiceRequestId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetSPRecommendedBySerReqAsync(Guid ServiceRequestId)
        {
            var response = await Sender.Send(new GetSPRecommendedBySerReqQuery { ServiceRequestId = ServiceRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpPost("SPRgrid")]
        [ShouldHavePermission(CimAction.View, CimFeature.Spareparts_Recommended)]
        public async Task<IActionResult> GetSPRecommendedGridAsync([FromBody] BUBrand buBrand)
        {
            var response = await Sender.Send(new GetSPRecommendedGridQuery { BUBrand = buBrand });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        //SRP WorkDone
        [HttpPost("WDadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Service_Report)]
        public async Task<IActionResult> CreateSRPEngWorkDoneAsync([FromBody] SRPEngWorkDoneRequest createSRPEngWorkDone)
        {
            var response = await Sender.Send(new CreateSRPEngWorkDoneCommand { SRPEngWorkDoneRequest = createSRPEngWorkDone });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("WDupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Report)]
        public async Task<IActionResult> UpdateSRPEngWorkDoneAsync([FromBody] SRPEngWorkDoneRequest updateSRPEngWorkDone)
        {
            var response = await Sender.Send(new UpdateSRPEngWorkDoneCommand { SRPEngWorkDoneRequest = updateSRPEngWorkDone });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("WDdelete/{SRPEngWorkDoneId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Service_Report)]
        public async Task<IActionResult> DeleteSRPEngWorkDoneAsync(Guid SRPEngWorkDoneId)
        {
            var response = await Sender.Send(new DeleteSRPEngWorkDoneCommand { SRPEngWorkDoneId = SRPEngWorkDoneId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("WDby-id/{SRPEngWorkDoneId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetSRPEngWorkDoneByIdAsync(Guid SRPEngWorkDoneId)
        {
            var response = await Sender.Send(new GetSRPEngWorkDoneQuery { SRPEngWorkDoneId = SRPEngWorkDoneId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("WDall/{ServiceReportId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetSRPEngWorkDoneBySRPIdAsync(Guid ServiceReportId)
        {
            var response = await Sender.Send(new GetSRPEngWorkDoneBySRPIdQuery { ServiceReportId = ServiceReportId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        //SRPEngWorkTime
        [HttpPost("WTadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Service_Report)]
        public async Task<IActionResult> CreateSRPEngWorkTimeAsync([FromBody] SRPEngWorkTimeRequest createSRPEngWorkTime)
        {
            var response = await Sender.Send(new CreateSRPEngWorkTimeCommand { SRPEngWorkTimeRequest = createSRPEngWorkTime });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("WTupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Report)]
        public async Task<IActionResult> UpdateSRPEngWorkTimeAsync([FromBody] SRPEngWorkTimeRequest updateSRPEngWorkTime)
        {
            var response = await Sender.Send(new UpdateSRPEngWorkTimeCommand { SRPEngWorkTimeRequest = updateSRPEngWorkTime });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("WTdelete/{SRPEngWorkTimeId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Service_Report)]
        public async Task<IActionResult> DeleteSRPEngWorkTimeAsync(Guid SRPEngWorkTimeId)
        {
            var response = await Sender.Send(new DeleteSRPEngWorkTimeCommand { SRPEngWorkTimeId = SRPEngWorkTimeId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("WTby-id/{SRPEngWorkTimeId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetSRPEngWorkTimeByIdAsync(Guid SRPEngWorkTimeId)
        {
            var response = await Sender.Send(new GetSRPEngWorkTimeQuery { SRPEngWorkTimeId = SRPEngWorkTimeId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("WTall/{ServiceReportId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Report)]
        public async Task<IActionResult> GetSRPEngWorkTimeBySRPIdAsync(Guid ServiceReportId)
        {
            var response = await Sender.Send(new GetSRPEngWorkTimeBySRPIdQuery { ServiceReportId = ServiceReportId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        //PastServiceReports
        [HttpPost("PSRadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Past_Service_Report)]
        public async Task<IActionResult> CreatePastServiceReportAsync([FromBody] PastServiceReportRequest createPastServiceReport)
        {
            var response = await Sender.Send(new CreatePastServiceReportCommand { PastServiceReportRequest = createPastServiceReport });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("PSRupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Past_Service_Report)]
        public async Task<IActionResult> UpdatePastServiceReportAsync([FromBody] PastServiceReportRequest updatePastServiceReport)
        {
            var response = await Sender.Send(new UpdatePastServiceReportCommand { PastServiceReportRequest = updatePastServiceReport });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("PSRdelete/{PastServiceReportId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Past_Service_Report)]
        public async Task<IActionResult> DeletePastServiceReportAsync(Guid PastServiceReportId)
        {
            var response = await Sender.Send(new DeletePastServiceReportCommand { PastServiceReportId = PastServiceReportId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("PSRby-id/{PastServiceReportId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Past_Service_Report)]
        public async Task<IActionResult> GetPastServiceReportByIdAsync(Guid PastServiceReportId)
        {
            var response = await Sender.Send(new GetPastServiceReportQuery { PastServiceReportId = PastServiceReportId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("PSRall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Past_Service_Report)]
        public async Task<IActionResult> GetPastServiceReportsAsync()
        {
            var response = await Sender.Send(new GetPastServiceReportsQuery() );
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
