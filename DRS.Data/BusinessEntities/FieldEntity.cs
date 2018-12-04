using System;
using System.Collections.Generic;
using DRS.Model.Models;

namespace DRS.Data.BusinessEntities
{
    public partial class FieldEntity
    {
        public FieldEntity()
        {
            DocumentField = new HashSet<DocumentFieldEntity>();
            DocumentFieldSetting = new HashSet<DocumentFieldSettingEntity>();
            ProcessHandlerField = new HashSet<ProcessHandlerFieldEntity>();
            ProcessQueueField = new HashSet<ProcessQueueFieldEntity>();
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

        public virtual ICollection<DocumentFieldEntity> DocumentField { get; set; }
        public virtual ICollection<DocumentFieldSettingEntity> DocumentFieldSetting { get; set; }
        public virtual ICollection<ProcessHandlerFieldEntity> ProcessHandlerField { get; set; }
        public virtual ICollection<ProcessQueueFieldEntity> ProcessQueueField { get; set; }
    }
}
