using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plagiarism_Engine.Models
{
    /*
     * A single section of code that has been copied from an owner of the report(submisson that the report is being created for)
     *  where each submission has copied from
     */
    public class Plag
    {

        public Plag(bool isMethod = false)
        {
            this.isMethod = isMethod;
        }

		// the methodness of the plag
		public bool isMethod { get; private set; }

		//the start line of the owners code that has been copied
		public int reportStartLine { get; set; }

        //the end line of the owners code that has been copied
        public int reportEndLine { get; set; }

        //the start line of the from code that has been copied
        public int fromStart { get; set; }

        //the end line of the from code that has been copied
        public int fromEnd { get; set; }
    }
}
