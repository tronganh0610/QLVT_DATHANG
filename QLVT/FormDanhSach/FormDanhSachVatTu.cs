using DevExpress.XtraReports.UI;
using Microsoft.Reporting.WinForms;
using QLVT.inBaoCao;
using QLVT.ReportForm;
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
    public partial class FormDanhSachVatTu : Form
    {
        public FormDanhSachVatTu()
        {
            InitializeComponent();
        }

        private void FormDanhSachVatTu_Load(object sender, EventArgs e)
        {
            DS_SV1.EnforceConstraints = false;


            this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vattuTableAdapter.Fill(this.DS_SV1.Vattu);

            this.reportViewer1.RefreshReport();
        }

        private void vattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsVatTu.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS_SV1);

        }
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        private void btnXemTruoc_Click(object sender, EventArgs e)
        {
            Form f = this.CheckExists(typeof(DsVatTu));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                DsVatTu form = new DsVatTu();
                ReportPrintTool printTool = new ReportPrintTool(form);
                printTool.ShowPreviewDialog();
            }
        }

        private void btnInDanhSach_Click(object sender, EventArgs e)
        {

            try
            {
                DsVatTu report = new DsVatTu();
                /*GAN TEN CHI NHANH CHO BAO CAO*/
                //report.txtChiNhanh.Text = chiNhanh.ToUpper();
                if (File.Exists(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\DanhSachVatTu.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File DanhSachVatTu.pdf tại thư mục ExportPDF đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\DanhSachVatTu.pdf");
                        MessageBox.Show("File DanhSachVatTu đã được ghi thành công tại thư mục ExportPDF",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    report.ExportToPdf(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\DanhSachVatTu.pdf");
                    MessageBox.Show("File DanhSachVatTu đã được ghi thành công tại thư mục ExportPDF",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file DanhSachVatTu.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }


    }
}

