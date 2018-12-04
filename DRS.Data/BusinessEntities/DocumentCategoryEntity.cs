using System;
using System.Collections.Generic;
using DRS.Model.Models;

namespace  DRS.Data.BusinessEntities
{
    public partial class DocumentCategoryEntity
    {
        public DocumentCategoryEntity()
        {
            Document = new HashSet<DocumentEntity>();
        }

        public int DocumentCategoryId { get; set; }
        public int ArchiveId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        //public byte[] Stamp { get; set; }

        public virtual ICollection<DocumentEntity> Document { get; set; }
        public virtual ArchiveEntity Archive { get; set; }
    }
}
