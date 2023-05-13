using connection;
using connection.Nodes;

namespace connection_winforms
{
    public partial class ConnectionGraphEditor : Form
    {
        private Node DataGridConnectionNode;

        public ConnectionGraphEditor()
        {
            InitializeComponent();

            graph = new ConnectionGraphControl();
            graph.Size = ClientSize;
            this.Resize += (e, s) => { graph.Size = ClientSize; Invalidate(); };
            splitContainer1.Panel1.Controls.Add(graph);
            ResumeLayout(false);
            PerformLayout();

            graph.OnSelectionChanged += Graph_OnSelectionChanged;
        }

        private void Graph_OnSelectionChanged(object sender, List<connection.Node> selection)
        {
            DataGridConnectionNode = null;
            dataGridView1.Rows.Clear();
            var nodes = selection.Where(n => !(n is LabelNode));
            if (nodes.Count() == 1)
            {
                var node = nodes.First();
                DataGridConnectionNode = node;
                LoadTags(DataGridConnectionNode);
            }
        }

        private void LoadTags(Node node)
        {
            foreach (var entry in node.Tags)
            {
                int id = dataGridView1.Rows.Add(entry.Key, entry.Value);
                if (entry.Key == "Position" || entry.Key == "Label")
                    dataGridView1.Rows[id].Cells[0].ReadOnly = true;
            }
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

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (DataGridConnectionNode != null)
            {
                var key = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                var value = dataGridView1.Rows[e.RowIndex].Cells[1].Value;

                if (key == null || value == null)
                    return;

                if (key.ToString().Trim() == "" || value.ToString().Trim() == "")
                    return;

                DataGridConnectionNode.AddTag(key.ToString(), value.ToString());
                this.graph.Invalidate();
                this.Invalidate();
            }
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (DataGridConnectionNode == null) return;

            var tagNames = DataGridConnectionNode.Tags.Keys.ToList();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var v = dataGridView1.Rows[i].Cells[0].Value;
                if (v == null || v.ToString().Trim() == "") continue;

                var key = v.ToString();
                if (tagNames.Contains(key)) tagNames.Remove(key);
            }

            foreach (var tag in tagNames)
            {
                DataGridConnectionNode.RemoveTag(tag.ToString());
            }
            this.graph.Invalidate();
            this.Invalidate();
        }
    }
}