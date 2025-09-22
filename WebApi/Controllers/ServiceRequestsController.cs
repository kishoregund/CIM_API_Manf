using Application.Features.ServiceRequests.Commands;
using Application.Features.ServiceRequests.Queries;
using Application.Features.ServiceRequests.Requests;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;
using Application.Features.SRAssignedHistorys.Commands;
using Application.Features.SRAssignedHistorys.Queries;
using Application.Features.Customers.Queries;
using Application.Models;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ServiceRequestsController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Service_Request)]
        public async Task<IActionResult> CreateServiceRequestAsync([FromBody] ServiceRequestRequest createServiceRequest)
        {
            var response = await Sender.Send(new CreateServiceRequestCommand { ServiceRequestRequest = createServiceRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Request)]
        public async Task<IActionResult> UpdateServiceRequestAsync([FromBody] ServiceRequestRequest updateServiceRequest)
        {
            var response = await Sender.Send(new UpdateServiceRequestCommand { ServiceRequestRequest = updateServiceRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{ServiceRequestId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Service_Request)]
        public async Task<IActionResult> DeleteServiceRequestAsync(Guid ServiceRequestId)
        {
            var response = await Sender.Send(new DeleteServiceRequestCommand { ServiceRequestId = ServiceRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("by-id/{ServiceRequestId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetServiceRequestByIdAsync(Guid ServiceRequestId)
        {
            var response = await Sender.Send(new GetServiceRequestQuery { ServiceRequestId = ServiceRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("all")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetServiceRequestsAsync()
        {
            var response = await Sender.Send(new GetServiceRequestsQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("siteusers/{Id}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetSiteUsersAsync(Guid Id)
        {
            var response = await Sender.Send(new GetSiteUsersQuery { SiteId = Id});
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        

        [HttpPost("allbybubrand")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetServiceRequestsByBUBrandAsync([FromBody] BUBrand buBrand)
        {
            var response = await Sender.Send(new GetDetailServiceRequestsQuery { BUBrand = buBrand });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SRInsbyinstr/{instrumentId}/{siteId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetInstrumentByInstrumentIdAsync(Guid instrumentId, Guid siteId)
        {
            var response = await Sender.Send(new GetInstrumentDetailByInstrumentQuery { InstrumentId = instrumentId, SiteId = siteId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SRno")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetServiceRequestNoAsync()
        {
            var response = await Sender.Send(new GetServiceRequestNoQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        /// EngScheduler
        [HttpPost("ESadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Scheduler)]
        public async Task<IActionResult> CreateEngSchedulerAsync([FromBody] EngSchedulerRequest createEngScheduler)
        {
            var response = await Sender.Send(new CreateEngSchedulerCommand { EngSchedulerRequest = createEngScheduler });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("ESupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Scheduler)]
        public async Task<IActionResult> UpdateEngSchedulerAsync([FromBody] EngSchedulerRequest updateEngScheduler)
        {
            var response = await Sender.Send(new UpdateEngSchedulerCommand { EngSchedulerRequest = updateEngScheduler });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpDelete("ESdelete/{EngSchedulerId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Scheduler)]
        public async Task<IActionResult> DeleteEngSchedulerAsync(Guid EngSchedulerId)
        {
            var response = await Sender.Send(new DeleteEngSchedulerCommand { EngSchedulerId = EngSchedulerId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("ESby-id/{EngSchedulerId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Scheduler)]
        public async Task<IActionResult> GetEngSchedulerByIdAsync(Guid EngSchedulerId)
        {
            var response = await Sender.Send(new GetEngSchedulerQuery { EngSchedulerId = EngSchedulerId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpGet("ESbyengid/{engineerId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Scheduler)]
        public async Task<IActionResult> GetEngSchedulerByEngineerAsync(Guid engineerId)
        {
            var response = await Sender.Send(new GetEngSchedulerByEngineerQuery { EngineerId = engineerId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpGet("ESall/{ServiceRequestId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Scheduler)]
        public async Task<IActionResult> GetEngSchedulersAsync(Guid ServiceRequestId)
        {
            var response = await Sender.Send(new GetEngSchedulerBySRIdQuery { ServiceRequestId = ServiceRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // SRAssigned Histotory
        [HttpPost("AHadd")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Request)]
        public async Task<IActionResult> CreateSRAssignedHistoryAsync([FromBody] SRAssignedHistoryRequest createSRAssignedHistory)
        {
            var response = await Sender.Send(new CreateSRAssignedHistoryCommand { SRAssignedHistoryRequest = createSRAssignedHistory });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("AHupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Request)]
        public async Task<IActionResult> UpdateSRAssignedHistoryAsync([FromBody] SRAssignedHistoryRequest updateSRAssignedHistory)
        {
            var response = await Sender.Send(new UpdateSRAssignedHistoryCommand { SRAssignedHistoryRequest = updateSRAssignedHistory });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpDelete("AHdelete/{SRAssignedHistoryId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Service_Request)]
        public async Task<IActionResult> DeleteSRAssignedHistoryAsync(Guid SRAssignedHistoryId)
        {
            var response = await Sender.Send(new DeleteSRAssignedHistoryCommand { SRAssignedHistoryId = SRAssignedHistoryId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("AHby-id/{SRAssignedHistoryId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetSRAssignedHistoryByIdAsync(Guid SRAssignedHistoryId)
        {
            var response = await Sender.Send(new GetSRAssignedHistoryQuery { SRAssignedHistoryId = SRAssignedHistoryId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("AHall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetSRAssignedHistorysAsync(Guid ServiceRequestId)
        {
            var response = await Sender.Send(new GetSRAssignedHistoryBySRIdQuery { ServiceRequestId = ServiceRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // SR Audit Trail

        [HttpPost("ATadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Service_Request)]
        public async Task<IActionResult> CreateSRAuditTrailAsync([FromBody] SRAuditTrailRequest createSRAuditTrail)
        {
            var response = await Sender.Send(new CreateSRAuditTrailCommand { SRAuditTrailRequest = createSRAuditTrail });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("ATupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Request)]
        public async Task<IActionResult> UpdateSRAuditTrailAsync([FromBody] SRAuditTrailRequest updateSRAuditTrail)
        {
            var response = await Sender.Send(new UpdateSRAuditTrailCommand { SRAuditTrailRequest = updateSRAuditTrail });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("ATdelete/{SRAuditTrailId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Service_Request)]
        public async Task<IActionResult> DeleteSRAuditTrailAsync(Guid SRAuditTrailId)
        {
            var response = await Sender.Send(new DeleteSRAuditTrailCommand { SRAuditTrailId = SRAuditTrailId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("ATby-id/{SRAuditTrailId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetSRAuditTrailByIdAsync(Guid SRAuditTrailId)
        {
            var response = await Sender.Send(new GetSRAuditTrailQuery { SRAuditTrailId = SRAuditTrailId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("ATall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetSRAuditTrailsAsync(Guid ServiceRequestId)
        {
            var response = await Sender.Send(new GetSRAuditTrailBySRIdQuery { ServiceRequestId = ServiceRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // SR Action
        [HttpPost("EAadd")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Request)]
        public async Task<IActionResult> CreateSREngActionAsync([FromBody] SREngActionRequest createSREngAction)
        {
            var response = await Sender.Send(new CreateSREngActionCommand { SREngActionRequest = createSREngAction });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("EAupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Request)]
        public async Task<IActionResult> UpdateSREngActionAsync([FromBody] SREngActionRequest updateSREngAction)
        {
            var response = await Sender.Send(new UpdateSREngActionCommand { SREngActionRequest = updateSREngAction });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("EAdelete/{SREngActionId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Service_Request)]
        public async Task<IActionResult> DeleteSREngActionAsync(Guid SREngActionId)
        {
            var response = await Sender.Send(new DeleteSREngActionCommand { SREngActionId = SREngActionId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("EAby-id/{SREngActionId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetSREngActionByIdAsync(Guid SREngActionId)
        {
            var response = await Sender.Send(new GetSREngActionQuery { SREngActionId = SREngActionId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("EAall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetSREngActionsAsync(Guid ServiceRequestId)
        {
            var response = await Sender.Send(new GetSREngActionBySRIdQuery { ServiceRequestId = ServiceRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        //SREngComments
        [HttpPost("ECadd")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Request)]
        public async Task<IActionResult> CreateSREngCommentsAsync([FromBody] SREngCommentsRequest createSREngComments)
        {
            var response = await Sender.Send(new CreateSREngCommentsCommand { SREngCommentsRequest = createSREngComments });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("ECupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Service_Request)]
        public async Task<IActionResult> UpdateSREngCommentsAsync([FromBody] SREngCommentsRequest updateSREngComments)
        {
            var response = await Sender.Send(new UpdateSREngCommentsCommand { SREngCommentsRequest = updateSREngComments });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("ECdelete/{SREngCommentsId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Service_Request)]
        public async Task<IActionResult> DeleteSREngCommentsAsync(Guid SREngCommentsId)
        {
            var response = await Sender.Send(new DeleteSREngCommentsCommand { SREngCommentId = SREngCommentsId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("ECby-id/{SREngCommentsId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetSREngCommentsByIdAsync(Guid SREngCommentsId)
        {
            var response = await Sender.Send(new GetSREngCommentsQuery { SREngCommentsId = SREngCommentsId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("ECall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Service_Request)]
        public async Task<IActionResult> GetSREngCommentsAsync(Guid ServiceRequestId)
        {
            var response = await Sender.Send(new GetSREngCommentsBySRIdQuery { ServiceRequestId = ServiceRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
