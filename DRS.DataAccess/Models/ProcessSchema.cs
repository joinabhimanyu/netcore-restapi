using System;
using System.Collections.Generic;

namespace DRS.DataAccess.Models
{
    public partial class ProcessSchema
    {
        public ProcessSchema()
        {
            DocumentRule = new HashSet<DocumentRule>();
            ImportMapping = new HashSet<ImportMapping>();
            ProcessHandler = new HashSet<ProcessHandler>();
            ProcessQueue = new HashSet<ProcessQueue>();
            ProcessSchemaStep = new HashSet<ProcessSchemaStep>();
        }

        public int ProcessSchemaId { get; set; }
        public Guid ProcessSchemaGuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<DocumentRule> DocumentRule { get; set; }
        public virtual ICollection<ImportMapping> ImportMapping { get; set; }
        public virtual ICollection<ProcessHandler> ProcessHandler { get; set; }
        public virtual ICollection<ProcessQueue> ProcessQueue { get; set; }
        public virtual ICollection<ProcessSchemaStep> ProcessSchemaStep { get; set; }
    }
}
