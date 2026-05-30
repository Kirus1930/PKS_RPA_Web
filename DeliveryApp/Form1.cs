using System;
using System.Data;
using System.Windows.Forms;

namespace DeliveryApp
{
    public partial class Form1 : Form
    {
        private DataTable deliveriesTable;
        private int nextId = 1;

        // Элементы управления (создаются дизайнером или вручную)
        private TextBox txtPackage;
        private TextBox txtDriver;
        private ComboBox cboDeliveryType;
        private ComboBox cboPriority;
        private Button btnAdd;
        private Button btnClear;
        private DataGridView dgvDeliveries;

        public Form1()
        {
            InitializeComponent();
            SetupDataTable();
            UpdateAddButtonState();
        }

        private void InitializeComponent()
        {
            this.txtPackage = new TextBox();
            this.txtDriver = new TextBox();
            this.cboDeliveryType = new ComboBox();
            this.cboPriority = new ComboBox();
            this.btnAdd = new Button();
            this.btnClear = new Button();
            this.dgvDeliveries = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeliveries)).BeginInit();
            this.SuspendLayout();

            // txtPackage
            this.txtPackage.Location = new System.Drawing.Point(30, 30);
            this.txtPackage.Size = new System.Drawing.Size(200, 23);
            this.txtPackage.TextChanged += new EventHandler(this.OnInputChanged);

            // txtDriver
            this.txtDriver.Location = new System.Drawing.Point(30, 70);
            this.txtDriver.Size = new System.Drawing.Size(200, 23);
            this.txtDriver.TextChanged += new EventHandler(this.OnInputChanged);

            // cboDeliveryType
            this.cboDeliveryType.Location = new System.Drawing.Point(30, 110);
            this.cboDeliveryType.Size = new System.Drawing.Size(200, 23);
            this.cboDeliveryType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboDeliveryType.Items.AddRange(new object[] { "Standard", "Express", "Same-day" });
            this.cboDeliveryType.SelectedIndexChanged += new EventHandler(this.OnInputChanged);

            // cboPriority
            this.cboPriority.Location = new System.Drawing.Point(30, 150);
            this.cboPriority.Size = new System.Drawing.Size(200, 23);
            this.cboPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboPriority.Items.AddRange(new object[] { "Low", "Medium", "High" });
            this.cboPriority.SelectedIndexChanged += new EventHandler(this.OnInputChanged);

            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(30, 200);
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new EventHandler(this.BtnAdd_Click);

            // btnClear
            this.btnClear.Location = new System.Drawing.Point(150, 200);
            this.btnClear.Text = "Clear Form";
            this.btnClear.Click += new EventHandler(this.BtnClear_Click);

            // dgvDeliveries
            this.dgvDeliveries.Location = new System.Drawing.Point(30, 250);
            this.dgvDeliveries.Size = new System.Drawing.Size(700, 200);
            this.dgvDeliveries.AllowUserToAddRows = false;
            this.dgvDeliveries.ReadOnly = true;

            // Form1
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.AddRange(new Control[] { txtPackage, txtDriver, cboDeliveryType, cboPriority, btnAdd, btnClear, dgvDeliveries });
            this.Text = "Delivery Registration";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeliveries)).EndInit();
            this.ResumeLayout(false);
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

        private void OnInputChanged(object sender, EventArgs e)
        {
            UpdateAddButtonState();
        }

        private void UpdateAddButtonState()
        {
            bool isValid =
                !string.IsNullOrWhiteSpace(txtPackage.Text) &&
                !string.IsNullOrWhiteSpace(txtDriver.Text) &&
                cboDeliveryType.SelectedItem != null &&
                cboPriority.SelectedItem != null;

            btnAdd.Enabled = isValid;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // Добавление записи в таблицу
            DataRow newRow = deliveriesTable.NewRow();
            newRow["ID"] = nextId++;
            newRow["Package"] = txtPackage.Text.Trim();
            newRow["Driver"] = txtDriver.Text.Trim();
            newRow["Delivery Type"] = cboDeliveryType.SelectedItem.ToString();
            newRow["Priority"] = cboPriority.SelectedItem.ToString();
            newRow["Status"] = "New";
            deliveriesTable.Rows.Add(newRow);

            // Очистка формы (опционально, но по условию Clear Form отдельно)
            // Здесь не очищаем автоматически, чтобы можно было быстро добавить похожую запись
            // Пользователь использует кнопку Clear для очистки.
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtPackage.Clear();
            txtDriver.Clear();
            cboDeliveryType.SelectedIndex = -1;
            cboPriority.SelectedIndex = -1;
            UpdateAddButtonState();
        }
    }
}