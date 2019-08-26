using ComparadorDadosSQL.Helpers;
using ComparadorDadosSQL.Repositorios;
using ComparadorDadosSQL.Servicos;
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

namespace ComparadorDadosSQL.Consultas
{
    public partial class ComparadorEditView : UserControl
    {
        private ComparadorServico servico;
        public ComparadorEditView()
        {
            InitializeComponent();
            ConfigurarView();

            string connectionStringFireBird = ConfigurationManager.ConnectionStrings["FireBirdConnectionString"].ToString();
            string connectionStringSql = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ToString();
            string dataBaseNameVsicoci = ConfigurationManager.AppSettings["BancoOdbc"];
            string senhaBancoOdbc = ConfigurationManager.AppSettings["SenhaBancoOdbc"];
            servico = new ComparadorServico(connectionStringSql, dataBaseNameVsicoci, senhaBancoOdbc, connectionStringFireBird);
        }

        #region Ações

        private void ConfigurarView()
        {
            GridHelper.ConfigurarGridNaoEditavel(deGridView);
            GridHelper.ConfigurarGridNaoEditavel(comGridView);
            GridHelper.ConfigurarGridNaoEditavel(resultadoGridView);
            exporteExcelSimpleButton.Enabled = false;

            deMemoEdit.Properties.NullValuePrompt = @"Ex: --SQL
	            SELECT Top 2 Id as KeyProdutoId,
		                CpCodigoExterno,
                        Descricao 
                FROM	TBESTPRODUTO
                WHERE	EmpresaId = 1
                ORDER BY Id
	
            Todas as colunas que tiverem Key no inicio sera utilizada como chave.
            Todas as colunas que tiverem Cp no inicio sera utilizada como comparador.";

            comMemoEdit.Properties.NullValuePrompt = @"Ex: --ODBC 
	            SELECT 	TOP 2 B.ID_HOSPDNET AS KeyProdutoId,
                        A.CODIGO_INT AS CpCodigoExterno,
		                A.DESCRICAO AS Descricao 
                FROM TBESTPRODUTO A
	                INNER JOIN TBESTPRODUTO_LIG_HOSPDNET B ON
		                A.EMPRESA = B.EMPRESA AND
		                A.CODIGO_INT = B.CODIGO
                WHERE A.EMPRESA = '001'
                ORDER BY B.ID_HOSPDNET
	
            Todas as colunas que tiverem Key no inicio sera utilizada como chave.
            Todas as colunas que tiverem Cp no inicio sera utilizada como comparador.";

            filtroResultadoMemoEdit.Properties.NullValuePrompt = @"Ex: [CpProdutoId] = 1";

            tipoDeImageComboBoxEdit.AddEnum<ConexaoTipo>();
            tipoComImageComboBoxEdit.AddEnum<ConexaoTipo>();
            comparadorTipoImageComboBoxEdit.AddEnum<ComparadorTipo>();
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

        private bool PermiteComparar(ComparadorTipo? tipo, DataTable de, DataTable com)
        {
            IList<string> mensagens = new List<string>();

            if (tipo == null)
            {
                mensagens.Add("Tipo de comparação inválido.");
            }

            if (de == null || de.Rows.Count == 0)
            {
                mensagens.Add("É Preciso ter dados 'De' para comparação.");
            }

            if (com == null || com.Rows.Count == 0)
            {
                mensagens.Add("É Preciso ter dados 'Com' para comparação.");
            }

            if (mensagens.Any())
            {
                MessageBox.Show(string.Join(Environment.NewLine, mensagens));
                return false;
            }

            return true;
        }

        private void ConfigurarBotaoExportar()
        {
            exporteExcelSimpleButton.Enabled =
                (resultadoGridControl.DataSource is DataTable) &&
                (resultadoGridControl.DataSource as DataTable).Rows.Count > 0;
        }

        private void LimparGridView(GridView gridView, GridControl gridControl)
        {
            gridControl.DataSource = null;
            gridControl.RefreshDataSource();
            gridView.Columns.Clear();
            gridView.RefreshData();
        }

        private void CriarSummaryGridView(GridView gridView, GridControl gridControl)
        {
            gridView.Columns[0].Summary.Clear();
            gridView.Columns[0].Summary.Add(SummaryItemType.Count, gridView.Columns[0].FieldName);
            gridControl.RefreshDataSource();
            gridView.RefreshData();
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

        public string InserirNoInicioSeNaoForIgual(string texto, string textoInserir)
        {
            if (texto.IndexOf(textoInserir) != 0)
            {
                texto = textoInserir + texto;
            }

            return texto;
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

            return deMemoEdit.Text;
        }

        #endregion

        #region Eventos

        private void execultarConsultaDeSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                ExibirLoading();
                LimparGridView(deGridView, deGridControl);

                ConexaoTipo? tipo = (ConexaoTipo?)tipoDeImageComboBoxEdit.EditValue;
                string scriptOrPath = ObterScriptOuPathPorTipo(tipo);

                if (!PermiteConsultar(tipo, scriptOrPath))
                {
                    return;
                }

                DataTable registros = servico.ObterPorTipo(tipo.Value, scriptOrPath);
                deGridControl.DataSource = registros;
                CriarSummaryGridView(deGridView, deGridControl);
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

        private void execultarConsultaComSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                ExibirLoading();
                LimparGridView(comGridView, comGridControl);

                ConexaoTipo? tipo = (ConexaoTipo?)tipoComImageComboBoxEdit.EditValue;
                string script = comMemoEdit.Text;

                if (!PermiteConsultar(tipo, script))
                {
                    return;
                }

                if (tipo == ConexaoTipo.Excel)
                {

                }

                DataTable registros = servico.ObterPorTipo(tipo.Value, script);
                comGridControl.DataSource = registros;
                CriarSummaryGridView(comGridView, comGridControl);
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

        private void compararSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                ExibirLoading();
                LimparGridView(resultadoGridView, resultadoGridControl);

                ComparadorTipo? tipo = (ComparadorTipo?)comparadorTipoImageComboBoxEdit.EditValue;
                DataTable de = deGridControl.DataSource as DataTable;
                DataTable com = comGridControl.DataSource as DataTable;

                if (!PermiteComparar(tipo, de, com))
                {
                    return;
                }

                DataTable registros = servico.ObterResultadoComparacaoPorTipo(tipo.Value, de.Copy(), com.Copy(), distinctCheckBox.Checked);
                resultadoGridControl.DataSource = registros;
                CriarSummaryGridView(resultadoGridView, resultadoGridControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConfigurarBotaoExportar();
                FecharLoadind();
            }
        }

        private void filtroResultadoSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                resultadoGridView.ActiveFilterString = filtroResultadoMemoEdit.Text;
                resultadoGridControl.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConfigurarBotaoExportar();
            }
        }

        private void exporteExcelSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                ExibirLoading();
                string file = FileHelper.CreateFile("Resultado");
                resultadoGridControl.ExportToXlsx(file);
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

        private void deLimparSimpleButton_Click(object sender, EventArgs e)
        {
            LimparGridView(deGridView, deGridControl);
        }

        private void comLimparSimpleButton_Click(object sender, EventArgs e)
        {
            LimparGridView(comGridView, comGridControl);
        }

        private void deExcelSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                ExibirLoading();
                string file = FileHelper.CreateFile("De");
                deGridControl.ExportToXlsx(file);
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

        private void comExcelSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                ExibirLoading();
                string file = FileHelper.CreateFile("Com");
                comGridControl.ExportToXlsx(file);
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
