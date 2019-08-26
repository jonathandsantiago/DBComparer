using DevExpress.XtraWaitForm;
using System;
using System.Drawing;

namespace ComparadorDadosSQL
{
    public partial class LoadingForm : WaitForm
    {
        public LoadingForm()
        {
            InitializeComponent();
            ShowOnTopMode = ShowFormOnTopMode.AboveParent;
            progressPanel.AutoHeight = true;
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            progressPanel.Caption = caption;
            ClientSize = new Size(progressPanel.ClientSize.Width + 16, progressPanel.ClientSize.Height + 16);
            Refresh();
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            progressPanel.Description = description;
            ClientSize = new Size(progressPanel.ClientSize.Width + 16, progressPanel.ClientSize.Height + 16);
            Refresh();
        }

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
            WaitFormCommand command = (WaitFormCommand)cmd;

            if (command == WaitFormCommand.BringToFront)
            {
                Activate();
            }
        }
        
        #endregion

        public enum WaitFormCommand
        {
            BringToFront
        }
    }
}