using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plagiarism_Engine.Models
{
    public class Node
    {
        public List<Node> children;

        public int lineNumber { get; set; }//the line number this node appears on

        public String val { get; set; }//rule name

        public Node parent { get; set; }//the parent of this node

        public String name { get; set; }//if node is an identifier then identifier name

        public int fromFile { get; set; }//the ID of the file this node belongs to

        public Node(String val, int lineNum, String name, int id)
        {
            this.val = val;
            this.lineNumber = lineNum;
            children = new List<Node>();
            this.name = name;
            this.fromFile = id;
        }

        public void addChild(Node c)
        {
            children.Add(c);
        }
    }
}
