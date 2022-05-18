namespace QLVT
{
    partial class FormDanhSachNhanVien
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label mACNLabel;
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.NhanVienBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DS_SV1 = new QLVT.DS_SV1();
            this.bdsNhanVien = new System.Windows.Forms.BindingSource(this.components);
            this.tableAdapterManager = new QLVT.DS_SV1TableAdapters.TableAdapterManager();
            this.nhanVienTableAdapter = new QLVT.DS_SV1TableAdapters.NhanVienTableAdapter();
            this.label1 = new System.Windows.Forms.Label();
            this.btnInDanhSach = new System.Windows.Forms.Button();
            this.btnXemTruoc = new System.Windows.Forms.Button();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.cmbChiNhanh = new System.Windows.Forms.ComboBox();
            mACNLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NhanVienBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DS_SV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsNhanVien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mACNLabel
            // 
            mACNLabel.AutoSize = true;
            mACNLabel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            mACNLabel.Location = new System.Drawing.Point(191, 168);
            mACNLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            mACNLabel.Name = "mACNLabel";
            mACNLabel.Size = new System.Drawing.Size(114, 24);
            mACNLabel.TabIndex = 1;
            mACNLabel.Text = "Chi Nhánh";
            // 
            // NhanVienBindingSource
            // 
            this.NhanVienBindingSource.DataMember = "NhanVien";
            this.NhanVienBindingSource.DataSource = this.DS_SV1;
            // 
            // DS_SV1
            // 
            this.DS_SV1.DataSetName = "DS_SV1";
            this.DS_SV1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bdsNhanVien
            // 
            this.bdsNhanVien.DataMember = "NhanVien";
            this.bdsNhanVien.DataSource = this.DS_SV1;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.ChiNhanhTableAdapter = null;
            this.tableAdapterManager.CTDDHTableAdapter = null;
            this.tableAdapterManager.CTPNTableAdapter = null;
            this.tableAdapterManager.CTPXTableAdapter = null;
            this.tableAdapterManager.DatHangTableAdapter = null;
            this.tableAdapterManager.KhoTableAdapter = null;
            this.tableAdapterManager.NhanVienTableAdapter = this.nhanVienTableAdapter;
            this.tableAdapterManager.PhieuNhapTableAdapter = null;
            this.tableAdapterManager.PhieuXuatTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = QLVT.DS_SV1TableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.VattuTableAdapter = null;
            // 
            // nhanVienTableAdapter
            // 
            this.nhanVienTableAdapter.ClearBeforeFill = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(240, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(417, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "DANH SÁCH NHÂN VIÊN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnInDanhSach
            // 
            this.btnInDanhSach.BackColor = System.Drawing.Color.DarkGray;
            this.btnInDanhSach.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInDanhSach.ForeColor = System.Drawing.Color.Black;
            this.btnInDanhSach.Location = new System.Drawing.Point(479, 308);
            this.btnInDanhSach.Margin = new System.Windows.Forms.Padding(8);
            this.btnInDanhSach.Name = "btnInDanhSach";
            this.btnInDanhSach.Size = new System.Drawing.Size(264, 72);
            this.btnInDanhSach.TabIndex = 4;
            this.btnInDanhSach.Text = "Xuất PDF";
            this.btnInDanhSach.UseVisualStyleBackColor = false;
            this.btnInDanhSach.Click += new System.EventHandler(this.btnInDanhSach_Click);
            // 
            // btnXemTruoc
            // 
            this.btnXemTruoc.BackColor = System.Drawing.Color.DarkGray;
            this.btnXemTruoc.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXemTruoc.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnXemTruoc.Location = new System.Drawing.Point(103, 308);
            this.btnXemTruoc.Margin = new System.Windows.Forms.Padding(8);
            this.btnXemTruoc.Name = "btnXemTruoc";
            this.btnXemTruoc.Size = new System.Drawing.Size(258, 72);
            this.btnXemTruoc.TabIndex = 3;
            this.btnXemTruoc.Text = "Xem Danh Sách Nhân Viên";
            this.btnXemTruoc.UseVisualStyleBackColor = false;
            this.btnXemTruoc.Click += new System.EventHandler(this.btnXemTruoc_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.reportViewer1);
            this.panelControl1.Controls.Add(this.cmbChiNhanh);
            this.panelControl1.Controls.Add(this.btnInDanhSach);
            this.panelControl1.Controls.Add(this.btnXemTruoc);
            this.panelControl1.Controls.Add(mACNLabel);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(8);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(882, 553);
            this.panelControl1.TabIndex = 2;
            // 
            // reportViewer1
            // 
            reportDataSource1.Name = "NhanVien";
            reportDataSource1.Value = this.NhanVienBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "QLVT.Report.RpDSNhanVien.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(-574, 489);
            this.reportViewer1.Margin = new System.Windows.Forms.Padding(4);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(494, 307);
            this.reportViewer1.TabIndex = 3;
            // 
            // cmbChiNhanh
            // 
            this.cmbChiNhanh.FormattingEnabled = true;
            this.cmbChiNhanh.Location = new System.Drawing.Point(349, 168);
            this.cmbChiNhanh.Margin = new System.Windows.Forms.Padding(8);
            this.cmbChiNhanh.Name = "cmbChiNhanh";
            this.cmbChiNhanh.Size = new System.Drawing.Size(289, 24);
            this.cmbChiNhanh.TabIndex = 5;
            this.cmbChiNhanh.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // FormDanhSachNhanVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 553);
            this.Controls.Add(this.panelControl1);
            this.Name = "FormDanhSachNhanVien";
            this.Text = "FormDanhSachNhanVien";
            this.Load += new System.EventHandler(this.FormDanhSachNhanVien_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NhanVienBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DS_SV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsNhanVien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource bdsNhanVien;
        private DS_SV1 DS_SV1;
        private DS_SV1TableAdapters.TableAdapterManager tableAdapterManager;
        private DS_SV1TableAdapters.NhanVienTableAdapter nhanVienTableAdapter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnInDanhSach;
        private System.Windows.Forms.Button btnXemTruoc;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.ComboBox cmbChiNhanh;
        private System.Windows.Forms.BindingSource NhanVienBindingSource;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}