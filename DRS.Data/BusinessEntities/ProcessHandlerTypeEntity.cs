using System;
using System.Collections.Generic;

namespace DRS.Data.BusinessEntities
{
    public partial class ProcessHandlerTypeEntity
    {
        public ProcessHandlerTypeEntity()
        {
            ProcessHandler = new HashSet<Data.BusinessEntities.ProcessHandlerEntity>();
        }

        public int ProcessHandlerTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<Data.BusinessEntities.ProcessHandlerEntity> ProcessHandler { get; set; }
    }
}
