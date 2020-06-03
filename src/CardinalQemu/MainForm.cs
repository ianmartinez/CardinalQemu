using System;
using Eto.Forms;
using Eto.Drawing;
using CardinalLib.Qemu;
using CardinalLib.Hardware;
using CardinalLib.Machines;
using CardinalLib.Core;
using System.Linq;

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
        Scrollable MachineInfoPanel = new Scrollable
        {
            Visible = false,
            Border = BorderType.None,Padding = 0
        };

        // Machine Title
        StackLayout TitleCard = new StackLayout
        {
            Orientation = Orientation.Horizontal,
            Padding = new Padding(5),
            VerticalContentAlignment = VerticalAlignment.Center
        };

        StackLayout TitleInnerPanel = new StackLayout
        {
            Orientation = Orientation.Vertical,
            Padding = new Padding(5)
        };

        ImageView MachineIcon = new ImageView
        {
            Image = Icons.Get("vm", IconSize.Large)
        };

        Label MachineTitle = new Label
        {
            Font = new Font(SystemFont.Bold, 26)
        };

        Label MachineArch = new Label
        {
            Font = new Font(SystemFont.Default)
        };

        // Machine Notes
        StackLayout NotesCard = new StackLayout
        {
            Orientation = Orientation.Vertical,
            Padding = new Padding(5),
            HorizontalContentAlignment = HorizontalAlignment.Stretch
        };

        Label NotesTitle = new Label
        {
            Font = new Font(SystemFont.Bold, 16),
            Text = "Notes:"
        };

        Label NotesValue = new Label {
        };

        // Machine Ram Info
        StackLayout RamCard = new StackLayout
        {
            Orientation = Orientation.Vertical,
            Padding = new Padding(5),
            HorizontalContentAlignment = HorizontalAlignment.Stretch
        };

        Label RamTitle = new Label
        {
            Font = new Font(SystemFont.Bold, 16),
            Text = "RAM:"
        };

        Label RamValue = new Label {};

        // Machine Disk Info
        StackLayout DisksCard = new StackLayout
        {
            Orientation = Orientation.Vertical,
            Padding = new Padding(5),
            HorizontalContentAlignment = HorizontalAlignment.Stretch
        };

        Label DisksTitle = new Label
        {
            Text = "Disks:",
            Font = new Font(SystemFont.Bold, 16)
        };

        StackLayout DisksInnerPanel = new StackLayout
        {
            Orientation = Orientation.Vertical,
            Padding = new Padding(0, 5),
            HorizontalContentAlignment = HorizontalAlignment.Stretch
        };

        StackLayout InfoStack = new StackLayout
        {
            Orientation = Orientation.Vertical,
            Padding = new Padding(10, 15),
            HorizontalContentAlignment = HorizontalAlignment.Stretch
        };
        #endregion

        #region "UI"
        string appTitle = "Cardinal QEMU " + AppInfo.FormattedVersion;
        
        public MainForm()
        {
            Title = appTitle;
            MinimumSize = new Size(450, 300);
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

            // Machine Info
            int vPad = 10;

            // Machine title header
            TitleCard.Items.Add(MachineIcon);
            TitleCard.Items.Add(TitleInnerPanel);
            TitleInnerPanel.Items.Add(MachineTitle);
            TitleInnerPanel.Items.Add(MachineArch);
            InfoStack.Items.Add(TitleCard);

            // Ram
            InfoStack.Items.Add(new Panel { Height = vPad });
            InfoStack.Items.Add(NotesCard);
            NotesCard.Items.Add(NotesTitle);
            NotesCard.Items.Add(NotesValue);

            // Ram
            InfoStack.Items.Add(new Panel { Height = vPad });
            InfoStack.Items.Add(RamCard);
            RamCard.Items.Add(RamTitle);
            RamCard.Items.Add(RamValue);

            // Disks
            InfoStack.Items.Add(new Panel { Height = vPad });
            InfoStack.Items.Add(DisksCard);
            DisksCard.Items.Add(DisksTitle);
            DisksCard.Items.Add(DisksInnerPanel);
                       
            MachineInfoPanel.Content = InfoStack;

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
            refreshCommand.Executed += OnRefresh;

            var disksCommand = new Command
            {
                MenuText = "Disk Manager",
                ToolBarText = "Disks",
                Image = Icons.Get("drive-harddisk")
            };
            disksCommand.Executed += OnDisks;

            var mediaInsertCommand = new Command
            {
                MenuText = "Insert Media",
                ToolBarText = "ISO/IMG",
                Image = Icons.Get("media-optical")
            };
            mediaInsertCommand.Executed += OnMediaInsert;

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
                    refreshCommand,
                    new SeparatorToolItem() { Type = SeparatorToolItemType.FlexibleSpace },
                    mediaInsertCommand,
                    startCommand,
                    settingsCommand
                },
                Style = "NativeToolbar"
            };

            LoadMachines();
        }
        #endregion

        public bool MachinesLoaded { get; set; } = false;

        private void LoadMachines()
        {
            Machines = Machine.GetAll();
            MachineSelectorItems.Clear();
            var machineIcon =  Icons.Get("vm", IconSize.Large);

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

            if (MachineSelectorItems.Count > 0)
                MachineSelector.SelectedRow = 0;

            MachinesLoaded = true;
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            if (CurrentMachine != null)
                Title = string.Format("{0} - {1}", appTitle, CurrentMachine.Name);
            else
                Title = appTitle;
        }

        private void UpdateSelectedMachine()
        {
            MachineInfoPanel.Visible = CurrentMachine != null;

            if(CurrentMachine != null)
            {
                MachineTitle.Text = CurrentMachine.Name;
                MachineArch.Text = CurrentMachine.Arch;

                NotesCard.Visible = !string.IsNullOrEmpty(CurrentMachine.Notes);
                NotesValue.Text = CurrentMachine.FormattedNotes;
                RamValue.Text = CurrentMachine.Ram.Format(ByteFormat.MB);

                DisksInnerPanel.Items.Clear();
                foreach(var disk in CurrentMachine.Disks)
                {
                    var diskTitlePanel = new StackLayout
                    {
                        Orientation = Orientation.Vertical,
                        Padding = new Padding(0, 0),
                        Spacing = 5,
                        HorizontalContentAlignment = HorizontalAlignment.Stretch
                    };

                    diskTitlePanel.Items.Add(new Label
                    {
                        Text = string.Format("{0} ({1}):", disk.Drive, disk.Name),
                        Font = new Font(SystemFont.Bold),
                        VerticalAlignment = VerticalAlignment.Center
                    });

                    diskTitlePanel.Items.Add(new Label
                    {
                        Text = disk.Info.FormattedText
                    });

                    var diskPanel = new StackLayout
                    {
                        Orientation = Orientation.Vertical,
                        HorizontalContentAlignment = HorizontalAlignment.Stretch
                    };

                    diskPanel.Items.Add(diskTitlePanel);
                    var diskStatus = new ProgressBar
                    {
                        MinValue = 0,
                        MaxValue = 100,
                        Value = disk.Info.PercentageInt
                    };
                    diskPanel.Items.Add(diskStatus);
                    DisksInnerPanel.Items.Add(diskPanel);
                }
            }
        }

        /**
         * Event handlers
         */

        // Machine Menu
        public void OnNew(object sender, EventArgs e)
        {
            var newMachineDialog = new Dialogs.NewMachineDialog();
            var newMachine = newMachineDialog.ShowModal(this);

            if (newMachine != null)
            {
                
            }
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            var lastSelected = CurrentMachine;

            LoadMachines();

            if (lastSelected != null)
            {
                var matchingMachine = (from machine in Machines
                                      where machine.RootDirectory.Equals(lastSelected.RootDirectory)
                                      select machine).FirstOrDefault();
                if (matchingMachine != null)
                {
                    MachineSelector.SelectedRow = Array.IndexOf(Machines, matchingMachine);
                }
            }
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

        public void OnMediaInsert(object sender, EventArgs e)
        {
            var insertDiskDialog = new Dialogs.InsertDiskDialog(CurrentMachine?.TempImage);
            var diskImage = insertDiskDialog.ShowModal(this);

            if(diskImage != null && CurrentMachine != null)
            {
                CurrentMachine.TempImage = diskImage; 
            }
        }

        // Application Menu
        public void OnAbout(object sender, EventArgs e)
        {
            var aboutDialog = new AboutDialog
            {
                Title = string.Format("About Cardinal QEMU {0}", AppInfo.FormattedVersion),
                Version = string.Format("Version {0}", AppInfo.FormattedVersion),
                Copyright = string.Format("©2019-{0} Ian Martinez", AppInfo.CopyrightYear),
                ProgramDescription = "A cross-platform GUI for QEMU."
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
            UpdateSelectedMachine();
        }
    }
}
