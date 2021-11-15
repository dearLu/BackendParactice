Use Library

SELECT Author.First_Name,Author.Last_Name, Genre.Genre_Name , COUNT(Book_Genre.Genre_id) AS CountBook
FROM Author,Book,Genre ,Book_Genre 
Where Author.Id = Book.Author_Id AND Book.Id =Book_Genre.Book_Id AND Book_Genre.Genre_id = Genre.Id
GROUP BY Author.First_Name,Author.Last_Name,Book_Genre.Genre_id,Genre.Genre_Name 

