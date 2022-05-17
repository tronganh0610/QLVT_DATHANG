using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.ReportForm
{
    public partial class ReportDanhSachVatTu : Form
    {
        public ReportDanhSachVatTu()
        {
            InitializeComponent();
        }

        private void ReportDanhSachVatTu_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReportEmbeddedResource = "QLVT.Report.RpVatTu.rdlc";
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "VatTu";
            reportDataSource.Value = Program.ExecSqlDataTable("select * from Vattu");
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }
    }
}
