using System;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using ComparadorDadosSQL.Sistemas;
using FirebirdSql.Data.FirebirdClient;
using System.IO;
using ExcelDataReader;
using ComparadorDadosSQL.Helpers;

namespace ComparadorDadosSQL.Repositorios
{
    public class ComparadorRepositorio
    {
        protected FbConnection conectionFireBird;
        protected SqlConnection conectionSql;
        protected SqlTransaction sqlTransaction;
        protected OdbcConnection conectionOdbc;
        protected OdbcTransaction odbcTransaction;

        public ComparadorRepositorio(string connectionStringSql, string dataBaseNameVsicoci, string senhaBancoOdbc, string connectionStringFireBird)
        {
            conectionFireBird = DBHelper.GetInstance().GetFireBirdConnection(connectionStringFireBird);
            conectionSql = DBHelper.GetInstance().GetSqlConnection(connectionStringSql);
            conectionOdbc = DBHelper.GetInstance().GetOdbcConnection(dataBaseNameVsicoci, senhaBancoOdbc);
        }

        public static string ValidateConnection(string connectionStringSql, string dataBaseNameVsicoci, string senhaBancoOdbc, string connectionStringFireBird)
        {
            ComparadorRepositorio repositorio = new ComparadorRepositorio(connectionStringSql, dataBaseNameVsicoci, senhaBancoOdbc, connectionStringFireBird);
            string mensagem = string.Empty;

            if (!repositorio.TestarConexaoPorTipo(ConexaoTipo.SqlServer))
            {
                mensagem += "Não foi possivel estabelecer a conexão com o SqlServer." + Environment.NewLine;
            }

            if (!repositorio.TestarConexaoPorTipo(ConexaoTipo.Odbc))
            {
                mensagem += "Não foi possivel estabelecer a conexão com o SysBase." + Environment.NewLine;
            }

            return mensagem;
        }

        public bool TestarConexaoPorTipo(ConexaoTipo tipo, string table = "")
        {
            try
            {
                IDbConnection connection = ObterConexaoPorTipo(tipo);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT NULL " + table;
                    cmd.ExecuteScalar();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataTable ObterPorTipo(ConexaoTipo tipo, string scriptOrPath)
        {
            switch (tipo)
            {
                case ConexaoTipo.SqlServer:
                    return GetSqlDataTable(scriptOrPath);
                case ConexaoTipo.Odbc:
                    return GetOdbcDataTable(scriptOrPath);
                case ConexaoTipo.Firebird:
                    return GetFireBirdDataTable(scriptOrPath);
                case ConexaoTipo.Excel:
                    return GetExcelDataTable(scriptOrPath);
                default:
                    throw new NotImplementedException();
            }
        }

        private IDbConnection ObterConexaoPorTipo(ConexaoTipo tipo)
        {
            switch (tipo)
            {
                case ConexaoTipo.SqlServer:
                    return conectionSql;
                case ConexaoTipo.Odbc:
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
                    throw new Exception("Falha ao carregar a planilha.");
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

    public enum ConexaoTipo
    {
        SqlServer,
        Odbc,
        Excel,
        Firebird
    }
}
