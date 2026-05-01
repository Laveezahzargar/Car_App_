namespace P2_Car_Crud
{
    partial class Car_form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            txtName = new TextBox();
            txtPrice = new TextBox();
            txtYear = new TextBox();
            cmbManufacturer = new ComboBox();
            cmbEngine = new ComboBox();
            cmbTransmission = new ComboBox();
            cmbFueltype = new ComboBox();
            button1 = new Button();
            dataGridViewCar = new DataGridView();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            label12 = new Label();
            label13 = new Label();
            label14 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCar).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(437, 69);
            label1.Name = "label1";
            label1.Size = new Size(49, 20);
            label1.TabIndex = 0;
            label1.Text = "Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(437, 103);
            label2.Name = "label2";
            label2.Size = new Size(116, 20);
            label2.TabIndex = 1;
            label2.Text = "Manufacturer_Id";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(437, 137);
            label3.Name = "label3";
            label3.Size = new Size(130, 20);
            label3.TabIndex = 2;
            label3.Text = "EngineCapacity_Id";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(437, 171);
            label4.Name = "label4";
            label4.Size = new Size(93, 20);
            label4.TabIndex = 3;
            label4.Text = "Transmission";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(437, 205);
            label5.Name = "label5";
            label5.Size = new Size(73, 20);
            label5.TabIndex = 4;
            label5.Text = "Fuel_Type";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(437, 234);
            label6.Name = "label6";
            label6.Size = new Size(41, 20);
            label6.TabIndex = 5;
            label6.Text = "Price";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(437, 267);
            label7.Name = "label7";
            label7.Size = new Size(37, 20);
            label7.TabIndex = 6;
            label7.Text = "Year";
            // 
            // txtName
            // 
            txtName.Location = new Point(649, 62);
            txtName.Name = "txtName";
            txtName.Size = new Size(557, 27);
            txtName.TabIndex = 7;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // txtPrice
            // 
            txtPrice.Location = new Point(648, 231);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(557, 27);
            txtPrice.TabIndex = 8;
            // 
            // txtYear
            // 
            txtYear.Location = new Point(648, 264);
            txtYear.Name = "txtYear";
            txtYear.Size = new Size(557, 27);
            txtYear.TabIndex = 9;
            // 
            // cmbManufacturer
            // 
            cmbManufacturer.FormattingEnabled = true;
            cmbManufacturer.Location = new Point(649, 95);
            cmbManufacturer.Name = "cmbManufacturer";
            cmbManufacturer.Size = new Size(556, 28);
            cmbManufacturer.TabIndex = 10;
            // 
            // cmbEngine
            // 
            cmbEngine.FormattingEnabled = true;
            cmbEngine.Location = new Point(648, 129);
            cmbEngine.Name = "cmbEngine";
            cmbEngine.Size = new Size(557, 28);
            cmbEngine.TabIndex = 11;
            // 
            // cmbTransmission
            // 
            cmbTransmission.FormattingEnabled = true;
            cmbTransmission.Location = new Point(648, 163);
            cmbTransmission.Name = "cmbTransmission";
            cmbTransmission.Size = new Size(557, 28);
            cmbTransmission.TabIndex = 12;
            // 
            // cmbFueltype
            // 
            cmbFueltype.FormattingEnabled = true;
            cmbFueltype.Location = new Point(648, 197);
            cmbFueltype.Name = "cmbFueltype";
            cmbFueltype.Size = new Size(557, 28);
            cmbFueltype.TabIndex = 13;
            // 
            // button1
            // 
            button1.Location = new Point(1053, 306);
            button1.Name = "button1";
            button1.Size = new Size(98, 47);
            button1.TabIndex = 14;
            button1.Text = "Add";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnAdd_Click;
            // 
            // dataGridViewCar
            // 
            dataGridViewCar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCar.Location = new Point(71, 369);
            dataGridViewCar.Name = "dataGridViewCar";
            dataGridViewCar.RowHeadersWidth = 51;
            dataGridViewCar.Size = new Size(1255, 350);
            dataGridViewCar.TabIndex = 15;
            dataGridViewCar.CellContentClick += dataGridView1_CellContentClick;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(260, 69);
            label8.Name = "label8";
            label8.Size = new Size(24, 20);
            label8.TabIndex = 16;
            label8.Text = "1 .";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(260, 103);
            label9.Name = "label9";
            label9.Size = new Size(24, 20);
            label9.TabIndex = 17;
            label9.Text = "2 .";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(260, 137);
            label10.Name = "label10";
            label10.Size = new Size(24, 20);
            label10.TabIndex = 18;
            label10.Text = "3 .";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(260, 171);
            label11.Name = "label11";
            label11.Size = new Size(24, 20);
            label11.TabIndex = 19;
            label11.Text = "4 .";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(260, 205);
            label12.Name = "label12";
            label12.Size = new Size(24, 20);
            label12.TabIndex = 20;
            label12.Text = "5 .";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(260, 238);
            label13.Name = "label13";
            label13.Size = new Size(24, 20);
            label13.TabIndex = 21;
            label13.Text = "6 .";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(260, 271);
            label14.Name = "label14";
            label14.Size = new Size(24, 20);
            label14.TabIndex = 22;
            label14.Text = "7 .";
            // 
            // Car_form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1402, 742);
            Controls.Add(label14);
            Controls.Add(label13);
            Controls.Add(label12);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(dataGridViewCar);
            Controls.Add(button1);
            Controls.Add(cmbFueltype);
            Controls.Add(cmbTransmission);
            Controls.Add(cmbEngine);
            Controls.Add(cmbManufacturer);
            Controls.Add(txtYear);
            Controls.Add(txtPrice);
            Controls.Add(txtName);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Car_form";
            Text = "Car_form";
            Load += Car_form_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewCar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox txtName;
        private TextBox txtPrice;
        private TextBox txtYear;
        private ComboBox cmbManufacturer;
        private ComboBox cmbEngine;
        private ComboBox cmbTransmission;
        private ComboBox cmbFueltype;
        private Button button1;
        private DataGridView dataGridViewCar;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
    }
}