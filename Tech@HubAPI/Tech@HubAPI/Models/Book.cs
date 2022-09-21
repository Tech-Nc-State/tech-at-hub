using System;

namespace Tech_HubAPI.Models
{
	public class Book
	{
		public Book(string title, DateTime publishDate, int authorId)
		{
			Title = title;
			PublishDate = publishDate;
			AuthorId = authorId;
		}

		public int Id { get; set; }

		public string Title { get; set; }

		public DateTime PublishDate { get; set; }

		public int AuthorId { get; set;}

		public Author Author { get; set; }
	}
}
