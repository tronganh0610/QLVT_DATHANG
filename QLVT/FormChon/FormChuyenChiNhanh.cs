using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.FormChon
{
    public partial class FormChuyenChiNhanh : Form
    {
        string chuyenchinhanh = "";
        public FormChuyenChiNhanh()
        {
            InitializeComponent();
        }

        private void FormChuyenChiNhanh_Load(object sender, EventArgs e)
        {
            txtHoTen.Text = Program.hoTen;
            txtMaNV.Text = Program.maNhanVienDuocChon;
            txtChiNhanh.Text = Program.Macn;
            if (txtChiNhanh.Text == "CN1")
            {
                btnCN1.Enabled = false;
            }
            if (txtChiNhanh.Text == "CN2")
            {
                btnCn2.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if ((btnCN1.Checked == false)  && (btnCn2.Checked == false) )
            {
                MessageBox.Show("Vui lòng chọn chi nhánh", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if ((btnCN1.Checked == true))
            {
                chuyenchinhanh = "CN1";
                
            }
            if ((btnCn2.Checked == true))
            {
                chuyenchinhanh = "CN2";
                
            }
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn chuyển nhân viên này đi ?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.OK)
            {
                String cauTruyVan =
                    " EXEC [dbo].[sp_ChuyenChiNhanh] " +

                          " @MANV = '" + Program.maNhanVienDuocChon + "',  " +

                          " @MACN = '" + chuyenchinhanh + "' ";

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
            
            Program.myReader.Close();
            }

            this.Dispose();
        }
    }
}
