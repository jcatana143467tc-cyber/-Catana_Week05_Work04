# Product Form (C# WinForms, .NET Framework 4.8) – Week 05 Work 04

A Windows Forms product-entry form built from the "C# Common Controls" material.

## Controls used
Form, Label, TextBox (single & multiline), ComboBox, GroupBox, RadioButton, CheckBox, Button.

## Improvements
- **Button events:** Save (validate + write to `products.csv`), Clear (reset with confirmation), Cancel (close with unsaved-changes prompt). Enter = Save, Esc = Cancel.
- **Input validation:** required Barcode/Name/Category, Cost > 0, Markup >= 0, shown with `ErrorProvider` icons.
- **Input restriction:** barcode accepts digits only; cost/markup accept numbers only.
- **Live barcode check:** "Barcode is available." / "already exists." based on saved records.
- **Calculation:** `Price = Cost + (Cost x Markup / 100)`, updated as you type.
- **Persistence:** products are appended to `products.csv` beside the executable.

## Run
Open `Catana_Week05_Work04.sln` in Visual Studio 2022 and press **F5**.
(Requires the ".NET desktop development" workload and the .NET Framework 4.8 targeting pack.)
