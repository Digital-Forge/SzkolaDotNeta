using Application.Constans;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FileController(IFileService _fileService) : ControllerBase
    {
        [HttpGet]
        public async Task<IFileService.FileModel> GetFile(Guid id)
        {
            return await _fileService.GetFileObjectAsync(id);
        }

        [HttpGet]
        [Route("Info")]
        public async Task<IFileService.DataFileInfoModel> GetInfo(Guid id)
        {
            return await _fileService.GetFileInfoAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = Constans.Role.Name.Administration)]
        public async Task<Guid> Create(IFileService.FileModel model)
        {
            return await _fileService.SaveFileAsync(model, true);
        }
    }
}
