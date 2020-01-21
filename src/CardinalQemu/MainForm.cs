using System;
using Eto.Forms;
using Eto.Drawing;
using System.Diagnostics;
using CardinalLib.Host;
using System.Threading;

namespace CardinalQemu
{
    public class MainForm : Form
    {
        #region "Controls"
        TreeGridItemCollection MachineSelectorItems = new TreeGridItemCollection();
        TreeGridView MachineSelector = new TreeGridView();
        Panel MainPanel = new Panel();
        Splitter MainSplitter = new Splitter();
        Panel MachineInfoPanel = new Panel();
        #endregion

        #region "UI"
        string appTitle = "Cardinal Emulator " + AppInfo.GetFormattedVersion();
        
        public MainForm()
        {
            Title = appTitle;
            ClientSize = new Size(700, 500);

            // Page Selector
            MachineSelector.Columns.Add(new GridColumn()
            {
                DataCell = new ImageTextCell(0, 1),
                AutoSize = true,
                Resizable = false
            });
            MachineSelector.ShowHeader = false;
            MachineSelector.Columns[0].AutoSize = true;
            MachineSelector.Border = BorderType.None;
            MachineSelector.SelectionChanged += OnChangeSelection;

            // Main Splitter
            MainSplitter.Panel1 = MachineSelector;
            MachineSelector.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

            MainSplitter.Panel2 = MachineInfoPanel;
            MainSplitter.Orientation = Orientation.Horizontal;
            MainSplitter.Position = 1 * (ClientSize.Width / 3);
            MainSplitter.FixedPanel = SplitterFixedPanel.Panel1;

            MainPanel.Content = MainSplitter;
            Content = MainPanel;

            // Commands - Application
            var quitCommand = new Command
            {
                MenuText = "Quit",
                Shortcut = Application.Instance.CommonModifier | Keys.Q
            };
            quitCommand.Executed += OnQuit;

            // Commands - File
            var newCommand = new Command
            {
                MenuText = "New Machine",
                ToolBarText = "New"
            };
            newCommand.Executed += OnNew;

            // Menu
            Menu = new MenuBar
            {
                Items =
                {
                    new ButtonMenuItem {
                        Text = "&File",
                        Items = { newCommand }
                    }
                },
                ApplicationItems =
                {
                    
                },
                QuitItem = quitCommand
            };

            // Toolbar		
            ToolBar = new ToolBar
            {
                Items = { newCommand }
            };
        }
        #endregion

        /**
         * Event handlers
         */

        // File Menu
        public void OnNew(object sender, EventArgs e)
        {
            var ppc = new Thread(() => Shell.Execute(new ShellCommand
            {
                Executable = "qemu-system-ppc"
            }));

            ppc.Start();
        }

        // Application Menu
        public void OnQuit(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }

        public void OnChangeSelection(object sender, EventArgs e)
        {

        }
    }
}
