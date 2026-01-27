using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvConvertor
{

    public class User
    {
        public string email { get; set; }
        public string full_name { get; set; }
        public bool is_manager { get; set; }
        public bool is_engineer { get; set; }
        public string phone { get; set; }
        public string id { get; set; }
        public bool is_operator { get; set; }
        public string role { get; set; }
        public string image { get; set; }
    }

}
