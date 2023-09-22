# Use the official Microsoft SQL Server Linux image as the base image
FROM mcr.microsoft.com/mssql/server:2019-latest

# Set environment variables for SQL Server (customize as needed)
ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=pa55wOrd!
ENV MSSQL_PID=Express  
ENV MSSQL_TCP_PORT=1433

# Create a custom database (optional)
# ENV MSSQL_DB=mydatabase
# COPY init.sql /usr/src/app/
# WORKDIR /usr/src/app/
# RUN /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i init.sql  

# Expose the SQL Server port
# EXPOSE 1433

# Start SQL Server
CMD ["/opt/mssql/bin/sqlservr"]
