using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{    
    /// <summary>
    /// 2.1.1 - Создать новую сущность-агрегатор (LibraryCard): человек, взявший для прочтения книгу + дата и время
    /// получения книги (DateTimeOffset) 
    /// </summary>
    public class LibraryCard
    {
        public int Id { get; set; }
        public HumanDTO reader { get; set; }
        public BookDTO book { get; set; }
        public DateTimeOffset dateTimeGetBook { get; set; }
    }
}
