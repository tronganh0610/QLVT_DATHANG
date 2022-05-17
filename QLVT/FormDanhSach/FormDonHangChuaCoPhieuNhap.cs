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
    public partial class FormDonHangChuaCoPhieuNhap : Form
    {
        public FormDonHangChuaCoPhieuNhap()
        {
            InitializeComponent();
        }

        private void btnXemTruoc_Click(object sender, EventArgs e)
        {
            
            string chiNhanh = (cmbChiNhanh.SelectedItem.ToString() == "CN1") ? "CN1" : "CN2" ;

            DonDatHangChuaCoPhieuNhap report = new DonDatHangChuaCoPhieuNhap();
            /*GAN TEN CHI NHANH CHO BAO CAO*/
            report.txtchiNhanh.Text = chiNhanh.ToUpper();
            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
        }

        private void btnInDanhSach_Click(object sender, EventArgs e)
        {
            try
            {
                
                string chiNhanh = (cmbChiNhanh.SelectedItem.ToString() == "CN1") ? "CN1" : "CN2";

                DonDatHangChuaCoPhieuNhap report = new DonDatHangChuaCoPhieuNhap();
                /*GAN TEN CHI NHANH CHO BAO CAO*/
                report.txtchiNhanh.Text = chiNhanh.ToUpper();
                if (File.Exists(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\DonDatHangChuaCoPhieuNhap.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File DonDatHangChuaCoPhieuNhap.pdf tại ổ D đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\DonDatHangChuaCoPhieuNhap.pdf");
                        MessageBox.Show("File DonDatHangChuaCoPhieuNhap.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    report.ExportToPdf(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\DonDatHangChuaCoPhieuNhap.pdf");
                    MessageBox.Show("File DonDatHangChuaCoPhieuNhap.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file ReportDonHangKhongPhieuNhap.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }

        private void FormDonHangChuaCoPhieuNhap_Load(object sender, EventArgs e)
        {
            cmbChiNhanh.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.brand;
            cmbChiNhanh.Enabled = false;
                
 
        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
