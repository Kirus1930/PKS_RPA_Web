#nullable disable
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic; // добавьте ссылку на Microsoft.VisualBasic

namespace DeliveryApp
{
    public class Form1 : Form
    {
        // Существующие элементы
        private Label lblPackage, lblDriver, lblDeliveryType, lblPriority;
        private TextBox txtPackage, txtDriver;
        private ComboBox cboDeliveryType, cboPriority;
        private Button btnAdd, btnClear;
        private DataGridView dgvDeliveries;

        // Новые кнопки
        private Button btnDispatch, btnAssignDriver;

        private DataTable deliveriesTable;
        private int nextId = 1;

        public Form1()
        {
            InitializeComponent();
            SetupDataTable();
            UpdateAddButtonState();
            this.Load += Form1_Load;
        }

        private void InitializeComponent()
        {
            this.Text = "Delivery Registration System";
            this.Size = new Size(850, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Поля ввода (как ранее)
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

            // Кнопки (старые)
            btnAdd = new Button() { Text = "Add", Location = new Point(140, 200), Size = new Size(100, 30), BackColor = Color.LightGreen, Enabled = false };
            btnClear = new Button() { Text = "Clear Form", Location = new Point(260, 200), Size = new Size(100, 30), BackColor = Color.LightSalmon };

            // НОВЫЕ КНОПКИ
            btnDispatch = new Button() { Text = "Dispatch Delivery", Location = new Point(400, 200), Size = new Size(120, 30), BackColor = Color.LightBlue, Enabled = false };
            btnAssignDriver = new Button() { Text = "Assign Driver", Location = new Point(540, 200), Size = new Size(100, 30), BackColor = Color.LightCoral, Enabled = false };

            // Таблица
            dgvDeliveries = new DataGridView()
            {
                Location = new Point(30, 260),
                Size = new Size(780, 300),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.ControlLightLight,
                BorderStyle = BorderStyle.FixedSingle,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            // Добавление элементов
            this.Controls.AddRange(new Control[] {
                lblPackage, txtPackage,
                lblDriver, txtDriver,
                lblDeliveryType, cboDeliveryType,
                lblPriority, cboPriority,
                btnAdd, btnClear, btnDispatch, btnAssignDriver,
                dgvDeliveries
            });

            // Подписка событий
            txtPackage.TextChanged += OnInputChanged;
            txtDriver.TextChanged += OnInputChanged;
            cboDeliveryType.SelectedIndexChanged += OnInputChanged;
            cboPriority.SelectedIndexChanged += OnInputChanged;
            btnAdd.Click += BtnAdd_Click;
            btnClear.Click += BtnClear_Click;
            btnDispatch.Click += BtnDispatch_Click;
            btnAssignDriver.Click += BtnAssignDriver_Click;
            dgvDeliveries.SelectionChanged += DgvDeliveries_SelectionChanged;
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
            // Настройка ширины колонок
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

        // НОВОЕ: обновление состояния кнопок Dispatch и Assign Driver
        private void UpdateActionButtonsState()
        {
            bool rowSelected = dgvDeliveries.SelectedRows.Count > 0;
            btnAssignDriver.Enabled = rowSelected;
            if (rowSelected)
            {
                DataGridViewRow row = dgvDeliveries.SelectedRows[0];
                string status = row.Cells["Status"].Value?.ToString();
                btnDispatch.Enabled = (status != "Dispatched");
            }
            else
            {
                btnDispatch.Enabled = false;
            }
        }

        // Событие выбора строки
        private void DgvDeliveries_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActionButtonsState();
        }

        // Обработчик Dispatch Delivery
        private void BtnDispatch_Click(object sender, EventArgs e)
        {
            if (dgvDeliveries.SelectedRows.Count == 0) return;
            DataGridViewRow row = dgvDeliveries.SelectedRows[0];
            // Проверка статуса (на всякий случай)
            if (row.Cells["Status"].Value?.ToString() == "Dispatched")
            {
                MessageBox.Show("Эта доставка уже отправлена.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            row.Cells["Status"].Value = "Dispatched";
            // Обновить состояние кнопок после изменения статуса
            UpdateActionButtonsState();
        }

        // Обработчик Assign Driver
        private void BtnAssignDriver_Click(object sender, EventArgs e)
        {
            if (dgvDeliveries.SelectedRows.Count == 0) return;
            DataGridViewRow row = dgvDeliveries.SelectedRows[0];
            string currentDriver = row.Cells["Driver"].Value?.ToString() ?? "";
            string newDriver = Interaction.InputBox(
                "Введите имя водителя:", 
                "Назначение водителя", 
                currentDriver, 
                -1, -1);
            if (!string.IsNullOrWhiteSpace(newDriver))
            {
                row.Cells["Driver"].Value = newDriver.Trim();
                // Опционально: можно обновить состояние кнопок (хотя оно не меняется от этого)
            }
        }
    }
}