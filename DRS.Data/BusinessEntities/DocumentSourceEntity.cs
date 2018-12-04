using System;
using System.Collections.Generic;

namespace DRS.Data.BusinessEntities
{
    public partial class DocumentSourceEntity
    {
        public DocumentSourceEntity()
        {
            Document = new HashSet<Data.BusinessEntities.DocumentEntity>();
        }

        public int DocumentSourceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Organization { get; set; }
        public int Priority { get; set; }
        public string DocumentUriPath { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<Data.BusinessEntities.DocumentEntity> Document { get; set; }
    }
}
