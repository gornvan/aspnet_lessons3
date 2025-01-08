# Synopticum
todo - add project description

## First Run
- Web API project needs to be configured for connection string - see `appsettings.json`
- Apply the migration(s) to initialize the DB - `SynopticumDAL/tools/migrate.sh`
- To run, run the project `SynopticumWebAPI`

## Testing
Before testing, a test DB needs to be initialized - run SynopticumTestsDbSeed.
The connection string for it is in `seedsettings.json`

# Migrations
Ensure entity framwork is installed
``` sh
dotnet tool install -g dotnet-ef
```

To add migration, use the tools:
``` sh
./SynopticumDAL/tools/newMigration.sh <migrationName>

```

# Auth Integarations
## KeyCloak
See Keycloak section of appsettings.Development.json

## Google
Run in the project directly before starting, to enable google auth
See more
https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-8.0
``` sh
dotnet user-secrets set "Authentication:Google:ClientId" "<client-id>"
dotnet user-secrets set "Authentication:Google:ClientSecret" "<client-secret>"
```