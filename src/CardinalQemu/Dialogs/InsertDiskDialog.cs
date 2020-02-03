using System;
using CardinalLib.Hardware;
using Eto.Drawing;
using Eto.Forms;

namespace CardinalQemu.Dialogs
{
    public class InsertDiskDialog : Dialog<DiskImage>
    {
        Button SelectResultButton = new Button
        {
            Text = "Select"
        };

        Button CancelResultButton = new Button
        {
            Text = "Cancel"
        };

        TextBox FileNameTextBox = new TextBox {
            Width = 300
        };

        Button SelectDiskButton = new Button {
            Text = "...", Width = -1
        };

        CheckBox BootDiskCheckBox = new CheckBox
        {
            Text = "Boot Disk"
        };

        CheckBox FloppyCheckBox = new CheckBox
        {
            Text = "Floppy"
        };

        FileFilter[] filters = {
            new FileFilter("Image Files (*.iso, *.img)", ".iso", ".img")
        };

        public InsertDiskDialog(DiskImage diskImage)
        {
            Title = "Insert Disk";
            ClientSize = new Size(400, -1);
            Resizable = false;
            Maximizable = false;
            Minimizable = false;

            PositiveButtons.Add(SelectResultButton);
            NegativeButtons.Add(CancelResultButton);
            DefaultButton = SelectDiskButton;

            if (diskImage != null)
            {
                FileNameTextBox.Text = diskImage.DiskFile;
                BootDiskCheckBox.Checked = diskImage.IsBootDisk;
                FloppyCheckBox.Checked = diskImage.IsFloppy;
            }

            var dialogStack = new StackLayout
            {
                Orientation = Orientation.Vertical,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Padding = new Padding(10,10),
                Spacing = 5
            };

            dialogStack.Items.Add(new Label {
                Text = "ISO/IMG file:"
            });

            var innerStack = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Padding = new Padding(0, 0),
                Spacing = 5,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            innerStack.Items.Add(new StackLayoutItem(FileNameTextBox, true));
            innerStack.Items.Add(SelectDiskButton);
            dialogStack.Items.Add(innerStack);

            dialogStack.Items.Add(new Panel { Height = 10 });
            dialogStack.Items.Add(BootDiskCheckBox);
            dialogStack.Items.Add(FloppyCheckBox);

            SelectDiskButton.Click += SelectDiskButon_Click;
            SelectResultButton.Click += SelectResultButton_Click;
            CancelResultButton.Click += CancelResultButton_Click;

            Content = dialogStack;
        }

        private void CancelResultButton_Click(object sender, EventArgs e)
        {
            Result = null;
            Close();
        }

        private void SelectResultButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FileNameTextBox.Text))
            {
                Result = new DiskImage
                {
                    DiskFile = FileNameTextBox.Text,
                    IsFloppy = (bool)FloppyCheckBox.Checked,
                    IsBootDisk = (bool)BootDiskCheckBox.Checked
                };
            }
            else
            {
                Result = null;
            }

            Close();
        }

        public void SelectDiskButon_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                FileName = FileNameTextBox.Text
            };

            foreach (var filter in filters)
                openDialog.Filters.Add(filter);

            openDialog.CurrentFilterIndex = 0;

            if (openDialog.ShowDialog(this) == DialogResult.Ok)
            {
                FileNameTextBox.Text = openDialog.FileName;
            }
        }
    }
}
