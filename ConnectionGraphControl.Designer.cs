using connection;

namespace connection_winforms
{
    partial class ConnectionGraphControl
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
            SuspendLayout();
            // 
            // ConnectionGraphControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            Name = "ConnectionGraphControl";
            Load += ConnectionGraphControl_Load;
            Paint += ConnectionGraphControl_Paint;
            KeyDown += ConnectionGraphControl_KeyDown;
            KeyUp += ConnectionGraphControl_KeyUp;
            MouseClick += ConnectionGraphControl_MouseClick;
            MouseDoubleClick += ConnectionGraphControl_MouseDoubleClick;
            MouseDown += ConnectionGraphControl_MouseDown;
            MouseMove += ConnectionGraphControl_MouseMove;
            MouseUp += ConnectionGraphControl_MouseUp;
            Resize += ConnectionGraphControl_Resize;
            ResumeLayout(false);
        }

        #endregion
    }
}
