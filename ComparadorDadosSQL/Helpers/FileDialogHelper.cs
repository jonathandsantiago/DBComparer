using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ComparadorDadosSQL.Helpers
{
    public static class FileDialogHelper
    {
        private static readonly string imageExtesions = "Imagens (*.bmp; *.jpg; *.jpeg; *.jpe; *.gif; *.png) | *.bmp; *.jpg; *.jpeg; *.jpe; *.gif; *.png";

        public static FileInfo GetFile(string dialogTitle, string[] extensions, long? limitSizeMb = 3000000, string extensionsName = "Arquivos")
        {
            OpenFileDialog fileDialog = CreateFileDialog(dialogTitle, false, extensionsName, extensions);

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            return GetFileInfo(limitSizeMb, fileDialog.FileName);
        }

        private static FileInfo GetFileInfo(long? limitSizeMb, string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            if (limitSizeMb != null && (limitSizeMb.Value / 1024m) < ((fileInfo.Length / 1024m) / 1024m))
            {
                throw new Exception(string.Format("O tamanho do arquivo não pode ser superior a {0} MB", (limitSizeMb.Value / 1024)));
            }

            return fileInfo;
        }

        private static OpenFileDialog CreateFileDialog(string title, bool multiselect, string extensionsName, string[] extensions)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = GetExtensions(extensionsName, extensions);
            fileDialog.Multiselect = multiselect;
            fileDialog.Title = title;

            return fileDialog;
        }

        private static string GetExtensions(string name, string[] extensions)
        {
            if (extensions == null || !extensions.Any())
            {
                return imageExtesions;
            }
            else
            {
                var retorno = string.Join("; ", extensions.Select(c => "*." + c));
                return string.Format("{0}({1}) | {1}", !string.IsNullOrEmpty(name) ? name + " " : null, retorno);
            }
        }
    }
}