docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=MyPasswordIsS3cure!" \
   -p 1433:1433 --name sql1 --hostname sql1 \
   -d \
   mcr.microsoft.com/mssql/server:2025-latest