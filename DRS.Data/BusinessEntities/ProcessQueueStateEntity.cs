using System;
using System.Collections.Generic;

namespace DRS.Data.BusinessEntities
{
    public partial class ProcessQueueStateEntity
    {
        public ProcessQueueStateEntity()
        {
            ProcessQueue = new HashSet<ProcessQueueEntity>();
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

        public virtual ICollection<ProcessQueueEntity> ProcessQueue { get; set; }
    }
}
