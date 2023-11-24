# empower_assessment

## Overview

This C# console application provides functionality to interact with Azure services, specifically Azure Data Factory (ADF) and Azure SQL with Azure Cognitive Search.
## Features

1. **Run Azure Data Factory Pipeline:**
   - The application allows you to initiate and monitor Azure Data Factory (ADF) pipelines.

2. **Monitor ADF Job:**
   - It provides features to monitor the progress and status of jobs executed by Azure Data Factory.

3. **Search Data in Azure SQL using Cognitive Search:**
   - Utilizing Azure Cognitive Search, the application facilitates enhanced searching capabilities for data stored in Azure SQL.
## Prerequisites

- .NET Core SDK [Download](https://dotnet.microsoft.com/download)
- Azure subscription with configured Data Factory and SQL Database.

## Usage

1. **Clone the Repository:**
    ```bash
    git clone https://github.com/oyeateeq/empower_assessment.git
    ```
   
2. **Configure Azure Credentials:**
   - Open `appsettings.json` and update the Azure configuration settings such as SubscriptionId, TenantId, ApplicationID, AuthenticationKey, etc.

3. **Build and Run:**
    ```bash
    cd your-repository
    dotnet build
    dotnet run
    ```

4. **Select Operation:**
    - Upon running the application, you'll be prompted to choose an operation:
      - Type `1` to run an Azure Data Factory pipeline.
      - Type `2` to search data in Azure SQL using Cognitive Search.

5. **Follow On-Screen Instructions:**
   - Depending on the selected operation, the application will guide you through the necessary steps.

