using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Plagiarism_Engine_Models;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plagiarism_Engine_Function
{
    public static class Compare
    {
        /// <summary>
        /// Service bus trigger for running the comparison system. This is the function app handler. Makes a call back to the profesor API to return the response of the Comparison.
        /// </summary>
        /// <param name="myQueueItem">the message that contains all of the files. This is a MessageModel</param>
        /// <param name="log">object that creates a log for this call</param>
		[FunctionName("Compare")]
		public static void Run([ServiceBusTrigger("incomingcomparisons", Connection = "SBConnection")]Message myQueueItem, ILogger log)
		{
			string queueItem = Encoding.UTF8.GetString(myQueueItem.Body);


			const string ServiceBusConnectionString = "Endpoint=sb://floss-engine-dev.servicebus.windows.net/;SharedAccessKeyName=engine;SharedAccessKey=zNs1q1NQ6+7nDGK85Ifq5KFkIpVitQYCgiBk35N/Pf4=;";

			IMessageReceiver messageReceiver = new MessageReceiver(ServiceBusConnectionString, "incomingcomparisons", ReceiveMode.PeekLock);

			log.LogInformation($"C# ServiceBus queue trigger function processed message: {queueItem}");

			//string storageConnection = _config.GetSection("AppSettings").GetValue<string>("BlobStorageConnectionString");
			string storageConnection = "DefaultEndpointsProtocol=https;AccountName=flossdev;AccountKey=EKT2bsPXFXLrBZjmBg4HxvCKATTSOPAZneYvERAwbRWiaoCL8ZKjAsapClaOQP5Wid6ny2vopu8iLTUjsV7pGw==;EndpointSuffix=core.windows.net";
			CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

			CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

			CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("appcontainer");

			//create a container if it is not already exists
			if (cloudBlobContainer.CreateIfNotExistsAsync().Result)
			{

				cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }).Wait();

			}

			//get Blob reference

			CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(queueItem);
			var messageModelJson = cloudBlockBlob.DownloadTextAsync().Result; // read file as string

			//cloudBlockBlob.DeleteIfExistsAsync().Wait(); // delete file after reading data



			MessageModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageModel>(messageModelJson);

			log.LogInformation($"C# ServiceBus queue trigger function read file from blob: {messageModelJson}");


			// Do the big call, that takes the big time
			Plagiarism_Engine.Services.RunComparisonService comparisonService = new Plagiarism_Engine.Services.RunComparisonService();
			comparisonService.message = model;
			Thread thread = new Thread(new ThreadStart(comparisonService.ProcessMessage));
			//BackgroundWorker bg = new BackgroundWorker();
			//bg.DoWork += new DoWorkEventHandler(Plagiarism_Engine.Services.RunComparisonService.ProcessMessage);

			var timer = System.Diagnostics.Stopwatch.StartNew();
			
			thread.Start();

			while (thread.IsAlive)
			{
				Thread.Sleep(1000);
				messageReceiver.RenewLockAsync(myQueueItem).Wait();
			}

			if (thread.ThreadState == ThreadState.Aborted)
			{
				throw new Exception($"The thread was aborted for reasons");
			}

			timer.Stop();
			var elapsedTimeSpan = timer.Elapsed;

			model.ReportString = comparisonService.result;

			string elapsedTimeString = $"{elapsedTimeSpan.Hours} hours {elapsedTimeSpan.Minutes} minutes and {elapsedTimeSpan.Seconds} seconds ({elapsedTimeSpan.ToString()})";

			log.LogInformation($"C# Comparison Completed in {elapsedTimeString}. Result: {Newtonsoft.Json.JsonConvert.SerializeObject(model)}");

			HttpClient client = new HttpClient();

			// call api to pass model back 
			var response = client.PostAsync(model.CallBackUrl,
				new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")).Result;

			if (response.IsSuccessStatusCode)
			{
				log.LogInformation($"C# Results successfully sent back to: {model.CallBackUrl}");
			}
			else
			{
				string errorMessage = response.Content.ReadAsStringAsync().Result;
				log.LogInformation($"C# An error occurred and the results could NOT be sent back to: {model.CallBackUrl}. Error message: {errorMessage}");
			}

		}




		//      [FunctionName("Compare")]
		////public static void Run([ServiceBusTrigger("incomingcomparisons", Connection = "SBConnection")]string myQueueItem, ILogger log)
		//public static void Run([ServiceBusTrigger("incomingcomparisons", Connection = "SBConnection")]Message myQueueItem, ILogger log)
		//{
		//	string queueItem = Encoding.UTF8.GetString(myQueueItem.Body);
		//	log.LogInformation($"C# ServiceBus queue trigger function processed message: {queueItem}");

		//	//string storageConnection = _config.GetSection("AppSettings").GetValue<string>("BlobStorageConnectionString");
		//	string storageConnection = "DefaultEndpointsProtocol=https;AccountName=flossdev;AccountKey=EKT2bsPXFXLrBZjmBg4HxvCKATTSOPAZneYvERAwbRWiaoCL8ZKjAsapClaOQP5Wid6ny2vopu8iLTUjsV7pGw==;EndpointSuffix=core.windows.net";
		//	CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

		//	CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

		//	CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("appcontainer");

		//	//create a container if it is not already exists
		//	if (cloudBlobContainer.CreateIfNotExistsAsync().Result)
		//	{

		//		cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }).Wait();

		//	}

		//	//get Blob reference

		//	CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(queueItem);
		//	var messageModelJson = cloudBlockBlob.DownloadTextAsync().Result; // read file as string

		//	cloudBlockBlob.DeleteIfExistsAsync().Wait(); // delete file after reading data lol



		//	MessageModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageModel>(messageModelJson);

		//	log.LogInformation($"C# ServiceBus queue trigger function read file from blob: {messageModelJson}");



		//	//Create a CTS to launch a task in charge of renewing the message lock
		//	var brokeredMessageRenewCancellationTokenSource = new CancellationTokenSource();

		//	try
		//	{
		//		var brokeredMessage = myQueueItem.Receive();

		//		var brokeredMessageRenew = Task.Factory.StartNew(() =>
		//		{
		//			while (!brokeredMessageRenewCancellationTokenSource.Token.IsCancellationRequested)
		//			{
		//				//Based on LockedUntilUtc property to determine if the lock expires soon
		//				if (DateTime.UtcNow > brokeredMessage.LockedUntilUtc.AddSeconds(-10))
		//				{
		//					// If so, we repeat the message
		//					brokeredMessage.RenewLock();
		//				}

		//				Thread.Sleep(500);
		//			}
		//		}, brokeredMessageRenewCancellationTokenSource.Token);

		//		// Do the big call, that takes the big time
		//		string results = Plagiarism_Engine.Services.RunComparisonService.ProcessMessage(model);

		//		model.ReportString = results;

		//		HttpClient client = new HttpClient();

		//		// call api to pass model back 
		//		var response = client.PostAsync(model.CallBackUrl,
		//			new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")).Result;

		//		if (response.IsSuccessStatusCode)
		//		{
		//			log.LogInformation($"C# Results successfully sent back to: {model.CallBackUrl}");
		//		}
		//		else
		//		{
		//			string errorMessage = response.Content.ReadAsStringAsync().Result;
		//			log.LogInformation($"C# An error occurred and the results could NOT be sent back to: {model.CallBackUrl}. Error message: {errorMessage}");
		//		}

		//		// We mark the message as completed
		//		brokeredMessage.Complete();
		//	}
		//	catch (MessageLockLostException)
		//	{
		//		// lock expired
		//	}
		//	catch (Exception)
		//	{
		//		brokeredMessage.Abandon();
		//	}
		//	finally
		//	{
		//		// Lock is stopped renewing task
		//		brokeredMessageRenewCancellationTokenSource.Cancel();
		//	}
		//}
	}
}
