using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Rest;

public class PipelineRunner
{
    private readonly DataFactoryManagementClient _client;

    public PipelineRunner(ServiceClientCredentials credentials, string subscriptionId)
    {

            _client = new DataFactoryManagementClient(credentials)
            {
                SubscriptionId = subscriptionId
            };
    }
    public string RunPipeline(string resourceGroup, string dataFactoryName, string pipelineName, Dictionary<string, object> parameters)
    {
        string error = "";

            CreateRunResponse runResponse = _client.Pipelines
                .CreateRunWithHttpMessagesAsync(resourceGroup, dataFactoryName, pipelineName, parameters: parameters)
                .Result.Body;
            if(runResponse == null ) 
            {
                Console.WriteLine(runResponse);
            }
            return runResponse.RunId;

    }
}
