using Plagiarism_Engine.Models;
using Plagiarism_Engine.Service;
using Plagiarism_Engine_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Plagiarism_Engine.Services
{
	public class RunComparisonService
	{
		public MessageModel message { get; set; }//model that is being passed in
		public string result { get; set; }//the result to return back to the Prof API

        /// <summary>
        /// Function app call for running the comparison. Is the same as Plagiarism_Engine.Controllers.CompareController, but gets the list of files from the message.
        /// </summary>
		public void ProcessMessage()
		{
			CompareService service = new CompareService();
			List<Report> ret;
			var files = message.Files;

			List<Node> roots = service.createASTsForAllUsers(files);
			List<List<Token>> tok = new List<List<Token>>();
			List<List<List<Token>>> splitTokens = new List<List<List<Token>>>();    //a list of each users token streams seperated into a list of methods which has a list of tokens
			ret = new List<Report>();

			//if (!ApiKeyService.isValid(apiKey)) //if the api key is bad
				//return null;

			for (int i = 0; i < files.Count; i++) //creates a report for each user
			{
				if (files[i].isExempt)
					continue;
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
            if (service.exempt != -2)
            foreach (List<List<Token>> l in splitTokens)
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


			result = Newtonsoft.Json.JsonConvert.SerializeObject(ret);
			//return Newtonsoft.Json.JsonConvert.SerializeObject(ret);



		}

	}
}
