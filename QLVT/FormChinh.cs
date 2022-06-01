
using QLVT.FormDanhSach;
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
            PageTongHop.Visible = true;
            btnTaoTK.Enabled = true;

            if (Program.role == "User")
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
            Form f = this.CheckExists(typeof(FormKho));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormKho form = new FormKho();
                form.MdiParent = this;
                form.Show();
            }
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
            btnDangXuat.Enabled = false;
            PageNhapXuat.Visible = false;
            PageTongHop.Visible = false;
            
        }

        private void FormChinh_Load(object sender, EventArgs e)
        {

            btnDangNhap.Enabled = true;
            btnDangXuat.Enabled = false;
            btnTaoTK.Enabled = false;
            PageNhapXuat.Visible = false;
            PageTongHop.Visible = false;

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
            PageTongHop.Visible = false;
            Program.formChinh.MANHANVIEN.Text = "MÃNV:";
            Program.formChinh.HOTEN.Text = "HỌ TÊN:";
            Program.formChinh.NHOM.Text = "NHÓM:";
        }

        private void MANHANVIEN_Click(object sender, EventArgs e)
        {

        }

        private void barButtonVatTu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormVatTu));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormVatTu form = new FormVatTu();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            Form f = this.CheckExists(typeof(FormDonDatHang));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormDonDatHang form = new FormDonDatHang();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormPhieuNhap));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormPhieuNhap form = new FormPhieuNhap();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormPhieuXuat));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormPhieuXuat form = new FormPhieuXuat();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void btnDSNhanVien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormDanhSachNhanVien));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormDanhSachNhanVien form = new FormDanhSachNhanVien();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void btnDSVatTu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormDanhSachVatTu));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormDanhSachVatTu form = new FormDanhSachVatTu();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormChiTietSoLuongTriGiaHangHoaNhapXuat));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormChiTietSoLuongTriGiaHangHoaNhapXuat form = new FormChiTietSoLuongTriGiaHangHoaNhapXuat();
                form.MdiParent = this;
                form.Show();
            }
        }

        

        private void btnTaoTK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormTaoTaiKhoan));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormTaoTaiKhoan form = new FormTaoTaiKhoan();
                //form.MdiParent = this;
                form.Show();

            }
        }

        private void btnHoatDongNV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormDanhSachHoatDongNhanVien));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormDanhSachHoatDongNhanVien form = new FormDanhSachHoatDongNhanVien();
                form.MdiParent = this;
                form.Show();
            }
        }

        private void btnTongHopNhapXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormTongHopNhapXuat));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormTongHopNhapXuat form = new FormTongHopNhapXuat();
                form.MdiParent = this;
                form.Show();
            }
        }
    }
}
