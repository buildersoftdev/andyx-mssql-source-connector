<p align="center">
  <img src="https://user-images.githubusercontent.com/10813728/202870468-20d05267-a8e6-4f93-a579-f249b8bd1268.png" alt="Sublime's custom image"/>
</p>

What is Andy X MSSQL Connector?
============

Andy X is an open-source distributed streaming platform designed to deliver the best performance possible for high-performance data pipelines, streaming analytics, streaming between microservices and data integrations. Andy X MSSQL Source Connector is an open source distributed platform for change data capture. Start it up, point it at your databases, and your apps can start responding to all of the inserts, updates, and deletes that other apps commit to your databases. Andy X MSSQL Source Connector is durable and fast, so your apps can respond quickly and never miss an event, even when things go wrong.

<b>Andy X MSSQL Connector works only with version 3 or later of Andy X.</b>

## Get Started

Follow the [Getting Started](https://andyx.azurewebsites.net/) instructions how to run Andy X.

For local development and testing, you can run Andy X within a Docker container, for more info click [here](https://hub.docker.com/u/buildersoftdev)

After you run Andy X you will have to configure Andy X MSSQL Source Connector. Configuring this adapter is quite easy only to json files need to be configured <b>dbengine_config.json</b> and <b>andyx_config.json</b>
> <b>dbengine_config.json</b> - SQL Connection Configuration file, it's the file where we specify tables which this adapter will check the changes and produce them.

> <b>andyx_config.json</b> - Andy X Configuration File, it's the file where we specify the connection with Andy X.

### Database Engine Configuration File
Below is an example of configuration file, this file should be saved into config directory of Andy X MSSQL Source Connector before running this service.

```json
	{
	  "Servers": [
	    {
	      "Name": "MSSQL",
	      "ConnectionString": "Data Source=localhost;Initial Catalog={databaseName/master};Integrated Security=False;User Id=sa;Password=YourStrong!Passw0rd;MultipleActiveResultSets=True",
	      "Databases": [
	        {
	          "Name": "{databaseName}",
	          "Tables": [
	            {
	              "Name": "{tableName}",
	              "IncludeOldVersion": true or false,
	              "Topic": {topicName}
	            },
	            {
	              "Name": "{tableName}",
	              "IncludeOldVersion": true or false,
	              "Topic": {topicName}
	            }
	          ]
	        }
	      ]
	    }
	  ]
	}
```

EngineType accepts only MSSQL, Oracle and PostgreSQL for now

### Andy X Configuration File
Below is an example of Andy X configuration file, this file should be saved into config directory of Andy X MSSQL Source Connector before running this service.
``` json
	{
		"ServiceUrls": ["http://localhost:9000"],
		"Tenant": "{tenantName}",
		"Product": "{productName}",
		"Component": "{componentName}"
	}
```

## How to Engage, Contribute, and Give Feedback

Some of the best ways to contribute are to try things out, file issues, join in design conversations,
and make pull-requests.

## Reporting security issues and bugs

Security issues and bugs should be reported privately, via email, suppoer@buildersoft.io. You should receive a response within 24 hours.

## Related projects

These are some other repos for related projects:

* [Andy X](https://github.com/buildersoftdev/andyx) - Andy X distributed streaming platform
* [Andy X Cli](https://github.com/buildersoftdev/andyx-cli) - Manage all resources of Andy X

## Deploying Andy X MSSQL Source Connector with docker-compose

Andy X MSSQL Source Connector can be easily deployed on a docker container using docker-compose, for more info click [here](https://hub.docker.com/r/buildersoftdev/andyx-mssql-source-connector)
``` yaml
    version: '3.4'
    
    services:
        andyx-mssql-source-connector:
        container_name: andyx-mssql-source-connector
        image: buildersoftdev/andyx-mssql-source-connector:3.0.0-preview1
    
        volumes:
            - ./tables_config.json:/app/config/tables_config.json
            - ./andyx_config.json:/app/config/andyx_config.json
```
Network configuration using docker-compose is not needed if only Andy X MSSQL Source Connector is deployed. Network should be configured if this adapter will be deployed together with Andy X and Andy X Storage.

Below is an example of deploying Andy X, Andy X Storage, Andy X MSSQL Source Connector and Microsoft SQL Server, if you have problems deploying Andy X via docker-compose please click [here](https://hub.docker.com/r/buildersoftdev/andyx).

```yaml
version: '3.4'

services:
	
	
	andyx-mssql-source-connector:
		container_name: andyx-andyx-mssql-source-connector
		image: buildersoftdev/andyx-mssql-source-connector:3.0.0-preview1
		volumes:
            # -- In the same folder with docker-compose should be these two files, before running docker-compose. 
			- ./tables_config.json:/app/config/tables_config.json
			- ./andyx_config.json:/app/config/andyx_config.json
		networks:
			- local
	
# ----------------------------------------------------------------------------------------------------
		
    andyx-portal:
      container_name: andyx-portal
      image: buildersoftdev/andyx-portal:v3.0.0
      ports:
        - 9000:80
      environment:
        - XNode:ServiceUrl=http://andyxnode:6540
      networks:
        - local

# ------------------------------------------------------------------------------------------------

    andyx:
      container_name: andyxnode
      image: buildersoftdev/andyx:3.0.0
      ports:
        - 6540:6540
      environment:
        - ANDYX_ENVIRONMENT=Development
        - ANDYX_URLS=http://andyxnode:6540
      volumes:
        - ./andyx-data:/app/data
      networks:
        - local
	
	# ----------------------------------------------------------------------------------------------------
			
	sql-server:
		image: mcr.microsoft.com/mssql/server
		hostname: sql-server
		container_name: sql-server
		ports:
			- "1433:1433"
		environment:
			- ACCEPT_EULA=Y
			- MSSQL_SA_PASSWORD=YourStrong!Passw0rd
			- MSSQL_PID=Express
		networks:
			- local
			
	# ----------------------------------------------------------------------------------------------------
	
	networks:
		local:
		driver: bridge
```
To run Andy X MSSQL Source Connector with docker-compse you should execute 

    docker-compose up -d

## Code of conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.

For more information, see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## Support
Let's do it together! You can support us by clicking on the link below!

[![alt text](https://img.buymeacoffee.com/api/?url=aHR0cHM6Ly9pbWcuYnV5bWVhY29mZmVlLmNvbS9hcGkvP3VybD1hSFIwY0hNNkx5OWpaRzR1WW5WNWJXVmhZMjltWm1WbExtTnZiUzkxY0d4dllXUnpMM0J5YjJacGJHVmZjR2xqZEhWeVpYTXZNakF5TVM4d09DOWxObVUwTkRWaU1UVXhPVGRqWm1JNFlXWTVZalV5TWpjek5qSXlaV05rTnk1d2JtYz0mc2l6ZT0zMDAmbmFtZT1BbmR5K1g=&creator=Andy+X&is_creating=free%20and%20open%20source%20Distributed%20Streaming%20Platform&design_code=1&design_color=%2379D6B5&slug=buildersoft)](https://www.buymeacoffee.com/buildersoft).
