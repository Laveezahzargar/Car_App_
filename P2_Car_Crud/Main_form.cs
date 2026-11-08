using Serilog;

namespace P2_Car_Crud
{
    public partial class Main_form : Form
    {
        public Main_form()
        {
            InitializeComponent();

            Color bg = Color.White;
            Color panel = Color.FromArgb(245, 245, 245);
            Color accent = Color.FromArgb(0, 120, 215);
            Color text = Color.Black;

            StyleButton(button1);
            StyleButton(button2);
            StyleButton(button3);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            CenterButtons();
            ApplyTheme();
        }
        private void StyleButton(Button btn)
        {
            btn.BackColor = Color.FromArgb(0, 120, 215);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
        }
        private void ApplyTheme()
        {
            this.BackColor = Color.White;
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            CenterButtons();
        }
        private void CenterButtons()
        {
            int centerX = this.ClientSize.Width / 2;

            button1.Left = centerX - (button1.Width / 2);
            button2.Left = centerX - (button2.Width / 2);
            button3.Left = centerX - (button3.Width / 2);

            // vertical spacing
            button1.Top = 120;
            button2.Top = button1.Bottom + 20;
            button3.Top = button2.Bottom + 20;
        }
        private void btnCars_Click(object sender, EventArgs e)
        {
            Car_form f = new Car_form();
            f.ShowDialog();
        }
        private void btnManf_Click(object sender, EventArgs e)
        {
            Manufacturer_form f = new Manufacturer_form();
            f.ShowDialog();
        }
        private void btnEngine_Click(object sender, EventArgs e)
        {
            EngineCapacity_form f = new EngineCapacity_form();
            f.ShowDialog();
        }

    }
}
