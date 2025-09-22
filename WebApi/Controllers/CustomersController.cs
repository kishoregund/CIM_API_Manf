using Application.Features.Customers;
using Application.Features.Customers.Commands;
using Application.Features.Customers.Queries;
using Application.Features.Customers.Requests;
using Application.Features.Instruments.Commands;
using Application.Features.Instruments.Queries;
using Application.Features.Instruments.Requests;
using Application.Features.ServiceRequests.Commands;
using Application.Features.ServiceRequests.Queries;
using Application.Features.ServiceRequests.Requests;
using Application.Features.Spares.Queries;
using Application.Models;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Customer)]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerRequest createCustomer)
        {
            var response = await Sender.Send(new CreateCustomerCommand { CustomerRequest = createCustomer });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Customer)]
        public async Task<IActionResult> UpdateCustomerAsync([FromBody] CustomerRequest updateCustomer)
        {
            var response = await Sender.Send(new UpdateCustomerCommand { CustomerRequest = updateCustomer });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{CustomerId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Customer)]
        public async Task<IActionResult> DeleteCustomerAsync(Guid CustomerId)
        {
            var response = await Sender.Send(new DeleteCustomerCommand { CustomerId = CustomerId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("by-id/{CustomerId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetCustomerByIdAsync(Guid CustomerId)
        {
            var response = await Sender.Send(new GetCustomerByIdQuery { CustomerId = CustomerId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("all")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var response = await Sender.Send(new GetCustomersQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("allbyuserid/{userId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer)]
        public async Task<IActionResult> GetCustomersByUserIdAsync(Guid userId)
        {
            var response = await Sender.Send(new GetCustomersByUserIdQuery { userId = userId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("distregionbycustid/{customerId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer)]
        public async Task<IActionResult> GetDistRegionsByCustomerAsync(Guid customerId)
        {
            var response = await Sender.Send(new GetDistRegionsByCustomerQuery { CustomerId = customerId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        // Site
        [HttpPost("Sadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Customer)]
        public async Task<IActionResult> CreateSiteAsync([FromBody] SiteRequest createSite)
        {
            var response = await Sender.Send(new CreateSiteCommand { SiteRequest = createSite });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("Supdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Customer)]
        public async Task<IActionResult> UpdateSiteAsync([FromBody] SiteRequest updateSite)
        {
            var response = await Sender.Send(new UpdateSiteCommand { SiteRequest = updateSite });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("Sdelete/{SiteId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Customer)]
        public async Task<IActionResult> DeleteSiteAsync(Guid SiteId)
        {
            var response = await Sender.Send(new DeleteSiteCommand { SiteId = SiteId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("Sby-id/{SiteId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer)]
        public async Task<IActionResult> GetSiteByIdAsync(Guid SiteId)
        {
            var response = await Sender.Send(new GetSiteByIdQuery { SiteId = SiteId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("Sall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer)]
        public async Task<IActionResult> GetSitesAsync(Guid CustomerId)
        {
            var response = await Sender.Send(new GetSitesQuery { CustomerId = CustomerId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("Sallbycontact/{contactId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetSitesByContactAsync(Guid contactId)
        {
            var response = await Sender.Send(new GetSitesByContactQuery { ContactId = contactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        // Sitecontact
        [HttpPost("SCadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Customer)]
        public async Task<IActionResult> CreateSiteContactAsync([FromBody] SiteContactRequest createSiteContact)
        {
            var response = await Sender.Send(new CreateSiteContactCommand { SiteContactRequest = createSiteContact });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("SCupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Customer)]
        public async Task<IActionResult> UpdateSiteContactAsync([FromBody] SiteContactRequest updateSiteContact)
        {
            var response = await Sender.Send(new UpdateSiteContactCommand { SiteContactRequest = updateSiteContact });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("SCdelete/{SiteContactId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Customer)]
        public async Task<IActionResult> DeleteSiteContactAsync(Guid SiteContactId)
        {
            var response = await Sender.Send(new DeleteSiteContactCommand { SiteContactId = SiteContactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("SCby-id/{SiteContactId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer)]
        public async Task<IActionResult> GetSiteContactByIdAsync(Guid SiteContactId)
        {
            var response = await Sender.Send(new GetSiteContactByIdQuery { SiteContactId = SiteContactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SCall/{SiteId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer)]
        public async Task<IActionResult> GetSiteContactsAsync(Guid SiteId)
        {
            var response = await Sender.Send(new GetSiteContactsQuery { SiteId = SiteId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SCbycustomer/{customerId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetCustomerContactsAsync(Guid customerId)
        {
            var response = await Sender.Send(new GetSiteContactsByCustomerQuery { CustomerId = customerId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpGet("SCbyuserid")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer)]
        public async Task<IActionResult> GetSiteContactsByUserIdAsync()
        {
            var response = await Sender.Send(new GetSiteContactsByUserIdQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // Spares inventory

        [HttpPost("SPIadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Customer_Spareparts_Inventory)]
        public async Task<IActionResult> CreateCustSPInventoryAsync([FromBody] CustSPInventoryRequest createCustSPInventory)
        {
            var response = await Sender.Send(new CreateCustSPInventoryCommand { CustSPInventoryRequest = createCustSPInventory });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("SPIupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Customer_Spareparts_Inventory)]
        public async Task<IActionResult> UpdateCustSPInventoryAsync([FromBody] CustSPInventoryRequest updateCustSPInventory)
        {
            var response = await Sender.Send(new UpdateCustSPInventoryCommand { CustSPInventoryRequest = updateCustSPInventory });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("SPIdelete/{CustSPInventoryId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Customer_Spareparts_Inventory)]
        public async Task<IActionResult> DeleteCustSPInventoryAsync(Guid CustSPInventoryId)
        {
            var response = await Sender.Send(new DeleteCustSPInventoryCommand { CustSPInventoryId = CustSPInventoryId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("SPIby-id/{CustSPInventoryId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Spareparts_Inventory)]
        public async Task<IActionResult> GetCustSPInventoryByIdAsync(Guid CustSPInventoryId)
        {
            var response = await Sender.Send(new GetCustSPInventoryByIdQuery { CustSPInventoryId = CustSPInventoryId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SPIall/{contactId}/{customerId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Spareparts_Inventory)]
        public async Task<IActionResult> GetCustSPInventorysAsync(Guid contactId, Guid CustomerId)
        {
            var response = await Sender.Send(new GetCustSPInventoryQuery { ContactId = contactId, CustomerId = CustomerId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SPIbypartno/{partNo}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Spareparts_Inventory)]
        public async Task<IActionResult> GetCustSPInventorysAsync(string partNo)
        {
            var response = await Sender.Send(new GetSparepartByPartNoQuery { partNo = partNo });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("SPIconsumedhistory/{custSPInventoryId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Spareparts_Inventory)]
        public async Task<IActionResult> GetSparepartConsumedHistoryAsync(Guid custSPInventoryId)
        {
            var response = await Sender.Send(new GetSparepartConsumedHistoryQuery { CustSPInventoryId = custSPInventoryId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut("SPIqtyupdate/{id}/{qty}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetCustSPInventorysAsync(Guid id, int qty)
        {
            var response = await Sender.Send(new UpdateCustSPInventoryQtyCommand { Id = id, Qty = qty });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        ////// Customer Instrument

        [HttpPost("CInsadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Customer_Instrument)]
        public async Task<IActionResult> CreateCustomerInstrumentAsync([FromBody] CustomerInstrumentRequest createInstrument)
        {
            var response = await Sender.Send(new CreateCustomerInstrumentCommand { CustomerInstrumentRequest = createInstrument });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("CInsupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Customer_Instrument)]
        public async Task<IActionResult> UpdateCustomerInstrumentAsync([FromBody] CustomerInstrumentRequest updateInstrument)
        {
            var response = await Sender.Send(new UpdateCustInstrumentCommand { CustInstrumentRequest = updateInstrument });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("CInsdelete/{CustInstrumentId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Customer_Instrument)]
        public async Task<IActionResult> DeleteCustomerInstrumentAsync(Guid CustInstrumentId)
        {
            var response = await Sender.Send(new DeleteCustInstrumentCommand { CustInstrumentId = CustInstrumentId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("CInsby-id/{CustInstrumentId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Instrument)]
        public async Task<IActionResult> GetCustomerInstrumentByIdAsync(Guid CustInstrumentId)
        {
            var response = await Sender.Send(new GetCustInstrumentByIdQuery { CustInstrumentId = CustInstrumentId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost("CInsall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetCustInstrumentsAsync([FromBody] BUBrand buBrand)
        {
            var response = await Sender.Send(new GetCustInstrumentQuery { BUBrand = buBrand });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("insbysite/{siteId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetCustomerInstrumentBySiteAsync(Guid siteId)
        {
            var response = await Sender.Send(new GetCustInstrumentBySiteQuery { SiteId = siteId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpGet("CIinsbyins/{instrumentId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetCustomerInstrumentByinstrumentAsync(Guid instrumentId)
        {
            var response = await Sender.Send(new GetCustInstrumentByInstrumentQuery { InstrumentId = instrumentId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("CInsspares/{InstrumentId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Instrument)]
        public async Task<IActionResult> GetInstrumentSparesAsync(Guid InstrumentId)
        {
            var response = await Sender.Send(new GetInstrumentSparesQuery { InstrumentId = InstrumentId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        // Customer Survey
        [HttpPost("CSSadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Customer_Satisfaction_Survey)]
        public async Task<IActionResult> CreateCustomerSurveyAsync([FromBody] CustomerSurveyRequest createCustomer)
        {
            var response = await Sender.Send(new CreateCustomerSurveyCommand { CustomerSurveyRequest = createCustomer });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("CSSupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Customer_Satisfaction_Survey)]
        public async Task<IActionResult> UpdateCustomerAsync([FromBody] CustomerSurveyRequest updateCustomer)
        {
            var response = await Sender.Send(new UpdateCustomerSurveyCommand { CustomerSurveyRequest = updateCustomer });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("CSSall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetCustomerSurveysAsync()
        {
            var response = await Sender.Send(new GetCustomerSurveysQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete("CSSdelete/{surveyId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Customer_Satisfaction_Survey)]
        public async Task<IActionResult> DeleteCustomerSUrveyAsync(Guid surveyId)
        {
            var response = await Sender.Send(new DeleteCustomerSurveyCommand { SurveyId = surveyId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("CSSby-id/{surveyId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Satisfaction_Survey)]
        public async Task<IActionResult> GetCustomerSurveyByIdAsync(Guid surveyId)
        {
            var response = await Sender.Send(new GetCustomerSurveyByIdQuery { SurveyId = surveyId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("CSSsrbydist/{distributorId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Satisfaction_Survey)]
        public async Task<IActionResult> GetServiceRequestByDistAsync(Guid distributorId)
        {
            var response = await Sender.Send(new GetServiceRequestByDistributorQuery { DistributorId = distributorId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("CSSsrbysrp/{serviceReportId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Customer_Satisfaction_Survey)]
        public async Task<IActionResult> GetServiceRequestBySRPAsync(Guid serviceReportId)
        {
            var response = await Sender.Send(new GetServiceRequestBySRPQuery { ServiceReportId = serviceReportId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
