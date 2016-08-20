namespace Spider.Data
{
    using System;
    using System.Data;

    public class CommonDbHelper
    {
        private CommonDbDataProviderFactory factory;

        public CommonDbHelper()
        {
            this.factory = new CommonDbDataProviderFactory();
        }

        public CommonDbHelper(string conncetionSettingName)
        {
            this.factory = new CommonDbDataProviderFactory(conncetionSettingName);
        }

        public IDbDataAdapter CraeteDbDataAdapter()
        {
            return this.factory.CreateDataAdapter();
        }

        public IDbCommand CreateDbCommand()
        {
            return this.factory.CreateCommand();
        }

        public IDbCommand CreateDbCommand(string commandText)
        {
            return this.factory.CreateCommand(commandText);
        }

        public IDbCommand CreateDbCommand(string commandText, CommandType commandType)
        {
            return this.factory.CreateCommand(commandText, commandType);
        }

        public IDbCommand CreateDbCommand(string commandText, IDbConnection con)
        {
            IDbCommand command = this.CreateDbCommand(commandText);
            command.Connection = con;
            return command;
        }

        public IDbCommand CreateDbCommand(string commandText, CommandType commandType, IDbConnection con)
        {
            IDbCommand command = this.CreateDbCommand(commandText, commandType);
            command.Connection = con;
            return command;
        }

        public IDbConnection CreateDbConnection()
        {
            return this.factory.CreateConnection();
        }

        public IDbConnection CreateDbConnection(string connectionString)
        {
            return this.factory.CreateConnection(connectionString);
        }

        public IDbDataParameter CreateDbDataParameter()
        {
            return this.factory.CreateParameter();
        }

        public IDbDataParameter CreateInDbDataParameter(string parameterName)
        {
            IDbDataParameter parameter = this.CreateDbDataParameter();
            parameter.ParameterName = parameterName;
            parameter.Direction = ParameterDirection.Input;
            return parameter;
        }

        public IDbDataParameter CreateInDbDataParameter(string parameterName, object value)
        {
            IDbDataParameter parameter = this.CreateInDbDataParameter(parameterName);
            parameter.Value = value ?? DBNull.Value;
            return parameter;
        }

        public IDbDataParameter CreateInDbDataParameter(string parameterName, object value, DbType dbType)
        {
            IDbDataParameter parameter = this.CreateInDbDataParameter(parameterName, value);
            parameter.DbType = dbType;
            return parameter;
        }

        public IDbDataParameter CreateInDbDataParameter(string parameterName, object value, DbType dbType, int size)
        {
            IDbDataParameter parameter = this.CreateInDbDataParameter(parameterName, value, dbType);
            parameter.Size = size;
            return parameter;
        }

        public IDbDataParameter CreateInDbDataParameter(string parameterName, object value, DbType dbType, int size, byte scale)
        {
            IDbDataParameter parameter = this.CreateInDbDataParameter(parameterName, value, dbType, size);
            parameter.Scale = scale;
            return parameter;
        }

        public IDbDataParameter CreateInOutDbDataParameter(string parameterName)
        {
            IDbDataParameter parameter = this.CreateDbDataParameter();
            parameter.ParameterName = parameterName;
            parameter.Direction = ParameterDirection.InputOutput;
            return parameter;
        }

        public IDbDataParameter CreateInOutDbDataParameter(string parameterName, object value)
        {
            IDbDataParameter parameter = this.CreateInOutDbDataParameter(parameterName);
            parameter.Value = value ?? DBNull.Value;
            return parameter;
        }

        public IDbDataParameter CreateInOutDbDataParameter(string parameterName, object value, DbType dbType)
        {
            IDbDataParameter parameter = this.CreateInOutDbDataParameter(parameterName, value);
            parameter.DbType = dbType;
            return parameter;
        }

        public IDbDataParameter CreateInOutDbDataParameter(string parameterName, object value, DbType dbType, int size)
        {
            IDbDataParameter parameter = this.CreateInOutDbDataParameter(parameterName, value, dbType);
            parameter.Size = size;
            return parameter;
        }

        public IDbDataParameter CreateInOutDbDataParameter(string parameterName, object value, DbType dbType, int size, byte scale)
        {
            IDbDataParameter parameter = this.CreateInOutDbDataParameter(parameterName, value, dbType, size);
            parameter.Scale = scale;
            return parameter;
        }

        public IDbDataParameter CreateOutDbDataParameter(string parameterName)
        {
            IDbDataParameter parameter = this.CreateDbDataParameter();
            parameter.ParameterName = parameterName;
            parameter.Direction = ParameterDirection.Output;
            return parameter;
        }

        public IDbDataParameter CreateOutDbDataParameter(string parameterName, object value)
        {
            IDbDataParameter parameter = this.CreateOutDbDataParameter(parameterName);
            parameter.Value = value ?? DBNull.Value;
            return parameter;
        }

        public IDbDataParameter CreateOutDbDataParameter(string parameterName, object value, DbType dbType)
        {
            IDbDataParameter parameter = this.CreateOutDbDataParameter(parameterName, value);
            parameter.DbType = dbType;
            return parameter;
        }

        public IDbDataParameter CreateOutDbDataParameter(string parameterName, object value, DbType dbType, int size)
        {
            IDbDataParameter parameter = this.CreateOutDbDataParameter(parameterName, value, dbType);
            parameter.Size = size;
            return parameter;
        }

        public IDbDataParameter CreateOutDbDataParameter(string parameterName, object value, DbType dbType, int size, byte scale)
        {
            IDbDataParameter parameter = this.CreateOutDbDataParameter(parameterName, value, dbType, size);
            parameter.Scale = scale;
            return parameter;
        }

        public IDbDataParameter CreateReturnDbDataParameter(string parameterName)
        {
            IDbDataParameter parameter = this.CreateDbDataParameter();
            parameter.ParameterName = parameterName;
            parameter.Direction = ParameterDirection.ReturnValue;
            return parameter;
        }

        public IDbDataParameter CreateReturnDbDataParameter(string parameterName, object value)
        {
            IDbDataParameter parameter = this.CreateReturnDbDataParameter(parameterName);
            parameter.Value = value ?? DBNull.Value;
            return parameter;
        }

        public IDbDataParameter CreateReturnDbDataParameter(string parameterName, object value, DbType dbType)
        {
            IDbDataParameter parameter = this.CreateInDbDataParameter(parameterName, value);
            parameter.DbType = dbType;
            return parameter;
        }

        public IDbDataParameter CreateReturnDbDataParameter(string parameterName, object value, DbType dbType, int size)
        {
            IDbDataParameter parameter = this.CreateReturnDbDataParameter(parameterName, value, dbType);
            parameter.Size = size;
            return parameter;
        }

        public IDbDataParameter CreateReturnDbDataParameter(string parameterName, object value, DbType dbType, int size, byte scale)
        {
            IDbDataParameter parameter = this.CreateReturnDbDataParameter(parameterName, value, dbType, size);
            parameter.Scale = scale;
            return parameter;
        }
    }
}

