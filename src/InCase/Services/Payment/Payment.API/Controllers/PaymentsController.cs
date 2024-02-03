using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Payment.API.Common;
using Payment.API.Filters;
using Payment.BLL.Interfaces;
using Payment.BLL.Models.Internal;
using Payment.DAL.Entities;

namespace Payment.API.Controllers;

[Route("api/payments")]
[ApiController]
public class PaymentsController(IPaymentService paymentService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [ProducesResponseType(typeof(ApiResult<InvoiceUrlResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpPost("invoice")]
    public async Task<IActionResult> CreateInvoice(InvoiceUrlRequest request, CancellationToken cancellationToken = default)
    {
        request.User = new User { Id = UserId };

        var response = await paymentService.CreateInvoiceUrlAsync(request, cancellationToken);

        return Ok(ApiResult<InvoiceUrlResponse>.Ok(response));
    }
}