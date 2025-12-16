using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace QLThuocApp.ultis
{
    public static class ExportHelper
    {
        public static void ToCSV(DataGridView dgv, string filename)
        {
            try
            {
                // Chọn đường dẫn lưu file
                var sfd = new SaveFileDialog
                {
                    Filter = "CSV (*.csv)|*.csv",
                    FileName = filename + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var sb = new StringBuilder();

                    // Header
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        sb.Append(dgv.Columns[i].HeaderText + ",");
                    }
                    sb.AppendLine();

                    // Rows
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            for (int i = 0; i < dgv.Columns.Count; i++)
                            {
                                // Xử lý dấu phẩy trong dữ liệu để tránh vỡ format CSV
                                string cellValue = row.Cells[i].Value?.ToString().Replace(",", " ") ?? "";
                                sb.Append(cellValue + ",");
                            }
                            sb.AppendLine();
                        }
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất file thành công: " + sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file: " + ex.Message);
            }
        }
    }
}