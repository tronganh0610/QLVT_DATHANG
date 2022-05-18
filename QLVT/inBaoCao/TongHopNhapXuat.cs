using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT.inBaoCao
{
    public partial class TongHopNhapXuat : DevExpress.XtraReports.UI.XtraReport
    {
        public TongHopNhapXuat(DateTime ngayBatDau, DateTime denNgay)
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
            this.sqlDataSource1.Queries[0].Parameters[0].Value = ngayBatDau;
            this.sqlDataSource1.Queries[0].Parameters[1].Value = denNgay;
            

            this.sqlDataSource1.Fill();
        }

    }
}
