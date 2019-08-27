using DBComparer.Helpers;
using DBComparer.Repositories;
using DBComparer.Services;
using DBComparer.Systems;
using DevExpress.Data;
using DevExpress.Office.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraRichEdit.Services;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DBComparer.Views
{
    public partial class QueryEditView : UserControl
    {
        private ServiceComparator appService;
        private string ConnectionStringFireBird { get { return ConfigurationManager.ConnectionStrings["FireBirdConnectionString"].ToString(); } }
        private string ConnectionStringSql { get { return ConfigurationManager.ConnectionStrings["SqlConnectionString"].ToString(); } }
        private string DataBaseNameVsicoci { get { return ConfigurationManager.AppSettings["BancoOdbc"]; } }
        private string SenhaBancoOdbc { get { return ConfigurationManager.AppSettings["SenhaBancoOdbc"]; } }

        public QueryEditView()
        {
            InitializeComponent();
            ConfigureView();
            ConfigureServices();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                Execult();
                return false;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ConfigureServices()
        {
            appService = new ServiceComparator(ConnectionStringSql, DataBaseNameVsicoci, SenhaBancoOdbc, ConnectionStringFireBird);
        }

        private void ConfigureView()
        {
            GridHelper.ConfigureGridNotEditable(gridView);
            GridHelper.ConfigureGridNotEditable(tabelasGridView);
            richEditControl.ReplaceService<ISyntaxHighlightService>(new CustomSyntaxHighlightService(richEditControl.Document));
            richEditControl.LoadDocument("Sql.sql");
            richEditControl.Document.Sections[0].Page.Width = Units.InchesToDocumentsF(80f);
            richEditControl.Document.DefaultCharacterProperties.FontName = "Courier New";
            typeImageComboBoxEdit.AddEnum<ConnectionType>(false);
            typeImageComboBoxEdit.SelectedIndex = 0;
        }

        private void OpenLoading()
        {
            Application.DoEvents();

            if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
            {
                CloseLoadind();
            }

            SplashScreenManager.ShowForm(typeof(LoadingForm), true, true);
        }

        private void CloseLoadind()
        {
            Application.DoEvents();

            if (SplashScreenManager.Default != null && !SplashScreenManager.Default.IsSplashFormVisible)
            {
                return;
            }

            SplashScreenManager.CloseForm(false, true);
        }

        private void Clear()
        {
            gridControl.DataSource = null;
            gridControl.RefreshDataSource();
            gridView.Columns.Clear();
            gridView.RefreshData();
        }

        private void ConfigureSummary()
        {
            gridView.Columns[0].Summary.Clear();
            gridView.Columns[0].Summary.Add(SummaryItemType.Count, gridView.Columns[0].FieldName);
            gridControl.RefreshDataSource();
            gridView.RefreshData();
        }

        private string GetScriptByType(ConnectionType? type)
        {
            if (type == ConnectionType.Excel)
            {
                FileInfo fileInfo = FileDialogHelper.GetFile("Select a file", new string[] { "xls", "xlsx" });

                if (fileInfo == null)
                {
                    return string.Empty;
                }

                return fileInfo.FullName;
            }

            string selectedText = richEditControl.Document.GetText(richEditControl.Document.Selection);
            return string.IsNullOrEmpty(selectedText) ? richEditControl.Text : selectedText;
        }

        private bool AllowsToConsult(ConnectionType? type, string script)
        {
            IList<string> messages = new List<string>();

            if (string.IsNullOrEmpty(script))
            {
                messages.Add(type == ConnectionType.Excel ? "Invalid directory." : "Required script.");
            }

            if (type == null)
            {
                messages.Add("Required type.");
            }

            if (messages.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, messages));
                return false;
            }

            return true;
        }

        private void LoadTableByType(ConnectionType type)
        {
            if (appService == null)
            {
                ConfigureServices();
            }

            switch (type)
            {
                case ConnectionType.SqlServer:
                    tablesGridControl.DataSource = appService.GetByType(ConnectionType.SqlServer, "SELECT name AS TABELA FROM SYS.tables ORDER BY name ASC");
                    tablesGridControl.RefreshDataSource();
                    break;
                case ConnectionType.Odbc:
                    tablesGridControl.DataSource = appService.GetByType(ConnectionType.Odbc, "SELECT CONVERT(VARCHAR(500),O.NAME) AS TABELA FROM SYSOBJECTS O WHERE TYPE = 'U' ORDER BY O.NAME");
                    tablesGridControl.RefreshDataSource();
                    break;
                case ConnectionType.Firebird:
                    tablesGridControl.DataSource = appService.GetByType(ConnectionType.Firebird, "SELECT a.RDB$RELATION_NAME TABELA FROM RDB$RELATIONS a");
                    tablesGridControl.RefreshDataSource();
                    break;
                default:
                    tablesGridControl.DataSource = null;
                    tablesGridControl.RefreshDataSource();
                    break;
            }
        }

        private void LoadDataByType(ConnectionType? type, string scriptOrPath)
        {
            if (type == null)
            {
                return;
            }

            gridControl.DataSource = appService.GetByType(type.Value, scriptOrPath);
            ConfigureSummary();
        }

        private void Execult()
        {
            try
            {
                OpenLoading();
                Clear();

                ConnectionType? type = (ConnectionType?)typeImageComboBoxEdit.EditValue;
                string scriptOrPath = GetScriptByType(type);

                if (!AllowsToConsult(type, scriptOrPath))
                {
                    return;
                }

                LoadDataByType(type, scriptOrPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseLoadind();
            }
        }

        #region Events

        private void execultSimpleButton_Click(object sender, EventArgs e)
        {
            Execult();
        }

        private void exportExcelSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLoading();
                string file = FileHelper.CreateFile("QueryResult");
                gridControl.ExportToXlsx(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseLoadind();
            }
        }

        private void clearSimpleButton_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void typeImageComboBoxEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                OpenLoading();
                Clear();
                ConnectionType? type = (ConnectionType?)(sender as ImageComboBoxEdit).EditValue;
                richLayoutControlItem.Visibility = type == ConnectionType.Excel ? LayoutVisibility.Never : LayoutVisibility.Always;

                if (type != null)
                {
                    LoadTableByType(type.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseLoadind();
            }
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                OpenLoading();
                Clear();

                GridView gridView = sender as GridView;
                DataRow row = gridView.GetFocusedDataRow();

                if (row != null && row["TABELA"] != DBNull.Value)
                {
                    richEditControl.Text = $"SELECT * FROM {row["TABELA"]}";
                    richEditControl.Update();
                    LoadDataByType((ConnectionType?)typeImageComboBoxEdit.EditValue, richEditControl.Text);
                    richEditControl.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseLoadind();
            }
        }

        #endregion
    }
}