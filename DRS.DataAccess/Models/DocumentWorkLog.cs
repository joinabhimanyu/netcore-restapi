using System;
using System.Collections.Generic;

namespace DRS.DataAccess.Models
{
    public partial class DocumentWorkLog
    {
        public int DocumentId { get; set; }
        public int WorkLogId { get; set; }
        public DateTime? Done { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }
    }
}
