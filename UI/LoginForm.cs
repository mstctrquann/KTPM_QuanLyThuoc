using System;
using System.Drawing;
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
            btnGuest.Click += (s, e) => { new GuestFeedbackForm().ShowDialog(); };
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

            this.AcceptButton = btnLogin; // Enter key triggers login
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
            var tk = controller.Login(txtUser.Text.Trim(), txtPass.Text.Trim());
            if (tk != null)
            {
                CurrentUser = tk; // Lưu session
                this.Hide();
                new MainForm().ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}