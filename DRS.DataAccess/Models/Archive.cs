﻿using System;
using System.Collections.Generic;

namespace DRS.DataAccess.Models
{
    public partial class Archive
    {
        public Archive()
        {
            DocumentCategory = new HashSet<DocumentCategory>();
        }

        public int ArchiveId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Organization { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<DocumentCategory> DocumentCategory { get; set; }
    }
}
