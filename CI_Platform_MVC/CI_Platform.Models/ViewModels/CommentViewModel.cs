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
    }
}
