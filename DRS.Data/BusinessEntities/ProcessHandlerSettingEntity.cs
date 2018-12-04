using System;

namespace DRS.Data.BusinessEntities
{
    public partial class ProcessHandlerSettingEntity
    {
        public int ProcessHandlerSettingId { get; set; }
        public int ProcessHandlerId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool Visible { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual Data.BusinessEntities.ProcessHandlerEntity ProcessHandler { get; set; }
    }
}
