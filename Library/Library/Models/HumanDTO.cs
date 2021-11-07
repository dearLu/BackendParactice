using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    /// <summary>
    /// 1.2.1 - Класс человека
    /// </summary>
    public class HumanDTO
    {
        /// <summary>
        /// 2.2.1 - Добавьте валидации в ваши сущности: все обязательные поля должны быть NotNull. 
        /// </summary>
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        public string Name {get; set;}

        [Required(ErrorMessage = "Не указана фамилия")]
        public string Surname {get; set;}

        [Required(ErrorMessage = "Не указано отчество")]
        public string Patronymic { get; set; } 

        [Required(ErrorMessage = "Не указана дата рождения")]
        [DataType(DataType.DateTime)]
        public DateTime Birthday { get; set; }
    }
}
