# Meetup-Azure-Microsoft-365-Community episode-02

During this journey, I will  talk about dockerization of web api and sql server linux database

So I will dockerize the web API and the SQL Server database : LogCorner.EduSync.Speech.Presentation and LogCorner.EduSync.Speech.Database.
This dockerization, will produce 2 images, which will be pushed to a repository of a container registry and used by Azure Kubernete Service.

# SQL SERVER LINUX DOCKERIZATION
From LogCorner.EduSync.Speech.Database project, open project properties, click on then Project Settings tab,  and then check Create script (.sql file)

Create script

it will create a script file for creating the database. Whenever the database is updated, this script will be updated.

Click on tab Build Events, create a Post-build event command as following :

xcopy “$(ProjectDir)bin\$(Configuration)\LogCorner.EduSync.Speech.Database_Create.sql” “$(ProjectDir)Docker\Restore” /Y

post_build_event

It will copy the sql script generated on previous step to a specfic directory (Docker/Restore on my case)

DockerFile will use this script to create the database image

script file

The sql server linux Dockerfile

Dockerfile-sql

Locate the directory where the SQL Server Dockerfile is located and run the following command to create database-image with SA_PASSWORD

docker build -t database-image . –build-arg SA_PASSWORD=’PassW0rd’

List all images

docker images –filter “dangling=false”

dockerimages-db

We have two addtionnal images : microsoft/mssql-server-linux and database-image

RUN CONTAINER
Run the following command to create a database container (database-container) based on database-image, mapped on port 1433 inside and outside the container

docker run -d  -p 1433:1433  –name database-container database-image

runcontainerdb

list running containers

docker ps -a

listcontainer-db

We have a new container based on database-image

Run the following command to attach shell on database-container

Docker exec -it database-container “bash”

bashon sql

Run the following command to connect to sql server instance of the running container

/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P ‘PassW0rd’

Run the following command to list all databases,

select name from sys.databases

go

Listoddatabases

we can see that the database LogCorner.EduSync.Speech.Database  is created via the script on Dockerfile

Run the following command  to select  [dbo].[Speech]  table on that database

use [LogCorner.EduSync.Speech.Database]

go

select * from [dbo].[Speech]
go

select table inside container

# WEB API DOCKERIZATION
Right click on  LogCorner.EduSync.Speech.Presentation project name, and select ContainerOrchestration Support 

add docker

Choose Docker Compose and click OK

Container orchestration

The following Dockerfile will be generated

Dockerfile

BUILD IMAGES

for the purposes of this demonstration, I delete all the containers and images, do not run this commands if don’t want to delete all your images

# Stop all containers
docker stop $(docker ps -a -q)

# Delete all containers
docker rm $(docker ps -a -q)
# Delete all images
docker rmi $(docker images -q)

To build the previous Dockerfile, locate CommandInterfaces directory  and run the following command: it build the DockerFile from the current directory as build context, and name the resulted images as webapi-image

docker build -t webapi-image -f LogCorner.EduSync.Speech\LogCorner.EduSync.Speech.Presentation\Dockerfile .

run the following command to list all images

docker images

dockerimages

docker images –filter “dangling=false”

dockerimagesnodanglig

the following images are created :

microsoft/dotnet:2.2-aspnetcore-runtime  (from Dockerfile)
microsoft/dotnet:2.2-sdk (from Dockerfile)
webapi-image (from build command)
RUN CONTAINER
Run the following command : it runs the webapi-image image , creates a container webapi-container and map port 80 of the container to port 8080 outside of the container

docker run -d  -p 8080:80  –name webapi-container webapi-image

runcontainerwebapi
The following command list all running containers

docker ps -a

listcontainerwebapi

We have a running container named webapi-container 

Run the following command to view the webapi-container logs

docker logs webapi-container

logscontainerwebapi

Web api is now running and listenning on port 80 inside the container and port 8080 outside the container

So http://localhost:8080/api/speech should hit the api as following

postman-speech

Run again docker logs webapi-container

docker logs webapi-container

logscontainerwebapi-2

The logs says that an error occured because it cannot connect to database

Let us fix it on next step



# DOCKER-COMPOSE 
Compose is a tool for defining and running multi-container Docker applications. To learn more about Compose refer to the following documentation : https://docs.docker.com/compose/overview/

Open the docker-compose.yml file, it already contains a logcorner.edusync.speech.presentation service.  update this service to make it depend on database service : logcorner.edusync.speech.presentation.data.

Add a logcorner.edusync.speech.presentation.data service, use the SQL Dockerfile create ealier and SA_PASSWORD as argument.

Docker-compose

The override file, as its name implies, can contain configuration overrides for existing services or entirely new services : https://docs.docker.com/compose/extends/

Open docker-compose.override.yml file, and set ASPNETCORE_ENVIRONMENT = Docker or something else . the goal is to use the appsettings.Docker.json file to set all configuration parameters specific to that environment.

web api service listen on port 80 inside the container and 8080 outside.

database service listen on port 1433 inside the container and 1433 outside.

Docker-compose-overrides

Open appsettings.Docker.json file and add a connectionString  to use SQL Server database

Data Source=logcorner.edusync.speech.presentation.data  (name of the database servcie)

Initial Catalog=LogCorner.EduSync.Speech.Database

User=sa;Password=PassW0rd

appsettings.Docker

BUILD IMAGES
docker-compose build

docker images –filter “dangling=false”

Docker-compose-images

RUN CONTAINER
docker-compose up

docker ps --all --format "table {{.ID}}\t{{.Image}}\t{{.Names}}"

Docker-compose-container

TESTING
RUN CONTAINER
docker-compose up

ATTACH SHELL
docker exec -it 1997 “bash”

CONNECT TO SQL SERVER INSTANCE OF THE RUNNING CONTAINER
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P ‘PassW0rd’

VERIFY THAT THE DATABASE  TABLE DBO.SPEECH   IS EMPTY
use [LogCorner.EduSync.Speech.Database]
go

select * from [dbo].[Speech]
go

0lignes

POST A REQUEST
Open postman and post a request

postman

VERIFY THAT THE DATABASE TABLE DBO.SPEECH  HAS ONE ROW
use [LogCorner.EduSync.Speech.Database]
go

select * from [dbo].[Speech]
go

1line

source code is available here Dockerization

