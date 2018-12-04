using System;
using System.Collections.Generic;

namespace DRS.DataAccess.Models
{
    public partial class ProcessLog
    {
        public int ProcessLogId { get; set; }
        public int? ProcessHandlerId { get; set; }
        public int? ProcessQueueId { get; set; }
        public int? ProcessSchemaStepId { get; set; }
        public int? TraceEventTypeId { get; set; }
        public int EventId { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual ProcessHandler ProcessHandler { get; set; }
        public virtual ProcessQueue ProcessQueue { get; set; }
        public virtual ProcessSchemaStep ProcessSchemaStep { get; set; }
    }
}
