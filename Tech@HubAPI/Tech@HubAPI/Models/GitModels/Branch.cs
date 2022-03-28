using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tech_HubAPI.Models.GitModels
{
    /// <summary>Class <c>Branch</c> contains information
    /// about Git branches. </summary>
    public class Branch
    {
        public Branch(string name, string hash)
        {
            Name = name;
            Hash = hash;
        }


        /// <summary> The name of the branch </summary>
        public string Name { get; set; }
        /// <summary> The hash associated with this branch. </summary>
        public string Hash { get; set; }
    }
}
