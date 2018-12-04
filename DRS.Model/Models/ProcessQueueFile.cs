using System;
using System.Collections.Generic;

using DRS.Model.Models;

namespace DRS.Model.Models
{
    public partial class ProcessQueueFile
    {
        public int ProcessQueueFileId { get; set; }
        public int ProcessQueueId { get; set; }
        public string Path { get; set; }
        public string Filename { get; set; }
        public int FileType { get; set; }
        public int ProcessState { get; set; }
        public DateTime? Created { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ProcessQueue ProcessQueue { get; set; }
    }
}
