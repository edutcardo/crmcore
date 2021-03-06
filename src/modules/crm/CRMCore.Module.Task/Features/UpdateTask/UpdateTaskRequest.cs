using System;
using System.ComponentModel.DataAnnotations;

namespace CRMCore.Module.Task.Features.UpdateTask
{
    public class UpdateTaskRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int DueType { get; set; }

        [Required]
        public Guid AssignedTo { get; set; }

        [Required]
        public int CategoryType { get; set; }
    }
}
