using QLVT.FormDanhSach;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormNhanVien : Form
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

        String maChiNhanh = "";
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

        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }
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

        public FormNhanVien()
        {
            InitializeComponent();
        }
        private void nhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsSV1.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS_SV1);

        }

        /*
         *Step 1: tat kiem tra khoa ngoai & do du lieu vao form
         *Step 2: lay du lieu dang nhap tu form dang nhap
         *Step 3: bat nut chuc nang theo vai tro khi dang nhap
         */
        public void FormNhanVien_Load(object sender, EventArgs e)
        {
            
            /*Step 1*/
            /*không kiểm tra khóa ngoại nữa*/
            DS_SV1.EnforceConstraints = false;
            
            // TODO: This line of code loads data into the 'DS_SV1.DatHang' table. You can move, or remove it, as needed.
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.DS_SV1.NhanVien);

            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.DS_SV1.DatHang);

            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.DS_SV1.PhieuNhap);

            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.DS_SV1.PhieuXuat);

            /*van con ton tai loi chua sua duoc*/
            maChiNhanh = ((DataRowView)bdsSV1[0])["MACN"].ToString();

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
                //this.btnCHUYENCHINHANH.Enabled = false;
                this.btnThoat.Enabled = true;
                
                this.panelControl2.Enabled = false;
                
            }

            /* CHI NHANH & USER co the xem - xoa - sua du lieu nhung khong the 
             chuyen sang chi nhanh khac*/
            if (Program.role == "ChiNhanh" || Program.role == "User")
            {
                cmbCHINHANH.Enabled = false;

                this.btnThem.Enabled = true;
                this.btnXoa.Enabled = true;
                this.btnGhi.Enabled = true;
                this.btnPhucHoi.Enabled = true;
                this.btnReload.Enabled = true;
                //this.btnCHUYENCHINHANH.Enabled = true;
                this.btnThoat.Enabled = true;
                this.panelControl2.Enabled = true;
                this.txtMANV.Enabled = true;
            }

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
                /*Do du lieu tu dataSet vao grid Control*/
                this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.nhanVienTableAdapter.Fill(this.DS_SV1.NhanVien);

                this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.datHangTableAdapter.Fill(this.DS_SV1.DatHang);

                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.DS_SV1.PhieuNhap);

                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuXuatTableAdapter.Fill(this.DS_SV1.PhieuXuat);
                
   
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtMANV_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtMANV_TextChanged(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }
        /*********************************************************************
         * bdsNhanVien.Position - vitri phuc vu cho btnPhucHoi. Gia su, co 5 nhan vien, con tro chuot
         * dang dung o vi tri nhan vien thu 2 thi chung ta an nut THEM
         * nhung neu chon btnPhucHoi, con tro chuot phai quay lai vi 
         * tri nhan vien thu 2, thay vi o vi tri duoi cung - tuc nhan vien so 5
         * 
         * neu nhap chu cho txtMANV thi se khong chuyen sang cac o khac duoc nua - bat buoc ghi so
         * 
         * Step 1: Kich hoat panel Nhap lieu & lay vi tri cua nhan vien hien tai
         * dat dangThemMoi = true
         * Step 2: gui lenh them moi toi bdsNHANVIEN - tu dong lay maChiNhanh - bo trong dteNGAYSINH
         * Step 3: vo hieu hoa cac nut chuc nang & gridControl - chi btnGHI & btnPhucHoi moi duoc hoat dong
         *********************************************************************/
        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 1*/
            /*lấy vị trí hiện tại của con trỏ*/
            viTri = bdsSV1.Position;
            this.panelControl2.Enabled = true;
            dangThemMoi = true;


            /*Step 2*/
            /*AddNew tự động nhảy xuống cuối thêm 1 dòng mới*/
            bdsSV1.AddNew();
            txtMACN.Text = maChiNhanh;
            deNGAYSINH.EditValue = "2000-05-01";
            txtLUONG.Value = 4000000;// dat san muc luong toi thieu


            /*Step 3*/
            this.txtMANV.Enabled = true;
            this.btnThem.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnGhi.Enabled = true;

            this.btnPhucHoi.Enabled = true;
            this.btnReload.Enabled = false;
            //this.btnCHUYENCHINHANH.Enabled = false;
            this.btnThoat.Enabled = true;
            this.checkBoxTTXoa.Checked = false;
            this.nhanVienGridControl.Enabled = false;
            this.panelControl2.Enabled = true;
        }

        private void nhanVienGridControl_Click(object sender, EventArgs e)
        {
           
        }
        private bool kiemTraDuLieuDauVao()
        {
            /*kiem tra txtMANV*/
            if (txtMANV.Text == "")
            {
                MessageBox.Show("Không bỏ trống mã nhân viên", "Thông báo", MessageBoxButtons.OK);
                txtMANV.Focus();
                return false;
            }

            if (Regex.IsMatch(txtMANV.Text, @"^[a-zA-Z0-9]+$") == false)
            {
                MessageBox.Show("Mã nhân viên chỉ chấp nhận số", "Thông báo", MessageBoxButtons.OK);
                txtMANV.Focus();
                return false;
            }
            /*kiem tra txtHO*/
            if (txtHO.Text == "")
            {
                MessageBox.Show("Không bỏ trống họ và tên", "Thông báo", MessageBoxButtons.OK);
                txtHO.Focus();
                return false;
            }
            //"^[0-9A-Za-z ]+$"
            if (Regex.IsMatch(txtHO.Text, @"^[A-Za-z ]+$") == false)
            {
                MessageBox.Show("Họ của người chỉ có chữ cái và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                txtHO.Focus();
                return false;
            }
            if (txtHO.Text.Length > 40)
            {
                MessageBox.Show("Họ không thể lớn hơn 40 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtHO.Focus();
                return false;
            }
            /*kiem tra txtTEN*/
            if (txtTEN.Text == "")
            {
                MessageBox.Show("Không bỏ trống họ và tên", "Thông báo", MessageBoxButtons.OK);
                txtTEN.Focus();
                return false;
            }

            if (Regex.IsMatch(txtTEN.Text, @"^[a-zA-Z ]+$") == false)
            {
                MessageBox.Show("Tên người chỉ có chữ cái và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                txtTEN.Focus();
                return false;
            }

            if (txtTEN.Text.Length > 10)
            {
                MessageBox.Show("Tên không thể lớn hơn 10 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtTEN.Focus();
                return false;
            }
            /*kiem tra txtDIACHI*/
            if (txtDIACHI.Text == "")
            {
                MessageBox.Show("Không bỏ trống địa chỉ", "Thông báo", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return false;
            }

            if (Regex.IsMatch(txtDIACHI.Text, @"^[a-zA-Z0-9, ]+$") == false)
            {
                MessageBox.Show("Địa chỉ chỉ chấp nhận chữ cái, số và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return false;
            }

            if (txtDIACHI.Text.Length > 100)
            {
                MessageBox.Show("Không bỏ trống địa chỉ", "Thông báo", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return false;
            }
            /*kiem tra dteNGAYSINH va txtLUONG*/
            if (CalculateAge(deNGAYSINH.DateTime) < 18)
            {
                MessageBox.Show("Nhân viên chưa đủ 18 tuổi", "Thông báo", MessageBoxButtons.OK);
                deNGAYSINH.Focus();
                return false;
            }

            if (txtLUONG.Value < 4000000 || txtLUONG.Value == 0)
            {
                MessageBox.Show("Mức lương không thể bỏ trống & tối thiểu 4.000.000 đồng", "Thông báo", MessageBoxButtons.OK);
                txtLUONG.Focus();
                return false;
            }
            return true;
        }
        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /* Step 0 */
            bool ketQua = kiemTraDuLieuDauVao();
            if (ketQua == false)
                return;

            /*Step 1*/
            /*Lay du lieu truoc khi chon btnGHI - phuc vu btnHOANTAC - sau khi OK thi da la du lieu moi*/
            String maNhanVien = txtMANV.Text.Trim();// Trim() de loai bo khoang trang thua
            DataRowView drv = ((DataRowView)bdsSV1[bdsSV1.Position]);
            String ho = drv["HO"].ToString();
            String ten = drv["TEN"].ToString();

            String diaChi = drv["DIACHI"].ToString();

            String ngaySinh = deNGAYSINH.Text.Trim();

            string luong = txtLUONG.Text.Trim();
            String maChiNhanh = drv["MACN"].ToString();
            int trangThai = (checkBoxTTXoa.Checked == true) ? 1 : 0;


            /*declare @returnedResult int
              exec @returnedResult = sp_TraCuu_KiemTraMaNhanVien '20'
              select @returnedResult*/
            String cauTruyVan =
                    "DECLARE	@result int " +
                    "EXEC @result = [dbo].[sp_TraCuu_KiemTraMaNhanVien] '" +
                    maNhanVien + "' " +
                    "SELECT 'Value' = @result"; ;
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



            /*Step 2*/
            int viTriConTro = bdsSV1.Position;
            int viTriMaNhanVien = bdsSV1.Find("MANV", txtMANV.Text);

            if (result == 1 && viTriConTro != viTriMaNhanVien)
            {
                MessageBox.Show("Mã nhân viên này đã được sử dụng !", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            else/*them moi | sua nhan vien*/
            {
                DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu vào cơ sở dữ liệu ?", "Thông báo",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        /*bật các nút về ban đầu*/
                        btnThem.Enabled = true;
                        btnXoa.Enabled = true;
                        btnGhi.Enabled = true;
                        btnPhucHoi.Enabled = true;

                        btnReload.Enabled = true;
                        //btnCHUYENCHINHANH.Enabled = true;
                        btnThoat.Enabled = true;
                        this.txtMANV.Enabled = true;
                        this.bdsSV1.EndEdit();
                        this.nhanVienTableAdapter.Update(this.DS_SV1.NhanVien);
                        this.nhanVienGridControl.Enabled = true;

                        /*lưu 1 câu truy vấn để hoàn tác yêu cầu*/
                        String cauTruyVanHoanTac = "";
                        /*trước khi ấn btnGHI là btnTHEM*/
                        if (dangThemMoi == true)
                        {
                            cauTruyVanHoanTac = "" +
                                "DELETE DBO.NHANVIEN " +
                                "WHERE MANV = " + txtMANV.Text.Trim();
                        }
                        /*trước khi ấn btnGHI là sửa thông tin nhân viên*/
                        else
                        {


                            cauTruyVanHoanTac =
                                "UPDATE DBO.NhanVien " +
                                "SET " +
                                "HO = '" + ho + "'," +
                                "TEN = '" + ten + "'," +
                                "DIACHI = '" + diaChi + "'," +
                                "NGAYSINH = CAST('" + ngaySinh + "' AS DATETIME)," +
                                "LUONG = '" + luong + "'," +
                                "TrangThaiXoa = " + trangThai + " " +
                                "WHERE MANV = '" + maNhanVien + "'";
                        }
                        Console.WriteLine(cauTruyVanHoanTac);

                        /*Đưa câu truy vấn hoàn tác vào undoList 
                         * để nếu chẳng may người dùng ấn hoàn tác thì quất luôn*/
                        undoList.Push(cauTruyVanHoanTac);
                        /*cập nhật lại trạng thái thêm mới cho chắc*/
                        dangThemMoi = false;
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {

                        bdsSV1.RemoveCurrent();
                        MessageBox.Show("Thất bại. Vui lòng kiểm tra lại!\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String maNV = ((DataRowView)bdsSV1[bdsSV1.Position])["MANV"].ToString();
            /*Step 1*/

            // khong cho xoa nguoi dang dang nhap ke ca nguoi do khong co don hang - phieu nhap - phieu xuat
            if (maNV == Program.userName)
            {
                MessageBox.Show("Không thể xóa chính tài khoản đang đăng nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (bdsSV1.Count == 0 )
            {
                btnXoa.Enabled = false;
            }

            /*if (bdsDatHang.Count > 0) 
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (bdsPhieuNhap.Count > 0)
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (bdsPhieuXuat.Count > 0)
            {
                MessageBox.Show("Không thể xóa nhân viên này vì đã lập phiếu xuất", "Thông báo", MessageBoxButtons.OK);
                return;
            }*/

            /* Phần này phục vụ tính năng hoàn tác
                    * Đưa câu truy vấn hoàn tác vào undoList 
                    * để nếu chẳng may người dùng ấn hoàn tác thì quất luôn*/
            int trangThai = (checkBoxTTXoa.Checked == true) ? 1 : 0;
            /*Lấy ngày sinh trong grid view*/
            String ngaySinh = deNGAYSINH.Text.Trim();


            string cauTruyVanHoanTac =
                string.Format("INSERT INTO DBO.NHANVIEN( MANV,HO,TEN,DIACHI,NGAYSINH,LUONG,MACN)" +
            "VALUES({0},'{1}','{2}','{3}',CAST({4} AS DATETIME), {5},'{6}')", txtMANV.Text, txtHO.Text, txtTEN.Text, txtDIACHI.Text, ngaySinh, txtLUONG.Value, txtMACN.Text.Trim());

            Console.WriteLine(cauTruyVanHoanTac);
            undoList.Push(cauTruyVanHoanTac);


            /*Step 2*/
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    /*Step 3*/
                    viTri = bdsSV1.Position;
                    bdsSV1.RemoveCurrent();

                    this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nhanVienTableAdapter.Update(this.DS_SV1.NhanVien);

                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnPhucHoi.Enabled = true;
                }
                catch (Exception ex)
                {
                    /*Step 4*/
                    MessageBox.Show("Lỗi xóa nhân viên. Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.nhanVienTableAdapter.Fill(this.DS_SV1.NhanVien);
                    // tro ve vi tri cua nhan vien dang bi loi
                    bdsSV1.Position = viTri;
                    //bdsNhanVien.Position = bdsNhanVien.Find("MANV", manv);
                    return;
                }
            }
            else
            {
                undoList.Pop();
            }
        }

        private void checkBoxTTXoa_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void bdsNhanVien1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void nhanVienGridControl_Click_1(object sender, EventArgs e)
        {

        }

        /**********************************************************************
         * moi lan nhan btnPhucHoi thi nen nhan them btnLAMMOI de 
         * tranh bi loi khi an btnTHEM lan nua
         * 
         * statement: chua cau y nghia chuc nang ngay truoc khi an btnPhucHoi.
         * Vi du: statement = INSERT | DELETE | CHANGEBRAND
         * 
         * bdsSV1.CancelEdit() - phuc hoi lai du lieu neu chua an btnGHI
         * Step 0: trường hợp đã ấn btnTHEM nhưng chưa ấn btnGHI
         * Step 1: kiểm tra undoList có trông hay không ?
         * Step 2: Neu undoList khong trống thì lấy ra khôi phục
         *********************************************************************/
        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /* Step 0 - */
            if ((dangThemMoi == true && this.btnThem.Enabled == false)  )
            {
                dangThemMoi = false;

                this.txtMANV.Enabled = true;
                this.btnThem.Enabled = true;
                this.btnXoa.Enabled = true;
                this.btnGhi.Enabled = true;
                
                this.btnPhucHoi.Enabled = true;
                this.btnReload.Enabled = true;
                //this.btnCHUYENCHINHANH.Enabled = true;
                this.btnThoat.Enabled = true;
                this.checkBoxTTXoa.Checked = false;

                this.nhanVienGridControl.Enabled = true;
                this.panelControl2.Enabled = true;

                bdsSV1.CancelEdit();
                /*xoa dong hien tai*/
                bdsSV1.RemoveCurrent();
                /* trở về lúc đầu con trỏ đang đứng*/
                bdsSV1.Position = viTri;
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
            bdsSV1.CancelEdit();
            String cauTruyVanHoanTac = undoList.Pop().ToString();
            //Console.WriteLine(cauTruyVanHoanTac);

            /*Step 2.1*/
            /*if (cauTruyVanHoanTac.Contains("sp_ChuyenChiNhanh"))
            {
                try
                {
                    String chiNhanhHienTai = Program.serverName;
                    String chiNhanhChuyenToi = Program.serverNameLeft;

                    Program.serverName = chiNhanhChuyenToi;
                    Program.loginName = Program.remoteLogin;
                    Program.loginPassword = Program.remotePassword;

                    if (Program.KetNoi() == 0)
                    {
                        return;
                    }


                    int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);

                    MessageBox.Show("Chuyển nhân viên trở lại thành công", "Thông báo", MessageBoxButtons.OK);
                    Program.serverName = chiNhanhHienTai;
                    Program.loginName = Program.currentLogin;
                    Program.loginPassword = Program.currentPassword;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chuyển nhân viên thất bại \n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    return;
                }

            }
            *//*Step 2.2*//*
            else
            {
                if (Program.KetNoi() == 0)
                {
                    return;
                }
                int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);

            }*/
            this.nhanVienTableAdapter.Fill(this.DS_SV1.NhanVien);




            /*
            bdsNhanVien.CancelEdit();
            String cauTruyVanHoanTac = undoList.Pop().ToString();
            Console.WriteLine(cauTruyVanHoanTac);
            int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);
            this.nhanVienTableAdapter.Fill(this.dataSet.NhanVien);
             */
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // do du lieu moi tu dataSet vao gridControl NHANVIEN
                this.nhanVienTableAdapter.Fill(this.DS_SV1.NhanVien);
                this.nhanVienGridControl.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Làm mới" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }

        private void deNGAYSINH_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void nhanVienGridControl_Click_2(object sender, EventArgs e)
        {

        }

        private void btnHieuChinh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mANVLabel_Click(object sender, EventArgs e)
        {

        }

        private void hOLabel_Click(object sender, EventArgs e)
        {

        }

        private void tENLabel_Click(object sender, EventArgs e)
        {

        }

        private void mACNLabel_Click(object sender, EventArgs e)
        {

        }

        private void nGAYSINHLabel1_Click(object sender, EventArgs e)
        {

        }

        private void dIACHILabel_Click(object sender, EventArgs e)
        {

        }

        private void lUONGLabel1_Click(object sender, EventArgs e)
        {

        }

        private void trangThaiXoaLabel1_Click(object sender, EventArgs e)
        {

        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxTTXoa_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void bdsSV1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void txtLUONG_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void barDockControl1_Click(object sender, EventArgs e)
        {

        }

        private void barDockControl2_Click(object sender, EventArgs e)
        {

        }

        private void barDockControl3_Click(object sender, EventArgs e)
        {

        }

        private void barDockControl4_Click(object sender, EventArgs e)
        {

        }

        private void txtDIACHI_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void deNGAYSINH_EditValueChanged_1(object sender, EventArgs e)
        {

        }

        private void txtMACN_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTEN_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtHO_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtMANV_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void bdsPhieuNhap_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void bdsPhieuXuat_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void bdsDatHang_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void vattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
           

        }

        private void nhanVienGridControl_Click_3(object sender, EventArgs e)
        {

        }

        private void btnInDSNV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormDanhSachNhanVien));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormDanhSachNhanVien form = new FormDanhSachNhanVien();
                
                form.ShowDialog();
            }
        }

        private void btnHoatDongNhanVien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormDanhSachHoatDongNhanVien));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
               
                DataRowView drv = ((DataRowView)(bdsSV1.Current));
                string maNhanVien = drv["MANV"].ToString().Trim();
                string ho = drv["HO"].ToString().Trim();
                string ten = drv["TEN"].ToString().Trim();
                string hoTen = ho + " " + ten;

                Program.maNhanVienDuocChon = maNhanVien;
                Program.hoTen = hoTen;


                FormDanhSachHoatDongNhanVien form = new FormDanhSachHoatDongNhanVien();
                form.ShowDialog();
            }
            
            
            

            
        }
    }
}
