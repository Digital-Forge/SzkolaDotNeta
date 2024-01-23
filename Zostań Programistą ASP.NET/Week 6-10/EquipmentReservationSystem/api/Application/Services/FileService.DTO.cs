using Application.Interfaces;
using Domain.Models.System;

namespace Application.Services
{
    public partial class FileService
    {
        public class DataFileInfoModel : IFileService.IDataFileInfoModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public bool IsMissing { get; set; }

            public static IFileService.IDataFileInfoModel Map(DataFile entity)
            {
                return new DataFileInfoModel
                {
                    Id = entity.Id,
                    Name = $"{entity.OriginName}.{entity.Format}",
                    Date = entity.CreateTime
                };
            }

            public static DataFile Map(IFileService.IDataFileInfoModel model, DataFile entity = null)
            {
                if (entity == null) entity = new DataFile();

                entity.Active = true;
                entity.Format = Path.GetExtension(model.Name);
                entity.OriginName = Path.GetFileNameWithoutExtension(model.Name);

                return entity;
            }
        }

        public class FileModel : IFileService.IFileModel
        {
            public IFileService.IDataFileInfoModel Info { get; set; }
            public string DataBase64{ get; set; }
        }
    }
}
