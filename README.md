# Demo app for custom instrumentation for ASP.NET Core agent 


## This is a simple demo app to show how custom attributes can be used to manually instrument attributes that are otherwise not available right out of the box when instrumenting a .NET application with New Relic.


## This demo aims to help solve the following:

1. Add details to existing transactions with Trace, per the documentation: https://docs.newrelic.com/docs/apm/agents/net-agent/custom-instrumentation/custom-instrumentation-attributes-net/

2. Troubleshoot attribute-based custom instrumentation issues, per the documentation: https://forum.newrelic.com/s/hubtopic/aAX8W0000008a5gWAA/relic-solution-troubleshooting-attributebased-custom-instrumentation-issues

3. Use the New Relic .NET agent API to create custom attributes and transaction-based attributes such as userId that can be tied to a transaction. Documentation: https://docs.newrelic.com/docs/apm/agents/net-agent/net-agent-api/guide-using-net-agent-api/ and https://docs.newrelic.com/docs/data-apis/custom-data/custom-events/collect-custom-attributes/

4. See transaction attributes in Distributed Tracing: Documentation: https://docs.newrelic.com/docs/data-apis/custom-data/custom-events/collect-custom-attributes/


## In order to run this demo application, the following steps are necessary:

1. Install the .NET CLI via the .NET SDK (.NET 7+) on your machine or environment. Documentation: https://learn.microsoft.com/en-us/dotnet/core/sdk

2. Install Postgresql (11+) on your environment. Documentation: https://www.postgresql.org/download/

3. Clone this repo in your project directory.

4. Install .NET Entity Framework (EF) in your project directory. Documentation: https://learn.microsoft.com/en-us/ef/core/get-started/overview/install

5. Run migrations using the .NET commands to create the necessary database tables from the Identity, Movie, and CartItems models. Documentation: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

6. Import the Movie.csv data into the Movie table to populate the data in the table. 


For questions and concerns, please email: astfort@newrelic.com. 
