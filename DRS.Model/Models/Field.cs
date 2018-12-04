using System;
using System.Collections.Generic;

using DRS.Model.Models;

namespace DRS.Model.Models
{
    public partial class Field
    {
        public Field()
        {
            DocumentField = new HashSet<DocumentField>();
            DocumentFieldSetting = new HashSet<DocumentFieldSetting>();
            ProcessHandlerField = new HashSet<ProcessHandlerField>();
            ProcessQueueField = new HashSet<ProcessQueueField>();
        }
        public int FieldId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FieldDataTypeNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<DocumentField> DocumentField { get; set; }
        public virtual ICollection<DocumentFieldSetting> DocumentFieldSetting { get; set; }
        public virtual ICollection<ProcessHandlerField> ProcessHandlerField { get; set; }
        public virtual ICollection<ProcessQueueField> ProcessQueueField { get; set; }
    }
}
