#!/bin/bash

echo "Starting migration"
dotnet ef database update --project src/Wernher.Data --startup-project src/Wernher.API
echo "Migration complete"