using Eto.Drawing;
using Eto.Forms;

namespace CardinalQemu
{
    public class InputControl<T> : StackLayout where T : Control
    {
        private Label TitleLabel { get; set; } = new Label();

        public string Title
        {
            get => TitleLabel.Text;
            set => TitleLabel.Text = value;
        }

        public T Value { get; set; }

        public InputControl(T control)
        {
            Value = control;

            Orientation = Orientation.Vertical;
            VerticalContentAlignment = VerticalAlignment.Center;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            Padding = new Padding(10, 10);
            Spacing = 5;

            Items.Add(Title);
            Items.Add(Value);
        }
    }
}
