using System;
using System.Data;
using System.IO;
using System.Linq;
using ComparadorDadosSQL.Helpers;
using ComparadorDadosSQL.Repositorios;
using ExcelDataReader;

namespace ComparadorDadosSQL.Servicos
{
    public class ComparadorServico
    {
        private ComparadorRepositorio repositorio;

        public ComparadorServico(string connectionStringSql, string dataBaseNameVsicoci, string senhaBancoOdbc, string connectionStringFireBird)
        {
            repositorio = new ComparadorRepositorio(connectionStringSql, dataBaseNameVsicoci, senhaBancoOdbc, connectionStringFireBird);
        }

        public DataTable ObterPorTipo(ConexaoTipo tipo, string scriptOrPath)
        {
            return repositorio.ObterPorTipo(tipo, scriptOrPath);
        }

        public DataTable ObterResultadoComparacaoPorTipo(ComparadorTipo tipo, DataTable deDataTable, DataTable comDataTable, bool distinct)
        {
            switch (tipo)
            {
                case ComparadorTipo.Diferente:
                    return ObterComparacaoEntreDadosDiferente(deDataTable, comDataTable, distinct);
                case ComparadorTipo.Igual:
                    return ObterComparacaoEntreDadosIguais(deDataTable, comDataTable, distinct);
                case ComparadorTipo.Merge:
                    return ObterDadosMergeados(deDataTable, comDataTable, distinct);
                default:
                    throw new NotImplementedException();
            }
        }

        private DataTable ObterComparacaoEntreDadosDiferente(DataTable deDataTable, DataTable comDataTable, bool distinct)
        {
            DataTable resultado = CriartabelaResultado(deDataTable);

            foreach (DataRow deDataRow in deDataTable.Rows.AsParallel())
            {
                DataRow comDataRow = ObterRegistroPorChave(deDataRow, comDataTable);

                if (comDataRow != null && RegistroIguais(deDataRow, comDataRow))
                {
                    RemoverColuna(comDataTable, comDataRow, distinct);
                    continue;
                }

                resultado.Rows.Add(ObterResultado(deDataRow, comDataRow, resultado.NewRow()));
                RemoverColuna(comDataTable, comDataRow, distinct);
            }

            return resultado;
        }

        private DataTable ObterComparacaoEntreDadosIguais(DataTable deDataTable, DataTable comDataTable, bool distinct)
        {
            DataTable resultado = CriartabelaResultado(deDataTable);

            foreach (DataRow deDataRow in deDataTable.Rows.AsParallel())
            {
                DataRow comDataRow = ObterRegistroPorChave(deDataRow, comDataTable);

                if (comDataRow == null || !RegistroIguais(deDataRow, comDataRow))
                {
                    RemoverColuna(comDataTable, comDataRow, distinct);
                    continue;
                }

                resultado.Rows.Add(ObterResultado(deDataRow, comDataRow, resultado.NewRow()));
                RemoverColuna(comDataTable, comDataRow, distinct);
            }

            return resultado;
        }

        private DataTable ObterDadosMergeados(DataTable deDataTable, DataTable comDataTable, bool distinct)
        {
            DataTable resultado = CriartabelaResultado(deDataTable);

            foreach (DataRow deDataRow in deDataTable.Rows.AsParallel())
            {
                DataRow comDataRow = ObterRegistroPorChave(deDataRow, comDataTable);
                resultado.Rows.Add(ObterResultado(deDataRow, comDataRow, resultado.NewRow()));
                RemoverColuna(comDataTable, comDataRow, distinct);
            }

            return resultado;
        }

        private DataTable CriartabelaResultado(DataTable deDataTable)
        {
            DataTable tabela = new DataTable();

            foreach (DataColumn columns in deDataTable.Columns)
            {
                tabela.Columns.Add(new DataColumn($"de{columns.ColumnName}"));
                tabela.Columns.Add(new DataColumn($"com{columns.ColumnName}"));
            }

            return tabela;
        }

        private DataRow ObterRegistroPorChave(DataRow deDataRow, DataTable comDataTable)
        {
            foreach (DataRow comDataRow in comDataTable.Rows.AsParallel())
            {
                bool igual = true;

                foreach (DataColumn coluna in comDataTable.Columns.AsParallel())
                {
                    if (!coluna.ColumnName.Contains("Key"))
                    {
                        continue;
                    }

                    if (!ResultadoIgual(deDataRow[coluna.ColumnName], comDataRow[coluna.ColumnName]))
                    {
                        igual = false;
                    }
                }

                if (igual)
                {
                    return comDataRow;
                }
            }

            return null;
        }

        private DataRow ObterResultado(DataRow deDataRow, DataRow comDataRow, DataRow resultadoDataRow)
        {
            foreach (DataColumn coluna in deDataRow.Table.Columns.AsParallel())
            {
                resultadoDataRow[$"de{coluna.ColumnName}"] = deDataRow[coluna.ColumnName];
                resultadoDataRow[$"com{coluna.ColumnName}"] = comDataRow == null ? DBNull.Value : comDataRow[coluna.ColumnName];
            }

            return resultadoDataRow;
        }

        private bool RegistroIguais(DataRow deDataRow, DataRow comDataRow)
        {
            foreach (DataColumn coluna in deDataRow.Table.Columns.AsParallel())
            {
                if (!coluna.ColumnName.StartsWith("Cp"))
                {
                    continue;
                }

                if (!ResultadoIgual(deDataRow[coluna.ColumnName], comDataRow[coluna.ColumnName]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ResultadoIgual(object deRow, object comRow)
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

        private void RemoverColuna(DataTable dataTable, DataRow dataRow, bool distinct)
        {
            if (!distinct || dataRow == null)
            {
                return;
            }

            dataTable.Rows.Remove(dataRow);
        }
    }
}