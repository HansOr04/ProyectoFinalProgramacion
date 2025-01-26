using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPPugaOrtizLopez.Models
{
    public class CalleSqlite
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Ciudad { get; set; }
        public string Calle { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
   
}
