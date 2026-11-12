using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public static class DetailedActionLogger
    {
        private static readonly HashSet<int> AttachedForms = new HashSet<int>();
        private static readonly HashSet<int> AttachedControls = new HashSet<int>();
        private static readonly object Sync = new object();

        public static void Attach(Form form)
        {
            if (form == null) return;
            int id = form.GetHashCode();
            lock (Sync)
            {
                if (AttachedForms.Contains(id)) return;
                AttachedForms.Add(id);
            }

            AppLogger.Log($"Открыта/инициализирована форма: {SafeName(form)}.");
            form.Shown += (_, _) => AppLogger.Log($"Форма показана: {SafeName(form)}.");
            form.FormClosing += (_, e) => AppLogger.Log($"Закрытие формы: {SafeName(form)}, причина: {e.CloseReason}.");
            form.FormClosed += (_, e) => AppLogger.Log($"Форма закрыта: {SafeName(form)}, причина: {e.CloseReason}.");

            AttachControlTree(form, form);
        }

        private static void AttachControlTree(Form owner, Control root)
        {
            AttachControl(owner, root);
            foreach (Control child in root.Controls)
                AttachControlTree(owner, child);
        }

        private static void AttachControl(Form owner, Control control)
        {
            int id = control.GetHashCode();
            lock (Sync)
            {
                if (AttachedControls.Contains(id)) return;
                AttachedControls.Add(id);
            }

            control.ControlAdded += (_, e) => AttachControlTree(owner, e.Control);

            if (control is ButtonBase button)
            {
                button.Click += (_, _) => AppLogger.Log($"Нажата кнопка: {SafeName(button)} на форме {SafeName(owner)}.");
            }

            if (control is ComboBox combo)
            {
                combo.SelectedIndexChanged += (_, _) =>
                    AppLogger.Log($"Изменён ComboBox {SafeName(combo)} на форме {SafeName(owner)}: {combo.Text}.");
            }

            if (control is DateTimePicker dtp)
            {
                dtp.ValueChanged += (_, _) =>
                    AppLogger.Log($"Изменена дата/время {SafeName(dtp)} на форме {SafeName(owner)}: {dtp.Value:dd.MM.yyyy HH:mm:ss}.");
            }

            if (control is NumericUpDown nud)
            {
                nud.ValueChanged += (_, _) =>
                    AppLogger.Log($"Изменено числовое поле {SafeName(nud)} на форме {SafeName(owner)}: {nud.Value}.");
            }

            if (control is CheckBox cb)
            {
                cb.CheckedChanged += (_, _) =>
                    AppLogger.Log($"Изменён флажок {SafeName(cb)} на форме {SafeName(owner)}: {cb.Checked}.");
            }

            if (control is RadioButton rb)
            {
                rb.CheckedChanged += (_, _) =>
                    AppLogger.Log($"Изменён переключатель {SafeName(rb)} на форме {SafeName(owner)}: {rb.Checked}.");
            }

            // Важно: не логируем изменения многострочных/служебных полей журнала,
            // иначе сам логгер начинает порождать новые записи бесконечно.
            if (control is TextBoxBase tb && !tb.Multiline && !tb.ReadOnly)
            {
                tb.Leave += (_, _) =>
                    AppLogger.Log($"Изменено текстовое поле {SafeName(tb)} на форме {SafeName(owner)}. Длина текста: {tb.TextLength}.");
            }

            if (control is DataGridView dgv)
            {
                dgv.CellClick += (_, e) =>
                    AppLogger.Log($"Щелчок по таблице {SafeName(dgv)} на форме {SafeName(owner)}: строка {e.RowIndex}, столбец {e.ColumnIndex}.");
                dgv.CellValueChanged += (_, e) =>
                    AppLogger.Log($"Изменена ячейка таблицы {SafeName(dgv)} на форме {SafeName(owner)}: строка {e.RowIndex}, столбец {e.ColumnIndex}.");
            }

            if (control is ToolStrip toolStrip)
            {
                AttachToolStripItems(owner, toolStrip.Items.Cast<ToolStripItem>());
            }
        }

        private static void AttachToolStripItems(Form owner, IEnumerable<ToolStripItem> items)
        {
            foreach (ToolStripItem item in items)
            {
                item.Click += (_, _) => AppLogger.Log($"Выбран пункт меню/панели: {SafeName(item)} на форме {SafeName(owner)}.");
                if (item is ToolStripDropDownItem dropDown)
                    AttachToolStripItems(owner, dropDown.DropDownItems.Cast<ToolStripItem>());
            }
        }

        private static string SafeName(Control control)
        {
            string name = string.IsNullOrWhiteSpace(control.Name) ? control.GetType().Name : control.Name;
            string text = string.IsNullOrWhiteSpace(control.Text) ? string.Empty : $" ('{control.Text}')";
            return $"{name}{text}";
        }

        private static string SafeName(ToolStripItem item)
        {
            string name = string.IsNullOrWhiteSpace(item.Name) ? item.GetType().Name : item.Name;
            string text = string.IsNullOrWhiteSpace(item.Text) ? string.Empty : $" ('{item.Text}')";
            return $"{name}{text}";
        }
    }
}
