using Authentication.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Authentication.DAL.Data;
using Authentication.API.Controllers;

namespace Authentication.UnitTests.Common.Factory;

public class AuthenticationConfirmControllerFactory : ControllerFactory
{
	public AuthenticationConfirmControllerFactory() : base() { }
	public override ControllerBase Create(ApplicationDbContext context)
	{
		AuthenticationService authService = new AuthenticationService(context,
		jwtService,
		basePublisher);

		AuthenticationConfirmService confirmService = 
			new AuthenticationConfirmService(context,
			 authService,
			 jwtService,
			 basePublisher);

		return new AuthenticationConfirmController(confirmService);
	}
}
