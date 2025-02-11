﻿using DevExpress.XtraGrid;
using QLVT.FormChon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormPhieuNhap : Form
    {
        /* vị trí của con trỏ trên grid view*/
        int viTri = 0;
       
        bool dangThemMoi = false;
        public string makho = "";
        string maChiNhanh = "";
       
        Stack undoList = new Stack();

        BindingSource bds = null;
        GridControl gc = null;
        string type = "";

        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }
        public FormPhieuNhap()
        {
            InitializeComponent();
        }

        private void phieuNhapBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsPhieuNhap.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS_SV1);

        }

        private void FormPhieuNhap_Load(object sender, EventArgs e)
        {
            /*Step 1*/
            /*không kiểm tra khóa ngoại nữa*/
            DS_SV1.EnforceConstraints = false;

            this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPNTableAdapter.Fill(this.DS_SV1.CTPN);

            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.DS_SV1.PhieuNhap);

            /*Step 2*/
            cmbCHINHANH.DataSource = Program.bindingSource;/*sao chep bingding source tu form dang nhap*/
            cmbCHINHANH.DisplayMember = "TENCN";
            cmbCHINHANH.ValueMember = "TENSERVER";
            cmbCHINHANH.SelectedIndex = Program.brand;
            /*Step 3*/
            /*CONG TY chi xem du lieu*/
            if (Program.role == "CongTy")
            {
                cmbCHINHANH.Enabled = true;

                this.btnThem.Enabled = false;
                this.btnXoa.Enabled = false;
                this.btnGhi.Enabled = false;

                this.btnPhucHoi.Enabled = false;
                this.btnReload.Enabled = true;
                this.btnCheDo.Enabled = false;
                this.btnThoat.Enabled = true;
                this.panelControl2.Enabled = false;

            }

            /* CHI NHANH & USER co the xem - xoa - sua du lieu nhung khong the 
             chuyen sang chi nhanh khac*/
            if (Program.role == "ChiNhanh" || Program.role == "User")
            {
                cmbCHINHANH.Enabled = false;

                this.btnThem.Enabled = false;
                this.btnXoa.Enabled = false;
                this.btnGhi.Enabled = false;

                this.btnPhucHoi.Enabled = false;
                this.btnReload.Enabled = true;
                this.btnCheDo.Enabled = true;
                this.btnThoat.Enabled = true;
                this.panelControl2.Enabled = false;

            }
        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mAPNLabel_Click(object sender, EventArgs e)
        {

        }

        private void txtMAPN_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbCHINHANH_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            /*Neu combobox khong co so lieu thi ket thuc luon*/
            if (cmbCHINHANH.SelectedValue.ToString() == "System.Data.DataRowView")
                return;

            Program.serverName = cmbCHINHANH.SelectedValue.ToString();

            /*Neu chon sang chi nhanh khac voi chi nhanh hien tai*/
            if (cmbCHINHANH.SelectedIndex != Program.brand)
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
                this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTPNTableAdapter.Fill(this.DS_SV1.CTPN);

                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.DS_SV1.PhieuNhap);
            }
        }

        private void btnPhieuNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 0*/
            btnCheDo.Links[0].Caption = "Phiếu Nhập";
            MessageBox.Show("Chế Độ Làm Việc Phiếu Nhập", "Thông báo", MessageBoxButtons.OK);
            /*Step 1*/
            bds = bdsPhieuNhap;
            gc = phieuNhapGridControl;


            /*Step 2*/
            /*Bat chuc nang cua phieu nhap*/
            this.txtMADDH.Enabled = false;
            this.txtMAPN.Enabled = true;
            this.deNGAY.Enabled = true;
            this.txtMANV.Enabled = true;
            this.txtMAKHO.Enabled = true;
            this.btnChonDonHang.Enabled = false;
            phieuNhapGridControl.Enabled = true;
            cTPNGridControl.Enabled = false;
            panelControl2.Enabled = true;

            /*Tat chuc nang cua chi tiet phieu nhap*/
            
            txtMAVT.Enabled = false;
            txtSOLUONG.Enabled = false;
            txtDONGIA.Enabled = false;
            btnChonChiTietDonHang.Enabled = false;
            
            this.btnThem.Enabled = true;
            bool turnOn = (bdsPhieuNhap.Count > 0) ? true : false;
            this.btnXoa.Enabled = turnOn;
            this.btnGhi.Enabled = true;

            this.btnPhucHoi.Enabled = true;
            this.btnReload.Enabled = true;
            this.btnCheDo.Enabled = true;
            this.btnThoat.Enabled = true;
        }

        private void btnChiTietPhieuNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 0*/
            btnCheDo.Links[0].Caption = "Chi Tiết Phiếu Nhập";

            /*Step 1*/
            bds = bdsCTPN;
            gc = phieuNhapGridControl;
            MessageBox.Show("Chế Độ Làm Việc Chi Tiết Phiếu Nhập", "Thông báo", MessageBoxButtons.OK);

            /*Step 2*/
            /* tat chuc nang cua phieunhap*/
            this.txtMADDH.Enabled = false;
            this.txtMAPN.Enabled = false;
            this.deNGAY.Enabled = false;
            this.txtMANV.Enabled = false;
            this.txtMAKHO.Enabled = false;
            this.btnChonDonHang.Enabled = false;
            

            /*bat chuc nang cua chi tiet phieunhap*/
            txtMADDH.Enabled = true;
            txtSOLUONG.Enabled = true;
            txtDONGIA.Enabled = true;
            btnChonChiTietDonHang.Enabled=false;
            phieuNhapGridControl.Enabled = true;
            cTPNGridControl.Enabled = true;
            panelControl2.Enabled = true;

            this.btnThem.Enabled = true;
            bool turnOn = (bdsCTPN.Count > 0) ? true : false;
            this.btnXoa.Enabled = turnOn;
            this.btnGhi.Enabled = true;

            this.btnPhucHoi.Enabled =true;
            this.btnReload.Enabled = true;
            this.btnCheDo.Enabled = true;
            this.btnThoat.Enabled = true;
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 1*/
            /*lấy vị trí hiện tại của con trỏ*/
            viTri = bds.Position;
            dangThemMoi = true;

            /*Step 2*/
            /*AddNew tự động nhảy xuống cuối thêm 1 dòng mới*/
            bds.AddNew();
            if (btnCheDo.Links[0].Caption == "Phiếu Nhập")
            {
                this.txtMAPN.Enabled = true;
                this.deNGAY.EditValue = DateTime.Now;
                this.deNGAY.Enabled = false;

                this.txtMADDH.Enabled = false;
                this.btnChonDonHang.Enabled = true;
                this.panelControl2.Enabled = true;
                this.txtMANV.Text = Program.userName;
                this.txtMAKHO.Text = Program.maKhoDuocChon;


                /*Gan tu dong may truong du lieu nay*/
                ((DataRowView)(bdsPhieuNhap.Current))["NGAY"] = DateTime.Now;
                ((DataRowView)(bdsPhieuNhap.Current))["MasoDDH"] = Program.maDonDatHangDuocChon;
                ((DataRowView)(bdsPhieuNhap.Current))["MANV"] = Program.userName;
                ((DataRowView)(bdsPhieuNhap.Current))["MAKHO"] =
                Program.maKhoDuocChon;

            }

            if (btnCheDo.Links[0].Caption == "Chi Tiết Phiếu Nhập")
            {
                DataRowView drv = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Bạn không thêm chi tiết phiếu nhập trên phiếu không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    bdsCTPN.RemoveCurrent();
                    return;
                }
                this.txtMAPN.Enabled = false;
                this.txtMADDH.Enabled = false;
                this.txtMAVT.Enabled = false;
                this.txtMAVT.Text = "";
                this.btnChonChiTietDonHang.Enabled = true;
                this.panelControl2.Enabled = true;
                this.txtSOLUONG.Enabled = true;
                this.txtDONGIA.Enabled = true;
                

                /*Gan tu dong may truong du lieu nay*/
                ((DataRowView)(bdsCTPN.Current))["MAPN"] = ((DataRowView)(bdsPhieuNhap.Current))["MAPN"];
                ((DataRowView)(bdsCTPN.Current))["MAVT"] =
                    Program.maVatTuDuocChon;
                ((DataRowView)(bdsCTPN.Current))["SOLUONG"] =
                    Program.soLuongVatTu;
                ((DataRowView)(bdsCTPN.Current))["DONGIA"] =
                    Program.donGia;
                

            }


            /*Step 3*/
            this.btnThem.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnGhi.Enabled = true;

            this.btnPhucHoi.Enabled = true;
            this.btnReload.Enabled = false;
            this.btnCheDo.Enabled = false;
            this.btnThoat.Enabled = true;

           
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // do du lieu moi tu dataSet vao gridControl NHANVIEN
                this.phieuNhapTableAdapter.Fill(this.DS_SV1.PhieuNhap);
                this.cTPNTableAdapter.Fill(this.DS_SV1.CTPN);


                string cheDo = (btnCheDo.Links[0].Caption == "Phiếu Nhập") ? "Phiếu Nhập" : "Chi Tiết Phiếu Nhập";
                if (cheDo == "Phiếu Nhập")
                {
                    this.txtMADDH.Enabled = false;
                    this.txtMAPN.Enabled = true;
                    this.deNGAY.Enabled = true;
                    this.txtMANV.Enabled = true;
                    this.txtMAKHO.Enabled = true;
                    this.btnChonDonHang.Enabled = true;
                    phieuNhapGridControl.Enabled = true;
                    cTPNGridControl.Enabled = false;
                    panelControl2.Enabled = true;
                    bdsPhieuNhap.Position = viTri;

                }
                if (cheDo == "Chi Tiết Phiếu Nhập")
                {
                    txtMADDH.Enabled = true;
                    txtSOLUONG.Enabled = true;
                    txtDONGIA.Enabled = true;
                    btnChonChiTietDonHang.Enabled = true;
                    phieuNhapGridControl.Enabled = true;
                    cTPNGridControl.Enabled = true;
                    panelControl2.Enabled = true;
                    bdsCTPN.Position = viTri;


                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Làm mới" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /* Step 0 */
            if (dangThemMoi == true && this.btnThem.Enabled == false)
            {
                dangThemMoi = false;

                /*dang o che do Phiếu Nhập*/
                if (btnCheDo.Links[0].Caption == "Phiếu Nhập")
                {
                    this.txtMADDH.Enabled = false;
                    deNGAY.Enabled = false;

                    txtMADDH.Enabled = false;
                    txtMAKHO.Enabled = false;

                    btnChonDonHang.Enabled = false;
                    
                }
                /*dang o che do Chi Tiết Phiếu Nhập*/
                if (btnCheDo.Links[0].Caption == "Chi Tiết Phiếu Nhập")
                {
                    this.txtMADDH.Enabled = false;
                    this.btnChonChiTietDonHang.Enabled = false;

                    this.txtMAVT.Enabled = false;
                    this.txtSOLUONG.Enabled = false;
                    this.txtDONGIA.Enabled = false;

                    this.btnXoa.Enabled = false;
                    
                }
                this.btnThem.Enabled = true;
                this.btnXoa.Enabled = true;
                this.btnGhi.Enabled = true;

                //this.btnHOANTAC.Enabled = false;
                this.btnReload.Enabled = true;
                this.btnCheDo.Enabled = true;
                this.btnThoat.Enabled = true;

                

                bds.CancelEdit();
                /*xoa dong hien tai*/
                //bds.RemoveCurrent();
                /* trở về lúc đầu con trỏ đang đứng*/
                bds.Position = viTri;
                return;
            }

            /*Step 1*/
            if (undoList.Count == 0)
            {
                MessageBox.Show("Không còn thao tác nào để khôi phục", "Thông báo", MessageBoxButtons.OK);
                btnPhucHoi.Enabled = false;
                return;
            }

            /*Step 2*/
            bds.CancelEdit();
            String cauTruyVanHoanTac = undoList.Pop().ToString();

            Console.WriteLine(cauTruyVanHoanTac);
            int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);

            this.phieuNhapTableAdapter.Fill(this.DS_SV1.PhieuNhap);
            this.cTPNTableAdapter.Fill(this.DS_SV1.CTPN);

            bdsPhieuNhap.Position = viTri;
        }
        private void capNhatSoLuongVatTu(string maVatTu, float soLuong)
        {
            string cauTruyVan = "EXEC sp_CapNhatSoLuongVatTu 'IMPORT','" + maVatTu + "', " + soLuong;
            int n = Program.ExecSqlNonQuery(cauTruyVan);
            Console.WriteLine(cauTruyVan);
        }
        
        
        private bool kiemTraDuLieuDauVao(String cheDo)
        {
            if (cheDo == "Phiếu Nhập")
            {
                if (txtMAPN.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã phiếu nhập !", "Thông báo", MessageBoxButtons.OK);
                    txtMAPN.Focus();
                    return false;
                }

                if (txtMANV.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã nhân viên !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if (txtMAKHO.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã kho !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if (txtMADDH.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã đơn đặt hàng !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
            }

            if (cheDo == "Chi Tiết Phiếu Nhập")
            {

                /*if (bdsCTPN.Count > 1)
                {
                    MessageBox.Show("Không thể thêm chi tiết phiếu nhập quá 1 lần", "Thông báo", MessageBoxButtons.OK);
                    bdsCTPN.RemoveCurrent();
                    return false;
                }*/

                if (txtMAVT.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã vật tư !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if (txtSOLUONG.Value < 0 ||
                    txtSOLUONG.Value > Program.soLuongVatTu)
                {
                    MessageBox.Show("Số lượng vật tư không thể lớn hơn số lượng vật tư trong chi tiết đơn hàng !", "Thông báo", MessageBoxButtons.OK);
                    txtSOLUONG.Focus();
                    return false;
                }

                if (txtDONGIA.Value < 1)
                {
                    MessageBox.Show("Đơn giá không thể nhỏ hơn 1 VND", "Thông báo", MessageBoxButtons.OK);
                    txtDONGIA.Focus();
                    return false;
                }
            }

            return true;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 1*/
            String cheDo = (btnCheDo.Links[0].Caption == "Phiếu Nhập") ? "Phiếu Nhập" : "Chi Tiết Phiếu Nhập";


            /*Step 2*/
            bool ketQua = kiemTraDuLieuDauVao(cheDo);
            if (ketQua == false) return;

            /*Step 3*/

            /*Step 4*/
            String maPhieuNhap = txtMAPN.Text.Trim();
            //Console.WriteLine(maPhieuNhap);
            String cauTruyVan =
                    "DECLARE	@result int " +
                    "EXEC @result = sp_KiemTraMaPhieuNhap '" +
                    maPhieuNhap + "' " +
                    "SELECT 'Value' = @result";
            SqlCommand sqlCommand = new SqlCommand(cauTruyVan, Program.conn);
            try
            {
                Program.myReader = Program.ExecSqlDataReader(cauTruyVan);
                /*khong co ket qua tra ve thi ket thuc luon*/
                if (Program.myReader == null)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();

            /*Step 5*/
            int viTriConTro = bdsPhieuNhap.Position;
            int viTriMaPhieuNhap = bdsPhieuNhap.Find("MAPN", maPhieuNhap);

            /*Dang them moi phieu nhap*/
            if (result == 1 && cheDo == "Phiếu Nhập" && viTriMaPhieuNhap != viTriConTro)
            {
                MessageBox.Show("Mã phiếu nhập đã được sử dụng !", "Thông báo", MessageBoxButtons.OK);
                txtMAPN.Focus();
                return;
            }
            else
            {
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu vào cơ sở dữ liệu ?", "Thông báo",
                         MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        

                        /*TH2: them moi chi tiet don hang*/
                        if (cheDo == "Chi Tiết Phiếu Nhập" && dangThemMoi == true)
                        {

                            string maVatTu = txtMAVT.Text.Trim();
                            float soLuong = (float)txtSOLUONG.Value;
                            
                            capNhatSoLuongVatTu(maVatTu, soLuong);

                        }

                        

                        this.bdsPhieuNhap.EndEdit();
                        this.bdsCTPN.EndEdit();
                        this.phieuNhapTableAdapter.Update(this.DS_SV1.PhieuNhap);
                        this.cTPNTableAdapter.Update(this.DS_SV1.CTPN);

                        this.btnThem.Enabled = true;
                        this.btnXoa.Enabled = true;
                        this.btnGhi.Enabled = true;

                        this.btnPhucHoi.Enabled = false;
                        this.btnReload.Enabled = true;
                        this.btnCheDo.Enabled = true;
                        this.btnThoat.Enabled = true;

                        this.phieuNhapGridControl.Enabled = true;
                        this.cTPNGridControl.Enabled = true;

                        this.txtSOLUONG.Enabled = false;
                        this.txtDONGIA.Enabled = false;
                        /*cập nhật lại trạng thái thêm mới cho chắc*/
                        dangThemMoi = false;
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        bds.RemoveCurrent();
                        MessageBox.Show("Da xay ra loi !\n\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
       
        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRowView drv;
            string cauTruyVanHoanTac = "";
            string cheDo = (btnCheDo.Links[0].Caption == "Phiếu Nhập") ? "Phiếu Nhập" : "Chi Tiết Phiếu Nhập";

            dangThemMoi = true;

            if (cheDo == "Phiếu Nhập")
            {
                drv = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Không xóa chi tiết phiếu xuất không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (bdsCTPN.Count > 0)
                {
                    MessageBox.Show("Không thể xóa phiếu nhập vì có chi tiết phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                drv = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
                DateTime ngay = ((DateTime)drv["NGAY"]);

                cauTruyVanHoanTac = "INSERT INTO DBO.PHIEUNHAP(MAPN, NGAY, MasoDDH, MANV, MAKHO) " +
                    "VALUES( '" + drv["MAPN"].ToString().Trim() + "', '" +
                    ngay.ToString("yyyy-MM-dd") + "', '" +
                    drv["MasoDDH"].ToString() + "', '" +
                    drv["MANV"].ToString() + "', '" +
                    drv["MAKHO"].ToString() + "')";
            }

            if (cheDo == "Chi Tiết Phiếu Nhập")
            {
                drv = ((DataRowView)bdsPhieuNhap[bdsPhieuNhap.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Bạn không xóa chi tiết phiếu nhập không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                drv = ((DataRowView)bdsCTPN[bdsCTPN.Position]);
                cauTruyVanHoanTac = "INSERT INTO DBO.CTPN(MAPN, MAVT, SOLUONG, DONGIA) " +
                    "VALUES('" + drv["MAPN"].ToString().Trim() + "', '" +
                    drv["MAVT"].ToString().Trim() + "', " +
                    drv["SOLUONG"].ToString().Trim() + ", " +
                    drv["DONGIA"].ToString().Trim() + ")";
            }

            undoList.Push(cauTruyVanHoanTac);
            

            /*Step 2*/
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    /*Step 3*/
                    viTri = bds.Position;
                    if (cheDo == "Phiếu Nhập")
                    {
                        bdsPhieuNhap.RemoveCurrent();
                    }
                    if (cheDo == "Chi Tiết Phiếu Nhập")
                    {
                        bdsCTPN.RemoveCurrent();
                    }


                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuNhapTableAdapter.Update(this.DS_SV1.PhieuNhap);

                    this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTPNTableAdapter.Update(this.DS_SV1.CTPN);

                    //bdsPhieuNhap.Position = viTri;
                    /*Cap nhat lai do ben tren can tao cau truy van nen da dat dangThemMoi = true*/
                    dangThemMoi = true;
                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnPhucHoi.Enabled = true;
                }
                catch (Exception ex)
                {
                    /*Step 4*/
                    MessageBox.Show("Lỗi xóa . Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuNhapTableAdapter.Update(this.DS_SV1.PhieuNhap);

                    this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTPNTableAdapter.Update(this.DS_SV1.CTPN);
                    // tro ve vi tri cua nhan vien dang bi loi
                    bds.Position = viTri;
                    //bdsNhanVien.Position = bdsNhanVien.Find("MANV", manv);
                    return;
                }
            }
            else
            {
                // xoa cau truy van hoan tac di
                undoList.Pop();
            }
        }

        private void btnChonChiTietDonHang_Click(object sender, EventArgs e)
        {
            /*Lay MasoDDH hien tai cua gcPhieuNhap de so sanh voi MasoDDH se duoc chon*/
            Program.maDonDatHangDuocChon = ((DataRowView)(bdsPhieuNhap.Current))["MasoDDH"].ToString().Trim();

            //Console.WriteLine(Program.maDonDatHangDuocChon);
            FormChonChiTietDonHang form = new FormChonChiTietDonHang();
            form.ShowDialog();

            this.txtMAVT.Text = Program.maVatTuDuocChon;
            this.txtSOLUONG.Value = Program.soLuongVatTu;
            
        }

        private void btnChonDonHang_Click(object sender, EventArgs e)
        {
            FormChonDonDatHang form = new FormChonDonDatHang();
            form.ShowDialog();

            this.txtMADDH.Text = Program.maDonDatHangDuocChon;
            this.txtMAKHO.Text = Program.maKhoDuocChon;
        }

    

        
        
    
    }
}
