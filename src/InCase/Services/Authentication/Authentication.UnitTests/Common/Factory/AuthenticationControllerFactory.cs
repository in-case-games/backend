using Authentication.API.Controllers;
using Authentication.BLL.Services;
using Authentication.DAL.Data;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.UnitTests.Common.Factory;

public class AuthenticationControllerFactory : ControllerFactory
{
    public AuthenticationControllerFactory() : base() { }
	public override ControllerBase Create(ApplicationDbContext context)
	{
		AuthenticationService service = new AuthenticationService(context,
		jwtService,
		basePublisher);

		return new AuthenticationController(service);
	}
}
