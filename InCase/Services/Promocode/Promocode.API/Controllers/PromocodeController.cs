using Microsoft.AspNetCore.Mvc;
using Promocode.API.Common;
using Promocode.API.Filters;
using Promocode.BLL.Interfaces;
using Promocode.BLL.Models;
using System.Net;

namespace Promocode.API.Controllers
{
    [Route("api/promocode")]
    [ApiController]
    public class PromocodeController : ControllerBase
    {
        private readonly IPromocodeService _promocodeService;

        public PromocodeController(IPromocodeService promocodeService)
        {
            _promocodeService = promocodeService;
        }

        [ProducesResponseType(typeof(ApiResult<List<PromocodeResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<PromocodeResponse> response = await _promocodeService.GetAsync();

            return Ok(ApiResult<List<PromocodeResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<PromocodeResponse>>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpGet("empty")]
        public async Task<IActionResult> GetEmpty()
        {
            List<PromocodeResponse> response = await _promocodeService.GetEmptyPromocodesAsync();

            return Ok(ApiResult<List<PromocodeResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<PromocodeResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            PromocodeResponse response = await _promocodeService.GetAsync(name);

            return Ok(ApiResult<PromocodeResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<List<PromocodeTypeResponse>>), 
            (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.All)]
        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            List<PromocodeTypeResponse> response = await _promocodeService.GetTypesAsync();

            return Ok(ApiResult<List<PromocodeTypeResponse>>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<PromocodeResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpPost]
        public async Task<IActionResult> Post(PromocodeRequest request)
        {
            PromocodeResponse response = await _promocodeService.CreateAsync(request);

            return Ok(ApiResult<PromocodeResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<PromocodeResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpPut]
        public async Task<IActionResult> Put(PromocodeRequest request)
        {
            PromocodeResponse response = await _promocodeService.UpdateAsync(request);

            return Ok(ApiResult<PromocodeResponse>.OK(response));
        }

        [ProducesResponseType(typeof(ApiResult<PromocodeResponse>), (int)HttpStatusCode.OK)]
        [AuthorizeByRole(Roles.AdminOwnerBot)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            PromocodeResponse response = await _promocodeService.DeleteAsync(id);

            return Ok(ApiResult<PromocodeResponse>.OK(response));
        }
    }
}
