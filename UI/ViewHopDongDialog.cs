using System.Windows.Forms;

namespace QLThuocApp.UI
{
    public partial class ViewHopDongDialog : Form
    {
        public ViewHopDongDialog(object hopDongEntity)
        {
            Text = "Chi tiết Hợp Đồng";
            Size = new System.Drawing.Size(450, 500);
            StartPosition = FormStartPosition.CenterParent;

            // PropertyGrid là control có sẵn của .NET rất tiện để xem object
            var pg = new PropertyGrid { Dock = DockStyle.Fill, SelectedObject = hopDongEntity, ToolbarVisible = false, HelpVisible = false };
            Controls.Add(pg);
        }
    }
}