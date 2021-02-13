# Payment Gateway microservice

Example microservice for a Payment Gateway that processes payments on behalf of merchants using the merchant's
acquiring bank (mocked), and store historical payments done through the gateway.

## Open API documentation

The Open API (Swagger) documentation is available [here](https://patituccia.github.io/payment-gateway)

## How to Run

### Visual Studio

Solution was created on Visual Studio 2019 and should be runnable with any of the debugging profiles.

### Docker

First build the image.
```
docker-compose build
```

To run the solutions there are two options...

Just using HTTP
```
docker-compose -f docker-compose.yml up 
```
Using HTTP and HTTPS (uses the docker-compose.override.yml file created by Visual Studio and requires a 
DEV certificate on a Windows machine)
```
docker-compose up
```

