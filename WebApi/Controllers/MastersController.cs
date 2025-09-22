using Application.Features.Masters.Commands;
using Application.Features.Masters.Queries;
using Application.Features.Masters.Requests;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class MastersController : BaseApiController
    {
        [HttpPost("CTVadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.ListTypeItems)]
        public async Task<IActionResult> CreateConfigTypeValuesAsync([FromBody] ConfigTypeValuesRequest createConfigTypeValues)
        {
            var response = await Sender.Send(new CreateConfigTypeValuesCommand { ConfigTypeValuesRequest = createConfigTypeValues });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("CTVupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.ListTypeItems)]
        public async Task<IActionResult> UpdateConfigTypeValuesAsync([FromBody] UpdateConfigTypeValuesRequest updateConfigTypeValues)
        {
            var response = await Sender.Send(new UpdateConfigTypeValuesCommand { ConfigTypeValuesRequest = updateConfigTypeValues });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("CTVdelete/{ConfigTypeValuesId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.ListTypeItems)]
        public async Task<IActionResult> DeleteConfigTypeValuesAsync(Guid ConfigTypeValuesId)
        {
            var response = await Sender.Send(new DeleteConfigTypeValuesCommand { ConfigTypeValuesId = ConfigTypeValuesId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("CTVby-id/{ConfigTypeValuesId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetConfigTypeValuesByIdAsync(Guid ConfigTypeValuesId)
        {
            var response = await Sender.Send(new GetConfigTypeValuesByIdQuery { ConfigTypeValuesId = ConfigTypeValuesId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
              
        [HttpGet("CTVall/{listTypeItemId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetConfigTypeValuesByTypeIdAsync(Guid ListTypeItemId)
        {
            var response = await Sender.Send(new GetConfigTypeValuesByTypeIdQuery { LisTypeItemId = ListTypeItemId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        //Country

        [HttpPost("COadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Country)]
        public async Task<IActionResult> CreateCountryAsync([FromBody] CountryRequest createCountry)
        {
            var response = await Sender.Send(new CreateCountryCommand { CountryRequest = createCountry });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("COupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Country)]
        public async Task<IActionResult> UpdateCountryAsync([FromBody] CountryRequest updateCountry)
        {
            var response = await Sender.Send(new UpdateCountryCommand { CountryRequest = updateCountry });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("COdelete/{CountryId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Country)]
        public async Task<IActionResult> DeleteCountryAsync(Guid CountryId)
        {
            var response = await Sender.Send(new DeleteCountryCommand { CountryId = CountryId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("COby-id/{CountryId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Country)]
        public async Task<IActionResult> GetCountryByIdAsync(Guid CountryId)
        {
            var response = await Sender.Send(new GetCountryByIdQuery { CountryId = CountryId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("COall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetCountriesAsync()
        {
            var response = await Sender.Send(new GetCountriesQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        //Currency

        [HttpPost("CUadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.Currency)]
        public async Task<IActionResult> CreateCurrencyAsync([FromBody] CurrencyRequest createCurrency)
        {
            var response = await Sender.Send(new CreateCurrencyCommand { CurrencyRequest = createCurrency });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("CUupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.Currency)]
        public async Task<IActionResult> UpdateCurrencyAsync([FromBody] CurrencyRequest updateCurrency)
        {
            var response = await Sender.Send(new UpdateCurrencyCommand { CurrencyRequest = updateCurrency });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("CUdelete/{CurrencyId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.Currency)]
        public async Task<IActionResult> DeleteCurrencyAsync(Guid CurrencyId)
        {
            var response = await Sender.Send(new DeleteCurrencyCommand { CurrencyId = CurrencyId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("CUby-id/{CurrencyId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Currency)]
        public async Task<IActionResult> GetCurrencyByIdAsync(Guid CurrencyId)
        {
            var response = await Sender.Send(new GetCurrencyByIdQuery { CurrencyId = CurrencyId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("CUall")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetCurrenciesAsync()
        {
            var response = await Sender.Send(new GetCurrenciesQuery ());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        //ListTypeItems
        [HttpPost("LTIadd")]
        [ShouldHavePermission(CimAction.Create, CimFeature.ListTypeItems)]
        public async Task<IActionResult> CreateListTypeItemsAsync([FromBody] ListTypeItemsRequest createListTypeItems)
        {
            var response = await Sender.Send(new CreateListTypeItemsCommand { ListTypeItemsRequest = createListTypeItems });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("LTIupdate")]
        [ShouldHavePermission(CimAction.Update, CimFeature.ListTypeItems)]
        public async Task<IActionResult> UpdateListTypeItemsAsync([FromBody] UpdateListTypeItemsRequest updateListTypeItems)
        {
            var response = await Sender.Send(new UpdateListTypeItemsCommand { ListTypeItemsRequest = updateListTypeItems });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("LTIdelete/{ListTypeItemsId}")]
        [ShouldHavePermission(CimAction.Delete, CimFeature.ListTypeItems)]
        public async Task<IActionResult> DeleteListTypeItemsAsync(Guid ListTypeItemsId)
        {
            var response = await Sender.Send(new DeleteListTypeItemsCommand { ListTypeItemsId = ListTypeItemsId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("LTIby-id/{ListTypeItemsId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)]
        public async Task<IActionResult> GetListTypeItemsByIdAsync(Guid ListTypeItemsId)
        {
            var response = await Sender.Send(new GetListTypeItemsByIdQuery { ListTypeItemsId = ListTypeItemsId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("LTIall")]
        [ShouldHavePermission(CimAction.View, CimFeature.ListTypeItems)]
        public async Task<IActionResult> GetListTypeItemsByListIdAsync(Guid ListTypeId)
        {
            var response = await Sender.Send(new GetListTypeItemsByListIdQuery { LisTypeId = ListTypeId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("LTIcode/{code}")]
        [ShouldHavePermission(CimAction.View, CimFeature.Base)] // permission for role are considered the method is needed in Role permissions
        public async Task<IActionResult> GetListTypeItemsByCodeAsync(string code)
        {
            var response = await Sender.Send(new GetListTypeItemsByListCodeQuery { ListCode = code });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("LTIlistid/{ListId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.ListTypeItems)]
        public async Task<IActionResult> GetVWListTypeItemsByListIdAsync(Guid ListId)
        {
            var response = await Sender.Send(new GetVWListTypeItemsByListIdQuery { ListId = ListId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("LTYall")]
        [ShouldHavePermission(CimAction.View, CimFeature.ListTypeItems)]
        public async Task<IActionResult> GetListTypesAsync()
        {
            var response = await Sender.Send(new GetListTypesQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("LTYby-id/{ListTypeId}")]
        [ShouldHavePermission(CimAction.View, CimFeature.ListTypeItems)]
        public async Task<IActionResult> GetListTypeByIdAsync(Guid ListTypeId)
        {
            var response = await Sender.Send(new GetListTypeByIdQuery { ListTypeId = ListTypeId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("LTIallscreens")]  // permission for role are considered the method is needed in Role permissions
        [ShouldHavePermission(CimAction.View, CimFeature.Role)]
        public async Task<IActionResult> GetScreensForRolePermissionsAsync()
        {
            var response = await Sender.Send(new GetScreensForRolePermissionsQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


    }
}
