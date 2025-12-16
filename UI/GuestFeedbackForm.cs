using System;
using System.Windows.Forms;
using QLThuocApp.Controllers;

namespace QLThuocApp.UI
{
    public class GuestFeedbackForm : Form
    {
        private TextBox txtHD, txtSDT, txtNoiDung;
        private NumericUpDown nudStar;
        private Button btnSend;
        private PhanHoiController controller = new PhanHoiController();

        public GuestFeedbackForm()
        {
            Text = "Gửi Phản Hồi"; Size = new System.Drawing.Size(300, 250);
            StartPosition = FormStartPosition.CenterParent;

            Controls.Add(new Label { Text = "Mã Hóa Đơn:", Location = new System.Drawing.Point(20, 20) });
            txtHD = new TextBox { Location = new System.Drawing.Point(100, 18) };
            Controls.Add(txtHD);

            Controls.Add(new Label { Text = "SĐT:", Location = new System.Drawing.Point(20, 50) });
            txtSDT = new TextBox { Location = new System.Drawing.Point(100, 48) };
            Controls.Add(txtSDT);

            Controls.Add(new Label { Text = "Nội dung:", Location = new System.Drawing.Point(20, 80) });
            txtNoiDung = new TextBox { Location = new System.Drawing.Point(100, 78) };
            Controls.Add(txtNoiDung);

            Controls.Add(new Label { Text = "Đánh giá:", Location = new System.Drawing.Point(20, 110) });
            nudStar = new NumericUpDown { Location = new System.Drawing.Point(100, 108), Maximum = 5, Minimum = 1 };
            Controls.Add(nudStar);

            btnSend = new Button { Text = "Gửi", Location = new System.Drawing.Point(100, 150) };
            btnSend.Click += (s, e) => {
                if (controller.AddPhanHoiGuest(txtHD.Text, txtSDT.Text, txtNoiDung.Text, (int)nudStar.Value, out string msg))
                {
                    MessageBox.Show("Cảm ơn đóng góp của bạn!");
                    this.Close();
                }
                else MessageBox.Show(msg);
            };
            Controls.Add(btnSend);
        }
    }
}