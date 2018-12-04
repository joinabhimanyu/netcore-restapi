using System;
using System.Collections.Generic;

namespace DRS.DataAccess.Models
{
    public partial class Document
    {
        public Document()
        {
            DocumentField = new HashSet<DocumentField>();
            DocumentFieldSetting = new HashSet<DocumentFieldSetting>();
            DocumentRule = new HashSet<DocumentRule>();
            ProcessQueue = new HashSet<ProcessQueue>();
        }

        public int DocumentId { get; set; }
        public string DocumentIdentity { get; set; }
        public string Number { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Pages { get; set; }
        public string CompanyCode { get; set; }
        public string BatchClass { get; set; }
        public string ExportFileType { get; set; }
        public string Link { get; set; }
        public int? DocumentTemplateId { get; set; }
        public int DocumentCategoryId { get; set; }
        public int DocumentSourceId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<DocumentField> DocumentField { get; set; }
        public virtual ICollection<DocumentFieldSetting> DocumentFieldSetting { get; set; }
        public virtual ICollection<DocumentRule> DocumentRule { get; set; }
        public virtual ICollection<ProcessQueue> ProcessQueue { get; set; }
        public virtual DocumentCategory DocumentCategory { get; set; }
        public virtual DocumentSource DocumentSource { get; set; }
    }
}
