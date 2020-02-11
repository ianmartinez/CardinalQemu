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
        TabPage MainPage = new TabPage { Text = "Main" };
        TabPage MemoryPage = new TabPage { Text = "Memory" };
        TabPage DisksPage = new TabPage { Text = "Disks" };

        // Buttons
        Button CreateResultButton = new Button{ Text = "Create" };
        Button CancelResultButton = new Button { Text = "Cancel" };

        // Main Tab
        InputControl<TextBox> MachineName = new InputControl<TextBox>(new TextBox()) {
            Title = "Machine Name:"
        };

        public NewMachineDialog()
        {
            Title = "New Machine";
            ClientSize = new Size(600, -1);
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

            DialogTabs.Pages.Add(MainPage);
            DialogTabs.Pages.Add(MemoryPage);
            DialogTabs.Pages.Add(DisksPage);

            MainPage.Content = MachineName;

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
