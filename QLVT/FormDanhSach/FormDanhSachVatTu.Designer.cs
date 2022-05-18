namespace QLVT.FormDanhSach
{
    partial class FormDanhSachVatTu
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.VattuBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DS_SV1 = new QLVT.DS_SV1();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.btnInDanhSach = new System.Windows.Forms.Button();
            this.btnXemTruoc = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bdsVatTu = new System.Windows.Forms.BindingSource(this.components);
            this.vattuTableAdapter = new QLVT.DS_SV1TableAdapters.VattuTableAdapter();
            this.tableAdapterManager = new QLVT.DS_SV1TableAdapters.TableAdapterManager();
            ((System.ComponentModel.ISupportInitialize)(this.VattuBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DS_SV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsVatTu)).BeginInit();
            this.SuspendLayout();
            // 
            // VattuBindingSource
            // 
            this.VattuBindingSource.DataMember = "Vattu";
            this.VattuBindingSource.DataSource = this.DS_SV1;
            // 
            // DS_SV1
            // 
            this.DS_SV1.DataSetName = "DS_SV1";
            this.DS_SV1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.reportViewer1);
            this.panelControl1.Controls.Add(this.btnInDanhSach);
            this.panelControl1.Controls.Add(this.btnXemTruoc);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(882, 553);
            this.panelControl1.TabIndex = 1;
            // 
            // reportViewer1
            // 
            reportDataSource2.Name = "VatTu";
            reportDataSource2.Value = this.VattuBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "QLVT.Report.RpVatTu.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(-420, 389);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(396, 246);
            this.reportViewer1.TabIndex = 5;
            // 
            // btnInDanhSach
            // 
            this.btnInDanhSach.BackColor = System.Drawing.Color.DarkGray;
            this.btnInDanhSach.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInDanhSach.ForeColor = System.Drawing.Color.Black;
            this.btnInDanhSach.Location = new System.Drawing.Point(477, 287);
            this.btnInDanhSach.Margin = new System.Windows.Forms.Padding(4);
            this.btnInDanhSach.Name = "btnInDanhSach";
            this.btnInDanhSach.Size = new System.Drawing.Size(236, 44);
            this.btnInDanhSach.TabIndex = 4;
            this.btnInDanhSach.Text = "XUẤT FILE PDF";
            this.btnInDanhSach.UseVisualStyleBackColor = false;
            this.btnInDanhSach.Click += new System.EventHandler(this.btnInDanhSach_Click);
            // 
            // btnXemTruoc
            // 
            this.btnXemTruoc.BackColor = System.Drawing.Color.DarkGray;
            this.btnXemTruoc.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXemTruoc.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnXemTruoc.Location = new System.Drawing.Point(147, 287);
            this.btnXemTruoc.Margin = new System.Windows.Forms.Padding(4);
            this.btnXemTruoc.Name = "btnXemTruoc";
            this.btnXemTruoc.Size = new System.Drawing.Size(260, 44);
            this.btnXemTruoc.TabIndex = 3;
            this.btnXemTruoc.Text = "XEM DANH SÁCH VẬT TƯ";
            this.btnXemTruoc.UseVisualStyleBackColor = false;
            this.btnXemTruoc.Click += new System.EventHandler(this.btnXemTruoc_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(251, 126);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(347, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "DANH SÁCH VẬT TƯ";
            // 
            // bdsVatTu
            // 
            this.bdsVatTu.DataMember = "Vattu";
            this.bdsVatTu.DataSource = this.DS_SV1;
            // 
            // vattuTableAdapter
            // 
            this.vattuTableAdapter.ClearBeforeFill = true;
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
            this.tableAdapterManager.NhanVienTableAdapter = null;
            this.tableAdapterManager.PhieuNhapTableAdapter = null;
            this.tableAdapterManager.PhieuXuatTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = QLVT.DS_SV1TableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.VattuTableAdapter = this.vattuTableAdapter;
            // 
            // FormDanhSachVatTu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 553);
            this.Controls.Add(this.panelControl1);
            this.Name = "FormDanhSachVatTu";
            this.Text = "FormDanhSachVatTu";
            this.Load += new System.EventHandler(this.FormDanhSachVatTu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.VattuBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DS_SV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsVatTu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Button btnInDanhSach;
        private System.Windows.Forms.Button btnXemTruoc;
        private System.Windows.Forms.Label label1;
        private DS_SV1 DS_SV1;
        private System.Windows.Forms.BindingSource bdsVatTu;
        private DS_SV1TableAdapters.VattuTableAdapter vattuTableAdapter;
        private DS_SV1TableAdapters.TableAdapterManager tableAdapterManager;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource VattuBindingSource;
    }
}