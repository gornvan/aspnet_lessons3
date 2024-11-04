using SynopticumDAL.Contract;
using SynopticumDAL.Services;

namespace SynopticumCoreTests
{
    internal class UoWInitializer
    {
        public static IUnitOfWork Initialize()
        {
            var dbContext = DbContextInitializer.Initialize("testsettings.json");
            return new UnitOfWork(dbContext);
        }
    }
}
