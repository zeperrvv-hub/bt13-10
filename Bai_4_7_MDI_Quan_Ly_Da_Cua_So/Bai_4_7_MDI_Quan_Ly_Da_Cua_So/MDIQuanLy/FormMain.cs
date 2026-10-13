namespace MDIQuanLy;

public class FormMain : Form
{
    private readonly MenuStrip menu = new();
    private readonly ToolStrip tool = new();
    private readonly StatusStrip status = new();
    private readonly ToolStripStatusLabel userLabel = new();
    private readonly ToolStripStatusLabel messageLabel = new();
    private readonly ToolStripStatusLabel timeLabel = new();
    private readonly System.Windows.Forms.Timer clock = new();

    public FormMain()
    {
        Text = "Bài 4.7 - Khung ứng dụng MDI";
        IsMdiContainer = true;
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(900, 600);
        BuildMenu();
        BuildToolStrip();
        BuildStatusStrip();
        MainMenuStrip = menu;
        Controls.Add(menu);
        Controls.Add(tool);
        Controls.Add(status);
        menu.Dock = DockStyle.Top;
        tool.Dock = DockStyle.Top;
        status.Dock = DockStyle.Bottom;

        Shown += (_, _) => SetStatus("Hệ thống sẵn sàng.");
        clock.Interval = 1000;
        clock.Tick += (_, _) => timeLabel.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        clock.Start();
    }

    private void BuildMenu()
    {
        var system = new ToolStripMenuItem("Hệ thống");
        system.DropDownItems.Add("Đăng xuất", null, (_, _) => Logout());
        system.DropDownItems.Add(new ToolStripSeparator());
        system.DropDownItems.Add("Thoát", null, (_, _) => Close());

        var catalog = new ToolStripMenuItem("Danh mục");
        catalog.DropDownItems.Add("Quản lý Sinh viên", null, (_, _) => OpenChild<FormStudent>());
        catalog.DropDownItems.Add("Quản lý Sản phẩm", null, (_, _) => OpenChild<FormProduct>());

        var window = new ToolStripMenuItem("Cửa sổ");
        window.DropDownItems.Add("Cascade", null, (_, _) => { LayoutMdi(MdiLayout.Cascade); SetStatus("Đã xếp cửa sổ kiểu Cascade."); });
        window.DropDownItems.Add("Tile Horizontal", null, (_, _) => { LayoutMdi(MdiLayout.TileHorizontal); SetStatus("Đã xếp cửa sổ theo chiều ngang."); });
        window.DropDownItems.Add("Tile Vertical", null, (_, _) => { LayoutMdi(MdiLayout.TileVertical); SetStatus("Đã xếp cửa sổ theo chiều dọc."); });
        window.DropDownItems.Add(new ToolStripSeparator());
        window.DropDownItems.Add("Đóng cửa sổ con", null, (_, _) => { ActiveMdiChild?.Close(); SetStatus("Đã đóng cửa sổ con hiện tại."); });

        menu.Items.AddRange(new ToolStripItem[] { system, catalog, window });
    }

    private void BuildToolStrip()
    {
        tool.GripStyle = ToolStripGripStyle.Hidden;
        tool.Items.Add(new ToolStripButton("👥 Sinh viên", null, (_, _) => OpenChild<FormStudent>()) { ToolTipText = "Mở quản lý sinh viên" });
        tool.Items.Add(new ToolStripButton("📦 Sản phẩm", null, (_, _) => OpenChild<FormProduct>()) { ToolTipText = "Mở quản lý sản phẩm" });
        tool.Items.Add(new ToolStripSeparator());
        tool.Items.Add(new ToolStripButton("Cascade", null, (_, _) => LayoutMdi(MdiLayout.Cascade)));
        tool.Items.Add(new ToolStripButton("Đóng cửa sổ", null, (_, _) => ActiveMdiChild?.Close()));
    }

    private void BuildStatusStrip()
    {
        userLabel.Text = "Người dùng: Admin";
        messageLabel.Text = "Trạng thái: Khởi tạo";
        timeLabel.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        status.Items.Add(userLabel);
        status.Items.Add(new ToolStripStatusLabel(" | "));
        status.Items.Add(messageLabel);
        status.Items.Add(new ToolStripStatusLabel { Spring = true });
        status.Items.Add(timeLabel);
    }

    public void SetStatus(string message)
    {
        messageLabel.Text = "Trạng thái: " + message;
    }

    private void Logout()
    {
        var result = MessageBox.Show("Bạn muốn đăng xuất và quay lại màn hình đăng nhập mẫu?",
            "Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            foreach (Form child in MdiChildren.ToArray())
                child.Close();
            userLabel.Text = "Người dùng: Chưa đăng nhập";
            SetStatus("Đã đăng xuất (mô phỏng).");
            MessageBox.Show("Đây là chức năng mô phỏng đăng xuất trong bài thực hành.", "Thông báo");
        }
    }

    private void OpenChild<T>() where T : Form, new()
    {
        foreach (Form child in MdiChildren)
        {
            if (child is T)
            {
                child.Activate();
                child.WindowState = FormWindowState.Normal;
                SetStatus("Đã kích hoạt cửa sổ: " + child.Text);
                return;
            }
        }

        var form = new T
        {
            MdiParent = this,
            StartPosition = FormStartPosition.Manual
        };
        form.FormClosed += (_, _) => SetStatus("Đã đóng cửa sổ: " + form.Text);
        form.Show();
        SetStatus("Đã mở cửa sổ: " + form.Text);
    }
}
