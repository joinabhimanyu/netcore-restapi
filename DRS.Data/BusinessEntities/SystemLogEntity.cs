using System;

namespace DRS.Data.BusinessEntities
{
    public partial class SystemLogEntity
    {
        public int LogId { get; set; }
        public int? EventId { get; set; }
        public int Priority { get; set; }
        public string Severity { get; set; }
        public string Title { get; set; }
        public DateTime Timestamp { get; set; }
        public string MachineName { get; set; }
        public string AppDomainName { get; set; }
        public string ProcessName { get; set; }
        public string Message { get; set; }
    }
}
