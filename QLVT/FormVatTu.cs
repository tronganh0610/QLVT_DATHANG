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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormVatTu : Form
    {
        
        int viTri = 0;
        
        bool dangThemMoi = false;

        String maChiNhanh = "";
        
        Stack undoList = new Stack();
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }
        public FormVatTu()
        {
            InitializeComponent();
        }

        private void vattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsVatTu.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS_SV1);

        }

        private void FormVatTu_Load(object sender, EventArgs e)
        {
            
            /*không kiểm tra khóa ngoại nữa*/
            DS_SV1.EnforceConstraints = false;

            this.CTPXTableAdapter.Connection.ConnectionString = Program.connstr;
            this.CTPXTableAdapter.Fill(this.DS_SV1.CTPX);

            this.CTPNTableAdapter.Connection.ConnectionString = Program.connstr;
            this.CTPNTableAdapter.Fill(this.DS_SV1.CTPN);

            this.CTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.CTDDHTableAdapter.Fill(this.DS_SV1.CTDDH);

            this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vattuTableAdapter.Fill(this.DS_SV1.Vattu);
            

            /*CONG TY chi xem du lieu*/
            if (Program.role == "CongTy")
            {
                
                this.btnThem.Enabled = false;
                this.btnXoa.Enabled = false;
                this.btnGhi.Enabled = false;

                this.btnPhucHoi.Enabled = false;
                this.btnReload.Enabled = true;
                
                this.btnThoat.Enabled = true;
                this.btnInDSNV.Enabled = true;
                this.panelControl2.Enabled = false;

            }

            // CHI NHANH & USER co the xem - xoa - sua du lieu 
            if (Program.role == "ChiNhanh" || Program.role == "User")
            {
                
                this.btnThem.Enabled = true;
                this.btnXoa.Enabled = true;
                this.btnGhi.Enabled = true;
                this.btnPhucHoi.Enabled = true;
                this.btnReload.Enabled = true;
                
                this.btnThoat.Enabled = true;
                this.panelControl2.Enabled = true;
             
            }
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
         
            /*lấy vị trí hiện tại của con trỏ*/
            viTri = bdsVatTu.Position;
            this.panelControl2.Enabled = true;
            dangThemMoi = true;
       
            /*AddNew tự động nhảy xuống cuối thêm 1 dòng mới*/
            bdsVatTu.AddNew();
            txtSLT.Value = 1;
   
            this.txtMAVT.Enabled = true;
            this.btnThem.Enabled = false;
            this.btnXoa.Enabled = false;
            this.btnGhi.Enabled = true;

            this.btnPhucHoi.Enabled = true;
            this.btnReload.Enabled = false;
            this.btnThoat.Enabled = false;


            this.vattuGridControl.Enabled = false;
            this.panelControl2.Enabled = true;
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            if (dangThemMoi == true && this.btnThem.Enabled == false)
            {
                dangThemMoi = false;

                this.txtMAVT.Enabled = false;
                this.btnThem.Enabled = true;
                this.btnXoa.Enabled = true;
                this.btnGhi.Enabled = true;

                this.btnPhucHoi.Enabled = false;
                this.btnReload.Enabled = true;
                this.btnThoat.Enabled = true;


                this.vattuGridControl.Enabled = true;
                this.panelControl2.Enabled = true;

                bdsVatTu.CancelEdit();
                /*xoa dong hien tai*/
                bdsVatTu.RemoveCurrent();
                /* trở về lúc đầu con trỏ đang đứng*/
                bdsVatTu.Position = viTri;
                return;
            }

            
            if (undoList.Count == 0)
            {
                MessageBox.Show("Không còn thao tác nào để khôi phục", "Thông báo", MessageBoxButtons.OK);
                btnPhucHoi.Enabled = false;
                return;
            }

            
            bdsVatTu.CancelEdit();
            String cauTruyVanHoanTac = undoList.Pop().ToString();
            
            int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);
            this.vattuTableAdapter.Fill(this.DS_SV1.Vattu);
        }
        private bool kiemTraDuLieuDauVao()
        {
            /*Kiem tra txtMAVT*/
            if (txtMAVT.Text == "")
            {
                MessageBox.Show("Không bỏ trống mã vật tư", "Thông báo", MessageBoxButtons.OK);
                txtMAVT.Focus();
                return false;
            }

            if (Regex.IsMatch(txtMAVT.Text, @"^[a-zA-Z0-9]+$") == false)
            {
                MessageBox.Show("Mã vật tư chỉ có chữ cái và số", "Thông báo", MessageBoxButtons.OK);
                txtMAVT.Focus();
                return false;
            }

            if (txtMAVT.Text.Length > 4)
            {
                MessageBox.Show("Mã vật tư không quá 4 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtMAVT.Focus();
                return false;
            }
            /*Kiem tra txtTENVT*/
            if (txtTenVT.Text == "")
            {
                MessageBox.Show("Không bỏ trống tên vật tư", "Thông báo", MessageBoxButtons.OK);
                txtTenVT.Focus();
                return false;
            }

            /*if (Regex.IsMatch(txtTenVT.Text, @"^[a-zA-Z0-9 ]+$") == false)
            {
                MessageBox.Show("Tên vật tư chỉ chấp nhận chữ, số và khoảng trắng", "Thông báo", MessageBoxButtons.OK);
                txtTenVT.Focus();
                return false;
            }*/

            if (txtTenVT.Text.Length > 30)
            {
                MessageBox.Show("Tên vật tư không quá 30 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtTenVT.Focus();
                return false;
            }
            /*Kiem tra txtDONVIVATTU*/
            if (txtDVT.Text == "")
            {
                MessageBox.Show("Không bỏ trống đơn vị tính", "Thông báo", MessageBoxButtons.OK);
                txtDVT.Focus();
                return false;
            }

            /*if (Regex.IsMatch(txtDVT.Text, @"^[a-zA-Z ]+$") == false)
            {
                MessageBox.Show("Đơn vị vật tư chỉ có chữ cái", "Thông báo", MessageBoxButtons.OK);
                txtDVT.Focus();
                return false;
            }*/

            if (txtDVT.Text.Length > 15)
            {
                MessageBox.Show("Đơn vị vật tự không quá 15 kí tự", "Thông báo", MessageBoxButtons.OK);
                txtDVT.Focus();
                return false;
            }
            /*Kiem tra txtSOLUONGTON*/
            if (txtSLT.Value < 0)
            {
                MessageBox.Show("Sô lượng tồn phải ít nhất bằng 0", "Thông báo", MessageBoxButtons.OK);
                txtSLT.Focus();
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
            
            String maVatTu = txtMAVT.Text.Trim();// Trim() de loai bo khoang trang thua
            DataRowView drv = ((DataRowView)bdsVatTu[bdsVatTu.Position]);
            String tenVatTu = drv["TENVT"].ToString();
            String donViTinh = drv["DVT"].ToString();
            String soLuongTon = (drv["SOLUONGTON"].ToString());

            String cauTruyVan =
                    "DECLARE	@result int " +
                    "EXEC @result = sp_KiemTraMaVatTu '" + maVatTu + "' " +
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
                
                return;
            }
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            
            Program.myReader.Close();



            /*Step 2*/
            int viTriConTro = bdsVatTu.Position;
            int viTriMaVatTu = bdsVatTu.Find("MAVT", txtMAVT.Text);

            if (result == 1 && viTriConTro != viTriMaVatTu)
            {
                MessageBox.Show("Mã vật tư này đã được sử dụng !", "Thông báo", MessageBoxButtons.OK);
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
                        /*bật các nút về ban đầu*/
                        btnThem.Enabled = true;
                        btnXoa.Enabled = true;
                        btnGhi.Enabled = true;
                        btnPhucHoi.Enabled = true;
                        btnReload.Enabled = true;
                        //btnCHUYENCHINHANH.Enabled = true;
                        btnThoat.Enabled = true;
                        this.vattuGridControl.Enabled = true;

                        /*lưu 1 câu truy vấn để hoàn tác yêu cầu*/
                        String cauTruyVanHoanTac = "";
                        /*trước khi ấn btnGHI là btnTHEM*/
                        if (dangThemMoi == true)
                        {
                            cauTruyVanHoanTac = "" +
                                "DELETE DBO.VATTU " +
                                "WHERE MAVT = '" + txtMAVT.Text.Trim() + "'";
                        }
                        /*trước khi ấn btnGHI là sửa thông tin nhân viên*/
                        else
                        {
                            cauTruyVanHoanTac =
                                "UPDATE DBO.VATTU " +
                                "SET " +
                                "TENVT = '" + tenVatTu + "'," +
                                "DVT = '" + donViTinh + "'," +
                                "SOLUONGTON = " + soLuongTon + " " +
                                "WHERE MAVT = '" + maVatTu + "'";
                        }
                        //Console.WriteLine("CAU TRUY VAN HOAN TAC");
                        //Console.WriteLine(cauTruyVanHoanTac);

                        
                        undoList.Push(cauTruyVanHoanTac);

                        this.bdsVatTu.EndEdit();
                        this.vattuTableAdapter.Update(this.DS_SV1.Vattu);
                        
                        dangThemMoi = false;
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        
                        bdsVatTu.RemoveCurrent();
                        MessageBox.Show("Tên vật tư có thể đã được dùng !\n\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // do du lieu moi tu dataSet vao gridControl NHANVIEN
                this.vattuTableAdapter.Fill(this.DS_SV1.Vattu);
                this.vattuGridControl.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Làm mới" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }
        

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            /*Step 1*/
            if (bdsVatTu.Count == 0)
            {
                btnXoa.Enabled = false;
            }

         
            String maVatTu = txtMAVT.Text.Trim();// Trim() de loai bo khoang trang thua

            string cauTruyVanHoanTac =
            "INSERT INTO DBO.VATTU( MAVT,TENVT,DVT,SOLUONGTON) " +
            " VALUES( '" + txtMAVT.Text + "','" +
                        txtTenVT.Text + "','" +
                        txtDVT.Text + "', " +
                        txtSLT.Value + " ) ";

            undoList.Push(cauTruyVanHoanTac);

            /*Step 2*/
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    /*Step 3*/
                    viTri = bdsVatTu.Position;
                    bdsVatTu.RemoveCurrent();

                    this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.vattuTableAdapter.Update(this.DS_SV1.Vattu);

                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnPhucHoi.Enabled = true;
                }
                catch (Exception ex)
                {
                    /*Step 4*/
                    MessageBox.Show("Lỗi xóa vật tư. Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.vattuTableAdapter.Fill(this.DS_SV1.Vattu);
                    
                    bdsVatTu.Position = viTri;
                    
                    return;
                }
            }
            else
            {
                
                undoList.Pop();
            }
        }

        private void btnInDSNV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = this.CheckExists(typeof(FormDanhSachVatTu));
            if (f != null)
            {
                f.Activate();
            }
            else
            {
                FormDanhSachVatTu form = new FormDanhSachVatTu();

                form.ShowDialog();
            }
        }

      
    }
}
