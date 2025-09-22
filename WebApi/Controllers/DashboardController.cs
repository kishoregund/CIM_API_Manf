using Application.Features.AppBasic.Commands;
using Application.Features.AppBasic.Queries;
using Application.Features.AppBasic.Requests;
using Application.Features.Dashboards.Queries;
using Application.Features.Dashboards.Requests;
using Infrastructure.Identity;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class DashboardController : BaseApiController
    {

        [HttpPost("DistDashData")]
        [ShouldHavePermission(CimAction.View, CimFeature.Distributor_Dashboard)]
        public async Task<IActionResult> GetDistDashboardDataAsync([FromBody] DashboardDateRequest dashboardDateRequest)
        {            
            var response = await Sender.Send(new GetDistDashboardDataQuery { DashboardDateRequest = dashboardDateRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("DistInsInstall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Distributor_Dashboard)]
        public async Task<IActionResult> GetInstrumentInstalledAsync([FromBody] DashboardDateRequest dashboardDateRequest)
        {
            var response = await Sender.Send(new GetInstrumentInstalledQuery { DashboardDateRequest = dashboardDateRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("DistCustRev")]
        [ShouldHavePermission(CimAction.View, CimFeature.Distributor_Dashboard)]
        public async Task<IActionResult> GetRevenueFromCustomerAsync([FromBody] DashboardDateRequest dashboardDateRequest)
        {
            var response = await Sender.Send(new GetRevenueFromCustomerQuery { DashboardDateRequest = dashboardDateRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("DistSerConRev")]
        [ShouldHavePermission(CimAction.View, CimFeature.Distributor_Dashboard)]
        public async Task<IActionResult> GetServiceContractRevenueAsync([FromBody] DashboardDateRequest dashboardDateRequest)
        {
            var response = await Sender.Send(new GetServiceContractRevenueQuery { DashboardDateRequest = dashboardDateRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        /// Customer Dashboard
        [HttpGet("ownershipcost/{id}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Dashboard)]
        public async Task<IActionResult> GetCostOfOwnerShipAsync(string id)
        {
            var response = await Sender.Send(new GetCostOfOwnershipQuery { Id = id});
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("costdata")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Dashboard)]
        public async Task<IActionResult> GetCostDataAsync([FromBody] DashboardDateRequest dashboardDateRequest)
        {
            var response = await Sender.Send(new GetCostDataQuery { DashboardDateRequest = dashboardDateRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("allserreq")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Dashboard)]
        public async Task<IActionResult> GetAllServiceRequestAsync()
        {
            var response = await Sender.Send(new GetAllServiceRequestQuery {  });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("allamc")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Dashboard)]
        public async Task<IActionResult> GetAllAmcAsync()
        {
            var response = await Sender.Send(new GetAllAmcQuery { });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("sparerecommended")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Dashboard)]
        public async Task<IActionResult> GetSparePartsRecommendedAsync()
        {
            var response = await Sender.Send(new GetSparePartsRecommendedQuery { });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("customerdetails")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Dashboard)]
        public async Task<IActionResult> GetCustomerDetailsAsync()
        {
            var response = await Sender.Send(new GetCustomerDetailsQuery { });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("alloffreq")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Dashboard)]
        public async Task<IActionResult> GetAllOfferrequestAsync()
        {
            var response = await Sender.Send(new GetAllOfferrequestQuery { });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("siteinstr/{id}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Dashboard)]
        public async Task<IActionResult> GetSiteInstrumentAsync(string id)
        {
            var response = await Sender.Send(new GetSiteInstrumentQuery { Id = id });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("serreqinstr/{id}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Dashboard)]
        public async Task<IActionResult> GetSerReqInstrumentAsync(string id)
        {
            var response = await Sender.Send(new GetSerReqInstrumentQuery { Id = id });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// Engineer Dashboard
        [HttpGet("engserreq/{date}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Engineer_Dashboard)]
        public async Task<IActionResult> GetServiceRequestAsync(string date)
        {
            var response = await Sender.Send(new GetEngServiceRequestQuery { Date = date });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("sparesrecom/{date}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Engineer_Dashboard)]
        public async Task<IActionResult> GetSparesRecommendedAsync(string date)
        {
            var response = await Sender.Send(new GetEngSparesRecommendedQuery { Date = date });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("sparescon/{date}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Engineer_Dashboard)]
        public async Task<IActionResult> GetSparesConsumedAsync(string date)
        {
            var response = await Sender.Send(new GetEngSparesConsumedQuery { Date = date });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }


        [HttpGet("engtravelexp/{date}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Engineer_Dashboard)]
        public async Task<IActionResult> GetTravelExpensesAsync(string date)
        {
            var response = await Sender.Send(new GetEngTravelExpensesQuery { Date = date });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
