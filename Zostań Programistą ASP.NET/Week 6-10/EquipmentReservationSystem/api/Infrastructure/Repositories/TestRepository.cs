using Domain.Interfaces.Repositories;
using Infrastructure.Attributes;
using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    [AutoRegisterTransientRepository(typeof(ITestRepository))]
    public class TestRepository(Context _context) : ITestRepository
    {
        public object Test()
        {
            return _context.Users.FirstOrDefault()?.Email ?? "test";
        }
    }
}
