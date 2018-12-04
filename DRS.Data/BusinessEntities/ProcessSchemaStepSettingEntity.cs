using System;

namespace DRS.Data.BusinessEntities
{
    public partial class ProcessSchemaStepSettingEntity
    {
        public int ProcessSchemaStepSettingId { get; set; }
        public int ProcessSchemaStepId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ProcessSchemaStepEntity ProcessSchemaStep { get; set; }
    }
}
