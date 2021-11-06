using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    /// <summary>
    /// 1.2.1 - Класс человека
    /// </summary>
    public class HumanDTO
    {
        public int Id { get; set; }
        public string Name {get; set;}
        public string Surname {get; set;}
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
    }
}
