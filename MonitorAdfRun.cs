using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest;
using System;
using ConsoleTables;

namespace empower_assessment_app
{
    internal class MonitorAdfRun
    {
        private readonly DataFactoryManagementClient _client;
        public MonitorAdfRun(ServiceClientCredentials credentials, string subscriptionId)
        {
            _client = new DataFactoryManagementClient(credentials)
            {
                SubscriptionId = subscriptionId
            };
        }
        public void getStatus(string resourceGroup, string dataFactoryName, string runId)
        {
            PipelineRun pipelineRun;

                while (true)
                {
                    pipelineRun = _client.PipelineRuns.Get(
                        resourceGroup, dataFactoryName, runId
                        );
                    if (pipelineRun.Status == "InProgress" || pipelineRun.Status == "Queued")
                    {
                        Console.WriteLine("Pipeline Status: " + pipelineRun.Status + ". Please wait...");
                        System.Threading.Thread.Sleep(1500);
                    }
                    else
                        break;
                }
                RunFilterParameters filterParams = new RunFilterParameters(
                    DateTime.UtcNow.AddMinutes(-10), DateTime.UtcNow.AddMinutes(10));
                ActivityRunsQueryResponse queryResponse = _client.ActivityRuns.QueryByPipelineRun(
                        resourceGroup, dataFactoryName, runId, filterParams);
                if (pipelineRun.Status == "Succeeded")
                {
                    Console.WriteLine("Pipeline Run Successfull. Please find below the details..");
                    string jsonResponse = @$"{queryResponse.Value.First().Output}";
                    dynamic response = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
                    var table = new ConsoleTable("Metric", "Value");
                    table.AddRow("Data Read", response.dataRead);
                    table.AddRow("Data Written", response.dataWritten);
                    table.AddRow("Source Peak Connections", response.sourcePeakConnections);
                    table.AddRow("Sink Peak Connections", response.sinkPeakConnections);
                    table.AddRow("Rows Read", response.rowsRead);
                    table.AddRow("Rows Copied", response.rowsCopied);
                    table.AddRow("Copy Duration", response.copyDuration);
                    table.AddRow("Throughput", response.throughput);
                    table.Write(Format.MarkDown);
                }

                else
                {
                    Console.WriteLine("Pipeline Run Failed with following details....");
                    Console.WriteLine(queryResponse.Value.First().Error);
                }

        }
    }
}
