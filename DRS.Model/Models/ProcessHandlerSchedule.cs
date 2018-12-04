using System;
using System.Collections.Generic;
using DRS.Model.Models;

namespace DRS.Model.Models
{
    public partial class ProcessHandlerSchedule
    {
        public int ProcessHandlerScheduleId { get; set; }
        public int ProcessHandlerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int FrequencyTypeId { get; set; }
        public int FrequencyFlags { get; set; }
        public DateTime? DailyFrequencyOccursAt { get; set; }
        public int? DailyFrequencyInterval { get; set; }
        public int? DailyFrequencyIntervalType { get; set; }
        public DateTime? DailyFrequencyStartAt { get; set; }
        public DateTime? DailyFrequencyEndAt { get; set; }
        public DateTime DurationStartDate { get; set; }
        public DateTime? DurationEndDate { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Stamp { get; set; }

        public virtual ProcessHandler ProcessHandler { get; set; }
    }
}
