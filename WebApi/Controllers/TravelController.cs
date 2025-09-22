using Application.Features.AppBasic.Commands;
using Application.Features.AppBasic.Queries;
using Application.Features.AppBasic.Requests;
using Application.Features.Travels.Commands;
using Application.Features.Travels.Queries;
using Application.Features.Travels.Requests;
using Application.Models;
using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TravelController : BaseApiController
    {
        ///  Business Unit
        [HttpPost("TEadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Travel_Expenses)]
        public async Task<IActionResult> CreateTravelExpenseAsync([FromBody] TravelExpenseRequest createTravelExpense)
        {            
            var response = await Sender.Send(new CreateTravelExpenseCommand { Request = createTravelExpense });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("TEupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Travel_Expenses)]
        public async Task<IActionResult> UpdateTravelExpenseAsync([FromBody] UpdateTravelExpenseRequest updateTravelExpense)
        {
            var response = await Sender.Send(new UpdateTravelExpenseCommand { Request = updateTravelExpense });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("TEdelete/{TravelExpenseId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Travel_Expenses)]
        public async Task<IActionResult> DeleteTravelExpenseAsync(Guid TravelExpenseId)
        {
            var response = await Sender.Send(new DeleteTravelExpenseByIdCommand { Id = TravelExpenseId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("TEby-id/{TravelExpenseId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Travel_Expenses)]
        public async Task<IActionResult> GetTravelExpenseByIdAsync(Guid TravelExpenseId)
        {
            var response = await Sender.Send(new GetTravelExpenseByIdQuery { TravelExpenseId = TravelExpenseId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
              
        [HttpPost("TEall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Travel_Expenses)]
        public async Task<IActionResult> GetTravelExpensesAsync([FromBody] BUBrand buBrand)
        {
            var response = await Sender.Send(new GetTravelExpensesQuery { BusinessUnitId = buBrand.BusinessUnitId, BrandId = buBrand.BrandId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        //TravelExpenseItems

        [HttpPost("TEIadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Travel_Expenses)]
        public async Task<IActionResult> CreateTravelExpenseItemsAsync([FromBody] TravelExpenseItemsRequest createTravelExpenseItems)
        {
            var response = await Sender.Send(new CreateTravelExpenseItemsCommand { Request = createTravelExpenseItems });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("TEIupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Travel_Expenses)]
        public async Task<IActionResult> UpdateTravelExpenseItemsAsync([FromBody] UpdateTravelExpenseItemsRequest updateTravelExpenseItems)
        {
            var response = await Sender.Send(new UpdateTravelExpenseItemsCommand { Request = updateTravelExpenseItems });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("TEIdelete/{TravelExpenseItemsId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Travel_Expenses)]
        public async Task<IActionResult> DeleteTravelExpenseItemsAsync(Guid TravelExpenseItemsId)
        {
            var response = await Sender.Send(new DeleteTravelExpenseItemsByIdCommand { Id = TravelExpenseItemsId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("TEIby-id/{TravelExpenseItemsId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Travel_Expenses)]
        public async Task<IActionResult> GetTravelExpenseItemsByIdAsync(Guid TravelExpenseItemsId)
        {
            var response = await Sender.Send(new GetTravelExpenseItemsByIdQuery { TravelExpenseItemsId = TravelExpenseItemsId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("TEIall/{TravelExpenseId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Travel_Expenses)] 
        public async Task<IActionResult> GetTravelExpenseItemsAsync(Guid TravelExpenseId)
        {
            var response = await Sender.Send(new GetTravelExpenseItemsQuery { TravelExpenseId = TravelExpenseId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        // TravelInvoice

        [HttpPost("TIadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Travel_Invoice)]
        public async Task<IActionResult> CreateTravelInvoiceAsync([FromBody] TravelInvoiceRequest createTravelInvoice)
        {
            var response = await Sender.Send(new CreateTravelInvoiceCommand { Request = createTravelInvoice });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("TIupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Travel_Invoice)]
        public async Task<IActionResult> UpdateTravelInvoiceAsync([FromBody] UpdateTravelInvoiceRequest updateTravelInvoice)
        {
            var response = await Sender.Send(new UpdateTravelInvoiceCommand { Request = updateTravelInvoice });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("TIdelete/{TravelInvoiceId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Travel_Invoice)]
        public async Task<IActionResult> DeleteTravelInvoiceAsync(Guid TravelInvoiceId)
        {
            var response = await Sender.Send(new DeleteTravelInvoiceByIdCommand { Id = TravelInvoiceId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("TIby-id/{TravelInvoiceId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Travel_Invoice)]
        public async Task<IActionResult> GetTravelInvoiceByIdAsync(Guid TravelInvoiceId)
        {
            var response = await Sender.Send(new GetTravelInvoiceByIdQuery { TravelInvoiceId = TravelInvoiceId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost("TIall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Travel_Invoice)]
        public async Task<IActionResult> GetTravelInvoicesAsync([FromBody] BUBrand buBrand)
        {
            var response = await Sender.Send(new GetTravelInvoicesQuery { BusinessUnitId = buBrand.BusinessUnitId, BrandId = buBrand.BrandId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // Advance Request 
        [HttpPost("ADVadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Advance_Request_Form)]
        public async Task<IActionResult> CreateAdvanceRequestAsync([FromBody] AdvanceRequest createAdvanceRequest)
        {
            var response = await Sender.Send(new CreateAdvanceRequestCommand { Request = createAdvanceRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("ADVupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Advance_Request_Form)]
        public async Task<IActionResult> UpdateAdvanceRequestAsync([FromBody] AdvanceRequest updateAdvanceRequest)
        {
            var response = await Sender.Send(new UpdateAdvanceRequestCommand { Request = updateAdvanceRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("ADVdelete/{AdvanceRequestId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Advance_Request_Form)]
        public async Task<IActionResult> DeleteAdvanceRequestAsync(Guid AdvanceRequestId)
        {
            var response = await Sender.Send(new DeleteAdvanceRequestByIdCommand { Id = AdvanceRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("ADVby-id/{AdvanceRequestId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Advance_Request_Form)]
        public async Task<IActionResult> GetAdvanceRequestByIdAsync(Guid AdvanceRequestId)
        {
            var response = await Sender.Send(new GetAdvanceRequestByIdQuery { AdvanceRequestId = AdvanceRequestId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPost("ADVall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Advance_Request_Form)]
        public async Task<IActionResult> GetAdvanceRequestsAsync([FromBody] BUBrand buBrand)
        {
            var response = await Sender.Send(new GetAdvanceRequestsQuery { BusinessUnitId = buBrand.BusinessUnitId, BrandId = buBrand.BrandId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        //// Bank Details
        [HttpPost("BDadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Advance_Request_Form)]
        public async Task<IActionResult> CreateBankDetailsAsync([FromBody] BankDetails createBankDetails)
        {
            var response = await Sender.Send(new CreateBankDetailsCommand { Request = createBankDetails });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("BDupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Advance_Request_Form)]
        public async Task<IActionResult> UpdateBankDetailsAsync([FromBody] BankDetails updateBankDetails)
        {
            var response = await Sender.Send(new UpdateBankDetailsCommand { Request = updateBankDetails });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("BDdelete/{BankDetailsId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Advance_Request_Form)]
        public async Task<IActionResult> DeleteBankDetailsAsync(Guid BankDetailsId)
        {
            var response = await Sender.Send(new DeleteBankDetailsByIdCommand { Id = BankDetailsId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("BDby-id/{id}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Advance_Request_Form)]
        public async Task<IActionResult> GetBankDetailsByIdAsync(Guid id)
        {
            var response = await Sender.Send(new GetBankDetailsByIdQuery { Id = id });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("BDbyconid/{contactId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Advance_Request_Form)]
        public async Task<IActionResult> GetBankDetailsByContactIdAsync(Guid contactId)
        {
            var response = await Sender.Send(new GetBankDetailsByContactIdQuery { ContactId = contactId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
