using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CS_305_Group_Project.Models
{
    public class NStick
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        //[Required (ErrorMessage = "Date AND time required")]
        public DateTime TimeStuck { get; set; }

        public string Description { get; set; }
        public DateTime TimeCreated { get; set; }
        
        public NStick()
        {
            TimeCreated = DateTime.Now;
        }
    }
}