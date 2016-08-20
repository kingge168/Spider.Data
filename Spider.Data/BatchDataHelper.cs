namespace Spider.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;

    public class BatchDataHelper
    {
        private CommonDbHelper helper;

        public BatchDataHelper()
        {
            this.helper = new CommonDbHelper();
        }

        public BatchDataHelper(string conncetionSettingName)
        {
            this.helper = new CommonDbHelper(conncetionSettingName);
        }

        private IDbConnection CreateDbConnection()
        {
            return this.helper.CreateDbConnection();
        }

        public void ProcessData(DataWrapper wrapper)
        {
            this.ProcessData(null, wrapper);
        }

        public void ProcessData(IEnumerable<DataWrapper> wrappers)
        {
            this.ProcessData(null, wrappers);
        }

        public void ProcessData(IDbTransaction transaction, DataWrapper wrapper)
        {
            this.ProcessData(transaction, new DataWrapper[] { wrapper });
        }

        public void ProcessData(IDbTransaction transaction, IEnumerable<DataWrapper> wrappers)
        {
            Action<DataWrapper> action4 = null;
            Action<DataWrapper> action = null;
            if (wrappers != null)
            {
                if (transaction != null)
                {
                    if (action == null)
                    {
                        if (action4 == null) action4 = delegate (DataWrapper wrapper) {
                            this.ProcessDataItem(transaction.Connection, transaction, wrapper);
                        };
                        action = action4;
                    }
                    wrappers.Each<DataWrapper>(action);
                }
                else
                {
                    using (IDbConnection connection = this.CreateDbConnection())
                    {
                        connection.Open();
                        Action<DataWrapper> action2 = null;
                        Action<DataWrapper> action3 = null;
                        using (IDbTransaction trans = connection.BeginTransaction())
                        {
                            if (action2 == null)
                            {
                                if (action3 == null) action3 = delegate (DataWrapper wrapper) {
                                    this.ProcessDataItem(trans.Connection, trans, wrapper);
                                };
                                action2 = action3;
                            }
                            wrappers.Each<DataWrapper>(action2);
                            trans.Commit();
                        }
                    }
                }
            }
        }

        private void ProcessDataItem(IDbConnection con, IDbTransaction trans, DataWrapper wrapper)
        {
            if (wrapper != null)
            {
                IDbCommand command = con.CreateCommand();
                command.CommandText = wrapper.CommandText;
                command.Transaction = trans;
                if (wrapper.ParameterProvider != null) command.AddParameters(wrapper.ParameterProvider(wrapper.Data));
                command.ExecuteNonQuery();
            }
        }

        public IList<dynamic> Query(string commandText, IEnumerable<KeyValuePair<string, object>> pairs)
        {
            using (IDbConnection connection = this.CreateDbConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                command.AddParameters(pairs);
                return command.ExecuteList();
            }
        }

        public IList<dynamic> Query(string commandText, IEnumerable<IDbDataParameter> parameters)
        {
            using (IDbConnection connection = this.CreateDbConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                command.AddParameters(parameters);
                return command.ExecuteList();
            }
        }

        public IList<T> Query<T>(string commandText, IEnumerable<KeyValuePair<string, object>> pairs, Func<IDataReader, T> parser)
        {
            using (IDbConnection connection = this.CreateDbConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                command.AddParameters(pairs);
                return command.ExecuteList(parser);
            }
        }

        public IList<T> Query<T>(string commandText, IEnumerable<IDbDataParameter> parameters, Func<IDataReader, T> parser)
        {
            using (IDbConnection connection = this.CreateDbConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                command.AddParameters(parameters);
                return command.ExecuteList(parser);
            }
        }

        public IDictionary<string, IList<dynamic>> QueryMany(string commandText, IEnumerable<KeyValuePair<string, object>> pairs)
        {
            using (IDbConnection connection = this.CreateDbConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                command.AddParameters(pairs);
                IDbDataAdapter adapter = this.helper.CraeteDbDataAdapter();
                adapter.SelectCommand = command;
                return adapter.ExecuteDictionary();
            }
        }

        public IDictionary<string, IList<dynamic>> QueryMany(string commandText, IEnumerable<IDbDataParameter> parameters)
        {
            using (IDbConnection connection = this.CreateDbConnection())
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandText = commandText;
                command.AddParameters(parameters);
                IDbDataAdapter adapter = this.helper.CraeteDbDataAdapter();
                adapter.SelectCommand = command;
                return adapter.ExecuteDictionary();
            }
        }
    }
}

