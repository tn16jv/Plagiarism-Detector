using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;

namespace Floss.Api.Controllers
{
    /// <summary>
    /// Class for adding custom features to the swagger ui
    /// </summary>
    public class FileUploadController : IOperationFilter
    {

        /// <summary>
        /// Adds custom features to the swagger ui
        /// </summary>
        /// <remarks>
        /// This functions applies the options to add the upload a file button onto the swagger ui.
        /// </remarks>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.OperationId.ToLower() == "apiassignmentuploadassignmentpost")
            {
               
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Assignment",
                    In = "formData",
                    Description = "Upload Assignment",
                    Required = true,
                    Type = "file",
                });
                operation.Consumes.Add("multipart/form-data");
            } // for the function post UploadAssignment
            if (operation.OperationId.ToLower() == "apiassignmentsubmissiondownloadassignmentget")
            {
                operation.Produces = new[] { "application/octet-stream" };
                operation.Responses["200"].Schema = new Schema { Type = "file", Description = "Download file" };
            }
            if (operation.OperationId.ToLower() == "apiplagiarismtestuploadtestpost")
            {

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Assignment",
                    In = "formData",
                    Description = "Upload Assignment",
                    Required = true,
                    Type = "file",
                });
                operation.Consumes.Add("multipart/form-data");
            } // for the function post UploadAssignment
        }

    }
}