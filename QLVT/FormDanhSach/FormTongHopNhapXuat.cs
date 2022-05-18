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
    public partial class FormTongHopNhapXuat : Form
    {
        public FormTongHopNhapXuat()
        {
            InitializeComponent();
        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void FormTongHopNhapXuat_Load(object sender, EventArgs e)
        {
            cmbChiNhanh.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.brand;
            cmbChiNhanh.Enabled = false;
            this.dateEditNgayBatDau.EditValue = "05-01-2022";
            this.dateEditNgayKetThuc.EditValue = "06-01-2022";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            DateTime ngayBatDau = dateEditNgayBatDau.DateTime;
            DateTime ngayKetThuc = dateEditNgayKetThuc.DateTime;
            TongHopNhapXuat report = new TongHopNhapXuat(ngayBatDau, ngayKetThuc);
            /*GAN TEN CHI NHANH CHO BAO CAO*/

            
            report.txtNgayBatDau.Text = ngayBatDau.ToString("dd / MM / yyyy");
            report.txtNgayKetThuc.Text = ngayKetThuc.ToString("dd / MM / yyyy");
            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ngayBatDau = dateEditNgayBatDau.DateTime;
                DateTime ngayKetThuc = dateEditNgayKetThuc.DateTime;

                TongHopNhapXuat report = new TongHopNhapXuat(ngayBatDau, ngayKetThuc);

                /*GAN TEN CHI NHANH CHO BAO CAO*/
                
                report.txtNgayBatDau.Text = ngayBatDau.ToString("MM / dd / yyyy");
                report.txtNgayKetThuc.Text = ngayKetThuc.ToString("MM / dd / yyyy");

                if (File.Exists(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\TongHopNhapXuat.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File TongHopNhapXuat.pdf tại thư mục ExportPDF đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\TongHopNhapXuat.pdf");
                        MessageBox.Show("File TongHopNhapXuat.pdf đã được ghi thành công tại thư mục ExportPDF",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    report.ExportToPdf(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\TongHopNhapXuat.pdf");
                    MessageBox.Show("File TongHopNhapXuat.pdf đã được ghi thành công tại thư mục ExportPDF",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file TongHopNhapXuat.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
