using System;
using System.Collections.Generic;

namespace DRS.Data.BusinessEntities
{
    public partial class ProcessSchemaStepEntity
    {
        public ProcessSchemaStepEntity()
        {
            ProcessLog = new HashSet<ProcessLogEntity>();
            ProcessQueue = new HashSet<ProcessQueueEntity>();
            ProcessSchemaStepSetting = new HashSet<ProcessSchemaStepSettingEntity>();
        }

        public int ProcessSchemaStepId { get; set; }
        public Guid ProcessSchemaStepGuid { get; set; }
        public int ProcessSchemaId { get; set; }
        public int ProcessHandlerId { get; set; }
        public int StepOrder { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? OnSuccessStepId { get; set; }
        public Guid? OnSuccessStepGuid { get; set; }
        public int? OnErrorStepId { get; set; }
        public Guid? OnErrorStepGuid { get; set; }
        public string FieldQueryRule { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<ProcessLogEntity> ProcessLog { get; set; }
        public virtual ICollection<ProcessQueueEntity> ProcessQueue { get; set; }
        public virtual ICollection<ProcessSchemaStepSettingEntity> ProcessSchemaStepSetting { get; set; }
        public virtual Data.BusinessEntities.ProcessHandlerEntity ProcessHandler { get; set; }
        public virtual ProcessSchemaEntity ProcessSchema { get; set; }
    }
}
