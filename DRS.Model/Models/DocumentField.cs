using System;
using System.Collections.Generic;
using DRS.Model.Models;

namespace  DRS.Model.Models
{
    public partial class DocumentField
    {
        public int DocumentId { get; set; }
        public int FieldId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Parameter { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual Document Document { get; set; }
        public virtual Field Field { get; set; }
        public virtual DocumentFieldParameter ParameterNavigation { get; set; }
    }
}
