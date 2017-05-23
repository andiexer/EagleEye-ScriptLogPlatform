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

- .Net Core 1.1
- Openshift (you can use minishift as a development platform on you computer)

## Built With

* [.Net Core](https://www.microsoft.com/net/download/core) - .Net Core 1.1
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
