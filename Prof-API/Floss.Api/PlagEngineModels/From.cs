using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.PlagEngineModels
{
    /*
     * An encapsulator for a single submission that contains a list of all the sections of code that have been copied from this submission
     */
    public class From
    {
        //the Id of the user that is being copied from
        public int userFromID { get; set; }

        //list of all the secions of code that have been copied from
        public ICollection<Plag> copiedFrom { get; set; }
    }
}
