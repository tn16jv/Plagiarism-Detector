using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plagiarism_Engine.Models;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Plagiarism_Engine.Service;
using Plagiarism_Engine.Services;
using System.Security.Claims;
using Plagiarism_Engine_Models;

namespace Plagiarism_Engine.Controllers
{
    [Produces("application/json")]
    [Route("api/Compare")]
    public class CompareController : Controller
    {

        CompareService service;

        /// <summary>
        /// Constructor
        /// </summary>
        public CompareController()
        {
            service = new CompareService();
        }


        // post: api/Compare
        /// <summary>
        /// Runs a comparison on all the files listed. Will exclude any code that is flagged as exempt
        /// </summary>
        /// <param name="files"> A list of source code for each user</param>
        /// <param name="apiKey">The api key used to verify an appropritate sender</param>
        /// <returns>A list of Reports detailing where each user copied from</returns>
        [HttpPost]
        [Route("Compare")]
        public ActionResult postCompare([FromBody]List<File> files, string apiKey)
        {
            List<Report> ret;
            try
            {

                List<Node> roots = service.createASTsForAllUsers(files);
                List<List<Token>> tok = new List<List<Token>>();
                List<List<List<Token>>> splitTokens = new List<List<List<Token>>>();    //a list of each users token streams seperated into a list of methods which has a list of tokens
                ret = new List<Report>();


                if (!ApiKeyService.isValid(apiKey)) //if the api key is bad
                    return null;

                for (int i = 0; i < files.Count; i++) //creates a report for each user
                {
                    if (files[i].isExempt)
                    {
                        continue;
                    }
                        
                    Report r = new Report();
                    r.userReportId = files[i].Id;
                    ret.Add(r);
                }

                foreach (Node n in roots)//generate token streams
                {
                    List<Token> tmp = new List<Token>();
                    tmp = service.NodesToTokens(n, tmp);
                    tok.Add(tmp);
                }

                //split up token streams by method for each user
                splitTokens = service.splitTokenStreamByMethod(tok);
                if(service.exempt != -2)
                foreach(List<List<Token>> l in splitTokens)
                {
                    service.removeExempt(l);
                }
                

                //compare each submission to every other submission
                Parallel.For(0, splitTokens.Count,
                    index => {
                        for (int j = index + 1; j < splitTokens.Count; j++)
                        {
                            
                            Report r1 = ret[index];
                            Report r2 = ret[j];
                            var tmp = service.compare(splitTokens[index], r1, splitTokens[j], r2);
                        //r1 = tmp.Item1;
                        //r1 = tmp.Item2;
                        }
                });

                foreach (Report r in ret)
                    service.getpercent(r);

                return Ok(ret);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
                
            
        }

        
    }
}
