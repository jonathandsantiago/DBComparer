using Microsoft.Win32;
using System;
using System.Configuration;

namespace DBComparer.Systems
{
    public static class SybaseOdbcManager
    {
        private static string OdbcIniRegPath = "SOFTWARE\\ODBC\\ODBC.INI\\";
        private static string OdbcInstIniRegPath = "SOFTWARE\\ODBC\\ODBCINST.INI\\";
        private static string driverName;
        private static string driverNameSybase12 = "SQL Anywhere 12";
        private static string driverNameSybase9 = "Adaptive Server Anywhere 9.0";
        private static string dsnName = "DBComparer";
        private static bool serviceDnsExists = false;

        static SybaseOdbcManager()
        { }

        public static bool ServiceDsnExists()
        {
            if (serviceDnsExists)
            {
                return true;
            }

            var driversKey = Registry.LocalMachine.CreateSubKey(OdbcIniRegPath + "ODBC Data Sources");

            if (driversKey == null)
            {
                throw new Exception("ODBC Registry key for drivers does not exist");
            }

            serviceDnsExists = driversKey.GetValue(dsnName) != null;

            return serviceDnsExists;
        }

        public static bool CreateServiceDns(string dataBaseName)
        {
            try
            {
                if (ServiceDsnExists())
                {
                    ChangeEngineName(dataBaseName);
                    return true;
                }

                string driverPath = GetDriverPath();

                var datasourcesKey = Registry.LocalMachine.CreateSubKey(OdbcIniRegPath + "ODBC Data Sources");

                if (datasourcesKey == null)
                {
                    throw new Exception("ODBC Registry key for datasources does not exist");
                }

                datasourcesKey.SetValue(dsnName, driverName);

                var dsnKey = Registry.LocalMachine.CreateSubKey(OdbcIniRegPath + dsnName);

                if (dsnKey == null)
                {
                    throw new Exception("ODBC Registry key for DSN was not created");
                }

                dsnKey.SetValue("AutoStop", "YES");
                dsnKey.SetValue("CommBufferSize", 1460);
                dsnKey.SetValue("CommLinks", "TCPIP,SharedMemory");
                dsnKey.SetValue("Description", driverName);
                dsnKey.SetValue("Driver", driverPath);
                dsnKey.SetValue("EngineName", dataBaseName);
                dsnKey.SetValue("Integrated", "No");
                dsnKey.SetValue("LiveNessTimeOut", 28600);
                dsnKey.SetValue("UID", "dba");
                dsnKey.SetValue("CHARSET", "CP1252");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Erro ao tentar criar DNS ODBC", ex));
            }
        }

        private static void ChangeEngineName(string dataBaseName)
        {
            if (string.IsNullOrEmpty(dataBaseName))
            {
                return;
            }

            var key = Registry.LocalMachine.OpenSubKey(OdbcIniRegPath + dsnName, true);

            if (key != null)
            {
                key.SetValue("EngineName", dataBaseName);
            }
        }

        public static string GetDriverPath()
        {
            RegistryKey driverKey = Registry.LocalMachine.CreateSubKey(OdbcInstIniRegPath + driverNameSybase12);

            driverName = driverKey == null ? driverNameSybase9 : driverNameSybase12;
            driverKey = driverKey ?? Registry.LocalMachine.CreateSubKey(OdbcInstIniRegPath + driverNameSybase9);

            if (driverKey == null)
            {
                throw new Exception(string.Format("ODBC Registry key for driver '{0}' does not exist", driverName));
            }

            return driverKey.GetValue("Driver").ToString();
        }

        public static string GetCurrentDsn()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ODBC"]))
            {
                return ConfigurationManager.AppSettings["ODBC"];
            }
            else if (serviceDnsExists)
            {
                return dsnName;
            }
            else
            {
                throw new InvalidOperationException("No ODBC DSN has been defined and the service could not create.");
            }
        }
    }
}
