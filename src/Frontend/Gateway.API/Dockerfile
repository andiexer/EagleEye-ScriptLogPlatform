FROM microsoft/aspnetcore:2.0.0
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "EESLP.Frontend.Gateway.API.dll"]

RUN apt-get update && apt-get install -y wget
RUN wget https://github.com/jwilder/dockerize/releases/download/v0.4.0/dockerize-linux-amd64-v0.4.0.tar.gz
RUN tar -C /usr/local/bin -xzvf dockerize-linux-amd64-v0.4.0.tar.gz