using System;

namespace DRS.Data.BusinessEntities
{
    public partial class DocumentWorkLogEntity
    {
        public int DocumentId { get; set; }
        public int WorkLogId { get; set; }
        public DateTime? Done { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }
    }
}
