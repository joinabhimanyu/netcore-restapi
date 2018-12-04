using System;
using System.Collections.Generic;

namespace DRS.DataAccess.Models
{
    public partial class ProcessSchemaStepSetting
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

        public virtual ProcessSchemaStep ProcessSchemaStep { get; set; }
    }
}
