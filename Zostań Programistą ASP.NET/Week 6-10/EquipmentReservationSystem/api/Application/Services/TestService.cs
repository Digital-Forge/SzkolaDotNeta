using Application.Attributes;
using Application.Interfaces.Services;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    [AutoRegisterTransientService(typeof(ITestService))]
    public class TestService(ITestRepository _testRepository) : ITestService
    {
        public object Test()
        {
            return _testRepository.Test();
        }
    }
}
