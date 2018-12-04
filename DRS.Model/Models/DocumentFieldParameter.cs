using System;
using System.Collections.Generic;

using DRS.Model.Models;

namespace DRS.Model.Models
{
    public partial class DocumentFieldParameter
    {
        public DocumentFieldParameter()
        {
            DocumentField = new HashSet<DocumentField>();
        }

        public int DocumentFieldParameterId { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<DocumentField> DocumentField { get; set; }
    }
}
