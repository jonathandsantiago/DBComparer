using DBComparer.Helpers;
using DBComparer.Repositories;
using DBComparer.Services;
using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
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
    public partial class ComparatorEditView : UserControl
    {
        private ServiceComparator appService;
        public ComparatorEditView()
        {
            InitializeComponent();
            ConfigureView();

            string connectionStringFireBird = ConfigurationManager.ConnectionStrings["FireBirdConnectionString"].ToString();
            string connectionStringSql = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ToString();
            string dataBaseNameVsicoci = ConfigurationManager.AppSettings["BancoOdbc"];
            string senhaBancoOdbc = ConfigurationManager.AppSettings["SenhaBancoOdbc"];
            appService = new ServiceComparator(connectionStringSql, dataBaseNameVsicoci, senhaBancoOdbc, connectionStringFireBird);
        }

        #region Actions

        private void ConfigureView()
        {
            GridHelper.ConfigureGridNotEditable(fromGridView);
            GridHelper.ConfigureGridNotEditable(withGridView);
            GridHelper.ConfigureGridNotEditable(resultGridView);
            exportExcelSimpleButton.Enabled = false;

            fromMemoEdit.Properties.NullValuePrompt = @"Ex: --SQL
	            SELECT Top 2 Id as KeyProdutoId,
		                CpCodigoExterno,
                        Descricao 
                FROM	TBESTPRODUTO
                WHERE	EmpresaId = 1
                ORDER BY Id
	
            All columns that have Key at the beginning will be used as key.
            All columns that have Cp at the beginning will be used as a comparator.";

            withMemoEdit.Properties.NullValuePrompt = @"Ex: --ODBC 
	            SELECT 	TOP 2 B.ID_HOSPDNET AS KeyProdutoId,
                        A.CODIGO_INT AS CpCodigoExterno,
		                A.DESCRICAO AS Descricao 
                FROM TBESTPRODUTO A
	                INNER JOIN TBESTPRODUTO_LIG_HOSPDNET B ON
		                A.EMPRESA = B.EMPRESA AND
		                A.CODIGO_INT = B.CODIGO
                WHERE A.EMPRESA = '001'
                ORDER BY B.ID_HOSPDNET
	
            All columns that have Key at the beginning will be used as key.
            All columns that have Cp at the beginning will be used as a comparator.";

            filterResultMemoEdit.Properties.NullValuePrompt = @"Ex: [CpProdutoId] = 1";

            typeFromImageComboBoxEdit.AddEnum<ConnectionType>();
            typeWithImageComboBoxEdit.AddEnum<ConnectionType>();
            comparatorTypeImageComboBoxEdit.AddEnum<ComparatorType>();
        }

        private bool AllowsToConsult(ConnectionType? type, string script)
        {
            IList<string> messages = new List<string>();

            if (string.IsNullOrEmpty(script))
            {
                messages.Add(type == ConnectionType.Excel ? "Invalid Directory." : "Required script.");
            }

            if (type == null)
            {
                messages.Add("Required Type.");
            }

            if (messages.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, messages));
                return false;
            }

            return true;
        }

        private bool AllowCompare(ComparatorType? type, DataTable from, DataTable with)
        {
            IList<string> mensagens = new List<string>();

            if (type == null)
            {
                mensagens.Add("Invalid comparison type.");
            }

            if (from == null || from.Rows.Count == 0)
            {
                mensagens.Add("Must have 'From' data for comparison.");
            }

            if (with == null || with.Rows.Count == 0)
            {
                mensagens.Add("Must have 'With' data for comparison.");
            }

            if (mensagens.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, mensagens));
                return false;
            }

            return true;
        }

        private void ConfigureExportButton()
        {
            exportExcelSimpleButton.Enabled =
                (resultGridControl.DataSource is DataTable) &&
                (resultGridControl.DataSource as DataTable).Rows.Count > 0;
        }

        private void ClearGridView(GridView gridView, GridControl gridControl)
        {
            gridControl.DataSource = null;
            gridControl.RefreshDataSource();
            gridView.Columns.Clear();
            gridView.RefreshData();
        }

        private void CreateSummaryGridView(GridView gridView, GridControl gridControl)
        {
            gridView.Columns[0].Summary.Clear();
            gridView.Columns[0].Summary.Add(SummaryItemType.Count, gridView.Columns[0].FieldName);
            gridControl.RefreshDataSource();
            gridView.RefreshData();
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

        public string InsertAtBeginningIfNotEqual(string text, string textInsert)
        {
            if (text.IndexOf(textInsert) != 0)
            {
                text = textInsert + text;
            }

            return text;
        }

        private string ObterScriptOuPathPorTipo(ConnectionType? type)
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

            return fromMemoEdit.Text;
        }

        #endregion

        #region Events

        private void executeQueryFromSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLoading();
                ClearGridView(fromGridView, fromGridControl);

                ConnectionType? type = (ConnectionType?)typeFromImageComboBoxEdit.EditValue;
                string scriptOrPath = ObterScriptOuPathPorTipo(type);

                if (!AllowsToConsult(type, scriptOrPath))
                {
                    return;
                }

                DataTable table = appService.GetByType(type.Value, scriptOrPath);
                fromGridControl.DataSource = table;
                CreateSummaryGridView(fromGridView, fromGridControl);
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

        private void executeQueryWithSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLoading();
                ClearGridView(withGridView, withGridControl);

                ConnectionType? type = (ConnectionType?)typeWithImageComboBoxEdit.EditValue;
                string script = withMemoEdit.Text;

                if (!AllowsToConsult(type, script))
                {
                    return;
                }

                if (type == ConnectionType.Excel)
                {

                }

                DataTable table = appService.GetByType(type.Value, script);
                withGridControl.DataSource = table;
                CreateSummaryGridView(withGridView, withGridControl);
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

        private void executeSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLoading();
                ClearGridView(resultGridView, resultGridControl);

                ComparatorType? type = (ComparatorType?)comparatorTypeImageComboBoxEdit.EditValue;
                DataTable from = fromGridControl.DataSource as DataTable;
                DataTable with = withGridControl.DataSource as DataTable;

                if (!AllowCompare(type, from, with))
                {
                    return;
                }

                DataTable table = appService.GetResultComparisonByType(type.Value, from.Copy(), with.Copy(), distinctCheckBox.Checked);
                resultGridControl.DataSource = table;
                CreateSummaryGridView(resultGridView, resultGridControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConfigureExportButton();
                CloseLoadind();
            }
        }

        private void filterResultSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                resultGridView.ActiveFilterString = filterResultMemoEdit.Text;
                resultGridControl.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConfigureExportButton();
            }
        }

        private void exporteExcelSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLoading();
                string file = FileHelper.CreateFile("Result");
                resultGridControl.ExportToXlsx(file);
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

        private void clearFromSimpleButton_Click(object sender, EventArgs e)
        {
            ClearGridView(fromGridView, fromGridControl);
        }

        private void clearWithSimpleButton_Click(object sender, EventArgs e)
        {
            ClearGridView(withGridView, withGridControl);
        }

        private void excelFromSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLoading();
                string file = FileHelper.CreateFile("From");
                fromGridControl.ExportToXlsx(file);
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

        private void excelWithSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenLoading();
                string file = FileHelper.CreateFile("With");
                withGridControl.ExportToXlsx(file);
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
