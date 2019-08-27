using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Scrolling;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Drawing;
using System.Reflection;

namespace DBComparer.Helpers
{
    public static class GridHelper
    {
        public static void ConfigurarGridPadrao(GridView gridView)
        {
            gridView.OptionsNavigation.UseTabKey = false;
            gridView.FocusRectStyle = DrawFocusRectStyle.None;
            gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView.OptionsBehavior.FocusLeaveOnTab = true;
            gridView.ColumnPanelRowHeight = 33;
            gridView.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            gridView.OptionsView.ShowGroupPanel = false;
        }

        public static void ConfigureGridNotEditable(GridView gridView)
        {
            gridView.BeginUpdate();
            ConfigurarGridPadrao(gridView);
            gridView.OptionsBehavior.Editable = false;
            gridView.EndUpdate();
        }

        public static void ConfigurarGridNaoEditavel(params GridView[] gridViews)
        {
            foreach (var item in gridViews)
            {
                ConfigureGridNotEditable(item);
            }
        }

        public static void BestSizeColumns(GridView gridView)
        {
            gridView.BestFitColumns();
        }

        public static bool InvalidRowHandle(int rowHandle)
        {
            return rowHandle == GridControl.InvalidRowHandle ||
                rowHandle == GridControl.AutoFilterRowHandle ||
                rowHandle == GridControl.NewItemRowHandle;
        }

        public static void BestHeightGridView(GridView gridView, int minHeight = 0, int maxHeight = 0)
        {
            GridControl gridControl = gridView.GridControl;
            GridViewInfo viewInfo = (GridViewInfo)gridView.GetViewInfo();
            FieldInfo fi = typeof(GridView).GetField("scrollInfo", BindingFlags.Instance | BindingFlags.NonPublic);
            ScrollInfo scrollInfo = (ScrollInfo)fi.GetValue(gridView);

            int height = viewInfo.CalcRealViewHeight(new Rectangle(0, 0, 0, maxHeight == 0 ? int.MaxValue : maxHeight));

            if (scrollInfo.HScrollVisible)
            {
                height += scrollInfo.HScrollSize;
            }

            height = Math.Max(minHeight, height);

            if (maxHeight > 0)
            {
                height = Math.Min(maxHeight - gridControl.Location.Y, height);
            }

            gridControl.Size = new Size(gridControl.Width, height);
            gridView.LayoutChanged();
        }

        public static void AutoHeightGridView(GridView gridView, int minHeight = 0, int maxHeight = 0, EventHandler<GridView> heightChanged = null)
        {
            gridView.RowCountChanged += (s, e) =>
            {
                BestHeightGridView(gridView);

                if (heightChanged != null)
                {
                    heightChanged(s, gridView);
                }
            };
        }
    }
}
