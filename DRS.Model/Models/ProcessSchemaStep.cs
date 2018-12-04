using System;
using System.Collections.Generic;

using DRS.Model.Models;

namespace DRS.Model.Models
{
    public partial class ProcessSchemaStep
    {
        public ProcessSchemaStep()
        {
            ProcessLog = new HashSet<ProcessLog>();
            ProcessQueue = new HashSet<ProcessQueue>();
            ProcessSchemaStepSetting = new HashSet<ProcessSchemaStepSetting>();
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

        public virtual ICollection<ProcessLog> ProcessLog { get; set; }
        public virtual ICollection<ProcessQueue> ProcessQueue { get; set; }
        public virtual ICollection<ProcessSchemaStepSetting> ProcessSchemaStepSetting { get; set; }
        public virtual ProcessHandler ProcessHandler { get; set; }
        public virtual ProcessSchema ProcessSchema { get; set; }
    }
}
