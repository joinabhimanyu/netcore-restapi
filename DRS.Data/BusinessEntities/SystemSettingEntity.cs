using System;

namespace DRS.Data.BusinessEntities
{
    public partial class SystemSettingEntity
    {
        public int SystemSettingId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int? SystemSettingTypeId { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual SystemSettingTypeEntity SystemSettingType { get; set; }
    }
}
