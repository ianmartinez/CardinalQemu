using System;
using CardinalLib.Machines;
using Eto.Drawing;
using Eto.Forms;

namespace CardinalQemu.Dialogs
{
    public class NewMachineDialog : Dialog<Machine>
    {
        // Tabs
        TabControl DialogTabs = new TabControl { };
        TabPage DisksPage = new TabPage { Text = "Disks" };

        // Buttons
        Button CreateResultButton = new Button{ Text = "Create" };
        Button CancelResultButton = new Button { Text = "Cancel" };

        // Main Page
        TabPage MainPage = new TabPage { Text = "Main" };
        TableLayout MainPageLayout = new TableLayout
        {
            Padding = new Padding(10, 10),
            Spacing = new Size(15, 10)
        };
        InputControl<TextBox> MachineName = new InputControl<TextBox>(new TextBox()) {
            Title = "Machine Name:"
        };

        InputControl<DropDown> MachineArch = new InputControl<DropDown>(new DropDown())
        {
            Title = "Architecture"
        };

        InputControl<DropDown> OsFamily = new InputControl<DropDown>(new DropDown())
        {
            Title = "OS Family"
        };

        InputControl<DropDown> OsName = new InputControl<DropDown>(new DropDown())
        {
            Title = "OS Name"
        };

        InputControl<DropDown> OsVersion = new InputControl<DropDown>(new DropDown())
        {
            Title = "OS Version"
        };

        // Memory Page
        TabPage MemoryPage = new TabPage { Text = "Memory" };
        TableLayout MemoryPageLayout = new TableLayout
        {
            Padding = new Padding(10, 10),
            Spacing = new Size(15, 10)
        };
        InputControl<NumericStepper> RamInput = new InputControl<NumericStepper>(new NumericStepper())
        {
            Title = "Ram"
        };

        public NewMachineDialog()
        {
            Title = "New Machine";
            ClientSize = new Size(540, -1);
            Resizable = false;
            Maximizable = false;
            Minimizable = false;
            PositiveButtons.Add(CreateResultButton);
            NegativeButtons.Add(CancelResultButton);
            DefaultButton = CreateResultButton;

            var dialogStack = new StackLayout
            {
                Orientation = Orientation.Vertical,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Padding = new Padding(10, 10),
                Spacing = 5
            };
            dialogStack.Items.Add(new StackLayoutItem(DialogTabs, true));

            // Setup main page
            DialogTabs.Pages.Add(MainPage);
            MainPageLayout.Rows.Add(new TableRow(
                new TableCell(MachineName, true),
                new TableCell(MachineArch, true)
            ));
            MainPageLayout.Rows.Add(new TableRow(
                new TableCell(OsFamily, true),
                new TableCell(OsName, true)
            ));
            MainPageLayout.Rows.Add(new TableRow(
                new TableCell(OsVersion, true),
                new TableCell(new Panel(), true)
            )); 
            MainPage.Content = MainPageLayout;

            // Setup Memory Page
            DialogTabs.Pages.Add(MemoryPage);
            MemoryPageLayout.Rows.Add(new TableRow(
               new TableCell(RamInput, true),
               new TableCell(new Panel(), true)
            ));
            MemoryPage.Content = MemoryPageLayout;

            DialogTabs.Pages.Add(DisksPage);

            Content = dialogStack;

            CreateResultButton.Click += CreateResultButton_Click;
            CancelResultButton.Click += CancelResultButton_Click;
        }

        private void CancelResultButton_Click(object sender, EventArgs e)
        {
            Result = null;
            Close();
        }

        private void CreateResultButton_Click(object sender, EventArgs e)
        {
            Result = new Machine();
            Close();
        }
    }
}
