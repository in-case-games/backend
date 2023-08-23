using Authentication.DAL.Data;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Authentication.UnitTests.Common
{
    public class TestApiBase : IDisposable
    {
        protected readonly ITestOutputHelper? outputHelper;
        protected readonly ApplicationDbContext Context;

        public TestApiBase(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            Context = AuthenticationContextFactory.Create();
        }

        public void Dispose() => AuthenticationContextFactory.Destroy(Context);
    }
}
