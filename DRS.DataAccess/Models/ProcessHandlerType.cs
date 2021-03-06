﻿using System;
using System.Collections.Generic;

namespace DRS.DataAccess.Models
{
    public partial class ProcessHandlerType
    {
        public ProcessHandlerType()
        {
            ProcessHandler = new HashSet<ProcessHandler>();
        }

        public int ProcessHandlerTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ICollection<ProcessHandler> ProcessHandler { get; set; }
    }
}
