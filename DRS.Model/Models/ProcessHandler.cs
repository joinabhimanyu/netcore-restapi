using System;
using System.Collections.Generic;

using DRS.Model.Models;

namespace DRS.Model.Models
{
    public partial class ProcessHandler
    {
        public ProcessHandler()
        {
            ImportMapping = new HashSet<ImportMapping>();
            ProcessHandlerField = new HashSet<ProcessHandlerField>();
            ProcessHandlerSchedule = new HashSet<ProcessHandlerSchedule>();
            ProcessHandlerSetting = new HashSet<ProcessHandlerSetting>();
            ProcessLog = new HashSet<ProcessLog>();
            ProcessQueue = new HashSet<ProcessQueue>();
            ProcessSchemaStep = new HashSet<ProcessSchemaStep>();
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

        public virtual ICollection<ImportMapping> ImportMapping { get; set; }
        public virtual ICollection<ProcessHandlerField> ProcessHandlerField { get; set; }
        public virtual ICollection<ProcessHandlerSchedule> ProcessHandlerSchedule { get; set; }
        public virtual ICollection<ProcessHandlerSetting> ProcessHandlerSetting { get; set; }
        public virtual ICollection<ProcessLog> ProcessLog { get; set; }
        public virtual ICollection<ProcessQueue> ProcessQueue { get; set; }
        public virtual ICollection<ProcessSchemaStep> ProcessSchemaStep { get; set; }
        public virtual ProcessHandlerType ProcessHandlerType { get; set; }
        public virtual ProcessSchema ProcessSchema { get; set; }
    }
}
