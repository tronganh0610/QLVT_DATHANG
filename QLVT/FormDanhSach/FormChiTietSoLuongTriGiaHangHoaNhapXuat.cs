using DevExpress.XtraReports.UI;
using QLVT.inBaoCao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.FormDanhSach
{
    public partial class FormChiTietSoLuongTriGiaHangHoaNhapXuat : Form
    {
        public FormChiTietSoLuongTriGiaHangHoaNhapXuat()
        {
            InitializeComponent();
        }

        private void FormChiTietSoLuongTriGiaHangHoaNhapXuat_Load(object sender, EventArgs e)
        {
            this.cmbLoaiPhieu.SelectedIndex = 0;
            this.dateEditNgayBatDau.EditValue = "05-01-2022";
            this.dateEditNgayKetThuc.EditValue = "06-01-2022";
        }

        private void dateEditNgayBatDau_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string vaiTro = Program.role;
            string loaiPhieu = (cmbLoaiPhieu.SelectedItem.ToString() == "NHAP") ? "NHAP" : "XUAT";

            DateTime fromDate = dateEditNgayBatDau.DateTime;
            DateTime toDate = dateEditNgayKetThuc.DateTime;
            ChiTietSoLuongTriGiaHangHoaNhapXuat report = new ChiTietSoLuongTriGiaHangHoaNhapXuat(vaiTro, loaiPhieu, fromDate, toDate );
            /*GAN TEN CHI NHANH CHO BAO CAO*/
            
            report.txtLoaiPhieu.Text = cmbLoaiPhieu.SelectedItem.ToString().ToUpper();
            report.txtNgayBatDau.Text = fromDate.ToString("dd / MM / yyyy");
            report.txtNgayKetThuc.Text = toDate.ToString("dd / MM / yyyy");
            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string vaiTro = Program.role;
            string loaiPhieu = (cmbLoaiPhieu.SelectedItem.ToString() == "NHAP") ? "NHAP" : "XUAT";

            try
            {
                DateTime fromDate = dateEditNgayBatDau.DateTime;
                DateTime toDate = dateEditNgayKetThuc.DateTime;

                ChiTietSoLuongTriGiaHangHoaNhapXuat report = new ChiTietSoLuongTriGiaHangHoaNhapXuat(vaiTro, loaiPhieu, fromDate, toDate);

                /*GAN TEN CHI NHANH CHO BAO CAO*/
                report.txtLoaiPhieu.Text = cmbLoaiPhieu.SelectedItem.ToString().ToUpper();
                report.txtNgayBatDau.Text = fromDate.ToString("MM / dd / yyyy");
                report.txtNgayKetThuc.Text = toDate.ToString("MM / dd / yyyy");

                if (File.Exists(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\ChiTietSoLuongGiaTriHangHoaNhapXuat.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File ChiTietSoLuongGiaTriHangHoaNhapXuat.pdf tại ổ D đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\ChiTietSoLuongGiaTriHangHoaNhapXuat.pdf");
                        MessageBox.Show("File ChiTietSoLuongGiaTriHangHoaNhapXuat.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    report.ExportToPdf(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\ChiTietSoLuongGiaTriHangHoaNhapXuat.pdf");
                    MessageBox.Show("File ChiTietSoLuongGiaTriHangHoaNhapXuat.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file ReportChiTietSoLuongTriGiaHangHoaNhapXuat.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }

        private void cmbLoaiPhieu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
