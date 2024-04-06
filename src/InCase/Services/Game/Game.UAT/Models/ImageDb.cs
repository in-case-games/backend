using Game.DAL.Data;
using Moq;

namespace Game.UAT.Models;
public class ImageDb
{
	public Mock<ApplicationDbContext> MockContext { get; set; } = null!;
	public DataDb Data { get; set; } = null!;
}