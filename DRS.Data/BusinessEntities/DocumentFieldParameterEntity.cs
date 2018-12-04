using System;
using System.Collections.Generic;

namespace DRS.Data.BusinessEntities
{
    public class DocumentFieldParameterEntity
    {
        public DocumentFieldParameterEntity()
        {
            DocumentField = new HashSet<DocumentFieldEntity>();
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

        public virtual ICollection<DocumentFieldEntity> DocumentField { get; set; }
    }
}
