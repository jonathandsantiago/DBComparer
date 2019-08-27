using FirebirdSql.Data.FirebirdClient;
using System.Data.Odbc;
using System.Data.SqlClient;

namespace DBComparer.Systems
{
    public class DBHelper
    {
        private static readonly DBHelper instance = new DBHelper();

        private DBHelper() { }

        public static DBHelper GetInstance()
        {
            return instance;
        }

        public FbConnection GetFireBirdConnection(string connetionString)
        {
            return new FbConnection(connetionString);
        }

        public SqlConnection GetSqlConnection(string connetionString)
        {
            return new SqlConnection(connetionString);
        }

        public OdbcConnection GetOdbcConnection(string dataBaseName, string passwordOdbc)
        {
            string connetionString = $"Dsn={ SybaseOdbcManager.GetCurrentDsn() };UID=dba;Pwd={passwordOdbc};Server={dataBaseName}";
            return new OdbcConnection(connetionString);
        }
    }
}
