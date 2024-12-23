# Build
docker build . -t backend

# Run in Dev mode
``` sh
docker run \
-p 8080:8080  \
-e ASPNETCORE_ENVIRONMENT=Development \
backend
```

# Run in Prod mode
docker run \
-p 8080:8080 \
backend
