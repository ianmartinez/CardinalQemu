using Eto.Drawing;
using Eto.Forms;

namespace CardinalQemu
{
    public class InputControl<TControl> : StackLayout where TControl : Control
    {
        public Label TitleLabel { get; set; } = new Label();
        public TControl Input { get; set; }

        public string Title
        {
            get => TitleLabel.Text;
            set => TitleLabel.Text = value;
        }

        public InputControl(TControl control)
        {
            Input = control;

            Orientation = Orientation.Vertical;
            VerticalContentAlignment = VerticalAlignment.Center;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            Padding = new Padding(0, 0);
            Spacing = 5;

            Items.Add(TitleLabel);
            Items.Add(Input);
        }
    }
}
