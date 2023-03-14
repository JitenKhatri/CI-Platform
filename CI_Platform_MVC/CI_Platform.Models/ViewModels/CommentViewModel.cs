using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class CommentViewModel
    {
        public Comment? User_Comment { get; set; }
     
        public User? user { get; set; }
      
        public long MissionId { get; set; }

        [Required(ErrorMessage = "Comment is required.")]
        [StringLength(200, ErrorMessage = "Comment cannot be longer than 200 characters.")]
        public string CommentText { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
