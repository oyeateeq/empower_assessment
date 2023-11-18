using empower_assessment_app;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AzureConfig
{
    public string SubscriptionId { get; set; }
    public string TenantId { get; set; }
    public string ApplicationID { get; set; }
    public string AuthenticationKey { get; set; }
    public string ResourceGroup { get; set; }
    public string DataFactoryName { get; set; }
    public string PipelineName { get; set; }
}
class Program
{

    static async Task Main()
    {
        Console.WriteLine("Please select from the following options:");
        Console.WriteLine("1. To run the ADF pipeline");
        Console.WriteLine("2. To search through Azure Cognitive Service");
        try
        {
            string userInput = Console.ReadLine();
            switch (userInput)
            {
                case "1":
                    runAdfPipeline();
                    break;
                case "2":
                    await SearchCognitiveService();
                    break;
                default:
                    Console.WriteLine("Invalid option. Exiting...");
                    break;

            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"An Error Occured: {ex.ToString()}");
            LogException(ex);
        }
    }
    static void runAdfPipeline()
    {
        Console.WriteLine("Please select from following..");
        Console.WriteLine("1 To load all tables.");
        Console.WriteLine("2 To load specific table.");
        string userInput = Console.ReadLine();
        string[] allTables = { "products", "categories", "orders" };
        string[] tableKeys = { "product_id", "category_id", "order_id" };
        
        switch (userInput)
        {
            case "1":
                for(int i = 0; i< allTables.Length; i++)
                {
                    Console.WriteLine("Loading Table.");
                    Console.WriteLine(allTables[i]);
                    Console.WriteLine(tableKeys[i]);
                    callPipeline(allTables[i], tableKeys[i]);
                }
                break;
            case "2":
                Console.WriteLine("Enter Table Name To Load.");
                string TableName = Console.ReadLine();
                string tableKey = "";
                if(TableName.ToLower() == "products")
                {
                    tableKey = "product_id";
                }
                else if (TableName.ToLower() == "categories")
                {
                    tableKey = "category_id";
                }
                else if (TableName.ToLower() == "orders")
                {
                    tableKey = "order_id";
                }
                else if(TableName.ToLower() == "orderproducts")
                {
                    tableKey = "";
                }
                callPipeline(TableName, tableKey);
                break;
            default:
                Console.WriteLine("Invalid Option. Exiting...");
                break;
        }

    }
    static void callPipeline(string TableName, string tableKey)
    {
        try
        {
            var configurationLoader = new ConfigurationLoader();
            IConfigurationRoot configuration = configurationLoader.LoadConfiguration();
            var azureConfig = configuration.GetSection("AzureConfig").Get<AzureConfig>();
            AzureConnector azureConnector = new AzureConnector(azureConfig.ApplicationID, azureConfig.AuthenticationKey, azureConfig.TenantId);
            ServiceClientCredentials credentials = azureConnector.GetCredentials();

            string subscriptionId = azureConfig.SubscriptionId;
            string resourceGroup = azureConfig.ResourceGroup;
            string dataFactoryName = azureConfig.DataFactoryName;
            string pipelineName = "full_load";
            Console.WriteLine("Loading Table: " + TableName);
            string[] keyCols;
            if (TableName == "orderproducts")
            {
                keyCols = new string[] { "order_id", "product_id" };
            }
            else {
                keyCols = new string[] { tableKey };
            }

            PipelineRunner pipelineRunner = new PipelineRunner(credentials, subscriptionId);
            MonitorAdfRun monitorAdfRun = new MonitorAdfRun(credentials, subscriptionId);
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "source_table_name", TableName },
                { "target_table_name", TableName },
                { "key_column_name", keyCols }
            };

            string runId = pipelineRunner.RunPipeline(resourceGroup, dataFactoryName, pipelineName, parameters);
            monitorAdfRun.getStatus(resourceGroup, dataFactoryName, runId);
            Console.WriteLine("Run ID: " + runId);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            LogException(ex);
        }
    }
    static async Task SearchCognitiveService()
    {
        string indexName = "azuresql-index";
        string apiKey = "hbIMVcnvPRRd2V9vekSl34bwEiewbFXfvxdiozOvhNAzSeDBSsDD";

        Console.Write("Enter your search query: ");
        string searchText = Console.ReadLine();

        SearchServiceClient serviceClient = new SearchServiceClient("assessment-cog-search", new SearchCredentials(apiKey));
        ISearchIndexClient indexClient = serviceClient.Indexes.GetClient(indexName);

        SearchParameters searchParameters = new SearchParameters();
        searchParameters.HighlightFields = new List<string> { "product_name" };
        try
        {
            var result = await indexClient.Documents.SearchAsync(searchText, searchParameters);

            foreach (var data in result.Results)
            {
                Console.WriteLine("Product Name: " + data.Document["product_name"].ToString());
                Console.WriteLine("Price: " + data.Document["price"].ToString());
                Console.WriteLine("Description: " + data.Document["description"].ToString());
                Console.WriteLine("Image URL: " + data.Document["image_url"].ToString());
                Console.WriteLine("Date Addedd: " + data.Document["date_added"].ToString());
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            LogException(ex);
        }
    }
    static void LogException(Exception ex)
    {
        string logFilePath =  "error_logs.txt";
        //string logFilePath = logPath; 

        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"[{DateTime.Now}] Exception: {ex.Message}");
                writer.WriteLine($"StackTrace: {ex.StackTrace}");
                writer.WriteLine(new string('-', 40));
            }
        }
        catch (Exception logEx)
        {
            Console.WriteLine($"Failed to write to the log file: {logEx.Message}");
        }
    }
}
