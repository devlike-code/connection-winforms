namespace connection_winforms
{
    public partial class ConnectionGraphEditor : Form
    {
        public ConnectionGraphEditor()
        {
            InitializeComponent();

            graph = new ConnectionGraphControl();
            graph.Size = ClientSize;
            this.Resize += (e, s) => { graph.Size = ClientSize; Invalidate(); };
            Controls.Add(graph);

            ResumeLayout(false);
            PerformLayout();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (graph != null)
            {
                graph.Clear();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (graph != null)
            {
                graph.StartLoad();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (graph != null)
            {
                graph.StartSave();
            }
        }
    }
}