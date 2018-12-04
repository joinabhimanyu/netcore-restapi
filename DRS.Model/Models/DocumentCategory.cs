using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DRS.Model.Models;

namespace  DRS.Model.Models
{
    public partial class DocumentCategory
    {
        public DocumentCategory()
        {
            Document = new HashSet<Document>();
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
        public byte[] Stamp { get; set; }

        [NotMapped]
        public virtual ICollection<Document> Document { get; set; }
        
        public virtual Archive Archive { get; set; }
    }
}
