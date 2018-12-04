using System;
using System.Collections.Generic;

namespace DRS.DataAccess.Models
{
    public partial class ProductGroupWeightingHelpingTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int ProductGroupWeightingTable { get; set; }
    }
}
