using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormChinh : Form
    {
        public FormChinh()
        {
            InitializeComponent();
        }
        /************************************************************
         * CheckExists:
         * Để tránh việc người dùng ấn vào 1 form đến 2 lần chúng ta 
         * cần sử dụng hàm này để kiểm tra xem cái form hiện tại đã 
         * có trong bộ nhớ chưa
         * Nếu có trả về "f"
         * Nếu không trả về "null"
         ************************************************************/
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }
        //Kích hoạt các tab chức năng và nút đăng xuất 
        public void enableButtons()
        {

            btnDangNhap.Enabled = false;
            btnDangXuat.Enabled = true;

            PageNhapXuat.Visible = true;
            PageBaoCao.Visible = true;
            btnTaoTK.Enabled = true;

            if (Program.role == "USER")
            {
                btnTaoTK.Enabled = false;
            }
        }

        //Dispose: Giải phóng các form khỏi bộ nhớ. Ví dụ form đăng nhập, nhân viên 
        // Close: đóng hoàn toàn chương trình lại
        private void logout()
        {
            foreach (Form f in this.MdiChildren)
                f.Dispose();
        }


        private void barButtonKhoHang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonNhanVien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Form f = this.CheckExists(typeof(FormNhanVien));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormNhanVien form = new FormNhanVien();
                form.MdiParent = this;
                form.Show();
            }
        }
        
        private void barButtonDangNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormDangNhap));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormDangNhap form = new FormDangNhap();
                //form.MdiParent = this;
                form.Show(xtraTabbedMdiManager1);
                
            }
            btnDangNhap.Enabled = false;
            btnDangXuat.Enabled = true;
            PageNhapXuat.Visible = true;
            PageBaoCao.Visible = true;
            
        }

        private void FormChinh_Load(object sender, EventArgs e)
        {

            btnDangNhap.Enabled = true;
            btnDangXuat.Enabled = false;
            btnTaoTK.Enabled = false;
            PageNhapXuat.Visible = false;
            PageBaoCao.Visible = false;

        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnDangXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            logout();

            btnDangNhap.Enabled = true;
            btnDangXuat.Enabled = false;
            btnTaoTK.Enabled = false;
            PageNhapXuat.Visible = false;
            PageBaoCao.Visible = false;
            Program.formChinh.MANHANVIEN.Text = "MÃNV:";
            Program.formChinh.HOTEN.Text = "HỌ TÊN:";
            Program.formChinh.NHOM.Text = "NHÓM:";
        }

        private void MANHANVIEN_Click(object sender, EventArgs e)
        {

        }
    }
}
