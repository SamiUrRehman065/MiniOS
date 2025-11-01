using System;
using System.Drawing;
using System.Windows.Forms;

namespace KernelApp
{
    public partial class MainForm : Form
    {
        private TabControl mainTabs;
        private TabPage consoleTab, processTab, memoryTab, syscallsTab;
        private RichTextBox consoleBox, syscallsBox;
        private DataGridView processGrid;
        private Panel memoryPanel;
        private int lastHoverIndex = -1;

        public MainForm()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            // === Window ===
            this.Text = "MiniOS Kernel Console";
            this.Size = new Size(950, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.Font = new Font("Consolas", 10);
            this.ForeColor = Color.White;

            // === Custom TabControl ===
            mainTabs = new TabControl()
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 11, FontStyle.Bold),
                DrawMode = TabDrawMode.OwnerDrawFixed,
                ItemSize = new Size(160, 40), // Bigger clickable tabs
                SizeMode = TabSizeMode.Fixed,
                Padding = new Point(15, 6),
            };

            mainTabs.DrawItem += DrawCustomTab;
            mainTabs.GetType()
           .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
           ?.SetValue(mainTabs, true, null);

            mainTabs.MouseMove += (s, e) =>
            {
                // Invalidate only if hover actually changes
                var hoverIndex = -1;
                for (int i = 0; i < mainTabs.TabPages.Count; i++)
                {
                    if (mainTabs.GetTabRect(i).Contains(e.Location))
                    {
                        hoverIndex = i;
                        break;
                    }
                }

                if (hoverIndex != lastHoverIndex)
                {
                    lastHoverIndex = hoverIndex;
                    mainTabs.Invalidate();
                }
            };


            // === Tabs ===
            consoleTab = new TabPage("📟 Console");
            processTab = new TabPage("⚙️ Processes");
            memoryTab = new TabPage("💾 Memory");
            syscallsTab = new TabPage("🧠 Syscalls");

            mainTabs.TabPages.AddRange(new[] { consoleTab, processTab, memoryTab, syscallsTab });
            this.Controls.Add(mainTabs);

            // === Console ===
            consoleBox = new RichTextBox()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                ForeColor = Color.Lime,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Consolas", 11),
            };
            consoleTab.Controls.Add(consoleBox);
            AddPlaceholder(consoleTab, "System boot logs and kernel messages will appear here.");

            // === Process Manager ===
            processGrid = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.FromArgb(25, 25, 25),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(45, 45, 45),
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = Color.FromArgb(40, 40, 40),
                    ForeColor = Color.White,
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Consolas", 10, FontStyle.Bold)
                },
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = Color.FromArgb(30, 30, 30),
                    ForeColor = Color.White
                },
                EnableHeadersVisualStyles = false
            };
            processGrid.Columns.Add("PID", "PID");
            processGrid.Columns.Add("Name", "Process Name");
            processGrid.Columns.Add("State", "State");
            processTab.Controls.Add(processGrid);
            AddPlaceholder(processTab, "Process management and scheduling simulation will be added here.");

            // === Memory Visualizer ===
            memoryPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(15, 15, 15),
            };
            memoryTab.Controls.Add(memoryPanel);
            AddPlaceholder(memoryTab, "Memory visualization and allocation map will appear here.");

            // === Syscalls Log ===
            syscallsBox = new RichTextBox()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                ForeColor = Color.Cyan,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                Font = new Font("Consolas", 11),
            };
            syscallsTab.Controls.Add(syscallsBox);
            AddPlaceholder(syscallsTab, "System call monitoring and logging will appear here.");
        }

        private void AddPlaceholder(TabPage tab, string text)
        {
            Label placeholder = new Label()
            {
                Text = text,
                Dock = DockStyle.Fill,
                ForeColor = Color.DimGray,
                Font = new Font("Consolas", 12, FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleCenter
            };
            tab.Controls.Add(placeholder);
            placeholder.BringToFront();
        }

        private void DrawCustomTab(object sender, DrawItemEventArgs e)
        {
            TabPage tabPage = mainTabs.TabPages[e.Index];
            Rectangle tabRect = mainTabs.GetTabRect(e.Index);
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color tabColor = isSelected ? Color.FromArgb(45, 45, 45) : Color.FromArgb(25, 25, 25);
            Color hoverColor = Color.FromArgb(60, 60, 60);
            Color textColor = isSelected ? Color.White : Color.Gray;

            if (tabRect.Contains(mainTabs.PointToClient(Cursor.Position)) && !isSelected)
                tabColor = hoverColor;

            using (SolidBrush brush = new SolidBrush(tabColor))
                e.Graphics.FillRectangle(brush, tabRect);

            TextRenderer.DrawText(
                e.Graphics,
                tabPage.Text,
                new Font("Consolas", 11, FontStyle.Bold),
                tabRect,
                textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );
        }
    }
}
