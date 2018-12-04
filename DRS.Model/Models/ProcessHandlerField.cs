using System;
using System.Collections.Generic;

using DRS.Model.Models;

namespace DRS.Model.Models
{
    public partial class ProcessHandlerField
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

        public virtual Field Field { get; set; }
        public virtual ProcessHandler ProcessHandler { get; set; }
    }
}
