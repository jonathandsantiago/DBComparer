using System;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using DBComparer.Systems;
using FirebirdSql.Data.FirebirdClient;
using System.IO;
using ExcelDataReader;
using DBComparer.Helpers;

namespace DBComparer.Repositories
{
    public class RepositoryComparator
    {
        protected FbConnection conectionFireBird;
        protected SqlConnection conectionSql;
        protected SqlTransaction sqlTransaction;
        protected OdbcConnection conectionOdbc;
        protected OdbcTransaction odbcTransaction;

        public RepositoryComparator(string connectionStringSql, string dataBaseNameOdbc, string passwordOdbc, string connectionStringFireBird)
        {
            conectionFireBird = DBHelper.GetInstance().GetFireBirdConnection(connectionStringFireBird);
            conectionSql = DBHelper.GetInstance().GetSqlConnection(connectionStringSql);
            conectionOdbc = DBHelper.GetInstance().GetOdbcConnection(dataBaseNameOdbc, passwordOdbc);
        }

        public static string ValidateConnection(string connectionStringSql, string dataBaseNameVsicoci, string senhaBancoOdbc, string connectionStringFireBird)
        {
            RepositoryComparator repository = new RepositoryComparator(connectionStringSql, dataBaseNameVsicoci, senhaBancoOdbc, connectionStringFireBird);
            string message = string.Empty;

            if (!repository.TestConnectionByType(ConnectionType.SqlServer))
            {
                message += "Could not connect to SqlServer." + Environment.NewLine;
            }

            if (!repository.TestConnectionByType(ConnectionType.Odbc))
            {
                message += "Could not connect to SysBase." + Environment.NewLine;
            }

            return message;
        }

        public bool TestConnectionByType(ConnectionType type, string tableName = "")
        {
            try
            {
                IDbConnection connection = ObterConexaoPorTipo(type);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT NULL " + tableName;
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataTable GetByType(ConnectionType type, string scriptOrPath)
        {
            switch (type)
            {
                case ConnectionType.SqlServer:
                    return GetSqlDataTable(scriptOrPath);
                case ConnectionType.Odbc:
                    return GetOdbcDataTable(scriptOrPath);
                case ConnectionType.Firebird:
                    return GetFireBirdDataTable(scriptOrPath);
                case ConnectionType.Excel:
                    return GetExcelDataTable(scriptOrPath);
                default:
                    throw new NotImplementedException();
            }
        }

        private IDbConnection ObterConexaoPorTipo(ConnectionType tipo)
        {
            switch (tipo)
            {
                case ConnectionType.SqlServer:
                    return conectionSql;
                case ConnectionType.Odbc:
                    return conectionOdbc;
                default:
                    throw new NotImplementedException();
            }
        }

        #region Sql server

        public virtual DataTable GetSqlDataTable(string scriptSelect)
        {
            try
            {
                if (conectionSql.State != ConnectionState.Open)
                {
                    conectionSql.Open();
                }

                if (scriptSelect.ToLower().Contains("select"))
                {
                    SqlCommand command = new SqlCommand(scriptSelect, conectionSql);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataSet dataSet = new DataSet();

                    dataAdapter.Fill(dataSet);

                    return dataSet?.Tables[0];
                }
                else
                {
                    return new SqlCommand(scriptSelect, conectionSql).ExecuteNonQuery() >= 0 ? GetDataTableDefault() : new DataTable();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                conectionSql.Close();
            }
        }

        public virtual SqlTransaction StartSqlTransaction()
        {
            conectionSql.Open();
            return conectionSql.BeginTransaction();
        }

        public virtual void SqlExecuteNonQuery(string scriptSelect, SqlTransaction transaction)
        {
            try
            {
                SqlCommand command = new SqlCommand(scriptSelect, conectionSql)
                {
                    Transaction = transaction
                };

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region Odbc        

        public virtual DataTable GetOdbcDataTable(string scriptSelect)
        {
            try
            {
                if (conectionOdbc.State != ConnectionState.Open)
                {
                    conectionOdbc.Open();
                }

                if (scriptSelect.ToLower().Contains("select"))
                {
                    OdbcCommand command = new OdbcCommand(scriptSelect, conectionOdbc);
                    OdbcDataAdapter dataAdapter = new OdbcDataAdapter(command);
                    DataSet dataSet = new DataSet();

                    dataAdapter.Fill(dataSet);

                    return dataSet?.Tables[0];
                }
                else
                {
                    return new OdbcCommand(scriptSelect, conectionOdbc).ExecuteNonQuery() >= 0 ? GetDataTableDefault() : new DataTable();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                conectionSql.Close();
            }
        }

        public virtual OdbcTransaction StartOdbcTransaction()
        {
            conectionOdbc.Open();
            return conectionOdbc.BeginTransaction();
        }

        public virtual void OdbcExecuteNonQuery(string scriptSelect, OdbcTransaction transaction)
        {
            try
            {
                OdbcCommand command = new OdbcCommand(scriptSelect, conectionOdbc)
                {
                    Transaction = transaction
                };

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region FireBird

        public virtual DataTable GetFireBirdDataTable(string scriptSelect)
        {
            try
            {
                if (conectionFireBird.State != ConnectionState.Open)
                {
                    conectionFireBird.Open();
                }

                if (scriptSelect.ToLower().Contains("select"))
                {
                    FbDataAdapter dataAdapter = new FbDataAdapter(new FbCommand(scriptSelect, conectionFireBird));

                    DataTable values = new DataTable();
                    dataAdapter.Fill(values);

                    return values;
                }
                else
                {
                    return new FbCommand(scriptSelect, conectionFireBird).ExecuteNonQuery() >= 0 ? GetDataTableDefault() : new DataTable();
                }
            }
            catch (FbException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                conectionFireBird.Close();
            }
        }


        private DataTable GetDataTableDefault()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Messages", typeof(string)));
            DataRow dataRow = table.NewRow();
            dataRow["Messages"] = "Command(s) completed successfully.";
            table.Rows.Add(dataRow);
            return table;
        }

        #endregion

        #region Excel

        public virtual DataTable GetExcelDataTable(string storePath)
        {
            FileStream stream = File.Open(storePath, FileMode.Open, FileAccess.Read);

            try
            {
                string fileExtension = Path.GetExtension(storePath);
                IExcelDataReader excelReader = null;

                if (fileExtension.ToLower() == ".xls")
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (fileExtension.ToLower() == ".xlsx")
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }

                DataSet result = excelReader.AsDataSet();

                if (result == null)
                {
                    throw new Exception("File failed to load.");
                }

                DataTable table = result.Tables[0];
                SetColumnNameByFirstRecord(table);

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                stream.Close();
            }
        }

        private void SetColumnNameByFirstRecord(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
            {
                return;
            }

            DataRow dataRow = table.Rows[0];

            foreach (DataColumn column in table.Columns)
            {
                var name = dataRow.GetValueDataRow<string>(column.ColumnName);

                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                column.ColumnName = name;
                column.Caption = name;
            }

            table.Rows[0].Delete();
            table.AcceptChanges();
        }

        #endregion
    }

    public enum ConnectionType
    {
        SqlServer,
        Odbc,
        Excel,
        Firebird
    }
}
