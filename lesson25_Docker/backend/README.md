# Build
docker build . -t backend

# Run in Dev mode
``` sh
docker run \
-p 8081:8081  \
-e ASPNETCORE_ENVIRONMENT=Development \
backend
```

# Run in Prod mode
docker run \
-p 8081:8081 \
backend
