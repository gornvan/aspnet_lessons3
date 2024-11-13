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