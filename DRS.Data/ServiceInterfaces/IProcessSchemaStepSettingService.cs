using DRS.Data.BusinessEntities;
using DRS.Model.Models;

namespace DRS.Data.ServiceInterfaces
{
    public interface IProcessSchemaStepSettingService<TEntity> :IGenericService<TEntity>
        where TEntity : class
    {
        PagedResult GetAllFromDocumentRule(PaginationArgs args);
    }
}
