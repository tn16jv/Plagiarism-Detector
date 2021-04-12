using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Plagiarism_Engine.Models;
using Plagiarism_Engine_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plagiarism_Engine.Services
{
    public class CService
    {
        /// <summary>
        /// Converts a file to the abstract syntax tree using the C grammar. It first creates a parse tree then converts that to an AST
        /// </summary>
        /// <param name="f">the file to convert</param>
        /// <returns>the root node of the AST</returns>
        public static Node getCASTForUser(File f)
        {
            ICharStream stream = CharStreams.fromstring(f.CodeFiles);
            ITokenSource lexer = new CLexer(stream);
            ITokenStream tokens = new CommonTokenStream(lexer);
            CParser parser = new CParser(tokens);
            ParserRuleContext ctx = parser.translationUnit();

            Node root = new Node(CParser.ruleNames[ctx.RuleIndex], ctx.Start.Line, "",f.Id);

            root = parseToAST(ctx, false, 0, root);

            return root;  
        }
        /// <summary>
        /// Coverts the parse tree to an AST. This is done by removing unsessesary nodes, and converts from antlr to Node
        /// </summary>
        /// <param name="ctx">the rule of the parent</param>
        /// <param name="verbose">a boolean checker to see if the parent is in the AST</param>
        /// <param name="indentation">an int that was used when visualizing the AST</param>
        /// <param name="par">the parent node</param>
        /// <returns>the root node of the AST</returns>
        public static Node parseToAST(ParserRuleContext ctx, bool verbose, int indentation, Node par)
        {
            bool toBeIgnored = !verbose && ctx.ChildCount == 1 && ctx.GetChild(0) is ParserRuleContext;
            if (!toBeIgnored)
            {
                String ruleName = CParser.ruleNames[ctx.RuleIndex];
                Node temp;
                if (ruleName.Equals("typedefName"))
                    temp = new Node(ruleName, ctx.Start.Line, ctx.GetText(),par.fromFile);
                else
                    temp = new Node(ruleName, ctx.Start.Line, "",par.fromFile);
                par.addChild(temp);
                temp.parent = par;
                par = temp;
            }
            for (int i = 0; i < ctx.ChildCount; i++)
            {
                IParseTree element = ctx.GetChild(i);
                if (element is RuleContext)
                {
                    ParserRuleContext child = (ParserRuleContext)element;

                    parseToAST((ParserRuleContext)element, verbose, indentation + (toBeIgnored ? 0 : 1), par);
                }
            }
            return par;
        }
        /// <summary>
        /// Takes a token stream for a file then splits it up by method. splits it up by functiondefinition and structOrUnionSpecifier rule
        /// </summary>
        /// <param name="list">the token stream to split</param>
        /// <returns>the split up token stream</returns>
        public static List<List<Token>> SplitMethods(List<Token> list)
        {
            //possibly remove structOrUnionSpecifier
            List<List<Token>> res = new List<List<Token>>();
            var indexlist = list.Where(x => x.val == "functionDefinition" || x.val == "structOrUnionSpecifier").Select(x => list.IndexOf(x)).ToList();

            for (int i = 0; i < indexlist.Count; i++)
            {
                List<Token> tmp = new List<Token>();
                try
                {
                    for (int j = indexlist[i]; j < indexlist[i + 1]; j++)
                    {
                        tmp.Add(list[j]);
                    }
                }
                catch (Exception e)
                {
                    for (int j = indexlist[i]; j < list.Count; j++)
                    {
                        tmp.Add(list[j]);
                    }
                }

                res.Add(tmp);
            }

            return res;
        }

    }
}
