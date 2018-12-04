using System;
using System.Collections.Generic;

namespace DRS.DataAccess.Models
{
    public partial class ProcessQueue
    {
        public ProcessQueue()
        {
            ProcessLog = new HashSet<ProcessLog>();
            ProcessQueueField = new HashSet<ProcessQueueField>();
            ProcessQueueFile = new HashSet<ProcessQueueFile>();
        }

        public int ProcessQueueId { get; set; }
        public int ImportHandlerId { get; set; }
        public int ProcessSchemaId { get; set; }
        public int? DocumentRuleId { get; set; }
        public int? NextProcessSchemaStepId { get; set; }
        public int ProcessQueueStateId { get; set; }
        public int ProcessErrorCount { get; set; }
        public double? ProcessTotaltime { get; set; }
        public DateTime? LastProcessDate { get; set; }
        public int? DocumentId { get; set; }
        public string DocumentKey { get; set; }
        public string DocumentIdentity { get; set; }
        public string DocumentName { get; set; }
        public string DocumentCategory { get; set; }
        public DateTime? DocumentStamp { get; set; }
        public int? DocumentPages { get; set; }
        public string DocumentArchiveKey { get; set; }
        public DateTime? Created { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<ProcessLog> ProcessLog { get; set; }
        public virtual ICollection<ProcessQueueField> ProcessQueueField { get; set; }
        public virtual ICollection<ProcessQueueFile> ProcessQueueFile { get; set; }
        public virtual Document Document { get; set; }
        public virtual DocumentRule DocumentRule { get; set; }
        public virtual ProcessHandler ImportHandler { get; set; }
        public virtual ProcessSchemaStep NextProcessSchemaStep { get; set; }
        public virtual ProcessQueueState ProcessQueueState { get; set; }
        public virtual ProcessSchema ProcessSchema { get; set; }
    }
}
