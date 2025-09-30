# Wait to be sure that SQL Server came up
sleep 60s

# Run the setup script to create the DB and the schema in the DB
# Note: make sure that your password matches what is in the Dockerfile
echo "seed database"
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P password123! -d master -i seed.sql -C
echo "done seeding database"
