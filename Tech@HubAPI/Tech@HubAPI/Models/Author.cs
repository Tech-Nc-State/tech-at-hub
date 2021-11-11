using System.Collections.Generic;

namespace Tech_HubAPI.Models
{
	public class Author
	{
		public Author(string name)
		{
			Name = name;
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public List<Book> Books { get; set; }
	}
}
