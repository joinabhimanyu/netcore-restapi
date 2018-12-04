using System;
using System.Collections.Generic;
using DRS.Model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace DRS.Model.Models
{
    public partial class DocumentSource
    {
        public DocumentSource()
        {
            Document = new HashSet<Document>();
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

        [NotMapped]
        public virtual ICollection<Document> Document { get; set; }
    }
}
