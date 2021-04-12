using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Plagiarism_Engine.Models;
using Plagiarism_Engine.Services;
using Plagiarism_Engine_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plagiarism_Engine.Service
{
    public class CompareService
    {

        private string fileType { get; set; }
        public int exempt = -2;
        List<List<Token>> exemptStreams;
        
        /// <summary>
        /// Generates Abstract syntax trees(AST) for each file submitted
        /// </summary>
        /// <param name="files">The list of files to be parsed</param>
        /// <returns> A list of nodes representing the AST</returns>
        public List<Node> createASTsForAllUsers(List<File> files)
        {
            
            List<Node> roots = new List<Node>();
            fileType = files.First<File>().FileType;//sets the type of file to compare to the first type of file that is passed in
            foreach (File f in files)
            {
                if (f.isExempt)
                    exempt = f.Id;
                switch (fileType.ToLower())
                {
                    case "java":
                        roots.Add(JavaService.getJavaASTForUser(f));
                        break;
                    case "c":
                        roots.Add(CppService.getCppASTForUser(f));
                        break;
                    case "cpp":
                        roots.Add(CppService.getCppASTForUser(f));
                        break;
                }
            }
            return roots;
        }

        /// <summary>
        /// Takes in all users token streams, and splits them up by method
        /// </summary>
        /// <param name="t">Each users token streams</param>
        /// <returns> Each users token streams seperated into methods</returns>
        public List<List<List<Token>>> splitTokenStreamByMethod(List<List<Token>> t)
        {
            List<List<List<Token>>> ret = new List<List<List<Token>>>();
            List<List<Token>> tmp;
            foreach (List<Token> l in t)
            {
                switch (fileType.ToLower())
                {
                    case "java":
                        tmp = JavaService.SplitMethods(l);
                        if (tmp.First().First().fromFile == exempt)
                        {
                            exemptStreams = tmp;
                            break;
                        }else       
                        ret.Add(tmp);
                        break;
                    case "c":
                        tmp = CppService.SplitMethods(l);
                        if (tmp.First().First().fromFile == exempt)
                        {
                            exemptStreams = tmp;
                            break;
                        }else
                        ret.Add(tmp);
                        break;
                    case "cpp":
                        tmp = CppService.SplitMethods(l);
                        if (tmp.First().First().fromFile == exempt)
                        {
                            exemptStreams = tmp;
                            break;
                        }else
                        ret.Add(tmp);
                        break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Takes in a AST and turns it into a token stream
        /// </summary>
        /// <param name="root">the root node </param>
        /// <param name="l">the return token stream </param>
        /// <returns>updated l</returns>
        public List<Token> NodesToTokens(Node root,List<Token> l)
        {
            Token t;
            try
            {
                t = new Token(root.val, root.lineNumber, root.parent.val, root.name,root.fromFile);
            }
            catch (Exception e)
            {
                t = new Token(root.val, root.lineNumber, "", root.name, root.fromFile);
            }

            l.Add(t);
            for (int i = 0; i < root.children.Count; i++)
            {
                NodesToTokens(root.children[i], l);
            }
            return l;
        }


        /// <summary>
        /// compares one submission against another
        /// </summary>
        /// <param name="l1">the first token stream</param>
        /// <param name="r1">the report for the first token stream</param>
        /// <param name="l2">the second token stream</param>
        /// <param name="r2">the report for the second token stream</param>
        /// <returns>the updated reports with the information for each match between l1 and l2</returns>
        public (Report,Report) compare(List<List<Token>> l1, Report r1, List<List<Token>> l2, Report r2)
        {
            
            int firstStart;
            int firstEnd;
            int secondStart;
            int secondEnd;

            int amount;
            int count;
            int matchedTokens = 0;
            int unmatchedTokens = 0;

            Dictionary<int, int> markedMethods = new Dictionary<int, int>();
            Dictionary<int, int> markedMethodsSecond = new Dictionary<int, int>();

            List<Token> alignment1;
            List<Token> alignment2;

            From f1 = new From(); //copied from l1 so add to r2
            From f2 = new From(); //copied from l2 so add to r1

            f1.userFromID = r1.userReportId;
            f2.userFromID = r2.userReportId;

            foreach (List<Token> l in l1)
            {
                firstStart = l.First<Token>().lineNumber;
                firstEnd = l.Last<Token>().lineNumber;

                

                foreach (List<Token> j in l2)
                {
                    secondStart = j.First<Token>().lineNumber;
                    secondEnd = j.Last<Token>().lineNumber;
                    
                    if (markedMethods.ContainsKey(firstStart) && markedMethods[firstStart] == firstEnd)
                        continue;
                    if (markedMethodsSecond.ContainsKey(secondStart) && markedMethodsSecond[secondStart] == secondEnd)
                        continue;



                    //if (!similarSize((firstEnd - firstStart), (secondEnd - secondStart)))// the methods have to be similar sizes
                    //continue;

                    //int amount = LCS(l, j);//amount of tokens simailar between the two methods
                    //int amount = NW(l, j);
                    //if (amount < 0)
                    //    amount = 0;

                    var output = SW(l, j);
                    amount = output.Item1;
                    

                    alignment1 = output.Item3;
                    alignment2 = output.Item2;


                    //List<int> indexList = new List<int>();
                    //indexList.Add(0);
                    //indexList.AddRange(alignment1.Where(x => x.val.Equals("") && x.lineNumber == -1).Select(x => alignment1.IndexOf(x)).ToList());
                    //indexList.Add(alignment1.Count);

                    //Plag p1;
                    //for (int i = 1; i < indexList.Count; i++)
                    //{
                    //    p1 = new Plag();
                    //    p1.reportStartLine = alignment1[i-1].lineNumber;
                    //    p1.reportEndLine = alignment1[i].lineNumber;
                    //    f2.copiedFrom.Add(p1);
                    //}

                    //indexList = new List<int>();
                    //indexList.Add(0);
                    //indexList.AddRange(alignment2.Where(x => x.val.Equals("") && x.lineNumber == -1).Select(x => alignment2.IndexOf(x)).ToList());
                    //indexList.Add(alignment1.Count);


                    //for (int i = 1; i < indexList.Count; i++)
                    //{
                    //    p1 = new Plag();
                    //    p1.reportStartLine = alignment2[i - 1].lineNumber;
                    //    p1.reportEndLine = alignment2[i].lineNumber;
                    //    f1.copiedFrom.Add(p1);
                    //}

                    matchedTokens = 0;
                    unmatchedTokens = 0;
                    foreach (Token t in alignment1)
                        if (!(t.val.Equals("") && t.lineNumber == -1))
                            matchedTokens++;
                        else
                            unmatchedTokens++;


                    count = 0;
                    if (l.Count < j.Count)
                        count = l.Count;
                    else
                        count = j.Count;


                    //if (!(unmatchedTokens>20))
                    //if((double) (amount) / (double)(count)>0.7 || (matchedTokens == l.Count && matchedTokens == j.Count))
                    //if ((2*(double)(amount)) / ((double)(l.Count) + (double)(j.Count)) > 0.70 || (matchedTokens == l.Count && matchedTokens == j.Count)) //75% match
                    //if (eValue(amount, l.Count, j.Count) < Math.Pow(10, -30))//always match identical methods
                    if ((double)(amount) / (double)(count) > 0.70 && count > 50 ||
                        amount == Math.Max(l.Count, j.Count) && count > 20)
                    {
                        using (var e1 = alignment1.GetEnumerator())
                        using (var e2 = alignment2.GetEnumerator())
                        {
                            int curLine1 = 0;
                            int curLine2 = 0;
                            
                            while (e1.MoveNext() && e2.MoveNext())
                            {
                                
                                Token t = e1.Current;
                                Token k = e2.Current;
                                
                                if ((t.val.Equals("") && t.lineNumber == -1) || (k.val.Equals("") && k.lineNumber == -1))
                                    continue;

                                Plag p1 = null;
                                Plag p2 = null;
                             
                                if (t.val.Equals(k.val))
                                {
                                    //curLine1 != t.lineNumber && curLine2 != k.lineNumber
                                    if (curLine1 != t.lineNumber && curLine2 != k.lineNumber)
                                    //if(true)
                                    {
                                        p1 = new Plag();

                                        p1.reportStartLine = t.lineNumber;
                                        p1.reportEndLine = t.lineNumber;
                                        p1.fromStart = k.lineNumber;
                                        p1.fromEnd = k.lineNumber;

                                        curLine1 = t.lineNumber;

                                        p2 = new Plag();

                                        p2.reportStartLine = k.lineNumber;
                                        p2.reportEndLine = k.lineNumber;
                                        p2.fromStart = t.lineNumber;
                                        p2.fromEnd = t.lineNumber;

                                        curLine2 = k.lineNumber;
                                    }

                                }

                                if(p1 != null)
                                {
                                    f2.copiedFrom.Add(p1);
                                    Plag tmp = new Plag(true);
                                    tmp.fromStart = secondStart;
                                    tmp.fromEnd = secondEnd;
                                    tmp.reportEndLine = firstEnd;
                                    tmp.reportStartLine = firstStart;
                                    f2.copiedFrom.Add(tmp);
                                }
                                    
                                if(p2 != null)
                                {
                                    f1.copiedFrom.Add(p2);
                                    
                                    Plag tmp = new Plag(true);
                                    tmp.fromStart = firstStart;
                                    tmp.fromEnd = firstEnd;
                                    tmp.reportEndLine = secondEnd;
                                    tmp.reportStartLine = secondStart;
                                    f1.copiedFrom.Add(tmp);
                                }

                                
                            }
                        }

                        if (!markedMethods.ContainsKey(firstStart))
                            markedMethods.Add(firstStart, firstEnd);
                        if (!markedMethodsSecond.ContainsKey(secondStart))
                            markedMethodsSecond.Add(secondStart, secondEnd);
                    }
                    
                    if (!f1.copiedFrom.Any())       // functionality down the line requires copiedFrom to not be empty
                        f1.copiedFrom.Add(new Plag());
                    if (!f2.copiedFrom.Any())
                        f2.copiedFrom.Add(new Plag());
                    

                }
            }

            f2.copiedFrom = f2.copiedFrom.OrderBy(x => x.isMethod == false).ToList();
            f1.copiedFrom = f1.copiedFrom.OrderBy(x => x.isMethod == false).ToList();

            if(f2.copiedFrom.Count > 0)
                r1.froms.Add(f2);
            
            
            if(f1.copiedFrom.Count > 0)
                r2.froms.Add(f1);

            return (r1,r2);
        }

        private bool similarSize(int firstSize,int secondSize)//checks if two blocks of code are similar size +/- 30 %
        {
            double percent = (double)firstSize *.3; //30 % of the size

            if (percent + (double)firstSize >= (double)secondSize && percent - (double)firstSize <= (double)secondSize)
                return true;
            else
                return false;
        }

        private int LCS(List<Token> l1, List<Token> l2)
        {
            int[,] c = new int[l1.Count, l2.Count];
            for (int i = 0; i < l1.Count; i++)
            {
                c[i, 0] = 0;
            }
            for (int i = 0; i < l2.Count; i++)
            {
                c[0, i] = 0;
            }

            for (int i = 1; i < l1.Count; i++)
            {
                for (int j = 1; j < l2.Count; j++)
                {
                    Token x = (Token)l1[i];
                    Token y = (Token)l2[j];
                    if (x.val.Equals(y.val))//&& x.parent.Equals(y.parent)
                        c[i, j] = c[i - 1, j - 1] + 1;
                    else
                        c[i, j] = Math.Max(c[i, j - 1], c[i - 1, j]);
                }
            }
            return c[l1.Count - 1, l2.Count - 1];
        }

        private double bitScore(int rawScore)
        {
            double lambda = 1.37;
            double K = 0.711;
            //double lambda = 0.239;
            //double K = 0.027;
            double result = ((lambda * rawScore) - Math.Log(K)) / Math.Log(2);
            return result;
        }

        private double eValue(int rawScore, int m, int n)
        {
            double aBitScore = bitScore(rawScore);
            double result = m * n * Math.Pow(2, -aBitScore);
            return result;
        }

        private void GST(List<Token> l1, List<Token> l2,int minmatch)
        {
            int maxmatch = 0;
            do
            {
                maxmatch = minmatch;

                for (int x = 0;x<l1.Count-maxmatch;x++)
                {
                    if (l1[x].marked)
                        continue;

                    for (int i = 0; i < l2.Count; i++)
                    {
                        if (l2[i].marked)
                            continue;

                        int j;
                        for (j = maxmatch - 1; j >= 0; j--)
                        {
                            if (!(l1[x + j].val.Equals(l2[i + j].val)) || l1[x + j].marked || l2[i + j].marked)
                            {
                                break;
                            }
                        }

                        j = maxmatch;
                        while(!(l1[x + j].val.Equals(l2[i + j].val)) && l1[x + j].marked && l2[i + j].marked)
                        {
                            j++;
                        }

                        if (j > maxmatch)
                            maxmatch = j;

                        //add match here
                    }

                }

                //for all matches mark each token

          
            } while (maxmatch != minmatch);
        }

        private (int, List<Token>, List<Token>) NW(List<Token> l1, List<Token> l2)
        {
            int[,] c = new int[l1.Count, l2.Count];
            int d = -2;
            int match = 1;
            int mismatch = 0;

            for (int i = 0; i < l1.Count; i++)
            {
                c[i, 0] = i * d;
            }
            for (int i = 0; i < l2.Count; i++)
            {
                c[0, i] = i * d;
            }

            for (int i = 1; i < l1.Count; i++)
            {
                for (int j = 1; j < l2.Count; j++)
                {
                    Token x = (Token)l1[i];
                    Token y = (Token)l2[j];
                    int matchScore;
                    int del;
                    int ins;
                    if (x.val.Equals(y.val))//&& x.parent.Equals(y.parent)
                        matchScore = c[i - 1, j - 1] + match;
                    else
                        matchScore = c[i - 1, j - 1] + mismatch;
                    del = c[i - 1, j] + d;
                    ins = c[i, j - 1] + d;

                    c[i, j] = Math.Max(matchScore, Math.Max(del, ins));
                }
            }
            var output = traceBack(c, l1.Count - 1, l2.Count - 1, match, mismatch, d, l1, l2);
            return (c[l1.Count-1, l2.Count - 1], output.Item1, output.Item2);
        }
        /// <summary>
        /// Smith waterman algorithm for generating the values matrix
        /// </summary>
        /// <param name="l1">the first token stream to be compared</param>
        /// <param name="l2">the second token stream</param>
        /// <returns>the alignments for each input token stream</returns>
        private (int, List<Token>, List<Token>) SW(List<Token> l1, List<Token> l2)
        {
            int[,] c = new int[l1.Count + 1, l2.Count + 1];
            int match = 1;
            int mismatch = 0;
            int d = -2;
            bool isIns = false;
            bool isDel = false;
            int insCount = 0;
            int delCount = 0;

            for (int i = 0; i < l1.Count; i++)
            {
                c[i, 0] = 0;
            }
            for (int i = 0; i < l2.Count; i++)
            {
                c[0, i] = 0;
            }

            int maxi = -1;
            int maxj = -1;
            int maxval = -1;
            for (int i = 1; i <= l1.Count; i++)
            {
                for (int j = 1; j <= l2.Count; j++)
                {
                    Token x = (Token)l1[i - 1];
                    Token y = (Token)l2[j - 1];
                    int matchScore;
                    int del;
                    int ins;
                    if (x.val.Equals(y.val))//&& x.parent.Equals(y.parent)
                        matchScore = c[i - 1, j - 1] + match;
                    else
                        matchScore = c[i - 1, j - 1] + mismatch;

                    del = c[i - 1, j] - (int)Math.Round(Math.Pow(2.0, -(0.25 * delCount - 1)), 0);

                    ins = c[i, j - 1] - (int)Math.Round(Math.Pow(2.0, -(0.25 * insCount - 1)), 0);

                    if (matchScore >= ins & matchScore >= del)
                    {
                        isDel = false;
                        isIns = false;
                        c[i, j] = matchScore;
                        insCount = 0;
                        delCount = 0;
                    }
                    else if (del >= matchScore & del >= ins)
                    {
                        isDel = true;
                        isIns = false;
                        c[i, j] = del;
                        insCount = 0;
                        delCount++;
                    }
                    else if (ins >= matchScore & ins >= del)
                    {
                        isDel = false;
                        isIns = true;
                        c[i, j] = ins;
                        insCount++;
                        delCount = 0;
                    }

                    //] = Math.Max(matchScore, Math.Max(del, ins));
                    if (c[i, j] > maxval)
                    {
                        maxi = i;
                        maxj = j;
                        maxval = c[i, j];
                    }

                }
            }
            var output = traceBack(c, maxi, maxj, match, mismatch, d, l1, l2);
            return (c[maxi, maxj], output.Item1, output.Item2);
        }
        /// <summary>
        /// Algorithm used to get the alignment sequences between two token streams
        /// </summary>
        /// <param name="c">the values matrix</param>
        /// <param name="i">the x coord of the highest value in c</param>
        /// <param name="j">the y coord of the highest value in c</param>
        /// <param name="match">the score for a match</param>
        /// <param name="mismatch">the score for a mismatch</param>
        /// <param name="gap">the score for a gap</param>
        /// <param name="l1">the first token stream being compared</param>
        /// <param name="l2">the second token stream being compared</param>
        /// <returns>the two alignments for the respective token streams</returns>
        private (List<Token>,List<Token>) traceBack(int[,] c, int i, int j, int match, int mismatch, int gap, List<Token> l1, List<Token> l2)
        {
            List<Token> AlignmentA = new List<Token>();
            List<Token> AlignmentB = new List<Token>();
            int m = i;
            int n = j;
            while (m > 0 && n > 0)
            {
                int scroeDiag = 0;

                //Remembering that the scoring scheme is +2 for a match, -1 for a mismatch and -2 for a gap (NW)
                if (l1[m - 1].val.Equals(l2[n - 1].val))
                    scroeDiag = match;
                else
                    scroeDiag = mismatch;

                if (m > 0 && n > 0 && c[m, n] == c[m - 1, n - 1] + scroeDiag)
                {
                    AlignmentA.Add(l2[n - 1]);
                    AlignmentB.Add(l1[m - 1]);
                    m = m - 1;
                    n = n - 1;
                }
                else if (n > 0 && c[m, n] == c[m, n - 1] + gap)
                {
                    AlignmentA.Add(l2[n - 1]);
                    AlignmentB.Add(new Token());
                    n = n - 1;
                }
                else //if (m > 0 && scoringMatrix[m, n] == scoringMatrix[m - 1, n] + -2)
                {
                    AlignmentA.Add(new Token());
                    AlignmentB.Add(l1[m - 1]);
                    m = m - 1;
                }
            }
            AlignmentA.Reverse();
            AlignmentB.Reverse();
            return (AlignmentA, AlignmentB);
        }

        /// <summary>
        /// gets the total number of lines copied from one report
        /// </summary>
        /// <param name="r">the submission report to get the number of lines copied</param>
        public void getpercent(Report r)
        {
            foreach(From f in r.froms)
            {
                int totalLines = 0;
                foreach (Plag p in f.copiedFrom)
                {
                    for (int i = p.fromStart; i <= p.fromEnd; i++)
                        totalLines++;

                }
                f.linesCopied = totalLines;
            }
        }
        /// <summary>
        /// sets a token stream to the exempt token stream
        /// </summary>
        /// <param name="t">The token stream that is exempt</param>
        public void setExempt(List<List<Token>> t)
        {
            if (t.First().First().fromFile == exempt)
                exemptStreams = t;
        }
        /// <summary>
        /// Removes any methods that are matched to any exempt code
        /// </summary>
        /// <param name="t">The token streams of a submission</param>
        public void removeExempt(List<List<Token>> t)
        {
            int amount;
            List<Token> alignmentA;
            List<Token> alignmentB;
            bool remove;
            for(int i=0;i<t.Count;i++)   
            {
                List<Token> f = t[i];
                remove = false;
                foreach(List<Token> k in exemptStreams)
                {
                    var output = NW(k, f);
                    amount = output.Item1;
                    alignmentA = output.Item2;
                    alignmentB = output.Item3;

                    if(amount == k.Count-1)
                    {
                        remove = true;
                    }
                    
                }
                if (remove)
                    t.RemoveAt(i);
            }
        }

    }
}
