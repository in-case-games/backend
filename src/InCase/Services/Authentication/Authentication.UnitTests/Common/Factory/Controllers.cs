using Authentication.DAL.Data;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.UnitTests.Common.Factory;

public class Controllers
{
	private ControllerFactory factory;
	public Controllers(ControllerFactory factory) => this.factory = factory;
	public ControllerBase Create(ApplicationDbContext context)
		=> factory.Create(context);
}
