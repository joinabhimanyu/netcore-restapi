using System;
using System.Collections.Generic;

namespace DRS.DataAccess.Models
{
    public partial class SettingSelectionValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }
    }
}
