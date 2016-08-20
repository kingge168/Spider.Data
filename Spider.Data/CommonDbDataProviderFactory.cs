namespace Spider.Data
{
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Security;
    using System.Security.Permissions;

    internal class CommonDbDataProviderFactory : DbProviderFactory
    {
        private const string ConncetionSettingName = "DefaultConnection";
        private string connectionString;
        private DbProviderFactory provider;

        public CommonDbDataProviderFactory() : this("DefaultConnection")
        {
        }

        public CommonDbDataProviderFactory(string conncetionSettingName)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conncetionSettingName];
            this.provider = DbProviderFactories.GetFactory(settings.ProviderName);
            this.connectionString = settings.ConnectionString;
        }

        public override DbCommand CreateCommand()
        {
            return this.provider.CreateCommand();
        }

        public DbCommand CreateCommand(string commandText)
        {
            DbCommand command = this.CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        public DbCommand CreateCommand(string commandText, CommandType commandType)
        {
            DbCommand command = this.CreateCommand(commandText);
            command.CommandType = commandType;
            return command;
        }

        public DbCommand CreateCommand(string commandText, DbConnection con)
        {
            DbCommand command = this.CreateCommand();
            command.Connection = con;
            command.CommandText = commandText;
            return command;
        }

        public DbCommand CreateCommand(string commandText, CommandType commandType, DbConnection con)
        {
            DbCommand command = this.CreateCommand(commandText, commandType);
            command.Connection = con;
            return command;
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            return this.provider.CreateCommandBuilder();
        }

        public override DbConnection CreateConnection()
        {
            return this.CreateConnection(this.connectionString);
        }

        public DbConnection CreateConnection(string connectionString)
        {
            DbConnection connection = this.provider.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return this.provider.CreateConnectionStringBuilder();
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return this.provider.CreateDataAdapter();
        }

        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return this.provider.CreateDataSourceEnumerator();
        }

        public override DbParameter CreateParameter()
        {
            return this.provider.CreateParameter();
        }

        public DbParameter CreateParameter(string parameterName)
        {
            DbParameter parameter = this.CreateParameter();
            parameter.ParameterName = parameterName;
            return parameter;
        }

        public DbParameter CreateParameter(string parameterName, object value)
        {
            DbParameter parameter = this.CreateParameter(parameterName);
            parameter.Value = value;
            return parameter;
        }

        public DbParameter CreateParameter(string parameterName, object value, DbType dbType)
        {
            DbParameter parameter = this.CreateParameter(parameterName, value);
            parameter.DbType = dbType;
            return parameter;
        }

        public DbParameter CreateParameter(string parameterName, object value, int size, DbType dbType)
        {
            DbParameter parameter = this.CreateParameter(parameterName, value, dbType);
            parameter.Size = size;
            return parameter;
        }

        public DbParameter CreateParameter(string parameterName, object value, int size, DbType dbType, ParameterDirection parameterDirection)
        {
            DbParameter parameter = this.CreateParameter(parameterName, value, size, dbType);
            parameter.Direction = parameterDirection;
            return parameter;
        }

        public override CodeAccessPermission CreatePermission(PermissionState state)
        {
            return this.provider.CreatePermission(state);
        }

        public override bool Equals(object obj)
        {
            return this.provider.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.provider.GetHashCode();
        }

        public override string ToString()
        {
            return this.provider.ToString();
        }

        public override bool CanCreateDataSourceEnumerator
        {
            get
            {
                return this.provider.CanCreateDataSourceEnumerator;
            }
        }
    }
}

