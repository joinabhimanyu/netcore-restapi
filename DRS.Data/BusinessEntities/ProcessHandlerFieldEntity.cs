using System;

namespace DRS.Data.BusinessEntities
{
    public  class ProcessHandlerFieldEntity
    {
        public int ProcessHandlerFieldId { get; set; }
        public int ProcessHandlerId { get; set; }
        public int FieldId { get; set; }
        public string FieldName { get; set; }
        public string FieldValueFormat { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual Data.BusinessEntities.FieldEntity Field { get; set; }
        public virtual ProcessHandlerEntity ProcessHandler { get; set; }
    }
}
