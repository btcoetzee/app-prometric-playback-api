# Api Documentation and Metrics

- [Api Docs](http://localhost:5000/docs)
- [Metrics](http://localhost:5000/metrics)

# Introduction
The Hello World app is a reference app. It core business is a Bookstore. This app allows you to manage the inventory of a book store. This app is a work in progress and it is not complete yet.

# Prerequisites
- [Docker](https://www.docker.com/)
- [.NET Core SDK (3.1 or above)](https://docs.microsoft.com/en-us/dotnet/core/install/sdk)

# Build and Test

## Adding Redis
Before starting, run an instance of Redis!

1. Add a redis section to your `appsettings.{env}.json`.

Use the corresponding environment name to replace the `{env}` segment.

For example, `appsettings.dev.json`. For local development, just add the snippet to `appsettings.json`.

```
  "redis": {
    "connectionString": "localhost",
    "instance": "books:"
  }
```

2. The next step is to add Redis to the .NET Core Dependency Injection engine (***Note: This should already be done in lates code within trunk branch**).
    Locate in Program.cs the method `ConfigureServices()` and find an extension method called `.AddInfrastructure()`.
    Edit that file and add `.AddRedis()` to the list of services. Voilá! You get Redis! Read the documentation on Azure on how to use the `DistributeCache` client for more information.

3. Run the following to spin up the Redis container in a detached state

``` docker run -d -p 6379:6379 --name redis1 redis```

## Building and running the api in docker.

First, build the docker image and tag it as 'hello-world-api'

``` docker build -t hello-world-api:latest . ```

Next, run the docker image 'hello-world-api' latest,
    removing the container after it exits,
    in an interactive terminal,
    mapping port 80 of the container to port 5000 on the host.

``` docker run --rm -it -p 5000:80 hello-world-api:latest ```

To run component and unit tests locally:
``` dotnet test /src ```
