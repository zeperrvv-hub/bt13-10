namespace MDIQuanLy;

public class FormProduct : Form
{
    public FormProduct()
    {
        Text = "Quản lý Sản phẩm";
        Size = new Size(650, 400);
        MinimumSize = new Size(500, 300);

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(25), ColumnCount = 1, RowCount = 3 };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(layout);

        layout.Controls.Add(new Label
        {
            Text = "QUẢN LÝ SẢN PHẨM",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.DarkBlue,
            AutoSize = true,
            Padding = new Padding(0, 0, 0, 15)
        }, 0, 0);
        layout.Controls.Add(new Label
        {
            Text = "Đây là Form con minh họa cho chức năng Quản lý Sản phẩm.\n"
                 + "Bạn có thể phát triển thêm các trường Mã SP, Tên SP, Đơn giá, Số lượng và bảng DataGridView.",
            AutoSize = true,
            Dock = DockStyle.Top
        }, 0, 1);
        var btn = new Button { Text = "Thông báo về Form cha", AutoSize = true };
        btn.Click += (_, _) =>
        {
            if (MdiParent is FormMain main)
                main.SetStatus("Form Sản phẩm đã gửi thông điệp lên FormMain.");
            MessageBox.Show("Đã gửi thông điệp lên thanh trạng thái FormMain.");
        };
        layout.Controls.Add(btn, 0, 2);
    }
}
