namespace DBComparer.Views
{
    partial class QueryEditView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryEditView));
            this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.consultaLayoutControl = new DevExpress.XtraLayout.LayoutControl();
            this.tablesGridControl = new DevExpress.XtraGrid.GridControl();
            this.tabelasGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.richLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.gridLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.typeImageComboBoxEdit = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.execultSimpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.deLimparSimpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.excelSimpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.typeLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.execultLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.execlLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
            this.layoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.consultaLayoutControl)).BeginInit();
            this.consultaLayoutControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablesGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabelasGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.richLayoutControlItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLayoutControlItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.typeImageComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.typeLayoutControlItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.execultLayoutControlItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.execlLayoutControlItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl
            // 
            this.layoutControl.Controls.Add(this.consultaLayoutControl);
            this.layoutControl.Controls.Add(this.typeImageComboBoxEdit);
            this.layoutControl.Controls.Add(this.execultSimpleButton);
            this.layoutControl.Controls.Add(this.deLimparSimpleButton);
            this.layoutControl.Controls.Add(this.excelSimpleButton);
            this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl.Location = new System.Drawing.Point(0, 0);
            this.layoutControl.Name = "layoutControl";
            this.layoutControl.Root = this.layoutControlGroup1;
            this.layoutControl.Size = new System.Drawing.Size(1145, 571);
            this.layoutControl.TabIndex = 0;
            this.layoutControl.Text = "layoutControl1";
            // 
            // consultaLayoutControl
            // 
            this.consultaLayoutControl.Controls.Add(this.tablesGridControl);
            this.consultaLayoutControl.Controls.Add(this.gridControl);
            this.consultaLayoutControl.Controls.Add(this.richEditControl);
            this.consultaLayoutControl.Location = new System.Drawing.Point(12, 38);
            this.consultaLayoutControl.Name = "consultaLayoutControl";
            this.consultaLayoutControl.Root = this.Root;
            this.consultaLayoutControl.Size = new System.Drawing.Size(1121, 521);
            this.consultaLayoutControl.TabIndex = 20;
            this.consultaLayoutControl.Text = "layoutControl2";
            // 
            // tablesGridControl
            // 
            this.tablesGridControl.Location = new System.Drawing.Point(2, 2);
            this.tablesGridControl.MainView = this.tabelasGridView;
            this.tablesGridControl.Name = "tablesGridControl";
            this.tablesGridControl.Size = new System.Drawing.Size(157, 517);
            this.tablesGridControl.TabIndex = 21;
            this.tablesGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.tabelasGridView});
            // 
            // tabelasGridView
            // 
            this.tabelasGridView.GridControl = this.tablesGridControl;
            this.tabelasGridView.Name = "tabelasGridView";
            this.tabelasGridView.OptionsView.ShowFooter = true;
            this.tabelasGridView.OptionsView.ShowGroupPanel = false;
            this.tabelasGridView.DoubleClick += new System.EventHandler(this.gridView_DoubleClick);
            // 
            // gridControl
            // 
            this.gridControl.Location = new System.Drawing.Point(163, 242);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.Size = new System.Drawing.Size(956, 277);
            this.gridControl.TabIndex = 20;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.ShowFooter = true;
            this.gridView.OptionsView.ShowGroupPanel = false;
            // 
            // richEditControl
            // 
            this.richEditControl.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            this.richEditControl.Location = new System.Drawing.Point(163, 2);
            this.richEditControl.Name = "richEditControl";
            this.richEditControl.Size = new System.Drawing.Size(956, 236);
            this.richEditControl.TabIndex = 18;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.richLayoutControlItem,
            this.gridLayoutControlItem,
            this.layoutControlItem2});
            this.Root.Location = new System.Drawing.Point(0, 0);
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(1121, 521);
            this.Root.TextVisible = false;
            // 
            // richLayoutControlItem
            // 
            this.richLayoutControlItem.Control = this.richEditControl;
            this.richLayoutControlItem.CustomizationFormText = "layoutControlItem";
            this.richLayoutControlItem.Location = new System.Drawing.Point(161, 0);
            this.richLayoutControlItem.Name = "richLayoutControlItem";
            this.richLayoutControlItem.Size = new System.Drawing.Size(960, 240);
            this.richLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
            this.richLayoutControlItem.TextVisible = false;
            // 
            // gridLayoutControlItem
            // 
            this.gridLayoutControlItem.Control = this.gridControl;
            this.gridLayoutControlItem.Location = new System.Drawing.Point(161, 240);
            this.gridLayoutControlItem.Name = "gridLayoutControlItem";
            this.gridLayoutControlItem.Size = new System.Drawing.Size(960, 281);
            this.gridLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
            this.gridLayoutControlItem.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.tablesGridControl;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(161, 521);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // typeImageComboBoxEdit
            // 
            this.typeImageComboBoxEdit.Location = new System.Drawing.Point(35, 12);
            this.typeImageComboBoxEdit.Name = "typeImageComboBoxEdit";
            this.typeImageComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.typeImageComboBoxEdit.Size = new System.Drawing.Size(880, 20);
            this.typeImageComboBoxEdit.StyleController = this.layoutControl;
            this.typeImageComboBoxEdit.TabIndex = 9;
            this.typeImageComboBoxEdit.SelectedIndexChanged += new System.EventHandler(this.typeImageComboBoxEdit_SelectedIndexChanged);
            // 
            // execultSimpleButton
            // 
            this.execultSimpleButton.Location = new System.Drawing.Point(949, 12);
            this.execultSimpleButton.MaximumSize = new System.Drawing.Size(90, 22);
            this.execultSimpleButton.MinimumSize = new System.Drawing.Size(90, 22);
            this.execultSimpleButton.Name = "execultSimpleButton";
            this.execultSimpleButton.Size = new System.Drawing.Size(90, 22);
            this.execultSimpleButton.StyleController = this.layoutControl;
            this.execultSimpleButton.TabIndex = 12;
            this.execultSimpleButton.Text = "Execult";
            this.execultSimpleButton.Click += new System.EventHandler(this.execultSimpleButton_Click);
            // 
            // deLimparSimpleButton
            // 
            this.deLimparSimpleButton.Location = new System.Drawing.Point(1043, 12);
            this.deLimparSimpleButton.MaximumSize = new System.Drawing.Size(90, 22);
            this.deLimparSimpleButton.MinimumSize = new System.Drawing.Size(90, 22);
            this.deLimparSimpleButton.Name = "deLimparSimpleButton";
            this.deLimparSimpleButton.Size = new System.Drawing.Size(90, 22);
            this.deLimparSimpleButton.StyleController = this.layoutControl;
            this.deLimparSimpleButton.TabIndex = 17;
            this.deLimparSimpleButton.Text = "Clear";
            this.deLimparSimpleButton.Click += new System.EventHandler(this.clearSimpleButton_Click);
            // 
            // excelSimpleButton
            // 
            this.excelSimpleButton.Image = ((System.Drawing.Image)(resources.GetObject("excelSimpleButton.Image")));
            this.excelSimpleButton.Location = new System.Drawing.Point(919, 12);
            this.excelSimpleButton.MaximumSize = new System.Drawing.Size(26, 22);
            this.excelSimpleButton.Name = "excelSimpleButton";
            this.excelSimpleButton.Size = new System.Drawing.Size(26, 22);
            this.excelSimpleButton.StyleController = this.layoutControl;
            this.excelSimpleButton.TabIndex = 16;
            this.excelSimpleButton.Click += new System.EventHandler(this.exportExcelSimpleButton_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.typeLayoutControlItem,
            this.execultLayoutControlItem,
            this.layoutControlItem8,
            this.execlLayoutControlItem,
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(1145, 571);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // typeLayoutControlItem
            // 
            this.typeLayoutControlItem.Control = this.typeImageComboBoxEdit;
            this.typeLayoutControlItem.CustomizationFormText = "Tipo";
            this.typeLayoutControlItem.Location = new System.Drawing.Point(0, 0);
            this.typeLayoutControlItem.Name = "typeLayoutControlItem";
            this.typeLayoutControlItem.Size = new System.Drawing.Size(907, 26);
            this.typeLayoutControlItem.Text = "Tipo";
            this.typeLayoutControlItem.TextSize = new System.Drawing.Size(20, 13);
            // 
            // execultLayoutControlItem
            // 
            this.execultLayoutControlItem.Control = this.execultSimpleButton;
            this.execultLayoutControlItem.CustomizationFormText = "layoutControlItem7";
            this.execultLayoutControlItem.Location = new System.Drawing.Point(937, 0);
            this.execultLayoutControlItem.Name = "execultLayoutControlItem";
            this.execultLayoutControlItem.Size = new System.Drawing.Size(94, 26);
            this.execultLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
            this.execultLayoutControlItem.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.deLimparSimpleButton;
            this.layoutControlItem8.CustomizationFormText = "layoutControlItem8";
            this.layoutControlItem8.Location = new System.Drawing.Point(1031, 0);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(94, 26);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // execlLayoutControlItem
            // 
            this.execlLayoutControlItem.Control = this.excelSimpleButton;
            this.execlLayoutControlItem.CustomizationFormText = "layoutControlItem5";
            this.execlLayoutControlItem.Location = new System.Drawing.Point(907, 0);
            this.execlLayoutControlItem.Name = "execlLayoutControlItem";
            this.execlLayoutControlItem.Size = new System.Drawing.Size(30, 26);
            this.execlLayoutControlItem.Text = "layoutControlItem5";
            this.execlLayoutControlItem.TextSize = new System.Drawing.Size(0, 0);
            this.execlLayoutControlItem.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.consultaLayoutControl;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1125, 525);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // QueryEditView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl);
            this.Name = "QueryEditView";
            this.Size = new System.Drawing.Size(1145, 571);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
            this.layoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.consultaLayoutControl)).EndInit();
            this.consultaLayoutControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablesGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabelasGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.richLayoutControlItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLayoutControlItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.typeImageComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.typeLayoutControlItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.execultLayoutControlItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.execlLayoutControlItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.ImageComboBoxEdit typeImageComboBoxEdit;
        private DevExpress.XtraEditors.SimpleButton execultSimpleButton;
        private DevExpress.XtraEditors.SimpleButton deLimparSimpleButton;
        private DevExpress.XtraEditors.SimpleButton excelSimpleButton;
        private DevExpress.XtraLayout.LayoutControlItem typeLayoutControlItem;
        private DevExpress.XtraLayout.LayoutControlItem execultLayoutControlItem;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlItem execlLayoutControlItem;
        private DevExpress.XtraLayout.LayoutControl consultaLayoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraRichEdit.RichEditControl richEditControl;
        private DevExpress.XtraLayout.LayoutControlItem richLayoutControlItem;
        private DevExpress.XtraLayout.LayoutControlItem gridLayoutControlItem;
        private DevExpress.XtraGrid.GridControl tablesGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView tabelasGridView;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}
