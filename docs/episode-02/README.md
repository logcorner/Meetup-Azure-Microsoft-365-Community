# Meetup-Azure-Microsoft-365-Community episode-02

During this journey, I will  talk about dockerization of web api and sql server linux database

So I will dockerize the web API and the SQL Server database : TodoList.WebApi and TodoList.Database.

# SQL SERVER LINUX DOCKERIZATION
From TodoList.Database project, open project properties, click on then Project Settings tab,  and then check Create script (.sql file)

Create script

it will create a script file for creating the database. Whenever the database is updated, this script will be updated.

Click on tab Build Events, create a Post-build event command as following :

xcopy "$(ProjectDir)bin\$(Configuration)\TodoList.Database_Create.sql" "$(ProjectDir)Docker\Restore" /Y

post_build_event

It will copy the sql script generated on previous step to a specfic directory (Docker/Restore on my case)

DockerFile will use this script to create the database image

script file

# The sql server linux Dockerfile

# pull microsoft/mssql-server-linux as base image
FROM mcr.microsoft.com/mssql/server:2019-latest

# add TodoList.Database_Create.sql to /home/resources/ and name it Restore.sql
ADD Restore/TodoList.Database_Create.sql /home/resources/init.sql

# add SA_PASSWORD as build-time variable
ARG SA_PASSWORD

# use that value to set the ENV variable
ENV SA_PASSWORD=${SA_PASSWORD}
ENV ACCEPT_EULA=Y

# run sqlservr, connect locally with sqlcmd and create Restore.sql script
RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "SQL Server is now ready for client connections" \
  && sleep 10 \
  && /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -i /home/resources/init.sql \
  && pkill sqlservr



Locate the directory where the SQL Server Dockerfile is located and run the following command to create database-image with SA_PASSWORD

docker build --build-arg SA_PASSWORD='MyC0m9l&xP@ssw0rd' -t workshop/database-image:1.0.0 .

List all images

docker images workshop/database-image


dockerimages-db

We have two addtionnal images : microsoft/mssql-server-linux and database-image

RUN CONTAINER
Run the following command to create a database container (database-container) based on database-image, mapped on port 1433 inside and outside the container


docker run  --name database-container -e "SA_PASSWORD=MyC0m9l&xP@ssw0rd"  -p 1433:1433 -d workshop/database-image:1.0.0


runcontainerdb

list running containers

docker ps -a

listcontainer-db

We have a new container based on database-image

Run the following command to attach shell on database-container

docker exec -it database-container "bash"

bashon sql

Run the following command to connect to sql server instance of the running container

/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "MyC0m9l&xP@ssw0rd"


Run the following command to list all databases,

select name from sys.databases

go


we can see that the database TodoList.Database  is created via the script on Dockerfile

Run the following command  to select  [dbo].[Todo]  table on that database

use [TodoList.Database]

go

select * from [dbo].[Todo]
go

select table inside container

# WEB API DOCKERIZATION
Right click on  TodoList.WebApi project name, and select ContainerOrchestration Support 

add docker

Choose Docker Compose and click OK

Container orchestration

The following Dockerfile will be generated

Dockerfile

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TodoList.WebApi/TodoList.WebApi.csproj", "TodoList.WebApi/"]
COPY ["TodoList.Domain/TodoList.Domain.csproj", "TodoList.Domain/"]
COPY ["TodoList.Application/TodoList.Application.csproj", "TodoList.Application/"]
COPY ["TodoList.Infrastructure/TodoList.Infrastructure.csproj", "TodoList.Infrastructure/"]
COPY ["TodoList.SharedKernel.Repository/TodoList.SharedKernel.Repository.csproj", "TodoList.SharedKernel.Repository/"]
RUN dotnet restore "TodoList.WebApi/TodoList.WebApi.csproj"
COPY . .
WORKDIR "/src/TodoList.WebApi"
RUN dotnet build "TodoList.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoList.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoList.WebApi.dll"]

BUILD IMAGES

for the purposes of this demonstration, I delete all the containers and images, do not run this commands if don’t want to delete all your images

# Stop all containers
docker stop $(docker ps -a -q)

# Delete all containers
docker rm $(docker ps -a -q)
# Delete all images
docker rmi $(docker images -q)

docker build   -f TodoList.WebApi/Dockerfile -t workshop/web-api-image:1.0.0 .

docker run  --name web-api-container -e "ASPNETCORE_ENVIRONMENT=Docker"  -p 8080:80 -d workshop/web-api-image:1.0.0
# DOCKER-COMPOSE 


