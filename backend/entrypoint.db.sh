#!/bin/bash

set -e
until dotnet ef database update --project CleanCityCore; do
>&2 echo "PostgreSQL Server is starting up"
sleep 1
done
