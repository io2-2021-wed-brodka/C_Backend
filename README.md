# CityBikes

### For all teams

To run backend for tests:

```
# in main repo folder run:
docker-compose up
```

### For our team

To run Selenium:

```
docker run --net=host -d -p 4444:4444 -p 7900:7900 -v /dev/shm:/dev/shm selenium/standalone-chrome:4.0.0-beta-4-prerelease-20210517
```

If you have Selenium container running, you can go to [http://localhost:7900](http://localhost:7900) to see how tests are running. VNC password is `secret`.

To run frontend:

```
# in Frontend folder run:
npm run user
npm run admin
```

To run tests, you have to have Selenium container, backend, and both admin and user frontends running. Then, type:
```
# in Backend/SeleniumTests2
dotnet test
```

#### Development containers

If you install `Remote - Containers` extension in VSCode, and open Backend folder or Frontend folder, you will get a notification from VSCode if you want to run a development container.
