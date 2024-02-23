using Application.Interfaces;
using Domain.Models.System;

namespace Application.Services
{
    public partial class FileService
    {
        public class DataFileInfoModel : IFileService.DataFileInfoModel
        {
            public static IFileService.DataFileInfoModel Map(DataFile entity)
            {
                return new DataFileInfoModel
                {
                    Id = entity.Id,
                    Name = $"{entity.OriginName}.{entity.Format}",
                    Date = entity.CreateTime
                };
            }

            public static DataFile Map(IFileService.DataFileInfoModel model, DataFile entity = null)
            {
                if (entity == null) entity = new DataFile();

                entity.Active = true;
                entity.Format = Path.GetExtension(model.Name);
                entity.OriginName = Path.GetFileNameWithoutExtension(model.Name);

                return entity;
            }
        }
    }
}
