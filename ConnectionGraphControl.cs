using connection;

using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace connection_winforms
{
    public partial class ConnectionGraphControl : UserControl
    {
        private GraphEditorTrait editor;
        private WinformsGraphics graphics;

        public void Clear()
        {
            editor.Reset();
            Invalidate();
        }

        public void StartSave()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "grasm";
            saveFileDialog.Filter = "GRASM layouts (*.grasm)|*.grasm";
            var result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                editor.SaveToFile(saveFileDialog.FileName);
            }
            Invalidate();
        }

        public void StartLoad()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "grasm";
            openFileDialog.Filter = "GRASM layouts (*.grasm)|*.grasm";
            var result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    editor.LoadFromFile(openFileDialog.FileName);
                    Invalidate();
                }
                catch { }
            }
            
        }

        public ConnectionGraphControl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint
                        | ControlStyles.UserPaint
                        | ControlStyles.DoubleBuffer, true);

            editor = new GraphEditorTrait();
            graphics = new WinformsGraphics();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void ConnectionGraphControl_Load(object sender, EventArgs e)
        {
#if (DEBUG)
            AllocConsole();
#endif
        }

        private void ConnectionGraphControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
            {
                editor.Logic.Trigger("esc");
                Invalidate();
            }
            else if (e.KeyCode == Keys.S && e.Modifiers.HasFlag(Keys.Control))
            {
                StartSave();
            }
            else if (e.KeyCode == Keys.O && e.Modifiers.HasFlag(Keys.Control))
            {
                StartLoad();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                editor.DeleteSelected();
            }

            editor.IsLinkingBothWays = ModifierKeys.HasFlag(Keys.Shift);
            this.Invalidate();
        }

        private void ConnectionGraphControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (GraphInternals.Hovered.Count == 0)
            {
                editor.Logic.Trigger("click empty");
            }
            else
            {
                editor.Logic.Trigger("click node");
            }
            Invalidate();
        }

        private void ConnectionGraphControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                editor.Logic.Reset();
                if (GraphInternals.Hovered.Count == 0)
                {
                    editor.Logic.Trigger("dblclick empty");
                }
                else
                {
                    editor.Logic.Trigger("dblclick node");
                }
                Invalidate();
            }
        }

        private void ConnectionGraphControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (GraphInternals.Hovered.Count == 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    editor.Logic.Trigger("mousedown left empty");
                }
                else
                {
                    editor.Logic.Trigger("mousedown right empty");
                }
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    editor.Logic.Trigger("mousedown left node");
                }
                else
                {
                    editor.Logic.Trigger("mousedown right node");
                }
            }
            Invalidate();
        }

        private void ConnectionGraphControl_MouseMove(object sender, MouseEventArgs e)
        {
            editor.UpdateMousePosition(PointToClient(Cursor.Position).ToFloat2());
            if (editor.Logic.IsRectSelecting())
            {
                if (editor.SelectingRect != null)
                {
                    var r = editor.SelectingRect.Value;
                    r.W = Rendering.Mouse.X - r.X;
                    r.H = Rendering.Mouse.Y - r.Y;
                    editor.SelectingRect = r;
                }
            }

            if (editor.Logic.IsMoving)
            {
                var delta = editor.UpdateMouseMoveDelta(Rendering.Mouse);
            }
            
            editor.IsLinkingBothWays = ModifierKeys.HasFlag(Keys.Shift);

            this.Invalidate();
        }

        private void ConnectionGraphControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (GraphInternals.Hovered.Count == 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    editor.Logic.Trigger("mouseup left empty");
                }
                else
                {
                    editor.Logic.Trigger("mouseup right empty");
                }
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {                    
                    editor.Logic.Trigger("mouseup left node");                    
                }
                else
                {
                    editor.Logic.Trigger("mouseup right node");
                }
            }

            editor.IsLinkingBothWays = ModifierKeys.HasFlag(Keys.Shift);
            this.Invalidate();
        }

        private void ConnectionGraphControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.Graphics = e.Graphics;

            Rendering.DrawGrid(graphics, this.Width, this.Height);

            editor.Nodes.Sort();

            foreach (var node in editor.Nodes)
            {
                if (node.Alive)
                    node.Draw(graphics);
            }

            if (editor.ConnectingStartNode != null)
            {
                graphics.DrawArrow(Tint.White, new List<Float2> {
                    editor.ConnectingStartNode.Origin, Rendering.Mouse
                }, editor.IsLinkingBothWays);
            }

            if (editor.SelectingRect != null && Math.Abs(editor.SelectingRect.Value.W) >= 2 && Math.Abs(editor.SelectingRect.Value.H) >= 2)
            {
                graphics.DrawRectangle(Tint.White, editor.SelectingRect.Value);
            }

            graphics.DrawText(Tint.Yellow, new Float2 { X = 10, Y = 10 }, $"Moving: {editor.Logic.IsMoving}");
            graphics.DrawText(Tint.Yellow, new Float2 { X = 10, Y = 30 }, $"Editing: {editor.EditingLabelNode}");
        }

        private void ConnectionGraphControl_Resize(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Size = this.Parent.Size;
            }

            Invalidate();
        }

        private void ConnectionGraphControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (editor.EditingLabelNode != null)
            {
                var text = editor.EditingLabelNode.GetTag("Label");

                if (!char.IsControl(e.KeyChar))
                {
                    text += e.KeyChar;
                    editor.EditingLabelNode.AddTag("Label", text);
                }
                else if (e.KeyChar == (char)8 && text.Length > 0)
                {
                    text = text.Substring(0, text.Length - 1);
                    editor.EditingLabelNode.AddTag("Label", text);
                }

                Invalidate();
            }
        }
    }
}