using DevExpress.XtraReports.UI;
using QLVT.inBaoCao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.FormDanhSach
{
    public partial class FormDanhSachHoatDongNhanVien : Form
    {
        public FormDanhSachHoatDongNhanVien()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void nhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsNhanVien.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS_SV1);

        }

        private void FormDanhSachHoatDongNhanVien_Load(object sender, EventArgs e)
        {
            DS_SV1.EnforceConstraints = false;
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.DS_SV1.NhanVien);


            cmbChiNhanh.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.brand;
            cmbChiNhanh.Enabled = false;
            /*cmbNhanVien.DataSource = bdsNhanVien;*//*sao chep bingding source tu form dang nhap*//*
            cmbNhanVien.DisplayMember = "MANV";
            cmbNhanVien.ValueMember = "TEN";
            cmbNhanVien.SelectedIndex = Program.brand;*/
            txtMaNV.Text = Program.maNhanVienDuocChon;
            txtHoTen.Text = Program.hoTen;
            dateEditNgayBatDau.EditValue = "05-01-2022";
            dateEditNgayKetThuc.EditValue = "06-01-2022";
            cmbLoaiPhieu.SelectedIndex = 0;

        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime ngayBatDau = dateEditNgayBatDau.DateTime;
            DateTime denNgay = dateEditNgayKetThuc.DateTime;

            DanhSachHoatDongNhanVien ds = new DanhSachHoatDongNhanVien(txtMaNV.Text, cmbLoaiPhieu.SelectedItem.ToString(), ngayBatDau, denNgay );

            ds.txtMANV.Text = this.txtMaNV.Text;
            ds.txtMACN.Text = this.cmbChiNhanh.Text;
            ds.txtTENNV.Text = this.txtHoTen.Text;
            ds.txtLOAIPHIEU.Text =this.cmbLoaiPhieu.SelectedItem.ToString();
            ds.txtNGAY1.Text = ngayBatDau.ToString("dd / MM / yyyy");
            ds.txtNGAY2.Text = denNgay.ToString("dd / MM / yyyy");

            ReportPrintTool printTool = new ReportPrintTool(ds);
            printTool.ShowPreviewDialog();
        }
    }
}
