using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManagementSystem.EF;

namespace TaskManagementSystem.DTOs
{
    public class TaskDTO
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public System.DateTime Deadline { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}