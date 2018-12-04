using System;
using System.Collections.Generic;

using DRS.Model.Models;

namespace DRS.Model.Models
{
    public partial class LsCommand
    {
        public int Id { get; set; }
        public string Command { get; set; }
        public string Request { get; set; }
        public int? Result { get; set; }
        public string ResultMessage { get; set; }
    }
}
