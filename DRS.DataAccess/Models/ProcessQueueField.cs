using System;
using System.Collections.Generic;

namespace DRS.DataAccess.Models
{
    public partial class ProcessQueueField
    {
        public int ProcessQueueFieldId { get; set; }
        public int ProcessQueueId { get; set; }
        public int FieldId { get; set; }
        public string FieldValue { get; set; }
        public DateTime? Created { get; set; }
        public byte[] Stamp { get; set; }

        public virtual Field Field { get; set; }
        public virtual ProcessQueue ProcessQueue { get; set; }
    }
}
