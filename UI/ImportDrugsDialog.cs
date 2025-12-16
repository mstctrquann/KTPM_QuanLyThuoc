using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QLThuocApp.Controllers;
using QLThuocApp.Entities;

namespace QLThuocApp.UI
{
    public class ImportDrugsDialog : Form
    {
        private TextBox txtFilePath;
        private Button btnBrowse, btnImport, btnCancel;
        private DataGridView dgvPreview;
        private ProgressBar progressBar;
        private Label lblStatus;
        private ThuocController controller = new ThuocController();
        private List<Thuoc> drugs = new List<Thuoc>();

        public ImportDrugsDialog()
        {
            Text = "Nhập hàng loạt thuốc từ file";
            Size = new System.Drawing.Size(900, 600);
            StartPosition = FormStartPosition.CenterParent;

            // Panel top
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = System.Drawing.Color.FromArgb(245, 245, 245), Padding = new Padding(10) };
            
            var lblFile = new Label 
            { 
                Text = "File CSV/TXT:", 
                Location = new System.Drawing.Point(10, 15), 
                AutoSize = true, 
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold) 
            };
            pnlTop.Controls.Add(lblFile);
            
            txtFilePath = new TextBox 
            { 
                Location = new System.Drawing.Point(10, 40), 
                Size = new System.Drawing.Size(650, 25), 
                ReadOnly = true,
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            pnlTop.Controls.Add(txtFilePath);
            
            btnBrowse = new Button 
            { 
                Text = "📁 Chọn file", 
                Location = new System.Drawing.Point(670, 38), 
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.FromArgb(52, 152, 219),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnBrowse.FlatAppearance.BorderSize = 0;
            btnBrowse.Click += BtnBrowse_Click;
            pnlTop.Controls.Add(btnBrowse);
            
            // Help text
            var lblHelp = new Label 
            { 
                Text = "Format: MaThuoc,TenThuoc,DonViTinh,GiaNhap,GiaBan,SoLuong,XuatXu,HanSuDung(dd/MM/yyyy)",
                Location = new System.Drawing.Point(10, 75),
                Size = new System.Drawing.Size(860, 20),
                Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic),
                ForeColor = System.Drawing.Color.Gray
            };
            pnlTop.Controls.Add(lblHelp);

            Controls.Add(pnlTop);

            // Preview grid
            dgvPreview = new DataGridView 
            { 
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvPreview.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvPreview.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvPreview.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvPreview.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPreview.EnableHeadersVisualStyles = false;
            dgvPreview.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

            Controls.Add(dgvPreview);

            // Bottom panel
            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 90, BackColor = System.Drawing.Color.FromArgb(245, 245, 245), Padding = new Padding(10) };
            
            lblStatus = new Label 
            { 
                Text = "Chưa chọn file", 
                Location = new System.Drawing.Point(10, 15), 
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Gray
            };
            pnlBottom.Controls.Add(lblStatus);
            
            progressBar = new ProgressBar 
            { 
                Location = new System.Drawing.Point(10, 40), 
                Size = new System.Drawing.Size(760, 25),
                Visible = false
            };
            pnlBottom.Controls.Add(progressBar);
            
            btnImport = new Button 
            { 
                Text = "✓ NHẬP VÀO HỆ THỐNG", 
                Location = new System.Drawing.Point(580, 10), 
                Size = new System.Drawing.Size(180, 50),
                BackColor = System.Drawing.Color.FromArgb(46, 204, 113),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnImport.FlatAppearance.BorderSize = 0;
            btnImport.Click += BtnImport_Click;
            pnlBottom.Controls.Add(btnImport);
            
            btnCancel = new Button 
            { 
                Text = "Đóng", 
                Location = new System.Drawing.Point(770, 10), 
                Size = new System.Drawing.Size(100, 50),
                BackColor = System.Drawing.Color.FromArgb(149, 165, 166),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            pnlBottom.Controls.Add(btnCancel);

            Controls.Add(pnlBottom);
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                ofd.Title = "Chọn file nhập thuốc";
                
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = ofd.FileName;
                    try
                    {
                        ParseFile(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi đọc file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lblStatus.Text = "Lỗi đọc file";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        private void ParseFile(string filePath)
        {
            drugs.Clear();
            var lines = File.ReadAllLines(filePath);
            
            if (lines.Length == 0)
            {
                throw new Exception("File rỗng");
            }

            int successCount = 0;
            int errorCount = 0;
            List<string> errors = new List<string>();

            // Skip header line
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {
                    var parts = line.Split(',');
                    if (parts.Length < 8)
                    {
                        errors.Add($"Dòng {i + 1}: Thiếu thông tin");
                        errorCount++;
                        continue;
                    }

                    var thuoc = new Thuoc
                    {
                        IdThuoc = parts[0].Trim(),
                        TenThuoc = parts[1].Trim(),
                        DonViTinh = parts[2].Trim(),
                        GiaNhap = double.Parse(parts[3].Trim()),
                        DonGia = double.Parse(parts[4].Trim()),
                        SoLuongTon = int.Parse(parts[5].Trim()),
                        XuatXu = parts[6].Trim(),
                        HanSuDung = DateTime.ParseExact(parts[7].Trim(), "dd/MM/yyyy", null)
                    };
                    
                    drugs.Add(thuoc);
                    successCount++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Dòng {i + 1}: {ex.Message}");
                    errorCount++;
                }
            }

            // Show preview
            dgvPreview.DataSource = null;
            dgvPreview.DataSource = drugs;
            FormatGridColumns();

            lblStatus.Text = $"Đọc thành công {successCount} thuốc, {errorCount} lỗi";
            lblStatus.ForeColor = successCount > 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            
            if (errors.Count > 0 && errors.Count <= 10)
            {
                lblStatus.Text += $"\n{string.Join(", ", errors.Take(10))}";
            }

            btnImport.Enabled = drugs.Count > 0;
        }

        private void FormatGridColumns()
        {
            if (dgvPreview.Columns.Count == 0) return;

            dgvPreview.Columns["IdThuoc"].HeaderText = "Mã Thuốc";
            dgvPreview.Columns["TenThuoc"].HeaderText = "Tên Thuốc";
            dgvPreview.Columns["DonViTinh"].HeaderText = "ĐVT";
            dgvPreview.Columns["GiaNhap"].HeaderText = "Giá Nhập";
            dgvPreview.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
            dgvPreview.Columns["DonGia"].HeaderText = "Giá Bán";
            dgvPreview.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvPreview.Columns["SoLuongTon"].HeaderText = "Số Lượng";
            dgvPreview.Columns["XuatXu"].HeaderText = "Xuất Xứ";
            dgvPreview.Columns["HanSuDung"].HeaderText = "Hạn SD";
            dgvPreview.Columns["HanSuDung"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (drugs.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc muốn nhập {drugs.Count} thuốc vào hệ thống?", 
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result != DialogResult.Yes) return;

            progressBar.Visible = true;
            progressBar.Maximum = drugs.Count;
            progressBar.Value = 0;
            btnImport.Enabled = false;
            btnBrowse.Enabled = false;

            int successCount = 0;
            int errorCount = 0;

            foreach (var thuoc in drugs)
            {
                try
                {
                    if (controller.Add(thuoc, out string msg))
                    {
                        successCount++;
                    }
                    else
                    {
                        errorCount++;
                    }
                }
                catch
                {
                    errorCount++;
                }
                
                progressBar.Value++;
                lblStatus.Text = $"Đang nhập... {progressBar.Value}/{drugs.Count}";
                Application.DoEvents();
            }

            progressBar.Visible = false;
            btnImport.Enabled = true;
            btnBrowse.Enabled = true;

            MessageBox.Show($"Hoàn tất!\n\nThành công: {successCount}\nThất bại: {errorCount}", 
                "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            if (successCount > 0)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
