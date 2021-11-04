using System.Collections.Generic;

namespace Tech_HubAPI.Models
{
	public class Repository
	{
		//sets the id of the repository
		public int Id { get; set; }

		//title of the repo
		public string Title { get; set; }

		//public User owner { get; set; }

		// id of the owner of the repo
		public int OwnerId { get; set; }

		// true if repo is public
		public bool IsPublic { get; set; }

		//file path/location of the repo in the file system
		public string FilePath { get; set; }
	}
}
