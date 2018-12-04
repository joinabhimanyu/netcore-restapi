using System;
using DRS.Model.Models;

namespace DRS.Data.BusinessEntities
{
    public class ProcessLogEntity
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

        public virtual Data.BusinessEntities.ProcessHandlerEntity ProcessHandler { get; set; }
        public virtual ProcessQueueEntity ProcessQueue { get; set; }
        public virtual ProcessSchemaStepEntity ProcessSchemaStep { get; set; }
    }
}
