namespace DBComparer
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.navBarControl = new DevExpress.XtraNavBar.NavBarControl();
            this.consultaNavBarGroup = new DevExpress.XtraNavBar.NavBarGroup();
            this.queryNavBarItem = new DevExpress.XtraNavBar.NavBarItem();
            this.comparatorNavBarItem = new DevExpress.XtraNavBar.NavBarItem();
            this.settingNavBarItem = new DevExpress.XtraNavBar.NavBarItem();
            this.importadorUnicusNavBarItem = new DevExpress.XtraNavBar.NavBarItem();
            this.splitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).BeginInit();
            this.splitContainerControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // navBarControl
            // 
            this.navBarControl.ActiveGroup = this.consultaNavBarGroup;
            this.navBarControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.consultaNavBarGroup});
            this.navBarControl.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.importadorUnicusNavBarItem,
            this.queryNavBarItem,
            this.comparatorNavBarItem,
            this.settingNavBarItem});
            this.navBarControl.Location = new System.Drawing.Point(0, 0);
            this.navBarControl.Name = "navBarControl";
            this.navBarControl.OptionsNavPane.ExpandedWidth = 179;
            this.navBarControl.Size = new System.Drawing.Size(179, 659);
            this.navBarControl.TabIndex = 0;
            this.navBarControl.Text = "navBarControl";
            // 
            // consultaNavBarGroup
            // 
            this.consultaNavBarGroup.Caption = "Consults";
            this.consultaNavBarGroup.Expanded = true;
            this.consultaNavBarGroup.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.SmallIconsText;
            this.consultaNavBarGroup.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.queryNavBarItem),
            new DevExpress.XtraNavBar.NavBarItemLink(this.comparatorNavBarItem),
            new DevExpress.XtraNavBar.NavBarItemLink(this.settingNavBarItem)});
            this.consultaNavBarGroup.Name = "consultaNavBarGroup";
            this.consultaNavBarGroup.SmallImage = ((System.Drawing.Image)(resources.GetObject("consultaNavBarGroup.SmallImage")));
            // 
            // queryNavBarItem
            // 
            this.queryNavBarItem.Caption = "Consult";
            this.queryNavBarItem.Name = "queryNavBarItem";
            this.queryNavBarItem.SmallImage = ((System.Drawing.Image)(resources.GetObject("queryNavBarItem.SmallImage")));
            this.queryNavBarItem.Tag = "QueryEditView";
            // 
            // comparatorNavBarItem
            // 
            this.comparatorNavBarItem.Caption = "To compare";
            this.comparatorNavBarItem.Name = "comparatorNavBarItem";
            this.comparatorNavBarItem.SmallImage = ((System.Drawing.Image)(resources.GetObject("comparatorNavBarItem.SmallImage")));
            this.comparatorNavBarItem.Tag = "ComparatorEditView";
            // 
            // settingNavBarItem
            // 
            this.settingNavBarItem.Caption = "Settings";
            this.settingNavBarItem.Name = "settingNavBarItem";
            this.settingNavBarItem.SmallImage = ((System.Drawing.Image)(resources.GetObject("settingNavBarItem.SmallImage")));
            this.settingNavBarItem.Tag = "settingEditView";
            this.settingNavBarItem.Visible = false;
            // 
            // importadorUnicusNavBarItem
            // 
            this.importadorUnicusNavBarItem.Caption = "Importador unicus";
            this.importadorUnicusNavBarItem.Name = "importadorUnicusNavBarItem";
            this.importadorUnicusNavBarItem.SmallImage = ((System.Drawing.Image)(resources.GetObject("importadorUnicusNavBarItem.SmallImage")));
            // 
            // splitContainerControl
            // 
            this.splitContainerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl.Name = "splitContainerControl";
            this.splitContainerControl.Panel1.Controls.Add(this.navBarControl);
            this.splitContainerControl.Panel1.Text = "Panel";
            this.splitContainerControl.Panel2.Text = "Panel";
            this.splitContainerControl.Size = new System.Drawing.Size(1200, 659);
            this.splitContainerControl.SplitterPosition = 179;
            this.splitContainerControl.TabIndex = 2;
            this.splitContainerControl.Text = "splitContainerControl1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 659);
            this.Controls.Add(this.splitContainerControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Data Comparator";
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).EndInit();
            this.splitContainerControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraNavBar.NavBarControl navBarControl;
        private DevExpress.XtraNavBar.NavBarItem importadorUnicusNavBarItem;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl;
        private DevExpress.XtraNavBar.NavBarGroup consultaNavBarGroup;
        private DevExpress.XtraNavBar.NavBarItem queryNavBarItem;
        private DevExpress.XtraNavBar.NavBarItem comparatorNavBarItem;
        private DevExpress.XtraNavBar.NavBarItem settingNavBarItem;
    }
}