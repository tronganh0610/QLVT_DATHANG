using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraDiagram;
using DevExpress.XtraReports.Design;
using Microsoft.Reporting.WinForms;
using QLVT;
using System.Configuration;


namespace QLVT.ReportForm
{
    public partial class ReportDanhSachNhanVien : Form
    {
        
        public ReportDanhSachNhanVien()
        {
            InitializeComponent();
            
        }

        
        public void ReportDanhSachNhanVien_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReportEmbeddedResource = "QLVT.Report.RpDSNhanVien.rdlc";
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "NhanVien";
            reportDataSource.Value = Program.ExecSqlDataTable("select * from NhanVien");
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
            
        }
        
        private void reportViewer1_Load(object sender, EventArgs e)
        {
            
        }
        public void exportPDF()
        {
            ReportDataSource reportDataSource = new ReportDataSource();
            reportViewer1.LocalReport.ReportEmbeddedResource = "QLVT.Report.RpDSNhanVien.rdlc";
            reportDataSource.Name = "NhanVien";
            reportDataSource.Value = Program.ExecSqlDataTable("select * from NhanVien");
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);


            SaveFileDialog save = new SaveFileDialog();

            var render = reportViewer1.LocalReport.ListRenderingExtensions();
            if (reportViewer1.ExportDialog(render[3]) == DialogResult.OK)
            {
                MessageBox.Show("Đã xuất file PDF", "Thông báo", MessageBoxButtons.OK);
            }
        }
    }
}
