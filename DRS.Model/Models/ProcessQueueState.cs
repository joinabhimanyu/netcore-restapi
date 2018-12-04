using System;
using System.Collections.Generic;

using DRS.Model.Models;

namespace DRS.Model.Models
{
    public partial class ProcessQueueState
    {
        public ProcessQueueState()
        {
            ProcessQueue = new HashSet<ProcessQueue>();
        }

        public int ProcessQueueStateId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] StateImage { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<ProcessQueue> ProcessQueue { get; set; }
    }
}
