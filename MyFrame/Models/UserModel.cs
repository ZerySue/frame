using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyFrame.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public string Account { get; set; }
        public int Sex { get; set; }
        public string Address { get; set; }
    }
}