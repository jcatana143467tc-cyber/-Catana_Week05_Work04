using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProductFormApp
{
    public partial class ProductForm : Form
    {
        private static readonly string DataFile =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "products.csv");

        private readonly HashSet<string> _savedBarcodes =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public ProductForm()
        {
            InitializeComponent();

            cboCategory.Items.AddRange(new object[]
            {
                "Beverages", "Snacks", "Canned Goods", "Personal Care",
                "Household", "Electronics", "Others"
            });

            LoadExistingBarcodes();
            WireEvents();
            UpdateBarcodeStatus();
        }

        // ---------- Event wiring ----------
        private void WireEvents()
        {
            btnSave.Click += btnSave_Click;
            btnClear.Click += btnClear_Click;
            btnCancel.Click += btnCancel_Click;

            txtBarcode.KeyPress += DigitsOnly_KeyPress;
            txtBarcode.TextChanged += delegate { UpdateBarcodeStatus(); };

            txtCost.KeyPress += Decimal_KeyPress;
            txtMarkup.KeyPress += Decimal_KeyPress;
            txtCost.TextChanged += delegate { RecalculatePrice(); };
            txtMarkup.TextChanged += delegate { RecalculatePrice(); };

            // Clear a field's error as soon as the user edits it again
            txtBarcode.TextChanged += delegate { errorProvider.SetError(txtBarcode, ""); };
            txtName.TextChanged += delegate { errorProvider.SetError(txtName, ""); };
            cboCategory.SelectedIndexChanged += delegate { errorProvider.SetError(cboCategory, ""); };
            txtCost.TextChanged += delegate { errorProvider.SetError(txtCost, ""); };
            txtMarkup.TextChanged += delegate { errorProvider.SetError(txtMarkup, ""); };

            FormClosing += ProductForm_FormClosing;
        }

        // ---------- Buttons ----------
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateAll()) return;

            try
            {
                SaveProduct();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Could not save the product:\n" + ex.Message, "Save Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Product saved successfully!", "Saved",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (!HasInput() ||
                MessageBox.Show("Clear all fields?", "Clear", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ClearForm();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ProductForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && HasInput())
            {
                DialogResult result = MessageBox.Show("You have unsaved changes. Close anyway?",
                    "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) e.Cancel = true;
            }
        }

        // ---------- Input restriction ----------
        private static void DigitsOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private static void Decimal_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null) return;
            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar)) return;

            string sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            bool isSep = e.KeyChar.ToString() == sep;
            if (!(isSep && !tb.Text.Contains(sep))) e.Handled = true;
        }

        // ---------- Barcode availability ----------
        private void UpdateBarcodeStatus()
        {
            string code = txtBarcode.Text.Trim();
            if (code.Length == 0)
            {
                lblBarcodeStatus.Text = "Enter a barcode (4-14 digits).";
                lblBarcodeStatus.ForeColor = SystemColors.GrayText;
            }
            else if (code.Length < 4)
            {
                lblBarcodeStatus.Text = "Barcode is too short.";
                lblBarcodeStatus.ForeColor = Color.DarkOrange;
            }
            else if (_savedBarcodes.Contains(code))
            {
                lblBarcodeStatus.Text = "Barcode already exists.";
                lblBarcodeStatus.ForeColor = Color.Firebrick;
            }
            else
            {
                lblBarcodeStatus.Text = "Barcode is available.";
                lblBarcodeStatus.ForeColor = Color.SeaGreen;
            }
        }

        // ---------- Calculation: Price = Cost + (Cost x Markup%) ----------
        private void RecalculatePrice()
        {
            decimal cost, markup;
            if (TryParseDecimal(txtCost.Text, out cost) && TryParseDecimal(txtMarkup.Text, out markup))
            {
                decimal price = Math.Round(cost * (1 + markup / 100m), 2, MidpointRounding.AwayFromZero);
                txtPrice.Text = price.ToString("N2", CultureInfo.CurrentCulture);
            }
            else
            {
                txtPrice.Text = "";
            }
        }

        private static bool TryParseDecimal(string text, out decimal value)
        {
            return decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out value);
        }

        // ---------- Validation ----------
        private bool ValidateAll()
        {
            bool ok = true;
            Control first = null;

            string barcode = txtBarcode.Text.Trim();
            if (barcode.Length == 0) Fail(txtBarcode, "Barcode is required.", ref ok, ref first);
            else if (barcode.Length < 4) Fail(txtBarcode, "Barcode must be 4-14 digits.", ref ok, ref first);
            else if (_savedBarcodes.Contains(barcode)) Fail(txtBarcode, "This barcode already exists.", ref ok, ref first);

            if (string.IsNullOrWhiteSpace(txtName.Text))
                Fail(txtName, "Product name is required.", ref ok, ref first);

            if (cboCategory.SelectedIndex < 0)
                Fail(cboCategory, "Please select a category.", ref ok, ref first);

            decimal cost, markup;
            if (!TryParseDecimal(txtCost.Text, out cost) || cost <= 0)
                Fail(txtCost, "Cost must be a number greater than 0.", ref ok, ref first);

            if (!TryParseDecimal(txtMarkup.Text, out markup) || markup < 0)
                Fail(txtMarkup, "Markup must be 0 or higher.", ref ok, ref first);

            if (first != null) first.Focus();
            return ok;
        }

        private void Fail(Control c, string message, ref bool ok, ref Control first)
        {
            errorProvider.SetError(c, message);
            ok = false;
            if (first == null) first = c;
        }

        private bool HasInput()
        {
            return txtBarcode.Text.Length > 0 || txtName.Text.Length > 0 ||
                   txtDescription.Text.Length > 0 || cboCategory.SelectedIndex >= 0 ||
                   chkActive.Checked || rdoAbroad.Checked ||
                   txtCost.Text.Length > 0 || txtMarkup.Text.Length > 0;
        }

        private void ClearForm()
        {
            txtBarcode.Clear();
            txtName.Clear();
            txtDescription.Clear();
            cboCategory.SelectedIndex = -1;
            rdoLocal.Checked = true;
            chkActive.Checked = false;
            txtCost.Clear();
            txtMarkup.Clear();
            txtPrice.Clear();

            foreach (Control c in new Control[] { txtBarcode, txtName, cboCategory, txtCost, txtMarkup })
                errorProvider.SetError(c, "");

            UpdateBarcodeStatus();
            txtBarcode.Focus();
        }

        // ---------- Persistence (CSV) ----------
        private void SaveProduct()
        {
            bool newFile = !File.Exists(DataFile);
            StringBuilder sb = new StringBuilder();
            if (newFile)
                sb.AppendLine("Barcode,Name,Description,Category,Origin,Status,Cost,Price,MarkupPercent");

            string barcode = txtBarcode.Text.Trim();
            decimal cost, markup;
            decimal.TryParse(txtCost.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out cost);
            decimal.TryParse(txtMarkup.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out markup);
            decimal price = Math.Round(cost * (1 + markup / 100m), 2, MidpointRounding.AwayFromZero);

            sb.AppendLine(string.Join(",",
                Csv(barcode),
                Csv(txtName.Text.Trim()),
                Csv(txtDescription.Text.Trim()),
                Csv(cboCategory.Text),
                rdoLocal.Checked ? "Local" : "Abroad",
                chkActive.Checked ? "Active" : "Inactive",
                cost.ToString("0.00", CultureInfo.InvariantCulture),
                price.ToString("0.00", CultureInfo.InvariantCulture),
                markup.ToString("0.##", CultureInfo.InvariantCulture)));

            File.AppendAllText(DataFile, sb.ToString(), Encoding.UTF8);
            _savedBarcodes.Add(barcode);
        }

        private void LoadExistingBarcodes()
        {
            if (!File.Exists(DataFile)) return;
            foreach (string line in File.ReadLines(DataFile).Skip(1))
            {
                int comma = line.IndexOf(',');
                string code = (comma < 0 ? line : line.Substring(0, comma)).Trim('"');
                if (code.Length > 0) _savedBarcodes.Add(code);
            }
        }

        private static string Csv(string s)
        {
            return "\"" + s.Replace("\"", "\"\"").Replace("\r", " ").Replace("\n", " ") + "\"";
        }
    }
}
