using System;
namespace Tech_HubAPI.Models.Git
{
    public class DirectoryEntry
    {
        public DirectoryEntry( string name, bool isDirectory )
        {
            Name = name;
            IsDirectory = isDirectory;
        }

        public string Name { get; set; }

        public bool IsDirectory { get; set; }
    }
}
