using DRS.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRS.Model.Repositories
{
    public class ProcessSchemaStepSettingRepository<TEntity> : GenericRepository<TEntity>
        where TEntity : ProcessSchemaStepSetting
    {
        public ProcessSchemaStepSettingRepository(DRSDBContext context) : base(context)
        {
        }
        //public IEnumerable<ProcessSchemaStepSetting> GetAllFromDocumentRule(int DocumentRuleId)
        //{
        //    return (from psss in Context.ProcessSchemaStepSetting
        //            join r in (
        //            (from psss in Context.ProcessSchemaStepSetting
        //             join r in (
        //             (from ps in Context.ProcessSchemaStep
        //              join b in (
        //              (from doc in Context.DocumentRule where doc.DocumentRuleId == DocumentRuleId select doc.ProcessSchemaId).Distinct()
        //              )
        //              on ps.ProcessSchemaId equals b
        //              select ps.ProcessSchemaStepId).Distinct()
        //             ) on psss.ProcessSchemaStepId equals r
        //             select psss.ProcessSchemaStepSettingId).Distinct()
        //            ) on psss.ProcessSchemaStepSettingId equals r
        //            select psss).ToList<ProcessSchemaStepSetting>();
        //}
    }
}
