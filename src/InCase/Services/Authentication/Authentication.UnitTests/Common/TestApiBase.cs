using Authentication.DAL.Data;
using Xunit.Abstractions;

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
