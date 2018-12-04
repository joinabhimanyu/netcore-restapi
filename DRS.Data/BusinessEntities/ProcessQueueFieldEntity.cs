using System;

namespace DRS.Data.BusinessEntities
{
    public partial class ProcessQueueFieldEntity
    {
        public int ProcessQueueFieldId { get; set; }
        public int ProcessQueueId { get; set; }
        public int FieldId { get; set; }
        public string FieldValue { get; set; }
        public DateTime? Created { get; set; }
        public byte[] Stamp { get; set; }

        public virtual Data.BusinessEntities.FieldEntity Field { get; set; }
        public virtual ProcessQueueEntity ProcessQueue { get; set; }
    }
}
