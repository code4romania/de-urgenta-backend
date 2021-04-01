# De urgenta - Backend API

[![GitHub contributors](https://img.shields.io/github/contributors/code4romania/de-urgenta-backend.svg?style=for-the-badge)](https://github.com/code4romania/de-urgenta-backend/graphs/contributors) [![GitHub last commit](https://img.shields.io/github/last-commit/code4romania/de-urgenta-backend.svg?style=for-the-badge)](https://github.com/code4romania/de-urgenta-backend/commits/master) [![License: MPL 2.0](https://img.shields.io/badge/license-MPL%202.0-brightgreen.svg?style=for-the-badge)](https://opensource.org/licenses/MPL-2.0)

De Urgență [was prototyped](https://civiclabs.ro/ro/solutions/stay-together) in [Code for Romania](https://code4.ro/ro)'s research project, [Civic Labs](https://civiclabs.ro/ro).

The application aims to inform citizens about how to react to the first critical hours in a crysis situation (like that of an earthquake). 

It also aims to build healthy habits that become ingrained with time, so that, when the critical moment arrives, each person knows what the key first steps to keeping themselves safe are. 

[See the project live](insert_link_here)

[Contributing](#contributing) | [Repos and projects](#repos-and-projects) | [Deployment](#deployment) | [Feedback](#feedback) | [License](#license) | [About Code4Ro](#about-code4ro)

## Contributing

This project is built by amazing volunteers and you can be one of them! Here's a list of ways in [which you can contribute to this project](https://github.com/code4romania/.github/blob/master/CONTRIBUTING.md). If you want to make any change to this repository, please **make a fork first**.

Help us out by testing this project in the [staging environment](INSERT_LINK_HERE). If you see something that doesn't quite work the way you expect it to, open an Issue. Make sure to describe what you _expect to happen_ and _what is actually happening_ in detail.

If you would like to suggest new functionality, open an Issue and mark it as a __[Feature request]__. Please be specific about why you think this functionality will be of use. If you can, please include some visual description of what you would like the UI to look like, if you are suggesting new UI elements. 

### Programming languages

.NET 5.0 (C#)

### Package managers

NuGet

### Database technology & provider

PostgreSQL

## Repos and projects

[Android repo](https://github.com/code4romania/de-urgenta-android)   
[iOS repo](https://github.com/code4romania/de-urgenta-ios)   
[Web app - frontend](https://github.com/code4romania/de-urgenta-client)   
[Web app - backend](https://github.com/code4romania/de-urgenta-backend)   

## Development Tips

Start a postgres server
```
docker-compose -f docker-compose-dep.yml up -d
```

Creating a EF Core migration 
```
dotnet ef migrations add <Migration-name> --project DeUrgenta.Domain --startup-project DeUrgenta.Api
```

## Deployment

Guide users through getting your code up and running on their own system. In this section you can talk about:
1. Installation process
2. Software dependencies
3. Latest releases
4. API references

Describe and show how to build your code and run the tests.

## Feedback

* Request a new feature on GitHub.
* Vote for popular feature requests.
* File a bug in GitHub Issues.
* Email us with other feedback contact@code4.ro

## License

This project is licensed under the MPL 2.0 License - see the [LICENSE](LICENSE) file for details

## About Code for Romania

Started in 2016, Code for Romania is a civic tech NGO, official member of the Code for All network. We have a community of around 2.000 volunteers (developers, ux/ui, communications, data scientists, graphic designers, devops, it security and more) who work pro-bono for developing digital solutions to solve social problems. #techforsocialgood. If you want to learn more details about our projects [visit our site](https://www.code4.ro/en/) or if you want to talk to one of our staff members, please e-mail us at contact@code4.ro.

Last, but not least, we rely on donations to ensure the infrastructure, logistics and management of our community that is widely spread across 11 timezones, coding for social change to make Romania and the world a better place. If you want to support us, [you can do it here](https://code4.ro/en/donate/).
