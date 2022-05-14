using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT.FormChon
{
    public partial class FormChonKhoHang : Form
    {
        public FormChonKhoHang()
        {
            InitializeComponent();
        }

        private void khoBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsKho.EndEdit();
            this.tableAdapterManager.UpdateAll(this.DS_SV1);

        }

        private void FormChonDonHang_Load(object sender, EventArgs e)
        {
            /*không kiểm tra khóa ngoại nữa*/
            DS_SV1.EnforceConstraints = false;
            this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
            // TODO: This line of code loads data into the 'dS_SV1.Kho' table. You can move, or remove it, as needed.
            this.khoTableAdapter.Fill(this.DS_SV1.Kho);

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            string maKhoHang = ((DataRowView)bdsKho.Current)["MAKHO"].ToString();

            /*Cach nay phai tuy bien ban moi chay duoc*/
            //Program.formDonDatHang.txtMaKho.Text = maKhoHang;
            Program.maKhoDuocChon = maKhoHang;
            this.Close();
        }
    }
}
