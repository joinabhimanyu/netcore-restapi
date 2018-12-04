using System;
using System.Collections.Generic;
using DRS.Model.Models;

namespace  DRS.Model.Models
{
    public partial class DocumentRule
    {
        public DocumentRule()
        {
            DocumentRuleSetting = new HashSet<DocumentRuleSetting>();
            ProcessQueue = new HashSet<ProcessQueue>();
        }

        public int DocumentRuleId { get; set; }
        public Guid DocumentRuleGuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DocumentId { get; set; }
        public int ProcessSchemaId { get; set; }
        public int RuleOrder { get; set; }
        public string FieldQueryRule { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<DocumentRuleSetting> DocumentRuleSetting { get; set; }
        public virtual ICollection<ProcessQueue> ProcessQueue { get; set; }
        public virtual Document Document { get; set; }
        public virtual ProcessSchema ProcessSchema { get; set; }
    }
}
