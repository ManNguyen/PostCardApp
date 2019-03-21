# Post Card Application

This application demonstrate how to use **JWT based authentication** in Single Page Application.

## Details

The purpose of this Post Card Applications is to sell "proprietary" sweet flower pictures to use as post card. 
There are 4 flower pictures stored in the server, and to access to those pictures, users have to Register then Login

JWT token is handle in the Front End using "Angular/Service/AuthenticateService.js", and handle in the Back End using "Service/TokenServices" and [TokenAuthenticateFilterAttribute].

## Built With

* [AngularJS 1.x](https://angularjs.org/) - Javascript Framework for Single Page Application
* [Bootstrap3](http://getbootstrap.com/) - CSS Framework
* [ui - router](https://ui-router.github.io/) - Used to route pages
* [ngSanitize](https://docs.angularjs.org/api/ngSanitize) - Sanitize module for AngularJS
* [ASP.Net 4.6](https://angularjs.org/) Server Coding
* [SQLite](https://www.sqlite.org/) Database

## Notes

* The Pages #/PostCard is secured in Front End and can be bypassed, but the WebAPI that actually serve pictures is secured in the Back-End via TokenFilterAttribute.

## How To Run

Download the whole project and then run with Visual Studio 2015

