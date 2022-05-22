using DevExpress.XtraGrid;
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
    public partial class FormPhieuXuat : Form
    {
        /* vị trí của con trỏ trên grid view*/
        int viTri = 0;
        /********************************************
         * đang thêm mới -> true -> đang dùng btnTHEM
         *              -> false -> có thể là btnGHI( chỉnh sửa) hoặc btnXOA
         *              
         * Mục đích: dùng biến này để phân biệt giữa btnTHEM - thêm mới hoàn toàn
         * và việc chỉnh sửa nhân viên( do mình ko dùng thêm btnXOA )
         * Trạng thái true or false sẽ được sử dụng 
         * trong btnGHI - việc này để phục vụ cho btnHOANTAC
         ********************************************/
        bool dangThemMoi = false;
        public string makho = "";
        string maChiNhanh = "";
        /**********************************************************
         * undoList - phục vụ cho btnHOANTAC -  chứa các thông tin của đối tượng bị tác động 
         * 
         * nó là nơi lưu trữ các đối tượng cần thiết để hoàn tác các thao tác
         * 
         * nếu btnGHI sẽ ứng với INSERT
         * nếu btnXOA sẽ ứng với DELETE
         * nếu btnCHUYENCHINHANH sẽ ứng với CHANGEBRAND
         **********************************************************/
        Stack undoList = new Stack();



        /********************************************************
         * chứa những dữ liệu hiện tại đang làm việc
         * gc chứa grid view đang làm việc
         ********************************************************/
        BindingSource bds = null;
        GridControl gc = null;

        /************************************************************
         * CheckExists:
         * Để tránh việc người dùng ấn vào 1 form đến 2 lần chúng ta 
         * cần sử dụng hàm này để kiểm tra xem cái form hiện tại đã 
         * có trong bộ nhớ chưa
         * Nếu có trả về "f"
         * Nếu không trả về "null"
         ************************************************************/
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        string type = "";
        public FormPhieuXuat()
        {
            InitializeComponent();
        }

        private void phieuXuatBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsPhieuXuat.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS_SV1);

        }

        private void FormPhieuXuat_Load(object sender, EventArgs e)
        {
            /*Step 1*/
            /*không kiểm tra khóa ngoại nữa*/
            DS_SV1.EnforceConstraints = false;

            this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPXTableAdapter.Fill(this.DS_SV1.CTPX);

            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.DS_SV1.PhieuXuat);

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

        

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        private void btnCheDoPhieuXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 0*/
            btnCheDo.Links[0].Caption = "Phiếu Xuất";
            MessageBox.Show("Chế Độ Làm Việc Phiếu Xuất", "Thông báo", MessageBoxButtons.OK);
            /*Step 1*/
            bds = bdsPhieuXuat;
            gc = phieuXuatGridControl;


            /*Step 2*/
            /*Bat chuc nang cua phieu nhap*/
            this.txtMAPX.Enabled = false;
            this.txtMaNV.Enabled = true;
            this.deNgay.Enabled = true;
            this.txtHoTenKH.Enabled = true;
            this.btnChonKhoHang.Enabled = false;
            phieuXuatGridControl.Enabled = true;
            cTPXGridControl.Enabled = false;
            panelControl2.Enabled = true;

            /*Tat chuc nang cua chi tiet phieu nhap*/
            txtMaVT.Enabled = false;
            txtSoLuong.Enabled = false;
            txtDonGia.Enabled = false;
            btnChonVatTu.Enabled = false;

            this.btnThem.Enabled = true;
            bool turnOn = (bdsPhieuXuat.Count > 0) ? true : false;
            this.btnXoa.Enabled = turnOn;
            this.btnGhi.Enabled = true;
            panelControl2.Enabled = true;
            this.btnPhucHoi.Enabled = true;
            this.btnReload.Enabled = true;
            this.btnCheDo.Enabled = true;
            this.btnThoat.Enabled = true;
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
                this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTPXTableAdapter.Fill(this.DS_SV1.CTPX);

                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuXuatTableAdapter.Fill(this.DS_SV1.PhieuXuat);
            }
        }

        private void btnCheDoChiTietPhieuXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 0*/
            btnCheDo.Links[0].Caption = "Chi Tiết Phiếu Xuất";
            MessageBox.Show("Chế Độ Làm Việc Chi Tiết Phiếu Xuất", "Thông báo", MessageBoxButtons.OK);
            /*Step 1*/
            bds = bdsCTPX;
            gc = phieuXuatGridControl;


            /*Step 2*/
            /*Bat chuc nang cua phieu nhap*/
            
            this.txtMaNV.Enabled = false;
            this.deNgay.Enabled = false;
            this.txtHoTenKH.Enabled = false;
            this.btnChonKhoHang.Enabled = false;
            phieuXuatGridControl.Enabled = true;
            cTPXGridControl.Enabled = true;
            

            /*Tat chuc nang cua chi tiet phieu nhap*/
            txtMaVT.Enabled = true;
            txtSoLuong.Enabled = true;
            txtDonGia.Enabled = true;
            btnChonVatTu.Enabled = false;
            panelControl2.Enabled = true;


            this.btnThem.Enabled = true;
            bool turnOn = (bdsCTPX.Count > 0) ? true : false;
            this.btnXoa.Enabled = turnOn;
            this.btnGhi.Enabled = true;

            this.btnPhucHoi.Enabled = true;
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
            if (btnCheDo.Links[0].Caption == "Phiếu Xuất")
            {
                this.txtMAPX.Enabled = true;
                this.deNgay.EditValue = DateTime.Now;
                this.deNgay.Enabled = false;
                this.txtHoTenKH.Enabled = true;

                this.txtMaVT.Enabled = false;
                this.btnChonKhoHang.Enabled = true;
                this.panelControl2.Enabled = true;
                this.txtMaNV.Text = Program.userName;
                this.txtMaKho.Text = Program.maKhoDuocChon;


                /*Gan tu dong may truong du lieu nay*/
                ((DataRowView)(bdsPhieuXuat.Current))["NGAY"] = DateTime.Now;
                ((DataRowView)(bdsPhieuXuat.Current))["MANV"] = Program.userName;
                ((DataRowView)(bdsPhieuXuat.Current))["MAKHO"] =Program.maKhoDuocChon;
            }

            if (btnCheDo.Links[0].Caption == "Chi Tiết Phiếu Xuất")
            {
                DataRowView drv = ((DataRowView)bdsPhieuXuat[bdsPhieuXuat.Position]);
               String maNhanVien = drv["MANV"].ToString();
                
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Bạn không thêm chi tiết phiếu xuất trên phiếu không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                   bdsCTPX.RemoveCurrent();
                    return;
                }
                this.panelControl2.Enabled = true;
                this.txtMaVT.Enabled = false;
                this.txtMaVT.Text = "";
                this.btnChonVatTu.Enabled = true;
                this.panelControl2.Enabled = true;
                this.txtSoLuong.Enabled = true;
                this.txtDonGia.Enabled = true;


                /*Gan tu dong may truong du lieu nay*/
                ((DataRowView)(bdsCTPX.Current))["MAPX"] = ((DataRowView)(bdsPhieuXuat.Current))["MAPX"];
                ((DataRowView)(bdsCTPX.Current))["MAVT"] = Program.maVatTuDuocChon;

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

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // do du lieu moi tu dataSet vao gridControl NHANVIEN
                this.phieuXuatTableAdapter.Fill(this.DS_SV1.PhieuXuat);
                this.cTPXTableAdapter.Fill(this.DS_SV1.CTPX);


                string cheDo = (btnCheDo.Links[0].Caption == "Phiếu Xuất") ? "Phiếu Xuất" : "Chi Tiết Phiếu Xuất";
                if (cheDo == "Phiếu Xuất")
                {
                    this.txtMAPX.Enabled = false;
                    this.txtMaNV.Enabled = true;
                    this.deNgay.Enabled = true;
                    this.txtHoTenKH.Enabled = true;
                    this.btnChonKhoHang.Enabled = true;
                    phieuXuatGridControl.Enabled = true;
                    cTPXGridControl.Enabled = false;
                    panelControl2.Enabled = true;
                    bdsPhieuXuat.Position = viTri;

                }
                if (cheDo == "Chi Tiết Phiếu Xuất")
                {
                    txtMaVT.Enabled = true;
                    txtSoLuong.Enabled = true;
                    txtDonGia.Enabled = true;
                    btnChonVatTu.Enabled = true;
                    panelControl2.Enabled = true;
                    bdsCTPX.Position = viTri;


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi làm mời dữ liệu\n\n" + ex.Message, "Thông Báo", MessageBoxButtons.OK);
                Console.WriteLine(ex.Message);
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
                if (btnCheDo.Links[0].Caption == "Phiếu Xuất")
                {
                    this.txtMAPX.Enabled = false;
                    deNgay.Enabled = false;

                    txtMaNV.Enabled = false;
                    txtMaKho.Enabled = false;
                    txtHoTenKH.Enabled = false;
                    btnChonKhoHang.Enabled = false;

                }
                /*dang o che do Chi Tiết Phiếu Nhập*/
                if (btnCheDo.Links[0].Caption == "Chi Tiết Phiếu Xuất")
                {
                    this.txtMAPX.Enabled = false;
                    this.btnChonVatTu.Enabled = false;

                    this.txtMaVT.Enabled = false;
                    this.txtSoLuong.Enabled = false;
                    this.txtDonGia.Enabled = false;

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

            this.phieuXuatTableAdapter.Fill(this.DS_SV1.PhieuXuat);
            this.cTPXTableAdapter.Fill(this.DS_SV1.CTPX);

            bdsPhieuXuat.Position = viTri;
        }
        private bool kiemTraDuLieuDauVao(string cheDo)
        {
            if (cheDo == "Phiếu Xuất")
            {
                DataRowView drv = ((DataRowView)bdsPhieuXuat[bdsPhieuXuat.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Không thể sửa phiếu xuất do người khác tạo", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

                if (txtMAPX.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã phiếu xuất !", "Thông báo", MessageBoxButtons.OK);
                    txtMAPX.Focus();
                    return false;
                }

                if (txtMAPX.Text.Length > 8)
                {
                    MessageBox.Show("Mã phiếu xuất không thể quá 8 kí tự !", "Thông báo", MessageBoxButtons.OK);
                    txtMAPX.Focus();
                    return false;
                }

                if (txtHoTenKH.Text == "")
                {
                    MessageBox.Show("Không bỏ trống tên khách hàng !", "Thông báo", MessageBoxButtons.OK);
                    txtHoTenKH.Focus();
                    return false;
                }

                if (txtHoTenKH.Text.Length > 100)
                {
                    MessageBox.Show("Tên khách hàng không quá 100 kí tự !", "Thông báo", MessageBoxButtons.OK);
                   txtHoTenKH.Focus();
                    return false;
                }

                if (txtMaKho.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã kho !", "Thông báo", MessageBoxButtons.OK);
                    return false;
                }

            }

            if (cheDo == "Chi Tiết Phiếu Xuất")
            {
                DataRowView drv = ((DataRowView)bdsPhieuXuat[bdsPhieuXuat.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Không thể thêm chi tiết phiếu xuất với phiếu xuất do người khác tạo !", "Thông báo", MessageBoxButtons.OK);
                    bdsCTPX.RemoveCurrent();
                    return false;
                }

                if (txtMAPX.Text == "")
                {
                    MessageBox.Show("Không bỏ trống mã phiếu xuất !", "Thông báo", MessageBoxButtons.OK);
                    txtMAPX.Focus();
                    return false;
                }

                if (txtMAPX.Text.Length > 8)
                {
                    MessageBox.Show("Mã phiếu xuất không thể quá 8 kí tự !", "Thông báo", MessageBoxButtons.OK);
                    txtMAPX.Focus();
                    return false;
                }

                if (txtMaVT.Text == "")
                {
                    MessageBox.Show("Thiếu mã vật tư !", "Thông báo", MessageBoxButtons.OK);
                    txtMaVT.Focus();
                    return false;
                }

                if (txtMaVT.Text.Length > 4)
                {
                    MessageBox.Show("Mã vật tư không quá 4 kí tự !", "Thông báo", MessageBoxButtons.OK);
                    txtMaVT.Focus();
                    return false;
                }

                if (txtSoLuong.Value < 0 || txtSoLuong.Value > Program.soLuongVatTu)
                {
                    MessageBox.Show("Số lượng vật tư không thể bé hơn 0 & lớn hơn số lượng vật tư đang có trong kho hàng !", "Thông báo", MessageBoxButtons.OK);
                    txtSoLuong.Focus();
                    return false;
                }

                if (txtDonGia.Value < 0)
                {
                    MessageBox.Show("Đơn giá không thể bé hơn 0 VND !", "Thông báo", MessageBoxButtons.OK);
                    txtDonGia.Focus();
                    return false;
                }
            }

            return true;
        }
        private string taoCauTruyVanHoanTac(string cheDo)
        {
            String cauTruyVan = "";
            DataRowView drv;

            /*TH1: dang sua phieu xuat*/
            if (cheDo == "Phiếu Xuất" && dangThemMoi == false)
            {
                drv = ((DataRowView)(bdsPhieuXuat.Current));
                DateTime ngay = (DateTime)drv["NGAY"];


                cauTruyVan = "UPDATE DBO.PHIEUXUAT " +
                    "SET " +
                    "NGAY = CAST('" + ngay.ToString("yyyy-MM-dd") + "' AS DATETIME), " +
                    "HOTENKH = '" + drv["HOTENKH"].ToString().Trim() + "', " +
                    "MANV = '" + drv["MANV"].ToString().Trim() + "', " +
                    "MAKHO = '" + drv["MAKHO"].ToString().Trim() + "' " +
                    "WHERE MAPX = '" + drv["MAPX"].ToString().Trim() + "' ";
            }

            /*TH2: them moi phieu xuat*/
            if (cheDo == "Phiếu Xuất" && dangThemMoi == true)
            {
                // tao trong btnGHI thi hon
            }

            /*TH3: them moi chi tiet phieu xuat*/
            if (cheDo == "Chi Tiết Phiếu Xuất" && dangThemMoi == true)
            {
                // tao trong btnGHI thi hon
            }

            /*TH4: dang sua chi tiet phieu nhap*/
            if (cheDo == "Chi Tiết Phiếu Xuất" && dangThemMoi == false)
            {
                drv = ((DataRowView)(bdsCTPX.Current));
                int soLuong = int.Parse(drv["SOLUONG"].ToString().Trim());
                float donGia = float.Parse(drv["DONGIA"].ToString().Trim());
                String maPhieuXuat = drv["MAPX"].ToString().Trim();
                String maVatTu = drv["MAVT"].ToString().Trim();

                cauTruyVan = "UPDATE DBO.CTPX " +
                    "SET " +
                    "SOLUONG = " + soLuong + " " +
                    "DONGIA = " + donGia + " " +
                    "WHERE MAPX = '" + maPhieuXuat + "' " +
                    "AND MAVT = '" + maVatTu + "' ";
            }

            return cauTruyVan;
        }
        private void capNhatSoLuongVatTu(string maVatTu, int soLuong)
        {
            string cauTruyVan = "EXEC sp_CapNhatSoLuongVatTu 'EXPORT','" + maVatTu + "', " + soLuong;
            int n = Program.ExecSqlNonQuery(cauTruyVan);
            //Console.WriteLine("Line 536");
            //Console.WriteLine(cauTruyVan);
        }
        /*
         *Step 1: xac dinh xem minh dang GHI o che do nao
         *Step 2: kiem tra du lieu dau vao
         *Step 3: tao cau truy van hoan tac
         *Step 4: dung stored procedure kiem tra xem phieu nhap da ton tai chua ?
         *Step 5: xu ly du lieu neu co
         */
        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 1*/
            String cheDo = (btnCheDo.Links[0].Caption == "Phiếu Xuất") ? "Phiếu Xuất" : "Chi Tiết Phiếu Xuất";


            /*Step 2*/
            bool ketQua = kiemTraDuLieuDauVao(cheDo);
            if (ketQua == false) return;



            /*Step 3*/
            string cauTruyVanHoanTac = taoCauTruyVanHoanTac(cheDo);


            /*Step 4*/
            String maPhieuXuat = txtMAPX.Text.Trim();
            //Console.WriteLine(maPhieuNhap);
            String cauTruyVan =
                    "DECLARE	@result int " +
                    "EXEC @result = sp_KiemTraMaPhieuXuat '" +
                    maPhieuXuat + "' " +
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
            int viTriConTro = bdsPhieuXuat.Position;
            int viTriMaPhieuXuat = bdsPhieuXuat.Find("MAPX", maPhieuXuat);

            /*Dang them moi phieu nhap*/
            if (result == 1 && cheDo == "Phiếu Xuất" && viTriMaPhieuXuat != viTriConTro)
            {
                MessageBox.Show("Mã phiếu xuất đã được sử dụng !", "Thông báo", MessageBoxButtons.OK);
                txtMAPX.Focus();
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
                        //Console.WriteLine(txtMaNhanVien.Text);
                        /*TH1: them moi phieu nhap*/
                        if (cheDo == "Phiếu Xuất" && dangThemMoi == true)
                        {
                            cauTruyVanHoanTac = "Phiếu Xuất";
                        }

                        /*TH2: them moi chi tiet don hang*/
                        if (cheDo == "Chi Tiết Phiếu Xuất" && dangThemMoi == true)
                        {
                            cauTruyVanHoanTac ="Chi Tiết Phiếu Xuất";

                            string maVatTu = txtMaVT.Text.Trim();
                            int soLuong = (int)txtSoLuong.Value;

                            capNhatSoLuongVatTu(maVatTu, soLuong);
                        }

                        /*TH3: chinh sua phieu nhap -> chang co gi co the chinh sua
                         * duoc -> chang can xu ly*/
                        /*TH4: chinh sua chi tiet phieu nhap - > thi chi can may dong lenh duoi la xong*/
                        undoList.Push(cauTruyVanHoanTac);
                        

                        this.bdsPhieuXuat.EndEdit();
                        this.bdsCTPX.EndEdit();
                        this.phieuXuatTableAdapter.Update(this.DS_SV1.PhieuXuat);
                        this.cTPXTableAdapter.Update(this.DS_SV1.CTPX);

                        this.btnThem.Enabled = true;
                        this.btnXoa.Enabled = true;
                        this.btnGhi.Enabled = true;

                        this.btnPhucHoi.Enabled = true;
                        this.btnReload.Enabled = true;
                        this.btnCheDo.Enabled = true;
                        this.btnThoat.Enabled = true;

                        this.phieuXuatGridControl.Enabled = true;
                        this.cTPXGridControl.Enabled = true;

                        this.txtSoLuong.Enabled = false;
                        this.txtDonGia.Enabled = false;
                        /*cập nhật lại trạng thái thêm mới cho chắc*/
                        dangThemMoi = false;
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        
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
            string cheDo = (btnCheDo.Links[0].Caption == "Phiếu Xuất") ? "Phiếu Xuất" : "Chi Tiết Phiếu Xuất";

            if (cheDo == "Phiếu Xuất")
            {
                drv = ((DataRowView)bdsPhieuXuat[bdsPhieuXuat.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Không xóa chi tiết phiếu xuất không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (bdsCTPX.Count > 0)
                {
                    MessageBox.Show("Không thể xóa vì có chi tiết phiếu xuất", "Thông báo", MessageBoxButtons.OK);
                    return;
                }


                DateTime ngay = ((DateTime)drv["NGAY"]);

                cauTruyVanHoanTac = "INSERT INTO DBO.PHIEUXUAT(MAPX, NGAY, HOTENKH, MANV, MAKHO) " +
                    "VALUES( '" + drv["MAPX"].ToString().Trim() + "', '" +
                    ngay.ToString("yyyy-MM-dd") + "', '" +
                    drv["HOTENKH"].ToString() + "', '" +
                    drv["MANV"].ToString() + "', '" +
                    drv["MAKHO"].ToString() + "')";
            }

            if (cheDo == "Chi Tiết Phiếu Xuất")
            {
                drv = ((DataRowView)bdsPhieuXuat[bdsPhieuXuat.Position]);
                String maNhanVien = drv["MANV"].ToString();
                if (Program.userName != maNhanVien)
                {
                    MessageBox.Show("Bạn không xóa chi tiết phiếu xuất không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                drv = ((DataRowView)bdsCTPX[bdsCTPX.Position]);
                cauTruyVanHoanTac = "INSERT INTO DBO.CTPX(MAPX, MAVT, SOLUONG, DONGIA) " +
                    "VALUES('" + drv["MAPX"].ToString().Trim() + "', '" +
                    drv["MAVT"].ToString().Trim() + "', " +
                    drv["SOLUONG"].ToString().Trim() + ", " +
                    drv["DONGIA"].ToString().Trim() + ")";
            }

            undoList.Push(cauTruyVanHoanTac);
            //Console.WriteLine("Line 825");
            //Console.WriteLine(cauTruyVanHoanTac);


            /*Step 2*/
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    /*Step 3*/
                    viTri = bds.Position;
                    if (cheDo == "Phiếu Xuất")
                    {
                        bdsPhieuXuat.RemoveCurrent();
                    }
                    if (cheDo == "Chi Tiết Phiếu Xuất")
                    {
                        bdsCTPX.RemoveCurrent();
                    }


                    this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuXuatTableAdapter.Update(this.DS_SV1.PhieuXuat);

                    this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTPXTableAdapter.Update(this.DS_SV1.CTPX);

                    //bdsPhieuNhap.Position = viTri;
                    /*Cap nhat lai do ben tren can tao cau truy van nen da dat dangThemMoi = true*/
                    dangThemMoi = false;
                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnPhucHoi.Enabled = true;
                }
                catch (Exception ex)
                {
                    /*Step 4*/
                    MessageBox.Show("Lỗi xóa . Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuXuatTableAdapter.Update(this.DS_SV1.PhieuXuat);

                    this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTPXTableAdapter.Update(this.DS_SV1.CTPX);
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

        private void btnChonVatTu_Click(object sender, EventArgs e)
        {
            FormChonVatTu form = new FormChonVatTu();
            form.ShowDialog();
            this.txtMaVT.Text = Program.maVatTuDuocChon;
        }
        private void btnChonKhoHang_Click(object sender, EventArgs e)
        {
            FormChonKhoHang form = new FormChonKhoHang();
            form.ShowDialog();

            this.txtMaKho.Text = Program.maKhoDuocChon;
        }

    }
}
