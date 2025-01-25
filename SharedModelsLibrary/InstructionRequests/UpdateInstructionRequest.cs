using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModelsLibrary.InstructionRequests
{
    public class UpdateInstructionRequest
    {
        [Required]
        public Guid InstructionId { get; set; }
      
        [Required]
        public required string Content { get; set; }
    }
}
