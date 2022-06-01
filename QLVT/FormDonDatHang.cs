using DevExpress.XtraGrid;
using QLVT.FormDanhSach;
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
    public partial class FormDonDatHang : Form
    {
        /* vị trí của con trỏ trên grid view*/
        int viTri = 0;
        
        bool dangThemMoi = false;
        public string makho = "";
        string maChiNhanh = "";
       
        Stack undoList = new Stack();

        /********************************************************
         * bds chứa những dữ liệu hiện tại đang làm việc
         * gc chứa grid view đang làm việc
         ********************************************************/
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
        public FormDonDatHang()
        {
            InitializeComponent();
        }
        private void datHangBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsDatHang.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS_SV1);

        }


        private void FormDonDatHang_Load(object sender, EventArgs e)
        {
            /*Step 1*/
            /*không kiểm tra khóa ngoại nữa*/
            DS_SV1.EnforceConstraints = false;

            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Fill(this.DS_SV1.CTDDH);

            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.DS_SV1.PhieuNhap);

            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.DS_SV1.DatHang);
            

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
                cmbChiNhanh.Enabled = false;

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

        private void bdsDatHang_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void btnDonDatHang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 0*/
            btnCheDo.Links[0].Caption = "Đơn Đặt Hàng";

            /*Step 1*/
            bds = bdsDatHang;
            gc = datHangGridControl;
            MessageBox.Show("Chế Độ Làm Việc Đơn Đặt Hàng", "Thông báo", MessageBoxButtons.OK);

            /*Step 2*/
            /*Bat chuc nang cua don dat hang*/
            txtMaDDH.Enabled = true;
            deNgay.Enabled = true;
            txtNhaCC.Enabled = true;
            txtMaNV.Enabled = true;
            txtMaKho.Enabled = true;
            btnChonKhoHang.Enabled = false;
            panelControl2.Enabled = true;

            /*Tat chuc nang cua chi tiet don hang*/
            txtMaVT.Enabled = false;
            btnChonVatTu.Enabled = false;
            txtSoLuong.Enabled = false;
            txtDonGia.Enabled = false;

            /*Bat cac grid control len*/
            datHangGridControl.Enabled = true;
            cTDDHGridControl.Enabled = false;
            cmbChiNhanh.Enabled = false;

            this.btnThem.Enabled = true;
            bool turnOn = (bdsDatHang.Count > 0) ? true : false;
            this.btnXoa.Enabled = turnOn;
            this.btnGhi.Enabled = true;

            this.btnPhucHoi.Enabled = false;
            this.btnReload.Enabled = true;
            this.btnCheDo.Enabled = true;
            this.btnThoat.Enabled = true;
            this.txtMaDDH.Enabled = false;

        }

        private void btnCheDoCTDDH_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 0*/
            btnCheDo.Links[0].Caption = "Chi Tiết Đơn Đặt Hàng";

            /*Step 1*/
            bds = bdsCTDDH;
            gc = cTDDHGridControl;
            MessageBox.Show("Chế Độ Làm Việc Chi Tiết Đơn Đặt Hàng", "Thông báo", MessageBoxButtons.OK);

            /*Step 2*/
            /*Bat chuc nang cua don dat hang*/
            txtMaDDH.Enabled = false;
            deNgay.Enabled = false;
            txtNhaCC.Enabled = false;
            txtMaNV.Enabled = false;
            txtMaKho.Enabled = false;
            btnChonKhoHang.Enabled = false;

            /*Tat chuc nang cua chi tiet don hang*/
            txtMaVT.Enabled = true;
            btnChonVatTu.Enabled = false;
            txtSoLuong.Enabled = true;
            txtDonGia.Enabled = true;
            panelControl2.Enabled = true;
            /*Bat cac grid control len*/
            datHangGridControl.Enabled = true;
            cTDDHGridControl.Enabled = true;

            this.btnThem.Enabled = true;
            bool turnOn = (bdsDatHang.Count > 0) ? true : false;
            this.btnXoa.Enabled = turnOn;
            this.btnGhi.Enabled = true;

            this.btnPhucHoi.Enabled = false;
            this.btnReload.Enabled = true;
            this.btnCheDo.Enabled = true;
            this.btnThoat.Enabled = true;
            
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dangThemMoi = true;
            /*Step 1*/
            /*lấy vị trí hiện tại của con trỏ*/
            viTri = bds.Position;


            /*Step 2*/
            /*AddNew tự động nhảy xuống cuối thêm 1 dòng mới*/
            bds.AddNew();
            if (btnCheDo.Links[0].Caption == "Đơn Đặt Hàng")
            {
                this.txtMaDDH.Enabled = true;
                this.deNgay.EditValue = DateTime.Now;
                this.deNgay.Enabled = false;
                this.txtNhaCC.Enabled = true;
                this.txtMaNV.Text = Program.userName;
                this.btnChonKhoHang.Enabled = true;
                this.txtMaKho.Enabled = false;
                this.panelControl2.Enabled=true;

                /*Gan tu dong may truong du lieu nay*/
                ((DataRowView)(bdsDatHang.Current))["MANV"] = Program.userName;
                ((DataRowView)(bdsDatHang.Current))["NGAY"] = DateTime.Now;
                
            }

            if (btnCheDo.Links[0].Caption == "Chi Tiết Đơn Đặt Hàng")
            {
                DataRowView drv = ((DataRowView)bdsDatHang[bdsDatHang.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Bạn không thêm chi tiết đơn hàng trên phiếu không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    bdsCTDDH.RemoveCurrent();
                    return;
                }

                this.txtMaVT.Enabled = false;
                this.btnChonVatTu.Enabled = true;
                this.txtSoLuong.Enabled = true;
                this.txtSoLuong.EditValue = 1;

                this.txtDonGia.Enabled = true;
                this.txtDonGia.EditValue = 1;
                panelControl2.Enabled = true;
                
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
        
        private bool kiemTraDuLieuDauVao(String cheDo)
        {
            if (cheDo == "Đơn Đặt Hàng")
            {
                if (txtMaDDH.Text == "")
                {
                    MessageBox.Show("Không thể bỏ trống mã đơn hàng", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                if (txtMaDDH.Text.Length > 8)
                {
                    MessageBox.Show("Mã đơn đặt hàng không quá 8 kí tự", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                if (txtMaNV.Text == "")
                {
                    MessageBox.Show("Không thể bỏ trống mã nhân viên", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                if (txtNhaCC.Text == "")
                {
                    MessageBox.Show("Không thể bỏ trống nhà cung cấp", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                if (txtNhaCC.Text.Length > 100)
                {
                    MessageBox.Show("Tên nhà cung cấp không quá 100 kí tự", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                if (txtMaKho.Text == "")
                {
                    MessageBox.Show("Không thể bỏ trống mã kho", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
            }

            if (cheDo == "Chi Tiết Đơn Đặt Hàng")
            {
                if (txtMaVT.Text == "")
                {
                    MessageBox.Show("Không thể bỏ trống mã vật tư", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
                if (txtSoLuong.Value < 0 || txtDonGia.Value < 0)
                {
                    MessageBox.Show("Không thể nhỏ hơn 1", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if (txtSoLuong.Value > Program.soLuongVatTu)
                {
                    MessageBox.Show("Sô lượng đặt mua lớn hơn số lượng vật tư hiện có", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }
            }
            return true;
        }
        
        private String taoCauTruyVanHoanTac(String cheDo)
        {
            String cauTruyVan = "";
            DataRowView drv;

            /*Dang chinh sua don dat hang*/
            if (cheDo == "Đơn Đặt Hàng" && dangThemMoi == false)
            {
                drv = ((DataRowView)bdsDatHang[bdsDatHang.Position]);
                /*Ngay can duoc xu ly dac biet hon*/
                DateTime ngay = ((DateTime)drv["NGAY"]);

                cauTruyVan = "UPDATE DBO.DATHANG " +
                    "SET " +
                    "NGAY = CAST('" + ngay.ToString("yyyy-MM-dd") + "' AS DATETIME), " +
                    "NhaCC = '" + drv["NhaCC"].ToString().Trim() + "', " +
                    "MANV = '" + drv["MANV"].ToString().Trim() + "', " +
                    "MAKHO = '" + drv["MAKHO"].ToString().Trim() + "' " +
                    "WHERE MasoDDH = '" + drv["MasoDDH"].ToString().Trim() + "'";
            }
            /*Dang xoa don dat hang*/
            if (cheDo == "Đơn Đặt Hàng" && dangThemMoi == true)
            {
                drv = ((DataRowView)bdsDatHang[bdsDatHang.Position]);
                DateTime ngay = ((DateTime)drv["NGAY"]);
                cauTruyVan = "INSERT INTO DBO.DATHANG(MasoDDH, NGAY, NhaCC, MaNV, MaKho) " +
                    "VALUES('" + drv["MasoDDH"] + "', '" +
                    ngay.ToString("yyyy-MM-dd") + "', '" +
                    drv["NhaCC"].ToString() + "', '" +
                    drv["MaNV"].ToString() + "', '" +
                    drv["MaKho"].ToString() + "' )";

            }

            /*Dang chinh sua chi tiet don dat hang*/
            if (cheDo == "Chi Tiết Đơn Đặt Hàng" && dangThemMoi == false)
            {
                drv = ((DataRowView)bdsCTDDH[bdsCTDDH.Position]);

                cauTruyVan = "UPDATE DBO.CTDDH " +
                    "SET " +
                    "SOLUONG = " + drv["SOLUONG"].ToString() + " , " +
                    "DONGIA = " + drv["DONGIA"].ToString() + " " +
                    "WHERE MasoDDH = '" + drv["MasoDDH"].ToString().Trim() + "'" +
                    " AND MAVT = '" + drv["MAVT"].ToString().Trim() + "'";

            }
            if (cheDo == "Chi Tiết Đơn Đặt Hàng" && dangThemMoi == true)
            {
                drv = ((DataRowView)bdsCTDDH[bdsCTDDH.Position]);
                cauTruyVan = "INSERT INTO DBO.CTDDH(MasoDDH,MAVT,SOLUONG,DONGIA) " +
                    "VALUES('" + drv["MasoDDH"].ToString().Trim() + "', '" +
                    drv["MAVT"].ToString() + "', '" +
                    drv["SOLUONG"].ToString() + "', '" +
                    drv["DONGIA"].ToString() + "' )";

            }
            return cauTruyVan;
        }
        /**************************************************
       * Step 1: Kiem tra xem day co phai nguoi lap don hang hay không
       * Step 2: lay che do dang lam viec, kiem tra du lieu dau vao. Neu OK thi 
       * tiep tuc tao cau truy van hoan tac neu dangThemMoi = false
       * Step 3: kiem tra xem cai ma don hang nay da ton tai chua ?
       *          Neu co thi ket thuc luon
       *          Neu khong thi cho them moi
       **************************************************/
        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            viTri = bdsDatHang.Position;
            /*Step 1*/
            DataRowView drv = ((DataRowView)bdsDatHang[bdsDatHang.Position]);
            /*lay maNhanVien & maDonDatHang de phong truong hop them chi tiet don hang thi se co ngay*/
            String maNhanVien = drv["MANV"].ToString();
            String maDonDatHang = drv["MasoDDH"].ToString().Trim();

            if (Program.userName != maNhanVien && dangThemMoi == false)
            {
                MessageBox.Show("Bạn không thể sửa phiếu do người khác lập", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            /*Step 2*/
            String cheDo = (btnCheDo.Links[0].Caption == "Đơn Đặt Hàng") ? "Đơn Đặt Hàng" : "Chi Tiết Đơn Đặt Hàng";

            bool ketQua = kiemTraDuLieuDauVao(cheDo);
            if (ketQua == false) return;

            


            /*Step 3*/
            String maDonDatHangMoi = txtMaDDH.Text;
            String cauTruyVan =
                    "DECLARE	@result int " +
                    "EXEC @result = sp_KiemTraMaDonDatHang '" +
                    maDonDatHangMoi + "' " +
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



            /*Step 4*/
            //Console.WriteLine(txtMaNhanVien.Text);
            int viTriHienTai = bds.Position;
            int viTriMaDonDatHang = bdsDatHang.Find("MasoDDH", txtMaDDH.Text);
            
            if (result == 1 && cheDo == "Đơn Đặt Hàng" && viTriHienTai != viTriMaDonDatHang)
            {
                MessageBox.Show("Mã đơn hàng này đã được sử dụng !\n\n", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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


                        this.bdsDatHang.EndEdit();
                        this.bdsCTDDH.EndEdit();
                        this.datHangTableAdapter.Update(this.DS_SV1.DatHang);
                        this.cTDDHTableAdapter.Update(this.DS_SV1.CTDDH);

                        this.btnThem.Enabled = true;
                        this.btnXoa.Enabled = true;
                        this.btnGhi.Enabled = true;

                        this.btnPhucHoi.Enabled = false;
                        this.btnReload.Enabled = true;
                        this.btnCheDo.Enabled = true;
                        this.btnThoat.Enabled = true;
                        this.panelControl2.Enabled = false;
                        //this.groupBoxDonDatHang.Enabled = false;

                        /*cập nhật lại trạng thái thêm mới cho chắc*/
                        dangThemMoi = false;
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //bds.RemoveCurrent();
                        MessageBox.Show("Da xay ra loi !\n\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
        
        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /* Step 0 */
            if (dangThemMoi == true && this.btnThem.Enabled == false)
            {
                dangThemMoi = false;

                /*dang o che do Don Dat Hang*/
                if (btnCheDo.Links[0].Caption == "Đơn Đặt Hàng")
                {
                    
                    this.txtMaDDH.Enabled = false;

                    //this.dteNGAY.EditValue = DateTime.Now;
                    this.deNgay.Enabled = false;
                    this.txtNhaCC.Enabled = true;
                    //this.txtMaNhanVien.Text = Program.userName;
                    this.btnChonKhoHang.Enabled = true;
                   

                }
                /*dang o che do Chi Tiet Don Dat Hang*/
                if (btnCheDo.Links[0].Caption == "Chi Tiết Đơn Đặt Hàng")
                {               
                    this.txtMaVT.Enabled = false;
                    this.btnChonVatTu.Enabled = true;

                    this.txtSoLuong.Enabled = true;
                    this.txtSoLuong.EditValue = 1;

                    this.txtDonGia.Enabled = true;
                    this.txtDonGia.EditValue = 1;

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

            this.datHangTableAdapter.Fill(this.DS_SV1.DatHang);
            this.cTDDHTableAdapter.Fill(this.DS_SV1.CTDDH);

            bdsDatHang.Position = viTri;
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // do du lieu moi tu dataSet vao gridControl NHANVIEN
                this.datHangTableAdapter.Fill(this.DS_SV1.DatHang);
                this.cTDDHTableAdapter.Fill(this.DS_SV1.CTDDH);

                
                string cheDo = (btnCheDo.Links[0].Caption == "Đơn Đặt Hàng") ? "Đơn Đặt Hàng" : "Chi Tiết Đơn Đặt Hàng";
                if (cheDo == "Đơn Đặt Hàng")
                {
                    this.datHangGridControl.Enabled = true;
                    this.cTDDHGridControl.Enabled = false;
                    txtMaDDH.Enabled = true;
                    deNgay.Enabled = true;
                    txtNhaCC.Enabled = true;
                    txtMaNV.Enabled = true;
                    txtMaKho.Enabled = true;
                    btnChonKhoHang.Enabled = true;
                    panelControl2.Enabled = true;
                    bdsDatHang.Position = viTri;

                }
                if (cheDo == "Chi Tiết Đơn Đặt Hàng")
                {
                    this.datHangGridControl.Enabled = true;
                    this.cTDDHGridControl.Enabled = true;
                    txtMaVT.Enabled = true;
                    btnChonVatTu.Enabled = true;
                    txtSoLuong.Enabled = true;
                    txtDonGia.Enabled = true;
                    panelControl2.Enabled = true;
                    /*Bat cac grid control len*/
                    bdsCTDDH.Position = viTri;
                    
                   
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Làm mới" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }
        
        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string cauTruyVan = "";
            string cheDo = (btnCheDo.Links[0].Caption == "Đơn Đặt Hàng") ? "Đơn Đặt Hàng" : "Chi Tiết Đơn Đặt Hàng";

            dangThemMoi = true;// bat cai nay len de ung voi dieu kien tao cau truy van

            if (cheDo == "Đơn Đặt Hàng")
            {
                /*Cái bdsChiTietDonHangHang là đại diện cho binding source riêng biệt của CTDDH
                 *Còn cTDDHBindingSource là lấy ngay từ trong data source DATHANG
                 */
                if (bdsCTDDH.Count > 0)
                {
                    MessageBox.Show("Không thể xóa đơn đặt hàng này vì có chi tiết đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (bdsPhieuNhap.Count > 0)
                {
                    MessageBox.Show("Không thể xóa đơn đặt hàng này vì có phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                    return;
                }


            }
            if (cheDo == "Chi Tiết Đơn Đặt Hàng")
            {
                DataRowView drv = ((DataRowView)bdsDatHang[bdsDatHang.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Bạn không xóa chi tiết đơn hàng trên phiếu không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    //bdsChiTietDonDatHang.RemoveCurrent();
                    return;
                }
            }

            cauTruyVan = taoCauTruyVanHoanTac(cheDo);
            
            undoList.Push(cauTruyVan);

            /*Step 2*/
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    /*Step 3*/
                    viTri = bds.Position;
                    if (cheDo == "Đơn Đặt Hàng")
                    {
                        bdsDatHang.RemoveCurrent();
                    }
                    if (cheDo == "Chi Tiết Đơn Đặt Hàng")
                    {
                        bdsCTDDH.RemoveCurrent();
                    }


                    this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.datHangTableAdapter.Update(this.DS_SV1.DatHang);

                    this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTDDHTableAdapter.Update(this.DS_SV1.CTDDH);

                    /*Cap nhat lai do ben tren can tao cau truy van nen da dat dangThemMoi = true*/
                    dangThemMoi = true;
                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnPhucHoi.Enabled = true;
                }
                catch (Exception ex)
                {
                    /*Step 4*/
                    MessageBox.Show("Lỗi xóa nhân viên. Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.datHangTableAdapter.Update(this.DS_SV1.DatHang);

                    this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTDDHTableAdapter.Update(this.DS_SV1.CTDDH);
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

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
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
                this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTDDHTableAdapter.Fill(this.DS_SV1.CTDDH);

                this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.datHangTableAdapter.Fill(this.DS_SV1.DatHang);

                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.DS_SV1.PhieuNhap);
            }
        }

        private void btnChonKhoHang_Click(object sender, EventArgs e)
        {
            FormChon.FormChonKhoHang form = new FormChon.FormChonKhoHang();
            form.ShowDialog();

            this.txtMaKho.Text = Program.maKhoDuocChon;
            
        }

        private void btnChonVatTu_Click(object sender, EventArgs e)
        {
            FormChon.FormChonVatTu form = new FormChon.FormChonVatTu();
            form.ShowDialog();
            this.txtMaVT.Text = Program.maVatTuDuocChon;
        }


        

        private void btnDHChuaPhieuNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormDonHangChuaCoPhieuNhap));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormDonHangChuaCoPhieuNhap form = new FormDonHangChuaCoPhieuNhap();
                
                form.ShowDialog();
            }
        }

        private void datHangGridControl_Click(object sender, EventArgs e)
        {

        }
    }

 }

