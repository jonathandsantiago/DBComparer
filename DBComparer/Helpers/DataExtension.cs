using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DBComparer.Helpers
{
    public static class DataExtension
    {
        public static T GetValueDataRow<T>(this DataRow dataRow, string columnName)
        {
            var value = dataRow[columnName];

            if (value == DBNull.Value)
            {
                return default(T);
            }

            return (T)value;
        }

        public static T GetValueDataRow<T>(this DataRowView dataRow, string columnName)
        {
            var value = dataRow.Row[columnName];

            if (value == DBNull.Value)
            {
                return default(T);
            }

            return (T)value;
        }

        public static T ExecuteScalar<T>(this IDbCommand command)
        {
            return (T)command.ExecuteScalar();
        }

        public static IDbCommand GetScalarInsertCommand(this IDbCommand command, string nameTable, IList<CommandParameter> columns, bool isSqlServer = true)
        {
            command.CommandText = GetScriptInsert(command, nameTable, columns, isSqlServer) + " DECLARE @Id int  SET @Id = @@identity SELECT @Id";
            return command;
        }

        public static IDbCommand GetInsertCommand(this IDbCommand command, string nameTable, IList<CommandParameter> columns, bool isSqlServer = true)
        {
            command.CommandText = GetScriptInsert(command, nameTable, columns, isSqlServer);
            return command;
        }

        private static string GetScriptInsert(this IDbCommand command, string nameTable, IList<CommandParameter> columns, bool isSqlServer)
        {
            string into = string.Concat("INSERT INTO ", nameTable, "(");
            string values = "VALUES (";
            foreach (CommandParameter column in columns.Select((c, index) => new CommandParameter(c.Name, c.Value, index)))
            {
                if (column.Name == string.Empty)
                {
                    continue;
                }

                string nameColumn = string.Concat("@", column.Name);
                into += string.Concat(column.Index == 0 ? " " : " ,", column.Name);
                values += string.Concat(column.Index == 0 ? " " : ", ", isSqlServer ? nameColumn : " ?");

                command.CreateParameter(nameColumn, column.Value);
            }

            return string.Concat(into, ") ", values, ")");
        }

        public static IDbCommand GetUpdateCommand(this IDbCommand command, string nameTable, IList<CommandParameter> columns, IList<CommandParameter> parameters, bool isSqlServer = true)
        {
            string update = string.Concat("UPDATE ", nameTable, " SET ");
            foreach (CommandParameter column in columns.Select((c, index) => new CommandParameter(c.Name, c.Value, index)))
            {
                if (column.Name == string.Empty)
                {
                    continue;
                }
                string nameColumn = string.Concat("@", column.Name);
                update += string.Concat(column.Index == 0 ? " " : " ,", column.Name, isSqlServer ? " = " + nameColumn : " = ?");
                command.CreateParameter(nameColumn, column.Value);
            }

            foreach (CommandParameter parameter in parameters.Select((c, index) => new CommandParameter(c.Name, c.Value, index)))
            {
                string nameColumn = string.Concat("@", parameter.Name);
                update += string.Concat(parameter.Index == 0 ? " WHERE " : " AND ", parameter.Name, isSqlServer ? " = " + nameColumn : " = ?");
                command.CreateParameter(nameColumn, parameter.Value);
            }
            command.CommandText = update;

            return command;
        }

        public static IDbDataParameter CreateParameter(this IDbCommand command, string nome, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = nome;

            if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = value;
            }

            command.Parameters.Add(parameter);
            return parameter;
        }
    }

    public class CommandParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public int Index { get; set; }

        public CommandParameter()
        { }

        public CommandParameter(string name, object value, int index)
        {
            Name = name;
            Value = value;
            Index = index;
        }
    }
}
