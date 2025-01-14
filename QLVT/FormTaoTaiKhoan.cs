﻿using System;
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
    public partial class FormTaoTaiKhoan : Form
    {
        private string tenDangNhap = "";
        private string matKhau = "";
        private string maNhanVien = "";
        private string vaiTro = "";
        public FormTaoTaiKhoan()
        {
            InitializeComponent();
        }

        private void FormTaoTaiKhoan_Load(object sender, EventArgs e)
        {
            if (Program.role == "CongTy")
            {
                btnChiNhanh.Enabled = false;
                btnUser.Enabled= false;

            }
            if (Program.role == "ChiNhanh")
            {
                btnCongTy.Enabled = false;
            }

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private bool kiemTraDuLieuDauVao()
        {
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Thiếu Mã Nhân Viên", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            if (Regex.IsMatch(txtMaNV.Text, @"^[a-zA-Z0-9]+$") == false)
            {
                MessageBox.Show("Mã nhân viên chỉ chấp nhận số", "Thông báo", MessageBoxButtons.OK);
                txtMaNV.Focus();
                return false;
            }
            if (txtLogin.Text == "")
            {
                MessageBox.Show("Thiếu Tên Đăng Nhập", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            if(Regex.IsMatch(txtLogin.Text, @"^[a-zA-Z0-9]+$") == false)
            {
                MessageBox.Show("Mã nhân viên chỉ chấp nhận số", "Thông báo", MessageBoxButtons.OK);
                txtLogin.Focus();
                return false;
            }
            if (txtPassword.Text == "")
            {
                MessageBox.Show("Thiếu Mật Khẩu", "Thông báo", MessageBoxButtons.OK);
                return false;
            }
            if(Regex.IsMatch(txtPassword.Text, @"^[a-zA-Z0-9]+$") == false)
            {
                MessageBox.Show("Mã nhân viên chỉ chấp nhận số", "Thông báo", MessageBoxButtons.OK);
                txtPassword.Focus();
                return false;
            }
            if (btnCongTy.Checked == false && btnChiNhanh.Checked == false && btnUser.Checked == false)
            {
                MessageBox.Show("Vai trò không được thiếu!", "", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            bool ketQua = kiemTraDuLieuDauVao();
            if (ketQua == false) return;
            
            tenDangNhap = txtLogin.Text;
            matKhau = txtPassword.Text;
            maNhanVien = txtMaNV.Text;

            if (btnCongTy.Checked) vaiTro = "CongTy";
            if (btnChiNhanh.Checked) vaiTro = "ChiNhanh";
            if (btnUser.Checked) vaiTro = "User";
            String cauTruyVan =
                    "EXEC sp_TaoLogin '" + tenDangNhap + "' , '" + matKhau + "', '"
                    + maNhanVien + "', '" + vaiTro + "'";

            SqlCommand sqlCommand = new SqlCommand(cauTruyVan, Program.conn);
            try
            {


                Program.myReader = Program.ExecSqlDataReader(cauTruyVan);
                /*khong co ket qua tra ve thi ket thuc luon*/
                if (Program.myReader == null)
                {
                    return;
                }

                MessageBox.Show("Đăng kí tài khoản thành công\n\nTài khoản: " + tenDangNhap + "\nMật khẩu: " + matKhau + "\n Mã Nhân Viên: " + maNhanVien + "\n Vai Trò: " + vaiTro, "Thông Báo", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
