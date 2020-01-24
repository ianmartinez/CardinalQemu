using System;
using Eto.Forms;
using Eto.Drawing;
using System.Diagnostics;
using CardinalLib.Host;
using System.Threading;
using CardinalLib.Qemu;
using CardinalLib.Hardware;

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
        string appTitle = "Cardinal Emulator " + AppInfo.FormattedVersion;
        
        public MainForm()
        {
            Title = appTitle;
            MinimumSize = new Size(500, 300);
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
            var aboutCommand = new Command
            {
                MenuText = "About Cardinal QEMU"
            };
            aboutCommand.Executed += OnAbout;

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

            var importCommand = new Command
            {
                MenuText = "Import Machine",
                ToolBarText = "Import"
            };

            var exportCommand = new Command
            {
                MenuText = "Export Machine",
                ToolBarText = "Export"
            };

            var cloneCommand = new Command
            {
                MenuText = "Clone Machine",
                ToolBarText = "Clone"
            };

            var deleteCommand = new Command
            {
                MenuText = "Delete Machine",
                ToolBarText = "Delete"
            };

            var startCommand = new Command
            {
                MenuText = "Start Machine",
                ToolBarText = "Start"
            };
            startCommand.Executed += OnStart;

            var refreshCommand = new Command
            {
                MenuText = "Refresh Machine",
                ToolBarText = "Refresh"
            };

            var disksCommand = new Command
            {
                MenuText = "Disk Manager",
                ToolBarText = "Disks"
            };
            disksCommand.Executed += OnDisks;

            var settingsCommand = new Command
            {
                MenuText = "Machine Settings",
                ToolBarText = "Settings"
            };

            // Menu
            Menu = new MenuBar
            {
                Items =
                {
                    new ButtonMenuItem {
                        Text = "&Machine",
                        Items = {
                            newCommand,
                            startCommand
                        }
                    },
                    new ButtonMenuItem {
                        Text = "&Disks",
                        Items = {
                            disksCommand
                        }
                    }
                },
                ApplicationItems =
                {
                    aboutCommand
                },
                QuitItem = quitCommand,
                IncludeSystemItems = MenuBarSystemItems.None                
            };

            // Toolbar		
            ToolBar = new ToolBar
            {
                Items = {
                    newCommand,
                    importCommand,
                    exportCommand,
                    cloneCommand,
                    deleteCommand,
                    new SeparatorToolItem() { Type = SeparatorToolItemType.FlexibleSpace },
                    startCommand,
                    settingsCommand
                }
            };
        }
        #endregion

        /**
         * Event handlers
         */

        // Machine Menu
        public void OnNew(object sender, EventArgs e)
        {
            MessageBox.Show(string.Join("\n", QemuData.ArchNames));
        }

        public async void OnStart(object sender, EventArgs e)
        {
            var state = await QemuApps.Get("qemu-system-ppc").RunAsync();

            if(state.HasErrors)
            {

            }
        }

        // Disks Menu
        public void OnDisks(object sender, EventArgs e)
        {
            Disk d = new Disk("/Users/ianmartinez/CardinalMachines/Disks/nt4_fat.qcow", "hda");
        }

        // Application Menu
        public void OnAbout(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog
            {
                Title = string.Format("About Cardinal QEMU {0}", AppInfo.FormattedVersion),
                Version = string.Format("Version {0}", AppInfo.FormattedVersion),
                Copyright = string.Format("©2019-{0} Ian Martinez", AppInfo.CopyrightYear),
                ProgramDescription = "A cross-platform GUI for QEMU",
                
            };

            aboutDialog.ShowDialog(this);
        }

        public void OnQuit(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }

        public void OnChangeSelection(object sender, EventArgs e)
        {

        }
    }
}
