#nullable disable
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DeliveryApp
{
    public class Form1 : Form
    {
        private Label lblPackage, lblDriver, lblDeliveryType, lblPriority;
        private TextBox txtPackage, txtDriver;
        private ComboBox cboDeliveryType, cboPriority;
        private Button btnAdd, btnClear;
        private DataGridView dgvDeliveries;

        private DataTable deliveriesTable;
        private int nextId = 1;

        public Form1()
        {
            InitializeComponent();
            SetupDataTable();
            UpdateAddButtonState();
            this.Load += Form1_Load;  // подписываемся на событие загрузки формы
        }

        private void InitializeComponent()
        {
            this.Text = "Delivery Registration System";
            this.Size = new Size(800, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Метки
            lblPackage = new Label() { Text = "Package:", Location = new Point(30, 30), Size = new Size(100, 25) };
            txtPackage = new TextBox() { Location = new Point(140, 30), Size = new Size(220, 25) };

            lblDriver = new Label() { Text = "Driver:", Location = new Point(30, 70), Size = new Size(100, 25) };
            txtDriver = new TextBox() { Location = new Point(140, 70), Size = new Size(220, 25) };

            lblDeliveryType = new Label() { Text = "Delivery Type:", Location = new Point(30, 110), Size = new Size(100, 25) };
            cboDeliveryType = new ComboBox()
            {
                Location = new Point(140, 110),
                Size = new Size(220, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboDeliveryType.Items.AddRange(new object[] { "Standard", "Express", "Same-day" });

            lblPriority = new Label() { Text = "Priority:", Location = new Point(30, 150), Size = new Size(100, 25) };
            cboPriority = new ComboBox()
            {
                Location = new Point(140, 150),
                Size = new Size(220, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboPriority.Items.AddRange(new object[] { "Low", "Medium", "High" });

            // Кнопки
            btnAdd = new Button() { Text = "Add", Location = new Point(140, 200), Size = new Size(100, 30), BackColor = Color.LightGreen, Enabled = false };
            btnClear = new Button() { Text = "Clear Form", Location = new Point(260, 200), Size = new Size(100, 30), BackColor = Color.LightSalmon };

            // Таблица
            dgvDeliveries = new DataGridView()
            {
                Location = new Point(30, 260),
                Size = new Size(720, 230),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.ControlLightLight,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Добавляем элементы
            this.Controls.AddRange(new Control[] {
                lblPackage, txtPackage,
                lblDriver, txtDriver,
                lblDeliveryType, cboDeliveryType,
                lblPriority, cboPriority,
                btnAdd, btnClear,
                dgvDeliveries
            });

            // События
            txtPackage.TextChanged += OnInputChanged;
            txtDriver.TextChanged += OnInputChanged;
            cboDeliveryType.SelectedIndexChanged += OnInputChanged;
            cboPriority.SelectedIndexChanged += OnInputChanged;
            btnAdd.Click += BtnAdd_Click;
            btnClear.Click += BtnClear_Click;
        }

        private void SetupDataTable()
        {
            deliveriesTable = new DataTable();
            deliveriesTable.Columns.Add("ID", typeof(int));
            deliveriesTable.Columns.Add("Package", typeof(string));
            deliveriesTable.Columns.Add("Driver", typeof(string));
            deliveriesTable.Columns.Add("Delivery Type", typeof(string));
            deliveriesTable.Columns.Add("Priority", typeof(string));
            deliveriesTable.Columns.Add("Status", typeof(string));
            dgvDeliveries.DataSource = deliveriesTable;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Настройка ширины колонок после того, как форма загрузилась и колонки созданы
            if (dgvDeliveries.Columns.Contains("ID"))
                dgvDeliveries.Columns["ID"].Width = 50;
            if (dgvDeliveries.Columns.Contains("Package"))
                dgvDeliveries.Columns["Package"].Width = 120;
            if (dgvDeliveries.Columns.Contains("Driver"))
                dgvDeliveries.Columns["Driver"].Width = 120;
            if (dgvDeliveries.Columns.Contains("Delivery Type"))
                dgvDeliveries.Columns["Delivery Type"].Width = 100;
            if (dgvDeliveries.Columns.Contains("Priority"))
                dgvDeliveries.Columns["Priority"].Width = 80;
            if (dgvDeliveries.Columns.Contains("Status"))
                dgvDeliveries.Columns["Status"].Width = 80;
        }

        private void OnInputChanged(object sender, EventArgs e)
        {
            UpdateAddButtonState();
        }

        private void UpdateAddButtonState()
        {
            bool isValid = !string.IsNullOrWhiteSpace(txtPackage.Text) &&
                           !string.IsNullOrWhiteSpace(txtDriver.Text) &&
                           cboDeliveryType.SelectedItem != null &&
                           cboPriority.SelectedItem != null;
            btnAdd.Enabled = isValid;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            DataRow newRow = deliveriesTable.NewRow();
            newRow["ID"] = nextId++;
            newRow["Package"] = txtPackage.Text.Trim();
            newRow["Driver"] = txtDriver.Text.Trim();
            newRow["Delivery Type"] = cboDeliveryType.SelectedItem.ToString();
            newRow["Priority"] = cboPriority.SelectedItem.ToString();
            newRow["Status"] = "New";
            deliveriesTable.Rows.Add(newRow);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtPackage.Clear();
            txtDriver.Clear();
            cboDeliveryType.SelectedIndex = -1;
            cboPriority.SelectedIndex = -1;
        }
    }
}