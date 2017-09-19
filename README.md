# EagleEye ScriptLogPlatform (EESLP)

EagleEye ScriptLogPlatform, formerly known as PoshLogMonitor is Platform to handle Script Logfiles. The main purpose is to log scripts into EESLP and analyze the logs.

## Folder structure
```
├── src
|   ├── backend
|   |   ├── backend.gateway.api
|   |   ├── backend.powershellmodule
|   ├── common
|   ├── frontend
|   |   ├── frontend.gateway.api
|   |   ├── frontend.ui
|   ├── services
|   |   ├── alerting.api
|   |   ├── logging.api
|   |   ├── maintenance.api
|   |   ├── script.api
|   |   ├── user.api
```

## Architecture

The whole platform is build on docker microservices developed mostly with .net core/python. As a platform we have chosen [openshift](https://www.openshift.org/) (Basically Service Discovery has to be changed for other platforms). In the furture we will fork the project to support also other platforms natively.

## Main Features

- Log running scripts into script instances for easy management and analyzing the logs
- Set alerts per script (Email?, SMS?, Zabbix?, SCOM?, other Tools?)
- Powershell Module for easy integration into existing powershell scripts
- Simple centralized rollover logs per server
- more to come....

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

- .Net Core 2.0
- Docker (incl. Docker Toolbox)
- NPM
- Angular cli (install with 'npm install --global @angular/cli')

### Build project

> Attention: In this build the frontend ui has an issue that the base uri won't be changed correctly.
> Goto src/Frontend/UI/dist/assets/appconfig.json and change the ip to your dockerhost ip

run the script build-bits.ps1

available parameters:

- SolutionFile (default is EESLP-DockerBITSONly.sln)
- RemoveDockerImages (removes all running containers starting with eeslp* and all eeslp* iamges)

example:

```powershell
build-bits.ps1 -RemoveDockerImages
```

if you wanna remove all docker containers and images only use this powershell script:

```powershell
remove-dockerimages.ps1
```

### build docker containers and run with docker compose

go to the parent folder where the docker-compose.yml file is. build all containers with the command:

```
docker-compose build
```

and run it with following command (-d for)

> In case you've got an error from elasticsearch: **vm.max_map_count [65530] likely too low, increase to at least [262144]**.<p>Use this command to to fix in the docker host: **sudo sysctl -w vm.max_map_count=262144**

```
docker-compose up -d 
```

and stop it

```
docker-compose down
```

## Built With

* [.Net Core](https://www.microsoft.com/net/download/core) - .Net Core 2.0
* [Docker](https://www.docker.com/) - Docker
* [Openshift](https://openshift.org/) - Openshift

## Authors

* **Andi Exer** - *Initial work* - [AndiExer](https://github.com/AndiExer)
* **Manuel Habert** - *Initial work* - [Braiinzz](https://github.com/braiinzz)

See also the list of [contributors](https://github.com/andiexer/eagleeye-scriptlogplatform/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

- wanna be the first one here? :)
