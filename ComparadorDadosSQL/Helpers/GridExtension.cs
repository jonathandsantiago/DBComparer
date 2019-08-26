using DevExpress.XtraGrid.Views.Grid;

namespace ComparadorDadosSQL.Helpers
{
    public static class GridExtension
    {
        public static T GetFocusedRow<T>(this GridView gridView)
           where T : class
        {
            return gridView.GetFocusedRow() as T;
        }
    }
}
