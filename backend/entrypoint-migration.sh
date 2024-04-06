#!/bin/bash

echo "Starting migration"
dotnet ef migrations add --project src/Wernher.Data --startup-project src/Wernher.API "initial-database5"
dotnet ef database update --project src/Wernher.API
echo "Migration complete"