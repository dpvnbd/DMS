# Infrastructure layer
DbContext contains the domain entities that are connected with configurations
and identity entities which are maintained by framework.

Commands for migrations:

`dotnet ef migrations add <NAME> -s "../DMS.Web"`

`dotnet ef database update -s "../DMS.Web"`

