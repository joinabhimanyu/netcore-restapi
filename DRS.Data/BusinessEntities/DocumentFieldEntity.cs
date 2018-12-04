using System;

namespace  DRS.Data.BusinessEntities
{
    public class DocumentFieldEntity
    {
        public int DocumentId { get; set; }
        public int FieldId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Parameter { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual DocumentEntity Document { get; set; }
        public virtual FieldEntity Field { get; set; }
        public virtual DocumentFieldParameterEntity ParameterNavigation { get; set; }
    }
}
