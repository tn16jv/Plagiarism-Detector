using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floss.Api.PlagEngineModels
{
    /* A report that is generated for each submission made. Conatins a list of all the other submissions this submission copied from
     * 
     */
    public class Report
    {
        //the id of the user
        public int userReportId { get; set; }

        //A list of all the submissions this file has copied from
        public ICollection<From> froms { get; set; }
    }
}
