REM install java from https://www.oracle.com/pl/java/technologies/downloads/#jdk23-windows
REM or via winget, but make sure to setup a fresh one

REM download swaggen-codegen from https://repo1.maven.org/maven2/io/swagger/codegen/v3/swagger-codegen-cli/
REM get a new one : )
REM for example swagger-codegen-cli-3.0.62.jar - <jar>

REM start your Web API project
REM copy your swagger doc link
REM like this http://localhost:5016/swagger/v1/swagger.json - <link>

REM generate the client codegen
java -jar <jar> generate -i <link> -l csharp-dotnet2

REM eg
java -jar .\swagger-codegen-cli-3.0.62.jar generate -i http://localhost:5016/swagger/v1/swagger.json -l csharp-dotnet2 -o client_generated