﻿using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT.inBaoCao
{
    public partial class DonDatHangChuaCoPhieuNhap : DevExpress.XtraReports.UI.XtraReport
    {
        public DonDatHangChuaCoPhieuNhap()
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
           

            this.sqlDataSource1.Fill();
        }

    }
}
