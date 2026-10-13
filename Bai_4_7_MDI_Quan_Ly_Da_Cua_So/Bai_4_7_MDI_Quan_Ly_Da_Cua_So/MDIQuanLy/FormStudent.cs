namespace MDIQuanLy;

public class FormStudent : Form
{
    private readonly DataGridView grid = new();
    private readonly BindingSource binding = new();
    private readonly BindingList<StudentRecord> records = new();
    private readonly TextBox txtId = new();
    private readonly TextBox txtName = new();
    private readonly TextBox txtAge = new();
    private readonly ComboBox cboGender = new();

    public FormStudent()
    {
        Text = "Quản lý Sinh viên";
        Size = new Size(850, 520);
        MinimumSize = new Size(650, 400);
        BuildUi();
        LoadDemoData();
        binding.DataSource = records;
        grid.DataSource = binding;
    }

    private void BuildUi()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(10) };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(root);

        var input = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
        AddInput(input, "Mã SV", txtId, 100);
        AddInput(input, "Họ tên", txtName, 170);
        AddInput(input, "Tuổi", txtAge, 60);
        cboGender.DropDownStyle = ComboBoxStyle.DropDownList;
        cboGender.Items.AddRange(new object[] { "Nam", "Nữ" });
        cboGender.SelectedIndex = 0;
        AddInput(input, "Giới tính", cboGender, 90);
        root.Controls.Add(input, 0, 0);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(0, 5, 0, 8) };
        var add = new Button { Text = "Thêm", Width = 90 };
        var delete = new Button { Text = "Xóa dòng", Width = 100 };
        var refresh = new Button { Text = "Làm mới", Width = 100 };
        var detail = new Button { Text = "Xem chi tiết", Width = 110 };
        add.Click += (_, _) => AddStudent();
        delete.Click += (_, _) => DeleteSelected();
        refresh.Click += (_, _) => { binding.ResetBindings(false); NotifyParent("Đã làm mới danh sách sinh viên."); };
        detail.Click += (_, _) => ShowDetail();
        buttons.Controls.AddRange(new Control[] { add, delete, refresh, detail });
        root.Controls.Add(buttons, 0, 1);

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.RowHeadersVisible = false;
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(StudentRecord.StudentId), HeaderText = "Mã SV" });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(StudentRecord.FullName), HeaderText = "Họ tên" });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(StudentRecord.Age), HeaderText = "Tuổi" });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(StudentRecord.Gender), HeaderText = "Giới tính" });

        var context = new ContextMenuStrip();
        context.Items.Add("Xóa dòng", null, (_, _) => DeleteSelected());
        context.Items.Add("Làm mới dữ liệu", null, (_, _) => { binding.ResetBindings(false); NotifyParent("Đã làm mới danh sách sinh viên."); });
        context.Items.Add("Xem chi tiết", null, (_, _) => ShowDetail());
        grid.ContextMenuStrip = context;
        root.Controls.Add(grid, 0, 2);
    }

    private static void AddInput(FlowLayoutPanel panel, string label, Control input, int width)
    {
        var group = new Panel { Width = width + 8, Height = 54, Margin = new Padding(2) };
        group.Controls.Add(new Label { Text = label, Left = 2, Top = 0, Width = width, Height = 20 });
        input.SetBounds(2, 22, width, 27);
        group.Controls.Add(input);
        panel.Controls.Add(group);
    }

    private void LoadDemoData()
    {
        records.Add(new StudentRecord { StudentId = "SV001", FullName = "Nguyễn Minh Anh", Age = 20, Gender = "Nữ" });
        records.Add(new StudentRecord { StudentId = "SV002", FullName = "Trần Quốc Bảo", Age = 21, Gender = "Nam" });
    }

    private void AddStudent()
    {
        string id = txtId.Text.Trim(), name = txtName.Text.Trim();
        if (id.Length == 0 || name.Length == 0 || !int.TryParse(txtAge.Text, out int age) || age < 1 || age > 120)
        {
            MessageBox.Show("Nhập mã, họ tên và tuổi hợp lệ (1–120).", "Dữ liệu chưa hợp lệ");
            return;
        }
        if (records.Any(s => s.StudentId.Equals(id, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Mã sinh viên đã tồn tại.");
            return;
        }
        records.Add(new StudentRecord { StudentId = id, FullName = name, Age = age, Gender = cboGender.Text });
        binding.ResetBindings(false);
        txtId.Clear(); txtName.Clear(); txtAge.Clear();
        NotifyParent("Thêm sinh viên thành công: " + name);
    }

    private void DeleteSelected()
    {
        if (grid.CurrentRow?.DataBoundItem is not StudentRecord student)
        {
            MessageBox.Show("Vui lòng chọn một dòng để xóa.");
            return;
        }
        if (MessageBox.Show($"Xóa sinh viên {student.FullName}?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        records.Remove(student);
        binding.ResetBindings(false);
        NotifyParent("Đã xóa sinh viên: " + student.FullName);
    }

    private void ShowDetail()
    {
        if (grid.CurrentRow?.DataBoundItem is not StudentRecord student)
        {
            MessageBox.Show("Vui lòng chọn một dòng.");
            return;
        }
        MessageBox.Show($"Mã SV: {student.StudentId}\nHọ tên: {student.FullName}\nTuổi: {student.Age}\nGiới tính: {student.Gender}",
            "Chi tiết sinh viên", MessageBoxButtons.OK, MessageBoxIcon.Information);
        NotifyParent("Đang xem chi tiết sinh viên " + student.StudentId);
    }

    private void NotifyParent(string message)
    {
        if (MdiParent is FormMain main) main.SetStatus(message);
    }

    private sealed class StudentRecord
    {
        public string StudentId { get; set; } = "";
        public string FullName { get; set; } = "";
        public int Age { get; set; }
        public string Gender { get; set; } = "Nam";
    }
}
