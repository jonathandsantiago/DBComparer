using ComparadorDadosSQL.Helpers;
using ComparadorDadosSQL.Repositorios;
using ComparadorDadosSQL.Servicos;
using ComparadorDadosSQL.Sistemas;
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

namespace ComparadorDadosSQL.Consultas
{
    public partial class ConsultaEditView : UserControl
    {
        private ComparadorServico servico;
        private string ConnectionStringFireBird { get { return ConfigurationManager.ConnectionStrings["FireBirdConnectionString"].ToString(); } }
        private string ConnectionStringSql { get { return ConfigurationManager.ConnectionStrings["SqlConnectionString"].ToString(); } }
        private string DataBaseNameVsicoci { get { return ConfigurationManager.AppSettings["BancoOdbc"]; } }
        private string SenhaBancoOdbc { get { return ConfigurationManager.AppSettings["SenhaBancoOdbc"]; } }

        public ConsultaEditView()
        {
            InitializeComponent();
            ConfigurarView();
            ConfigurarServicos();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                ExecutarConsulta();
                return false;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ConfigurarServicos()
        {
            servico = new ComparadorServico(ConnectionStringSql, DataBaseNameVsicoci, SenhaBancoOdbc, ConnectionStringFireBird);
        }

        private void ConfigurarView()
        {
            GridHelper.ConfigurarGridNaoEditavel(gridView);
            GridHelper.ConfigurarGridNaoEditavel(tabelasGridView);
            richEditControl.ReplaceService<ISyntaxHighlightService>(new CustomSyntaxHighlightService(richEditControl.Document));
            richEditControl.LoadDocument("Sql.sql");
            richEditControl.Document.Sections[0].Page.Width = Units.InchesToDocumentsF(80f);
            richEditControl.Document.DefaultCharacterProperties.FontName = "Courier New";
            tipoImageComboBoxEdit.AddEnum<ConexaoTipo>(false);
            tipoImageComboBoxEdit.SelectedIndex = 0;
        }

        private void ExibirLoading()
        {
            Application.DoEvents();

            if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
            {
                FecharLoadind();
            }

            SplashScreenManager.ShowForm(typeof(LoadingForm), true, true);
        }

        private void FecharLoadind()
        {
            Application.DoEvents();

            if (SplashScreenManager.Default != null && !SplashScreenManager.Default.IsSplashFormVisible)
            {
                return;
            }

            SplashScreenManager.CloseForm(false, true);
        }

        private void Limpar()
        {
            gridControl.DataSource = null;
            gridControl.RefreshDataSource();
            gridView.Columns.Clear();
            gridView.RefreshData();
        }

        private void Totalizar()
        {
            gridView.Columns[0].Summary.Clear();
            gridView.Columns[0].Summary.Add(SummaryItemType.Count, gridView.Columns[0].FieldName);
            gridControl.RefreshDataSource();
            gridView.RefreshData();
        }

        private string ObterScriptOuPathPorTipo(ConexaoTipo? tipo)
        {
            if (tipo == ConexaoTipo.Excel)
            {
                FileInfo fileInfo = FileDialogHelper.GetFile("Selecione um arquivo", new string[] { "xls", "xlsx" });

                if (fileInfo == null)
                {
                    return string.Empty;
                }

                return fileInfo.FullName;
            }

            string selectedText = richEditControl.Document.GetText(richEditControl.Document.Selection);
            return string.IsNullOrEmpty(selectedText) ? richEditControl.Text : selectedText;
        }

        private bool PermiteConsultar(ConexaoTipo? tipo, string script)
        {
            IList<string> mensagens = new List<string>();

            if (string.IsNullOrEmpty(script))
            {
                mensagens.Add(tipo == ConexaoTipo.Excel ? "Diretório inválido." : "Script obrigatório.");
            }

            if (tipo == null)
            {
                mensagens.Add("Tipo obrigatório.");
            }

            if (mensagens.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, mensagens));
                return false;
            }

            return true;
        }

        private void CarregarTabelasPorTipo(ConexaoTipo tipo)
        {
            if (servico == null)
            {
                ConfigurarServicos();
            }

            switch (tipo)
            {
                case ConexaoTipo.SqlServer:
                    tabelasGridControl.DataSource = servico.ObterPorTipo(ConexaoTipo.SqlServer, "SELECT name AS TABELA FROM SYS.tables ORDER BY name ASC");
                    tabelasGridControl.RefreshDataSource();
                    break;
                case ConexaoTipo.Odbc:
                    tabelasGridControl.DataSource = servico.ObterPorTipo(ConexaoTipo.Odbc, "SELECT CONVERT(VARCHAR(500),O.NAME) AS TABELA FROM SYSOBJECTS O WHERE TYPE = 'U' ORDER BY O.NAME");
                    tabelasGridControl.RefreshDataSource();
                    break;
                case ConexaoTipo.Firebird:
                    tabelasGridControl.DataSource = servico.ObterPorTipo(ConexaoTipo.Firebird, "SELECT a.RDB$RELATION_NAME TABELA FROM RDB$RELATIONS a");
                    tabelasGridControl.RefreshDataSource();
                    break;
                default:
                    tabelasGridControl.DataSource = null;
                    tabelasGridControl.RefreshDataSource();
                    break;
            }
        }

        private void CarregarDadosPorTipo(ConexaoTipo? tipo, string scriptOrPath)
        {
            if (tipo == null)
            {
                return;
            }

            gridControl.DataSource = servico.ObterPorTipo(tipo.Value, scriptOrPath);
            Totalizar();
        }

        private void ExecutarConsulta()
        {
            try
            {
                ExibirLoading();
                Limpar();

                ConexaoTipo? tipo = (ConexaoTipo?)tipoImageComboBoxEdit.EditValue;
                string scriptOrPath = ObterScriptOuPathPorTipo(tipo);

                if (!PermiteConsultar(tipo, scriptOrPath))
                {
                    return;
                }

                CarregarDadosPorTipo(tipo, scriptOrPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                FecharLoadind();
            }
        }

        #region Eventos

        private void execultarConsultaDeSimpleButton_Click(object sender, EventArgs e)
        {
            ExecutarConsulta();
        }

        private void exportarExcelSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                ExibirLoading();
                string file = FileHelper.CreateFile("ResultadoConsulta");
                gridControl.ExportToXlsx(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                FecharLoadind();
            }
        }

        private void limparSimpleButton_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void tipoImageComboBoxEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ExibirLoading();
                Limpar();
                ConexaoTipo? tipo = (ConexaoTipo?)(sender as ImageComboBoxEdit).EditValue;
                richLayoutControlItem.Visibility = tipo == ConexaoTipo.Excel ? LayoutVisibility.Never : LayoutVisibility.Always;

                if (tipo != null)
                {
                    CarregarTabelasPorTipo(tipo.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                FecharLoadind();
            }
        }

        private void TabelasGridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ExibirLoading();
                Limpar();
                GridView gridView = sender as GridView;
                DataRow row = gridView.GetFocusedDataRow();

                if (row != null && row["TABELA"] != DBNull.Value)
                {
                    richEditControl.Text = $"SELECT * FROM {row["TABELA"]}";
                    richEditControl.Update();
                    CarregarDadosPorTipo((ConexaoTipo?)tipoImageComboBoxEdit.EditValue, richEditControl.Text);
                    richEditControl.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                FecharLoadind();
            }
        }

        #endregion
    }
}