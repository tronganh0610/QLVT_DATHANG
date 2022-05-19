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
using DevExpress.XtraReports.UI;
using Microsoft.Reporting.WinForms;
using QLVT.ReportForm;


namespace QLVT
{
    public partial class FormDanhSachNhanVien : Form
    {
        String maChiNhanh = "";
        public FormDanhSachNhanVien()
        {
            InitializeComponent();
        }

        private void FormDanhSachNhanVien_Load(object sender, EventArgs e)
        {
            reportViewer1.Visible = true;

            /*Step 1*/
            /*không kiểm tra khóa ngoại nữa*/
            DS_SV1.EnforceConstraints = false;

            // TODO: This line of code loads data into the 'DS_SV1.DatHang' table. You can move, or remove it, as needed.
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.DS_SV1.NhanVien);

            /*van con ton tai loi chua sua duoc*/
            maChiNhanh = ((DataRowView)bdsNhanVien[0])["MACN"].ToString();

            /*Step 2*/

            cmbChiNhanh.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.brand;

            /*Step 3*/
            /*CONG TY chi xem du lieu*/
            if (Program.role == "CongTy")
            {
                cmbChiNhanh.Enabled = true;

            }

            /* CHI NHANH & USER co the xem - xoa - sua du lieu nhung khong the 
             chuyen sang chi nhanh khac*/
            if (Program.role == "ChiNhanh" || Program.role == "User")
            {
                cmbChiNhanh.Enabled = false;          
            }
            this.reportViewer1.RefreshReport();
            
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            /*
            /*Neu combobox khong co so lieu thi ket thuc luon*/
            if (cmbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView")
                return;

            Program.serverName = cmbChiNhanh.SelectedValue.ToString();

            /*Neu chon sang chi nhanh khac voi chi nhanh hien tai*/
            if (cmbChiNhanh.SelectedIndex != Program.brand)
            {
                Program.loginName = Program.remoteLogin;
                Program.loginPassword = Program.remotePassword;
            }
            /*Neu chon trung voi chi nhanh dang dang nhap o formDangNhap*/
            else
            {
                Program.loginName = Program.currentLogin;
                Program.loginPassword = Program.currentPassword;
            }

            if (Program.KetNoi() == 0)
            {
                MessageBox.Show("Xảy ra lỗi kết nối với chi nhánh hiện tại", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                /*Do du lieu tu dataSet vao grid Control*/
                this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.nhanVienTableAdapter.Fill(this.DS_SV1.NhanVien);
                this.reportViewer1.RefreshReport();
            }
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
            Form f = this.CheckExists(typeof(DsNhanVien));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                DsNhanVien form = new DsNhanVien();
                /*GAN TEN CHI NHANH CHO BAO CAO*/
                //report.txtChiNhanh.Text = chiNhanh.ToUpper();
                ReportPrintTool printTool = new ReportPrintTool(form);
                printTool.ShowPreviewDialog();
            }
        }

        private void btnInDanhSach_Click(object sender, EventArgs e)
        {
            exportPDF();

        }
       
        private void exportPDF()
        {
            if (cmbChiNhanh.SelectedIndex == 0)
            {
                try
                {
                    DsNhanVien report = new DsNhanVien();
                    /*GAN TEN CHI NHANH CHO BAO CAO*/
                    //report.txtChiNhanh.Text = chiNhanh.ToUpper();
                    if (File.Exists(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\DanhSachNhanVienCN1.pdf"))
                    {
                        DialogResult dr = MessageBox.Show("File DanhSachNhanVienCN1.pdf tại thư mục ExportPDF đã có!\nBạn có muốn tạo lại?",
                            "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            report.ExportToPdf(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\DanhSachNhanVienCN1.pdf");
                            MessageBox.Show("File DanhSachNhanVienCN1 đã được ghi thành công tại thư mụcExportPDF",
                    "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        report.ExportToPdf(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\DanhSachNhanVienCN1.pdf");
                        MessageBox.Show("File DanhSachNhanVienCN1 đã được ghi thành công tại thư mục ExportPDF",
                    "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Vui lòng đóng file DanhSachNhanVienCN1.pdf",
                        "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    return;
                }
            }
            if (cmbChiNhanh.SelectedIndex == 1)
            {
                try
                {
                    DsNhanVien report = new DsNhanVien();
                    /*GAN TEN CHI NHANH CHO BAO CAO*/
                    //report.txtChiNhanh.Text = chiNhanh.ToUpper();
                    if (File.Exists(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\DanhSachNhanVienCN2.pdf"))
                    {
                        DialogResult dr = MessageBox.Show("File DanhSachNhanVienCN2.pdf tại thư phục đã có!\nBạn có muốn tạo lại?",
                            "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            report.ExportToPdf(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\DanhSachNhanVienCN2.pdf");
                            MessageBox.Show("File DanhSachNhanVienCN2 đã được ghi thành công tại thư mục ExportPDF",
                    "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        report.ExportToPdf(@"C:\Users\Admin\OneDrive\Desktop\Cơ sở dữ liệu phân tán\ExportPDF\DanhSachNhanVienCN2.pdf");
                        MessageBox.Show("File DanhSachNhanVienCN2 đã được ghi thành công tại thư mục ExportPDF",
                    "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Vui lòng đóng file DanhSachNhanVienCN2.pdf",
                        "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    return;
                }
            }

            /*ReportDataSource reportDataSource = new ReportDataSource();
            reportViewer1.LocalReport.ReportEmbeddedResource = "QLVT.Report.RpDSNhanVien.rdlc";
            reportDataSource.Name = "NhanVien";
            reportDataSource.Value = Program.ExecSqlDataTable("select * from NhanVien");
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            

            SaveFileDialog save = new SaveFileDialog();

            var render = reportViewer1.LocalReport.ListRenderingExtensions();
            if (reportViewer1.ExportDialog(render[3]) == DialogResult.OK)
            {
                MessageBox.Show("Đã xuất file PDF", "Thông báo", MessageBoxButtons.OK);
            }*/
        }
        
    }
}
