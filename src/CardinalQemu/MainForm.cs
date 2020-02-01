using System;
using Eto.Forms;
using Eto.Drawing;
using CardinalLib.Qemu;
using CardinalLib.Hardware;
using CardinalLib.Machines;

namespace CardinalQemu
{
    public class MainForm : Form
    {
        Machine[] Machines;
        Machine CurrentMachine
        {
            get
            {
                var machineIndex = MachineSelector.SelectedRow;

                if (Machines != null && machineIndex != -1 && machineIndex < Machines.Length)
                    return Machines[machineIndex];

                return null;
            }
        }

        #region "Controls"
        TreeGridItemCollection MachineSelectorItems = new TreeGridItemCollection();
        TreeGridView MachineSelector = new TreeGridView();
        Panel MainPanel = new Panel();
        Splitter MainSplitter = new Splitter();
        Panel MachineInfoPanel = new Panel();
        #endregion

        #region "UI"
        string appTitle = "Cardinal QEMU " + AppInfo.FormattedVersion;
        
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
            MachineSelector.Border = BorderType.None;
            MachineSelector.SelectionChanged += OnChangeSelection;

            // Main Splitter
            MainSplitter.Panel1 = MachineSelector;
            MainSplitter.Panel1.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
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
                ToolBarText = "New",
                Image = Icons.Get("list-add") 
            };
            newCommand.Executed += OnNew;

            var importCommand = new Command
            {
                MenuText = "Import Machine",
                ToolBarText = "Import",
                Image = Icons.Get("document-open")
            };
            importCommand.Executed += OnImport;

            var exportCommand = new Command
            {
                MenuText = "Export Machine",
                ToolBarText = "Export",
                Image = Icons.Get("document-save")
            };
            exportCommand.Executed += OnExport;

            var cloneCommand = new Command
            {
                MenuText = "Clone Machine",
                ToolBarText = "Clone",
                Image = Icons.Get("edit-copy")
            };

            var deleteCommand = new Command
            {
                MenuText = "Delete Machine",
                ToolBarText = "Delete",
                Image = Icons.Get("list-remove")
            };

            var startCommand = new Command
            {
                MenuText = "Start Machine",
                ToolBarText = "Start",
                Image = Icons.Get("media-playback-start")
            };
            startCommand.Executed += OnStart;

            var refreshCommand = new Command
            {
                MenuText = "Refresh Machine",
                ToolBarText = "Refresh",
                Image = Icons.Get("refresh")
            };

            var disksCommand = new Command
            {
                MenuText = "Disk Manager",
                ToolBarText = "Disks",
                Image = Icons.Get("drive-harddisk")
            };
            disksCommand.Executed += OnDisks;

            var settingsCommand = new Command
            {
                MenuText = "Machine Settings",
                ToolBarText = "Settings",
                Image = Icons.Get("config")
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
                    deleteCommand,
                    importCommand,
                    exportCommand,
                    cloneCommand,
                    new SeparatorToolItem() { Type = SeparatorToolItemType.FlexibleSpace },
                    startCommand,
                    settingsCommand
                },
                Style = "NativeToolbar"
            };

            LoadMachines();
        }
        #endregion

        private void LoadMachines()
        {
            Machines = Machine.GetAll();
            MachineSelectorItems.Clear();
            var machineIcon =  Icons.Get("vm");

            foreach (Machine machine in Machines)
            {
                var machineTreeGridItem = new TreeGridItem()
                {
                    Expanded = false,
                    Values = new object[] { machineIcon, machine.Name },
                };

                MachineSelectorItems.Add(machineTreeGridItem);
            }

            MachineSelector.DataStore = MachineSelectorItems;

            if (MachineSelectorItems.Count > 0 && Loaded)
                MachineSelector.SelectedRow = 0;

            UpdateTitle();
        }

        private void UpdateTitle()
        {
            if (CurrentMachine != null)
                Title = string.Format("{0} - {1}", appTitle, CurrentMachine.Name);
            else
                Title = appTitle;
        }

        /**
         * Event handlers
         */

        // Machine Menu
        public void OnNew(object sender, EventArgs e)
        {
        }

        FileFilter[] filters = {
            new FileFilter("Cardinal Machine Archive (*.cmarc)", ".cmarc")
        };

        public void OnImport(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();

            foreach (var filter in filters)
                openDialog.Filters.Add(filter);

            openDialog.CurrentFilterIndex = 0;

            if (openDialog.ShowDialog(this) == DialogResult.Ok)
            {
                
            }
        }

        public void OnExport(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();

            foreach (var filter in filters)
                saveDialog.Filters.Add(filter);

            saveDialog.CurrentFilterIndex = 0;

            if (saveDialog.ShowDialog(this) == DialogResult.Ok)
            {
            }
        }

        public async void OnStart(object sender, EventArgs e)
        {
            var bootResult = await CurrentMachine?.Boot();

            // If there was an errors launching the machine
            if(bootResult.HasErrors)
            {
                MessageBox.Show(this, string.Join(Environment.NewLine, bootResult.Errors), MessageBoxType.Error);
            }
        }

        // Disks Menu
        public void OnDisks(object sender, EventArgs e)
        {
            var disks = Disk.GetAll();
        }

        // Application Menu
        public void OnAbout(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog
            {
                Title = string.Format("About Cardinal QEMU {0}", AppInfo.FormattedVersion),
                Version = string.Format("Version {0}", AppInfo.FormattedVersion),
                Copyright = string.Format("©2019-{0} Ian Martinez", AppInfo.CopyrightYear),
                ProgramDescription = "A cross-platform GUI for QEMU"
            };

            aboutDialog.ShowDialog(this);
        }

        public void OnQuit(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }

        public void OnChangeSelection(object sender, EventArgs e)
        {
            UpdateTitle();
        }
    }
}
