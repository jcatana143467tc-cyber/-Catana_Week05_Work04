using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProductFormApp
{

partial class ProductForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblBarcode = new Label();
        txtBarcode = new TextBox();
        lblBarcodeStatus = new Label();
        lblName = new Label();
        txtName = new TextBox();
        lblDescription = new Label();
        txtDescription = new TextBox();
        lblCategory = new Label();
        cboCategory = new ComboBox();
        lblOrigin = new Label();
        grpManufactured = new GroupBox();
        rdoLocal = new RadioButton();
        rdoAbroad = new RadioButton();
        lblStatus = new Label();
        chkActive = new CheckBox();
        lblCost = new Label();
        txtCost = new TextBox();
        lblPrice = new Label();
        txtPrice = new TextBox();
        lblMarkup = new Label();
        txtMarkup = new TextBox();
        btnCancel = new Button();
        btnClear = new Button();
        btnSave = new Button();
        errorProvider = new ErrorProvider(components);
        grpManufactured.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();

        // Barcode
        lblBarcode.Text = "Barcode"; lblBarcode.Location = new Point(20, 24); lblBarcode.AutoSize = true;
        txtBarcode.Location = new Point(130, 20); txtBarcode.Size = new Size(240, 23); txtBarcode.MaxLength = 14;
        lblBarcodeStatus.Location = new Point(130, 46); lblBarcodeStatus.AutoSize = true;
        lblBarcodeStatus.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
        lblBarcodeStatus.Text = "Enter a barcode (4-14 digits).";

        // Name
        lblName.Text = "Name"; lblName.Location = new Point(20, 74); lblName.AutoSize = true;
        txtName.Location = new Point(130, 70); txtName.Size = new Size(240, 23); txtName.MaxLength = 100;

        // Description (multiline)
        lblDescription.Text = "Description"; lblDescription.Location = new Point(20, 104); lblDescription.AutoSize = true;
        txtDescription.Location = new Point(130, 100); txtDescription.Size = new Size(240, 70);
        txtDescription.Multiline = true; txtDescription.ScrollBars = ScrollBars.Vertical;
        txtDescription.AcceptsReturn = true; txtDescription.MaxLength = 250;

        // Category
        lblCategory.Text = "Category"; lblCategory.Location = new Point(20, 189); lblCategory.AutoSize = true;
        cboCategory.Location = new Point(130, 185); cboCategory.Size = new Size(240, 23);
        cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;

        // Origin
        lblOrigin.Text = "Origin"; lblOrigin.Location = new Point(20, 224); lblOrigin.AutoSize = true;
        grpManufactured.Text = "Manufactured"; grpManufactured.Location = new Point(130, 216); grpManufactured.Size = new Size(240, 52);
        grpManufactured.Controls.Add(rdoLocal); grpManufactured.Controls.Add(rdoAbroad);
        rdoLocal.Text = "Local"; rdoLocal.Location = new Point(20, 22); rdoLocal.AutoSize = true; rdoLocal.Checked = true;
        rdoAbroad.Text = "Abroad"; rdoAbroad.Location = new Point(120, 22); rdoAbroad.AutoSize = true;

        // Status
        lblStatus.Text = "Status"; lblStatus.Location = new Point(20, 284); lblStatus.AutoSize = true;
        chkActive.Text = "Active"; chkActive.Location = new Point(130, 282); chkActive.AutoSize = true;

        // Cost / Price / Markup
        lblCost.Text = "Cost"; lblCost.Location = new Point(20, 316); lblCost.AutoSize = true;
        txtCost.Location = new Point(20, 336); txtCost.Size = new Size(110, 23);
        lblPrice.Text = "Price (auto)"; lblPrice.Location = new Point(140, 316); lblPrice.AutoSize = true;
        txtPrice.Location = new Point(140, 336); txtPrice.Size = new Size(110, 23); txtPrice.ReadOnly = true; txtPrice.TabStop = false;
        lblMarkup.Text = "Markup (%)"; lblMarkup.Location = new Point(270, 316); lblMarkup.AutoSize = true;
        txtMarkup.Location = new Point(270, 336); txtMarkup.Size = new Size(100, 23);

        // Buttons
        btnCancel.Text = "Cancel"; btnCancel.Location = new Point(20, 385); btnCancel.Size = new Size(110, 32);
        btnClear.Text = "Clear"; btnClear.Location = new Point(140, 385); btnClear.Size = new Size(110, 32);
        btnSave.Text = "Save"; btnSave.Location = new Point(270, 385); btnSave.Size = new Size(100, 32);
        btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

        errorProvider.ContainerControl = this;
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

        // Form
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 9F);
        ClientSize = new Size(390, 440);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Product Form";
        AcceptButton = btnSave;
        CancelButton = btnCancel;
        Controls.AddRange(new Control[] {
            lblBarcode, txtBarcode, lblBarcodeStatus, lblName, txtName, lblDescription, txtDescription,
            lblCategory, cboCategory, lblOrigin, grpManufactured, lblStatus, chkActive,
            lblCost, txtCost, lblPrice, txtPrice, lblMarkup, txtMarkup,
            btnCancel, btnClear, btnSave });

        grpManufactured.ResumeLayout(false);
        grpManufactured.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblBarcode, lblBarcodeStatus, lblName, lblDescription,
        lblCategory, lblOrigin, lblStatus, lblCost, lblPrice, lblMarkup;
    private TextBox txtBarcode, txtName, txtDescription, txtCost, txtPrice, txtMarkup;
    private ComboBox cboCategory;
    private GroupBox grpManufactured;
    private RadioButton rdoLocal, rdoAbroad;
    private CheckBox chkActive;
    private Button btnCancel, btnClear, btnSave;
    private ErrorProvider errorProvider;
}
}
