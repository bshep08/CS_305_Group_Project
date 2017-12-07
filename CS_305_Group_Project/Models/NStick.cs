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

        public string TimeStuck { get; set; }
        public DateTime ComparableDate { get; set; }

        public string Description { get; set; }
        public DateTime TimeCreated { get; set; }

        public string Location { get; set; }

        public Boolean Handled { get; set; }

        public NStick()
        {
            TimeCreated = DateTime.Now;
            Handled = false;
        }
    }
}