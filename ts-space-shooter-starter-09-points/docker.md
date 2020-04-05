# Docker

## Umgang mit Docker Containern  

### "docker run"
~~~~bash
docker run <nameOfImage>
        #Starts the conainer w/o any config
docker run -it ubuntu /bin/bash     
        #-it: Starts interactive conainter with bin/bash
docker run -v E:\DockerTest:/app -it node /bin/bash
        #-v: Volume - Save everything from /app into the given directory (2-way)
docker run -v E:\DockerTest:/app -e PORT=80 -p 8081:80 -w /app node /bin/bash -c "node server.js"
        #-e: Environment-Variable
        #-p: Port-Forwarding --> <portOutside>:<portInside>
        #-w: Working-Directory
        #/bin/bash -c: Run the following command in the /bin/bash
docker run --name TestContainer -d ubuntu /bin/bash -c "echo Test"
        #--name: Name of the container
        #-d: Daemonized - Run in the background
~~~~
Link zur "docker run" Doku: https://docs.docker.com/v17.12/engine/reference/commandline/run/#options  

### "docker ps"
~~~~bash
docker ps
        #Get all running containers
docker ps -a
        #Get every container (also stopped ones)
~~~~
Link zur "docker ps" Doku: https://docs.docker.com/engine/reference/commandline/ps/

### "docker stop"
~~~~bash
docker stop <id>
        #Stop the running container
~~~~
Link zur "docker stop" Doku: https://docs.docker.com/engine/reference/commandline/stop/

### "docker rm"
~~~~bash
docker rm -f <id>
        #Remove container
        #-f: force
~~~~
Link zur "docker rm" Doku: https://docs.docker.com/engine/reference/commandline/rm/

### "docker attach"
~~~~bash
docker attach <id>
        #Connect to a running (deamonized) container
~~~~
Link zur "docker attach" Doku: https://docs.docker.com/engine/reference/commandline/attach/

### "docker container prune"
~~~~bash
docker container prune
        #Remove all stopped containers
~~~~
Link zur "docker container prune" Doku: https://docs.docker.com/engine/reference/commandline/container_prune/


## Umgang mit Docker Images

### "docker pull"
~~~~bash
docker pull <image>:<tag>
~~~~
Link zur "docker pull" Doku: https://docs.docker.com/engine/reference/commandline/pull/

### "docker push"
~~~~bash
docker login --username=tbrych
        #LogIn if necessary
docker tag <name/id> <userName>/<name>:<tag>
docker push <name>:<tag>
~~~~
Link zur "docker push" Doku: https://docs.docker.com/engine/reference/commandline/push/

### "docker build"
~~~~bash
docker build -t <name>:<tag> .
        #Build the Dockerfile to a Docker-Image
~~~~
Link zur "docker build" Doku: https://docs.docker.com/engine/reference/commandline/build/

## Single-Page App

Dockerfile:
~~~~docker
FROM nginx:alpine
COPY index.* /usr/share/nginx/html/
~~~~

Build the Dockerfile:
~~~~bash
docker build -t <name>:<tag> .
~~~~

Run the new Docker Image:
~~~~bash
docker run -p 8084:80 <newImage>
~~~~

## Node-API

Dockerfile:
~~~~docker
FROM node:alpine
WORKDIR /app
ENV PORT 80
EXPOSE 80

COPY package.json .
RUN npm install
COPY server.js .
CMD ["node", "server.js"]
~~~~

Build the Dockerfile:
~~~~bash
docker build -t <name>:<tag> .
~~~~

Run the new Docker Image:
~~~~bash
docker run -p 8083:80 <newImage>
~~~~

## .NET-Core App

### First version (shoudn't be used)
~~~~docker
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/DotNetCoreMiniSample.dll"]
~~~~
The problem with this version is the required space

### Better solution:
~~~~docker
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build-env
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out 

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "DotNetCoreMiniSample.dll"]
~~~~

Build the Dockerfile:
~~~~bash
docker build -t <name>:<tag> .
~~~~

Run the new Docker Image:
~~~~bash
docker run -p 5000:80 <newImage>:<tag>
~~~~

## Docker Networks
To communicate between docker container you have to create "Docker Networks"

List all networks:
~~~~bash
docker network ls
~~~~

Create a new network:
~~~~bash
docker network create api-net
~~~~

Start a conainer in a specific network
~~~~bash
docker run ... --net=api-net ...
~~~~

## .dockerignore
It is useful to create a dockerignore (can be compared to a gitignore)
Insert everything that does not need to be in the conainer (e.g. node_modules, Dockerfile, ...)