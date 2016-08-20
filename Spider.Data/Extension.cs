namespace Spider.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class Extension
    {
        public static IDbCommand AddInOutParameter(this IDbCommand command, string parameterName, object parameterValue)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Direction = ParameterDirection.InputOutput;
            parameter.Value = parameterValue ?? DBNull.Value;
            command.Parameters.Add(parameter);
            return command;
        }

        public static IDbCommand AddOutParameter(this IDbCommand command, string parameterName, object parameterValue)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Direction = ParameterDirection.Output;
            parameter.Value = parameterValue ?? DBNull.Value;
            command.Parameters.Add(parameter);
            return command;
        }

        public static IDbCommand AddParameter(this IDbCommand command, IDbDataParameter parameter)
        {
            command.Parameters.Add(parameter);
            return command;
        }

        public static IDbCommand AddParameter(this IDbCommand command, string parameterName, object parameterValue)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue ?? DBNull.Value;
            command.Parameters.Add(parameter);
            return command;
        }

        public static IDbCommand AddParameters(this IDbCommand command, IEnumerable<KeyValuePair<string, object>> pairs)
        {
            Action<KeyValuePair<string, object>> action2 = null;
            Action<KeyValuePair<string, object>> action = null;
            if (pairs != null)
            {
                if (action == null)
                {
                    if (action2 == null) action2 = delegate (KeyValuePair<string, object> pair) {
                        command.AddParameter(pair.Key, pair.Value);
                    };
                    action = action2;
                }
                pairs.Each<KeyValuePair<string, object>>(action);
            }
            return command;
        }

        public static IDbCommand AddParameters(this IDbCommand command, IEnumerable<IDbDataParameter> parameters)
        {
            Action<IDbDataParameter> action2 = null;
            Action<IDbDataParameter> action = null;
            if (parameters != null)
            {
                if (action == null)
                {
                    if (action2 == null) action2 = delegate (IDbDataParameter parameter) {
                        command.Parameters.Add(parameter);
                    };
                    action = action2;
                }
                parameters.Each<IDbDataParameter>(action);
            }
            return command;
        }

        public static IDbCommand AddReturnParameter(this IDbCommand command, string parameterName, object parameterValue)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Direction = ParameterDirection.ReturnValue;
            parameter.Value = parameterValue ?? DBNull.Value;
            command.Parameters.Add(parameter);
            return command;
        }

        public static void Each<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (action != null)
            {
                foreach (T local in collection)
                {
                    action(local);
                }
            }
        }

        public static IList<T> ExecuteDataTable<T>(this IDbCommand command, Func<IDataReader, T> parser)
        {
            return command.ExecuteReader().ToList(parser);
        }

        public static IDictionary<string, IList<dynamic>> ExecuteDictionary(this IDbDataAdapter adapter)
        {
            Dictionary<string, IList<dynamic>> dictionary = new Dictionary<string, IList<dynamic>>();
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
            {
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    dictionary.Add(dataSet.Tables[i].TableName, dataSet.Tables[i].ToList());
                }
            }
            return dictionary;
        }

        public static IList<dynamic> ExecuteList(this IDbCommand command)
        {
            return command.ExecuteReader().ToList();
        }

        public static IList<T> ExecuteList<T>(this IDbCommand command, Func<IDataReader, T> parser)
        {
            return command.ExecuteReader().ToList<T>(parser);
        }

        public static IDbDataParameter GetDbParameter(this IDataParameterCollection parameters, int index)
        {
            return (parameters[index] as IDbDataParameter);
        }

        public static IDbDataParameter GetDbParameter(this IDataParameterCollection parameters, string name)
        {
            return (parameters[name] as IDbDataParameter);
        }

        public static IList<dynamic> ToList(this DataTable datatable)
        {
            return datatable.ToList<dynamic>(null);
        }

        public static IList<dynamic> ToList(this IDataReader reader)
        {
            return reader.ToList<dynamic>(null);
        }

        public static IList<T> ToList<T>(this DataTable datatable, Func<DataRow, T> parser)
        {
            List<T> list = new List<T>();
            if (parser == null&&typeof(T)==typeof(object))
            {
                parser = row => {
                    dynamic obj = new SmartObject();
                    var columns = row.Table.Columns;
                    for (int j = 0; j < columns.Count; j++)
                    {
                        string columnName = columns[j].ColumnName;
                        object obj3 = row[j];
                        obj[columnName] = (obj3 == DBNull.Value) ? null : obj3;
                    }
                    return obj;
                };
            }
            if (datatable.Rows != null && datatable.Rows.Count > 0)
            {
                DataColumnCollection columns = datatable.Columns;
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    if (parser != null)
                    {
                        list.Add(parser(datatable.Rows[i]));
                    }        
                }
            }
            return list;
        }

        public static IList<T> ToList<T>(this IDataReader reader, Func<IDataReader, T> parser)
        {
            List<T> list = new List<T>();
            if (parser==null&&typeof(T)==typeof(object))
            {
                parser = reader2 =>
                 {
                     dynamic obj = new SmartObject();
                     for (int i = 0; i < reader2.FieldCount; i++)
                     {
                         string name = reader2.GetName(i);
                         object obj2 = reader2.GetValue(i);
                         obj[name] = (obj2 == DBNull.Value) ? null : obj2;
                     }
                     return obj;
                 };
            }
            if (!reader.IsClosed)
            {
                using (reader)
                {
                    while (reader.Read())
                    {
                        if (parser!=null)
                        {
                            list.Add(parser(reader));
                        }   
                    }
                }
            }
            return list;
        }
    }
}

