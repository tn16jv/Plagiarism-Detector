using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plagiarism_Engine.Models
{
    /*
     * An encapsulator for a single submission that contains a list of all the sections of code that have been copied from this submission
     */
    public class From
    {
        //the Id of the user that is being copied from
        public int userFromID { get; set; }

        //the number of lines copied from this user
        public int linesCopied = 0;

        //list of all the secions of code that have been copied from
        public ICollection<Plag> copiedFrom { get; set; }

        public From()
        {
            copiedFrom = new List<Plag>();
        }
    }
}
