using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class FormDangNhap : DevExpress.XtraEditors.XtraForm
    {
        private SqlConnection connPublisher = new SqlConnection();

        

        public FormDangNhap()
        {
            InitializeComponent();
        }
        private void layDanhSachPhanManh(String cmd)
        {
            if (connPublisher.State == ConnectionState.Closed)
            {
                connPublisher.Open();
            }
            DataTable dt = new DataTable();
            // adapter để đưa dữ liệu từ view sang database
            SqlDataAdapter da = new SqlDataAdapter(cmd, connPublisher);
            // dùng adapter thì mới đổ vào data base 
            da.Fill(dt);
            connPublisher.Close();
            Program.bindingSource.DataSource = dt;

            cmbCHINHANH.DataSource = Program.bindingSource;
            cmbCHINHANH.DisplayMember = "TENCN";
            cmbCHINHANH.ValueMember = "TENSERVER";
        }
        private int KetNoiDatabaseGoc()
        {
            if (connPublisher != null && connPublisher.State == ConnectionState.Open)
                connPublisher.Close();
            try
            {
                connPublisher.ConnectionString = Program.connstrPublisher;
                connPublisher.Open();
                return 1;
            }

            catch (Exception e)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu.\nBạn xem lại user name và password.\n " + e.Message, "", MessageBoxButtons.OK);
                return 0;
            }
        }

        
        
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void btnDANGNHAP_Click(object sender, EventArgs e)
        {
            // 1) Bắt lỗi Tài Khoản + Mật Khẩu có trống không 
            if (txtTAIKHOAN.Text.Trim() == "" || txtMATKHAU.Text.Trim() == "")
            {
                MessageBox.Show("Không thể để trống tài khoản hoặc mật khẩu", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            //2) Gán loginName và loginPassword với tài khoản được nhập
            Program.loginName = txtTAIKHOAN.Text.Trim();
            Program.loginPassword = txtMATKHAU.Text.Trim();
            if (Program.KetNoi() == 0) return;

            //3) Cập nhập currentLogin và currentPassword
            Program.brand = cmbCHINHANH.SelectedIndex;
            Program.currentLogin = Program.loginName;
            Program.currentPassword = Program.loginPassword;

            //4) chạy procedure sp_DangNhap để lấy thông tin người dùng 
            String statement = "exec sp_Login " + Program.loginName ;// exec sp_DangNhap 'LT'
            Program.myReader = Program.ExecSqlDataReader(statement);
            if (Program.myReader == null)
            {
                return;
            }
            // đọc 1 dòng của myReader - vì kết quả chỉ có 1 dòng
            Program.myReader.Read();


            // 5) Gán giá trị MANV, HỌ Tên, Nhóm vai trò ở góc màn hình
            Program.userName = Program.myReader.GetString(0); // lấy user name
            if (Convert.IsDBNull(Program.userName))
            {
                MessageBox.Show("Tài khoản này không có quyền truy cập \n Hãy thử tài khoản khác", "Thông Báo", MessageBoxButtons.OK);
            }

            Program.staff = Program.myReader.GetString(1);
            Program.role = Program.myReader.GetString(2);

            Program.myReader.Close();
            Program.conn.Close();

            Program.formChinh.MANHANVIEN.Text = "MÃ NHÂN VIÊN: " + Program.userName;
            Program.formChinh.HOTEN.Text = "HỌ TÊN: " + Program.staff;
            Program.formChinh.NHOM.Text = "VAI TRÒ: " + Program.role;
            
            //6 Ẩn form hiện tại và hiện các chức năng ở form chính
            this.Visible = false;
            Program.formChinh.enableButtons();
        }
        private void bthTHOAT_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.formChinh.Close();
        }

        private void txtMATKHAU_TextChanged(object sender, EventArgs e)
        {

        }


        private void FormDangNhap_Load_1(object sender, EventArgs e)
        {
            
            this.nhanVienTableAdapter.Fill(this.DS_SV1.NhanVien);
            
            this.view_DanhSachPhanManhTableAdapter.Fill(this.DS_SV1.view_DanhSachPhanManh);
            if (KetNoiDatabaseGoc() == 0)
                return;

             
            txtTAIKHOAN.Text = "TH"; 
            txtMATKHAU.Text = "0610";
            layDanhSachPhanManh("select * from LINK2.QLVT_DATHANG.dbo.view_DanhSachPhanManh");
            cmbCHINHANH.SelectedIndex = 0;
            cmbCHINHANH.SelectedIndex = 1;
        }

        private void btnTHOAT_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.formChinh.Close();

        }
        
        private void cmbCHINHANH_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                Program.serverName = cmbCHINHANH.SelectedValue.ToString();
            }
            catch (Exception)
            {

            }
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bdsDanhSachPhanManh_CurrentChanged(object sender, EventArgs e)
        {

        }
    }
}
