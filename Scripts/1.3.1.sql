Use Library
SELECT  Author.First_Name,Author.Last_Name,Book.Name,Genre.Genre_Name 
FROM Book, Author,Genre,Book_Genre
WHERE Author.Last_Name = 'Пушкин' AND
 Author.Id = Book.Author_Id AND Book.Id= Book_Genre.Book_Id AND Book_Genre.Genre_id = Genre.Id