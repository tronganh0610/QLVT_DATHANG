﻿using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT.inBaoCao
{
    public partial class ChiTietSoLuongTriGiaHangHoaNhapXuat : DevExpress.XtraReports.UI.XtraReport
    {
        public ChiTietSoLuongTriGiaHangHoaNhapXuat(String vaiTro, String loaiPhieu, DateTime fromDate, DateTime toDate)
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
            this.sqlDataSource1.Queries[0].Parameters[0].Value = vaiTro;
            this.sqlDataSource1.Queries[0].Parameters[1].Value = loaiPhieu;
            this.sqlDataSource1.Queries[0].Parameters[2].Value = fromDate;
            this.sqlDataSource1.Queries[0].Parameters[3].Value = toDate;

            this.sqlDataSource1.Fill();
        }

        private void pageInfo2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void pageInfo1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
