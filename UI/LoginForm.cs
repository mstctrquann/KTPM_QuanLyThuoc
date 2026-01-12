using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class LoginForm : Form
    {
        private TextBox txtUser, txtPass;
        private Button btnLogin, btnExit, btnGuest, btnRegister;
        private LoginController controller = new LoginController();

        // Biến static để lưu phiên đăng nhập toàn cục
        public static TaiKhoan? CurrentUser;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Đăng nhập hệ thống";
            this.ClientSize = new Size(400, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.AutoScaleMode = AutoScaleMode.Font;

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "QUẢN LÝ NHÀ THUỐC";
            lblTitle.Font = new Font("Arial", 16F, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(80, 20);
            lblTitle.ForeColor = Color.Blue;
            this.Controls.Add(lblTitle);

            // Username Label
            Label lblUser = new Label();
            lblUser.Text = "Tài khoản:";
            lblUser.Location = new Point(30, 80);
            lblUser.AutoSize = true;
            this.Controls.Add(lblUser);

            // Username TextBox
            txtUser = new TextBox();
            txtUser.Location = new Point(120, 78);
            txtUser.Size = new Size(200, 20);
            txtUser.TabIndex = 0;
            this.Controls.Add(txtUser);

            // Password Label
            Label lblPass = new Label();
            lblPass.Text = "Mật khẩu:";
            lblPass.Location = new Point(30, 120);
            lblPass.AutoSize = true;
            this.Controls.Add(lblPass);

            // Password TextBox
            txtPass = new TextBox();
            txtPass.Location = new Point(120, 118);
            txtPass.Size = new Size(200, 20);
            txtPass.UseSystemPasswordChar = true;
            txtPass.TabIndex = 1;
            this.Controls.Add(txtPass);

            // Login Button
            btnLogin = new Button();
            btnLogin.Text = "Đăng nhập";
            btnLogin.Location = new Point(120, 160);
            btnLogin.Size = new Size(90, 30);
            btnLogin.BackColor = Color.LightGreen;
            btnLogin.TabIndex = 2;
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            // Exit Button
            btnExit = new Button();
            btnExit.Text = "Thoát";
            btnExit.Location = new Point(230, 160);
            btnExit.Size = new Size(90, 30);
            btnExit.TabIndex = 3;
            btnExit.Click += (s, e) => Application.Exit();
            this.Controls.Add(btnExit);

            // Guest Button
            btnGuest = new Button();
            btnGuest.Text = "Khách hàng phản hồi";
            btnGuest.Location = new Point(120, 200);
            btnGuest.Size = new Size(200, 30);
            btnGuest.TabIndex = 4;
            btnGuest.Click += BtnGuest_Click;
            this.Controls.Add(btnGuest);

            // Register Employee Button
            btnRegister = new Button();
            btnRegister.Text = "📝 Đăng ký nhân viên";
            btnRegister.Location = new Point(120, 240);
            btnRegister.Size = new Size(200, 30);
            btnRegister.BackColor = Color.FromArgb(155, 89, 182);
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRegister.TabIndex = 5;
            btnRegister.Click += BtnRegister_Click;
            btnRegister.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnRegister);

            this.AcceptButton = btnLogin; 
        }

        private void BtnGuest_Click(object? sender, EventArgs e)
        {
            // Tạo form để hiển thị QR code
            Form qrForm = new Form
            {
                Text = "Phản hồi của khách hàng",
                ClientSize = new Size(500, 600),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            // Tiêu đề
            Label lblTitle = new Label
            {
                Text = "Quét mã QR để gửi phản hồi",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(100, 20),
                ForeColor = Color.FromArgb(52, 152, 219)
            };
            qrForm.Controls.Add(lblTitle);

            // PictureBox cho QR code
            PictureBox picQR = new PictureBox
            {
                Size = new Size(400, 400),
                Location = new Point(50, 70),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Load ảnh QR từ Resources
            string qrPath = @"C:\Users\PC\Downloads\KTPM-master\Resources\qr_feedback.jpg";
            
            
            if (!File.Exists(qrPath))
            {
                qrPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "qr_feedback.jpg");
            }

            if (File.Exists(qrPath))
            {
                picQR.Image = Image.FromFile(qrPath);
            }
            else
            {
                // Nếu không tìm thấy file, tạo label thông báo
                Label lblError = new Label
                {
                    Text = "Vui lòng đặt file 'qr_feedback.jpg' vào thư mục Resources",
                    AutoSize = false,
                    Size = new Size(380, 60),
                    Location = new Point(60, 220),
                    Font = new Font("Segoe UI", 11F),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Red
                };
                picQR.Controls.Add(lblError);
            }
            qrForm.Controls.Add(picQR);

            // Nút đóng
            Button btnClose = new Button
            {
                Text = "Đóng",
                Size = new Size(100, 35),
                Location = new Point(200, 490),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, ev) => qrForm.Close();
            qrForm.Controls.Add(btnClose);

            qrForm.ShowDialog();
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            using (var dialog = new RegisterEmployeeDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("✓ Đăng ký nhân viên thành công!\n\nBạn có thể đăng nhập bằng tài khoản vừa tạo.", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            try
            {
                var tk = controller.Login(txtUser.Text.Trim(), txtPass.Text.Trim());
                if (tk != null)
                {
                    CurrentUser = tk; // Lưu session
                    this.Hide();
                    var mainForm = new MainForm();
                    mainForm.ShowDialog();
                    // Sau khi MainForm đóng, hiện lại LoginForm và xóa mật khẩu
                    txtPass.Text = "";
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show($"Lỗi database:\n{ex.Message}\n\nMã lỗi: {ex.Number}\n\nChi tiết:\n{ex.StackTrace}", 
                    "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống:\n{ex.Message}\n\nChi tiết:\n{ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}