namespace AsyncProgressSinhVien;

public class FormMain : Form
{
    private readonly MenuStrip menu = new();
    private readonly StatusStrip status = new();
    private readonly ToolStripStatusLabel statusMessage = new();
    private readonly ToolStripStatusLabel statusProgress = new();
    private readonly DataGridView grid = new();
    private readonly ProgressBar progressBar = new();
    private readonly Label lblProgress = new();
    private readonly Button btnLoad = new();
    private readonly Button btnClear = new();
    private readonly BindingSource binding = new();
    private List<Student> students = new();

    public FormMain()
    {
        Text = "Bài 4.8 - Async/Await và ProgressBar";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1000, 650);
        MinimumSize = new Size(760, 480);
        BuildUi();
    }

    private void BuildUi()
    {
        var fileMenu = new ToolStripMenuItem("Hệ thống");
        fileMenu.DropDownItems.Add("Thoát", null, (_, _) => Close());
        var dataMenu = new ToolStripMenuItem("Dữ liệu");
        dataMenu.DropDownItems.Add("Nạp dữ liệu bất đồng bộ", null, async (_, _) => await LoadStudentsAsync());
        dataMenu.DropDownItems.Add("Xóa danh sách", null, (_, _) => ClearStudents());
        menu.Items.AddRange(new ToolStripItem[] { fileMenu, dataMenu });
        MainMenuStrip = menu;
        Controls.Add(menu);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(12)
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(root);
        root.BringToFront();

        var heading = new Label
        {
            Text = "QUẢN LÝ SINH VIÊN - NẠP DỮ LIỆU BẤT ĐỒNG BỘ",
            AutoSize = true,
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            ForeColor = Color.DarkBlue,
            Padding = new Padding(0, 5, 0, 12)
        };
        root.Controls.Add(heading, 0, 0);

        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
        btnLoad.Text = "Nạp dữ liệu";
        btnLoad.Width = 140;
        btnLoad.Height = 36;
        btnLoad.Click += async (_, _) => await LoadStudentsAsync();
        btnClear.Text = "Xóa danh sách";
        btnClear.Width = 140;
        btnClear.Height = 36;
        btnClear.Click += (_, _) => ClearStudents();
        actions.Controls.AddRange(new Control[] { btnLoad, btnClear });
        root.Controls.Add(actions, 0, 1);

        var progressPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = 2,
            Padding = new Padding(0, 8, 0, 12)
        };
        progressPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        progressPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        progressPanel.Controls.Add(new Label { Text = "Tiến trình nạp dữ liệu:", AutoSize = true }, 0, 0);
        progressBar.Dock = DockStyle.Fill;
        progressBar.Minimum = 0;
        progressBar.Maximum = 100;
        progressBar.Height = 22;
        progressPanel.Controls.Add(progressBar, 0, 1);
        lblProgress.Text = "0%";
        lblProgress.AutoSize = true;
        lblProgress.Anchor = AnchorStyles.Right;
        lblProgress.Padding = new Padding(12, 2, 0, 0);
        progressPanel.Controls.Add(lblProgress, 1, 1);
        root.Controls.Add(progressPanel, 0, 2);

        grid.Dock = DockStyle.Fill;
        grid.AutoGenerateColumns = false;
        grid.AllowUserToAddRows = false;
        grid.ReadOnly = true;
        grid.MultiSelect = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.RowHeadersVisible = false;
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Student.StudentId), HeaderText = "Mã SV" });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Student.FullName), HeaderText = "Họ tên" });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Student.Age), HeaderText = "Tuổi" });
        grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Student.Gender), HeaderText = "Giới tính" });
        grid.DataSource = binding;
        root.Controls.Add(grid, 0, 3);

        statusMessage.Text = "Sẵn sàng.";
        statusProgress.Text = "Tiến trình: 0%";
        status.Items.Add(statusMessage);
        status.Items.Add(new ToolStripStatusLabel { Spring = true });
        status.Items.Add(statusProgress);
        status.Dock = DockStyle.Bottom;
        Controls.Add(status);
    }

    private async Task LoadStudentsAsync()
    {
        if (!btnLoad.Enabled) return;

        btnLoad.Enabled = false;
        progressBar.Value = 0;
        lblProgress.Text = "0%";
        statusProgress.Text = "Tiến trình: 0%";
        statusMessage.Text = "Đang nạp dữ liệu...";
        btnClear.Enabled = false;

        var progress = new Progress<int>(percent =>
        {
            int safePercent = Math.Clamp(percent, 0, 100);
            progressBar.Value = safePercent;
            lblProgress.Text = $"{safePercent}%";
            statusProgress.Text = $"Tiến trình: {safePercent}%";
            statusMessage.Text = "Đang nạp dữ liệu...";
        });

        try
        {
            List<Student> loaded = await LoadStudentDataAsync(progress);
            students = loaded;
            binding.DataSource = null;
            binding.DataSource = students;
            statusMessage.Text = $"Nạp thành công {students.Count} sinh viên.";
            if (MdiParent is FormMain parent)
                parent.SetStatus($"Đã nạp {students.Count} sinh viên.");
        }
        catch (Exception ex)
        {
            statusMessage.Text = "Nạp dữ liệu thất bại.";
            MessageBox.Show("Có lỗi khi nạp dữ liệu:\n" + ex.Message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnLoad.Enabled = true;
            btnClear.Enabled = true;
            if (progressBar.Value == 100)
            {
                lblProgress.Text = "100%";
                statusProgress.Text = "Tiến trình: 100%";
            }
            if (statusMessage.Text.StartsWith("Nạp thành công"))
                statusMessage.Text += " Hoàn tất.";
            else if (statusMessage.Text != "Nạp dữ liệu thất bại.")
                statusMessage.Text = "Đã kết thúc tác vụ.";
        }
    }

    private static async Task<List<Student>> LoadStudentDataAsync(IProgress<int> progress)
    {
        return await Task.Run(async () =>
        {
            var result = new List<Student>();
            const int total = 20;

            for (int i = 1; i <= total; i++)
            {
                // Mô phỏng một bản ghi được đọc từ CSDL hoặc tệp.
                await Task.Delay(180);
                result.Add(new Student
                {
                    StudentId = $"SV{i:000}",
                    FullName = $"Sinh viên mẫu {i:00}",
                    Age = 18 + (i % 8),
                    Gender = i % 2 == 0 ? "Nữ" : "Nam"
                });

                int percent = i * 100 / total;
                progress.Report(percent);
            }

            return result;
        });
    }

    private void ClearStudents()
    {
        students.Clear();
        binding.DataSource = null;
        binding.DataSource = students;
        progressBar.Value = 0;
        lblProgress.Text = "0%";
        statusProgress.Text = "Tiến trình: 0%";
        statusMessage.Text = "Đã xóa danh sách.";
    }

    public void SetStatus(string message)
    {
        statusMessage.Text = message;
    }
}
