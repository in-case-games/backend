using Microsoft.AspNetCore.Mvc;
using Payment.API.Common;
using Payment.API.Filters;
using Payment.BLL.Interfaces;
using Payment.BLL.Models.External.YooKassa;
using Payment.BLL.Models.Internal;
using Payment.BLL.Services;
using System.Net;
using System.Security.Claims;

namespace Payment.API.Controllers;
[Route("api/payment")]
[ApiController]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);

    [ProducesResponseType(typeof(ApiResult<UserPaymentResponse>), (int)HttpStatusCode.OK)]
    [AuthorizeByRole(Roles.All)]
    [HttpPost]
    public async Task<IActionResult> ProcessingInvoiceNotificationAsync(InvoiceNotificationResponse response, CancellationToken cancellationToken)
    {
        response.Object!.UserId = UserId;
        var userResponse = await paymentService.ProcessingInvoiceNotificationAsync(response, cancellationToken);

        return Ok(ApiResult<UserPaymentResponse>.Ok(userResponse));
    }
}
