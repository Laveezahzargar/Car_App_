namespace P2_Car_Crud
{
    public partial class Main_form : Form
    {
        public Main_form()
        {
            InitializeComponent();
        }
        private void btnCars_Click(object sender, EventArgs e)
        {
            Car_form f = new Car_form();
            f.Show();
        }
        private void btnManf_Click(object sender, EventArgs e)
        {
            Manufacturer_form f = new Manufacturer_form();
            f.Show();
        }
        private void btnEngine_Click(object sender, EventArgs e)
        {
            EngineCapacity_form f = new EngineCapacity_form();
            f.Show();
        }

    }
}
