using System;
using System.Collections.Generic;
using System.Text;

namespace Plagiarism_Engine.Models
{
    /// <summary>
    /// A representation of a token that is used to compare. Contains info about a specific token. If no value is passed in it is initilaized to a blank token.
    /// </summary>
    public class Token
    {

        public int lineNumber { get; set; }

        public String val { get; set; }

        public String parent { get; set; }

        public String name { get; set; }

        public int fromFile { get; set; }

        public bool marked { get; set; }


        public Token(String val= "", int lineNum = -1, String par = "", String name = "",int id = -1)
        {
            this.val = val;
            this.lineNumber = lineNum;
            this.parent = par;
            this.name = name;
            this.fromFile = id;
            this.marked = false;
        }
    }
}