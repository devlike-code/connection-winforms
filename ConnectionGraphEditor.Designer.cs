namespace connection_winforms
{
    partial class ConnectionGraphEditor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            graph = new ConnectionGraphControl();
            SuspendLayout();
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(graph);
            Name = "Form1";
            Text = "//connection [WinForms]";
            // 
            // graph
            // 
            graph.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            graph.AutoSize = true;
            graph.Location = new Point(0, 0);
            graph.Name = "graph";
            graph.Size = ClientSize;
            graph.TabIndex = 0;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ConnectionGraphControl graph;
    }
}