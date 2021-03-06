using System;
using System.ComponentModel.DataAnnotations;

namespace CRMCore.Module.Task.Features.CreateTask
{
    public class AddTaskRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid AssignedTo { get; set; }
    }
}
