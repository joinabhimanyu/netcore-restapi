using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRS.Model.Models;
using DRS.Data.BusinessEntities;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace DRS.Data.ServiceInterfaces
{
    public interface IGenericService<TEntity> where TEntity : class
    {
        TEntity GetById(int productId);
        TEntity GetSingleByProp(GetSingleArgs args);
        PagedResult GetAll([Optional]PaginationArgs args);
        int Create(TEntity entity);
        bool Update(int entityId, TEntity entity);
        bool Delete(int entityId);
        JArray GetTypeHierarchy();
    }

    public interface IGenericListService<TEntity> :IGenericService<TEntity>
        where TEntity : class
    {
        object[] Create(ICollection<TEntity> entities);
        bool Update(ICollection<TEntity> entities);
        bool Delete(ICollection<TEntity> entities);
    }
}
