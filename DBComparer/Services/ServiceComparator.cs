using DBComparer.Repositories;
using System;
using System.Data;
using System.Linq;

namespace DBComparer.Services
{
    public class ServiceComparator
    {
        private RepositoryComparator repository;

        public ServiceComparator(string connectionStringSql, string dataBaseNameOdbc, string passwordOdbc, string connectionStringFireBird)
        {
            repository = new RepositoryComparator(connectionStringSql, dataBaseNameOdbc, passwordOdbc, connectionStringFireBird);
        }

        public DataTable GetByType(ConnectionType type, string scriptOrPath)
        {
            return repository.GetByType(type, scriptOrPath);
        }

        public DataTable GetResultComparisonByType(ComparatorType type, DataTable fromDataTable, DataTable withDataTable, bool distinct)
        {
            switch (type)
            {
                case ComparatorType.Different:
                    return GetBetweenDifferentData(fromDataTable, withDataTable, distinct);
                case ComparatorType.Equals:
                    return GetBetweenEqualData(fromDataTable, withDataTable, distinct);
                case ComparatorType.Merge:
                    return GetMergeData(fromDataTable, withDataTable, distinct);
                default:
                    throw new NotImplementedException();
            }
        }

        private DataTable GetBetweenDifferentData(DataTable fromDataTable, DataTable withDataTable, bool distinct)
        {
            DataTable result = CreateTableResult(fromDataTable);

            foreach (DataRow deDataRow in fromDataTable.Rows.AsParallel())
            {
                DataRow withDataRow = GetByKey(deDataRow, withDataTable);

                if (withDataRow != null && IsEquals(deDataRow, withDataRow))
                {
                    RemoveColumn(withDataTable, withDataRow, distinct);
                    continue;
                }

                result.Rows.Add(GetResult(deDataRow, withDataRow, result.NewRow()));
                RemoveColumn(withDataTable, withDataRow, distinct);
            }

            return result;
        }

        private DataTable GetBetweenEqualData(DataTable fromDataTable, DataTable withDataTable, bool distinct)
        {
            DataTable result = CreateTableResult(fromDataTable);

            foreach (DataRow deDataRow in fromDataTable.Rows.AsParallel())
            {
                DataRow withDataRow = GetByKey(deDataRow, withDataTable);

                if (withDataRow == null || !IsEquals(deDataRow, withDataRow))
                {
                    RemoveColumn(withDataTable, withDataRow, distinct);
                    continue;
                }

                result.Rows.Add(GetResult(deDataRow, withDataRow, result.NewRow()));
                RemoveColumn(withDataTable, withDataRow, distinct);
            }

            return result;
        }

        private DataTable GetMergeData(DataTable fromDataTable, DataTable withDataTable, bool distinct)
        {
            DataTable result = CreateTableResult(fromDataTable);

            foreach (DataRow deDataRow in fromDataTable.Rows.AsParallel())
            {
                DataRow withDataRow = GetByKey(deDataRow, withDataTable);
                result.Rows.Add(GetResult(deDataRow, withDataRow, result.NewRow()));
                RemoveColumn(withDataTable, withDataRow, distinct);
            }

            return result;
        }

        private DataTable CreateTableResult(DataTable fromDataTable)
        {
            DataTable tabela = new DataTable();

            foreach (DataColumn columns in fromDataTable.Columns)
            {
                tabela.Columns.Add(new DataColumn($"from{columns.ColumnName}"));
                tabela.Columns.Add(new DataColumn($"with{columns.ColumnName}"));
            }

            return tabela;
        }

        private DataRow GetByKey(DataRow deDataRow, DataTable withDataTable)
        {
            foreach (DataRow withDataRow in withDataTable.Rows.AsParallel())
            {
                bool igual = true;

                foreach (DataColumn coluna in withDataTable.Columns.AsParallel())
                {
                    if (!coluna.ColumnName.Contains("Key"))
                    {
                        continue;
                    }

                    if (!IsEquals(deDataRow[coluna.ColumnName], withDataRow[coluna.ColumnName]))
                    {
                        igual = false;
                    }
                }

                if (igual)
                {
                    return withDataRow;
                }
            }

            return null;
        }

        private DataRow GetResult(DataRow deDataRow, DataRow withDataRow, DataRow resultadoDataRow)
        {
            foreach (DataColumn coluna in deDataRow.Table.Columns.AsParallel())
            {
                resultadoDataRow[$"from{coluna.ColumnName}"] = deDataRow[coluna.ColumnName];
                resultadoDataRow[$"with{coluna.ColumnName}"] = withDataRow == null ? DBNull.Value : withDataRow[coluna.ColumnName];
            }

            return resultadoDataRow;
        }

        private bool IsEquals(DataRow deDataRow, DataRow withDataRow)
        {
            foreach (DataColumn coluna in deDataRow.Table.Columns.AsParallel())
            {
                if (!coluna.ColumnName.StartsWith("Cp"))
                {
                    continue;
                }

                if (!IsEquals(deDataRow[coluna.ColumnName], withDataRow[coluna.ColumnName]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsEquals(object deRow, object comRow)
        {
            if (DBNull.Value.Equals(deRow) && DBNull.Value.Equals(comRow))
            {
                return true;
            }

            if (!DBNull.Value.Equals(deRow) && DBNull.Value.Equals(comRow) ||
                (DBNull.Value.Equals(deRow) && !DBNull.Value.Equals(comRow)))
            {
                return false;
            }

            if (deRow.GetType() == typeof(int) ||
                deRow.GetType() == typeof(long))
            {
                return Convert.ToInt32(deRow) == Convert.ToInt32(comRow);
            }

            if (deRow.GetType() == typeof(decimal) ||
                deRow.GetType() == typeof(double) ||
                deRow.GetType() == typeof(float))
            {
                return Convert.ToDecimal(deRow) == Convert.ToDecimal(comRow);
            }

            if (deRow.GetType() == typeof(string) ||
                deRow.GetType() == typeof(char))
            {
                return Convert.ToString(deRow) == Convert.ToString(comRow);
            }

            if (deRow.GetType() == typeof(bool))
            {
                return Convert.ToBoolean(deRow) == Convert.ToBoolean(comRow);
            }

            if (deRow.GetType() == typeof(DateTime))
            {
                return Convert.ToDateTime(deRow) == Convert.ToDateTime(comRow);
            }

            return deRow == comRow;
        }

        private void RemoveColumn(DataTable dataTable, DataRow dataRow, bool distinct)
        {
            if (!distinct || dataRow == null)
            {
                return;
            }

            dataTable.Rows.Remove(dataRow);
        }
    }
}