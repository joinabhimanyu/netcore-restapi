using System;
using System.Collections.Generic;
using DRS.Model.Models;

namespace DRS.Data.BusinessEntities
{
    public partial class ProcessHandlerEntity
    {
        public ProcessHandlerEntity()
        {
            ImportMapping = new HashSet<ImportMappingEntity>();
            ProcessHandlerField = new HashSet<Data.BusinessEntities.ProcessHandlerFieldEntity>();
            ProcessHandlerSchedule = new HashSet<ProcessHandlerScheduleEntity>();
            ProcessHandlerSetting = new HashSet<ProcessHandlerSettingEntity>();
            ProcessLog = new HashSet<ProcessLogEntity>();
            ProcessQueue = new HashSet<ProcessQueueEntity>();
            ProcessSchemaStep = new HashSet<ProcessSchemaStepEntity>();
        }

        public int ProcessHandlerId { get; set; }
        public Guid ProcessHandlerGuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
        public bool IsActive { get; set; }
        public int ProcessHandlerTypeId { get; set; }
        public int ProcessSchemaId { get; set; }
        public int Priority { get; set; }
        public DateTime? LastExecuteDate { get; set; }
        public DateTime? LastProcessDate { get; set; }
        public DateTime? NextProcessDate { get; set; }
        public int NumberOfItemsToProcess { get; set; }
        public int NumberOfRetries { get; set; }
        public int WaitIntervalBetweenRetries { get; set; }
        public int NumberOfErrors { get; set; }
        public int WaitIntervalOnErrors { get; set; }
        public int ProcessErrorCount { get; set; }
        public string ProcessServers { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<ImportMappingEntity> ImportMapping { get; set; }
        public virtual ICollection<Data.BusinessEntities.ProcessHandlerFieldEntity> ProcessHandlerField { get; set; }
        public virtual ICollection<ProcessHandlerScheduleEntity> ProcessHandlerSchedule { get; set; }
        public virtual ICollection<ProcessHandlerSettingEntity> ProcessHandlerSetting { get; set; }
        public virtual ICollection<ProcessLogEntity> ProcessLog { get; set; }
        public virtual ICollection<ProcessQueueEntity> ProcessQueue { get; set; }
        public virtual ICollection<ProcessSchemaStepEntity> ProcessSchemaStep { get; set; }
        public virtual ProcessHandlerTypeEntity ProcessHandlerType { get; set; }
        public virtual ProcessSchemaEntity ProcessSchema { get; set; }
    }
}
