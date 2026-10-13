using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace QuanLySinhVien;

public class MainForm : Form
{
    private readonly BindingList<Student> students = new();
    private readonly BindingSource source = new();
    private readonly DataGridView grid = new();
    private readonly TextBox txtStudentId = new();
    private readonly TextBox txtFullName = new();
    private readonly TextBox txtAge = new();
    private readonly ComboBox cboGender = new();
    private readonly PictureBox picAvatar = new();
    private readonly Label lblAvatarPath = new();
    private readonly Button btnChooseAvatar = new();
    private readonly Button btnAdd = new();
    private readonly Button btnDelete = new();
    private readonly Button btnUpdate = new();
    private readonly Button btnExport = new();

    public MainForm()
    {
        Text = "Bài 4.6 - Quản lý sinh viên";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(900, 580);
        Size = new Size(1150, 720);
        Font = new Font("Segoe UI", 10F);
        BuildInterface();
        ConfigureBinding();
        LoadSampleData();
        source.ResetBindings(false);
        UpdateAvatar();
    }

    private void BuildInterface()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(12),
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64));
        Controls.Add(root);

        var left = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(8),
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        };
        left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(left, 0, 0);

        var heading = new Label
        {
            Text = "THÔNG TIN SINH VIÊN",
            Dock = DockStyle.Fill,
            AutoSize = true,
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.DarkBlue,
            Padding = new Padding(3, 5, 3, 12)
        };
        left.Controls.Add(heading, 0, 0);

        var fields = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 6,
            AutoScroll = true,
            Padding = new Padding(2)
        };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
        for (int i = 0; i < 5; i++)
            fields.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fields.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        left.Controls.Add(fields, 0, 1);

        AddField(fields, "Mã sinh viên:", txtStudentId, 0);
        AddField(fields, "Họ và tên:", txtFullName, 1);
        AddField(fields, "Tuổi:", txtAge, 2);

        cboGender.DropDownStyle = ComboBoxStyle.DropDownList;
        cboGender.Items.AddRange(new object[] { "Nam", "Nữ" });
        AddField(fields, "Giới tính:", cboGender, 3);

        var avatarPanel = new Panel { Dock = DockStyle.Fill, MinimumSize = new Size(100, 180) };
        picAvatar.Dock = DockStyle.Top;
        picAvatar.Height = 150;
        picAvatar.BorderStyle = BorderStyle.FixedSingle;
        picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
        picAvatar.BackColor = Color.WhiteSmoke;
        lblAvatarPath.Dock = DockStyle.Top;
        lblAvatarPath.Height = 36;
        lblAvatarPath.AutoEllipsis = true;
        lblAvatarPath.Text = "Chưa chọn ảnh";
        btnChooseAvatar.Text = "Chọn ảnh...";
        btnChooseAvatar.Dock = DockStyle.Top;
        btnChooseAvatar.Height = 34;
        btnChooseAvatar.Click += ChooseAvatar_Click;
        avatarPanel.Controls.Add(btnChooseAvatar);
        avatarPanel.Controls.Add(lblAvatarPath);
        avatarPanel.Controls.Add(picAvatar);
        fields.Controls.Add(new Label { Text = "Avatar:", Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(0, 8, 0, 0) }, 0, 4);
        fields.Controls.Add(avatarPanel, 1, 4);

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            WrapContents = true,
            Padding = new Padding(0, 8, 0, 2)
        };
        ConfigureButton(btnAdd, "Thêm");
        ConfigureButton(btnDelete, "Xóa");
        ConfigureButton(btnUpdate, "Cập nhật");
        ConfigureButton(btnExport, "Xuất CSV");
        btnAdd.Click += Add_Click;
        btnDelete.Click += Delete_Click;
        btnUpdate.Click += Update_Click;
        btnExport.Click += ExportCsv_Click;
        buttons.Controls.AddRange(new Control[] { btnAdd, btnDelete, btnUpdate, btnExport });
        left.Controls.Add(buttons, 0, 2);

        var right = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Padding = new Padding(8)
        };
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.Controls.Add(right, 1, 0);

        right.Controls.Add(new Label
        {
            Text = "DANH SÁCH SINH VIÊN",
            Dock = DockStyle.Fill,
            AutoSize = true,
            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = Color.DarkBlue,
            Padding = new Padding(3, 5, 3, 12)
        }, 0, 0);

        grid.Dock = DockStyle.Fill;
        grid.AutoGenerateColumns = false;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.ReadOnly = true;
        grid.MultiSelect = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.RowHeadersVisible = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.BackgroundColor = Color.White;
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Student.StudentId), HeaderText = "Mã SV", FillWeight = 75 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Student.FullName), HeaderText = "Họ và tên", FillWeight = 150 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Student.Age), HeaderText = "Tuổi", FillWeight = 45 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Student.Gender), HeaderText = "Giới tính", FillWeight = 70 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Student.AvatarPath), HeaderText = "Đường dẫn ảnh", FillWeight = 130 });
        grid.SelectionChanged += (_, _) => UpdateAvatar();
        right.Controls.Add(grid, 0, 1);
    }

    private static void AddField(TableLayoutPanel panel, string caption, Control control, int row)
    {
        panel.Controls.Add(new Label
        {
            Text = caption,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Padding = new Padding(0, 8, 3, 8)
        }, 0, row);
        control.Dock = DockStyle.Top;
        control.Margin = new Padding(3, 5, 3, 5);
        panel.Controls.Add(control, 1, row);
    }

    private static void ConfigureButton(Button button, string text)
    {
        button.Text = text;
        button.Width = 95;
        button.Height = 36;
        button.Margin = new Padding(3);
    }

    private void ConfigureBinding()
    {
        source.DataSource = students;
        grid.DataSource = source;

        txtStudentId.DataBindings.Add("Text", source, nameof(Student.StudentId), true, DataSourceUpdateMode.OnPropertyChanged);
        txtFullName.DataBindings.Add("Text", source, nameof(Student.FullName), true, DataSourceUpdateMode.OnPropertyChanged);
        txtAge.DataBindings.Add("Text", source, nameof(Student.Age), true, DataSourceUpdateMode.OnPropertyChanged, 0);
        cboGender.DataBindings.Add("SelectedItem", source, nameof(Student.Gender), true, DataSourceUpdateMode.OnPropertyChanged);

        source.CurrentChanged += (_, _) => UpdateAvatar();
        source.ListChanged += (_, _) => UpdateAvatar();
    }

    private void LoadSampleData()
    {
        students.Add(new Student { StudentId = "SV001", FullName = "Nguyễn Minh Anh", Age = 20, Gender = "Nữ" });
        students.Add(new Student { StudentId = "SV002", FullName = "Trần Quốc Bảo", Age = 21, Gender = "Nam" });
        if (students.Count > 0) source.Position = 0;
    }

    private Student? CurrentStudent => source.Current as Student;

    private void Add_Click(object? sender, EventArgs e)
    {
        if (!TryReadForm(out string id, out string name, out int age, out string gender))
            return;
        if (students.Any(s => string.Equals(s.StudentId, id, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Mã sinh viên đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var student = new Student { StudentId = id, FullName = name, Age = age, Gender = gender };
        students.Add(student);
        source.Position = students.Count - 1;
        MessageBox.Show("Đã thêm sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void Delete_Click(object? sender, EventArgs e)
    {
        if (CurrentStudent is not Student student)
        {
            MessageBox.Show("Vui lòng chọn sinh viên cần xóa.");
            return;
        }
        if (MessageBox.Show($"Bạn có chắc muốn xóa sinh viên {student.FullName}?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        int oldPosition = source.Position;
        students.Remove(student);
        if (students.Count > 0) source.Position = Math.Min(oldPosition, students.Count - 1);
        else ClearForm();
    }

    private void Update_Click(object? sender, EventArgs e)
    {
        if (CurrentStudent is not Student student)
        {
            MessageBox.Show("Vui lòng chọn sinh viên cần cập nhật.");
            return;
        }
        if (!TryReadForm(out string id, out string name, out int age, out string gender))
            return;
        if (students.Any(s => !ReferenceEquals(s, student) &&
            string.Equals(s.StudentId, id, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Mã sinh viên này đã được sử dụng.");
            return;
        }

        student.StudentId = id;
        student.FullName = name;
        student.Age = age;
        student.Gender = gender;
        source.ResetCurrentItem();
        grid.Refresh();
        MessageBox.Show("Đã cập nhật thông tin sinh viên.");
    }

    private bool TryReadForm(out string id, out string name, out int age, out string gender)
    {
        id = txtStudentId.Text.Trim();
        name = txtFullName.Text.Trim();
        gender = cboGender.SelectedItem?.ToString() ?? "Nam";
        age = 0;

        if (id.Length == 0 || name.Length == 0)
        {
            MessageBox.Show("Vui lòng nhập Mã sinh viên và Họ tên.", "Thiếu thông tin",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        if (!int.TryParse(txtAge.Text.Trim(), out age) || age < 1 || age > 120)
        {
            MessageBox.Show("Tuổi phải là số nguyên từ 1 đến 120.", "Dữ liệu không hợp lệ",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtAge.Focus();
            return false;
        }
        return true;
    }

    private void ChooseAvatar_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Chọn ảnh đại diện",
            Filter = "Tệp hình ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Tất cả tệp (*.*)|*.*",
            CheckFileExists = true,
            Multiselect = false
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        if (CurrentStudent is Student student)
        {
            student.AvatarPath = dialog.FileName;
            source.ResetCurrentItem();
            UpdateAvatar();
        }
        else
        {
            MessageBox.Show("Hãy thêm hoặc chọn một sinh viên trước khi gán ảnh.");
        }
    }

    private void UpdateAvatar()
    {
        string path = CurrentStudent?.AvatarPath ?? "";
        lblAvatarPath.Text = string.IsNullOrWhiteSpace(path) ? "Chưa chọn ảnh" : path;
        Image? old = picAvatar.Image;
        picAvatar.Image = null;
        old?.Dispose();

        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
        {
            try
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var temp = Image.FromStream(stream);
                picAvatar.Image = new Bitmap(temp);
            }
            catch (Exception ex)
            {
                lblAvatarPath.Text = "Không thể tải ảnh";
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }

    private void ExportCsv_Click(object? sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Xuất danh sách sinh viên ra CSV",
            Filter = "CSV UTF-8 (*.csv)|*.csv",
            DefaultExt = "csv",
            AddExtension = true,
            FileName = "DanhSachSinhVien.csv",
            OverwritePrompt = true
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            using var writer = new StreamWriter(dialog.FileName, false, new UTF8Encoding(true));
            writer.WriteLine("StudentId,FullName,Age,Gender,AvatarPath");
            foreach (Student s in students)
            {
                writer.WriteLine(string.Join(",",
                    Csv(s.StudentId), Csv(s.FullName), Csv(s.Age.ToString(CultureInfo.InvariantCulture)),
                    Csv(s.Gender), Csv(s.AvatarPath)));
            }
            MessageBox.Show("Xuất file CSV thành công.", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string Csv(string value)
    {
        value ??= "";
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }

    private void ClearForm()
    {
        txtStudentId.Clear();
        txtFullName.Clear();
        txtAge.Clear();
        cboGender.SelectedIndex = -1;
        UpdateAvatar();
    }
}
