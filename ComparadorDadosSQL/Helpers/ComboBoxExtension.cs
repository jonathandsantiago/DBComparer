using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ComparadorDadosSQL.Helpers
{
    public static class ComboBoxExtension
    {
        public static void AddEnum<TEnum>(this ImageComboBoxEdit imageComboBox, bool ordenacaoAlfabetica = true, bool permitirNulo = false)
        where TEnum : struct
        {
            var list = Enum.GetValues(typeof(TEnum))
                .OfType<TEnum>()
                .Where(c => !(c as Enum).IsHidden<TEnum>())
                .OfType<object>();

            if (ordenacaoAlfabetica)
            {
                list = list.OrderBy(c => EnumHelper.GetDisplayName(c));
            }

            imageComboBox.Properties.Items.Clear();

            foreach (var item in list)
            {
                imageComboBox.Properties.Items.Add(new ImageComboBoxItem(EnumHelper.GetDisplayName(item), item));
            }

            if (permitirNulo)
            {
                imageComboBox.KeyDown += (sender, e) =>
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        (sender as ImageComboBoxEdit).EditValue = null;
                        (sender as ImageComboBoxEdit).DoValidate();
                    }
                };
            }
        }
    }

    public class HiddenAttribute : Attribute
    {
        private static Dictionary<string, Func<object, bool>> functions = new Dictionary<string, Func<object, bool>>();

        public HiddenAttribute()
        { }

        public static bool IsHidden<T>(object value)
        {
            var attribute = EnumHelper.GetAttribute<HiddenAttribute>(value);

            if (attribute == null)
            {
                return false;
            }

            string key = typeof(T).FullName;

            if (!functions.ContainsKey(key))
            {
                return true;
            }

            return functions[key](value);
        }
    }

    public static class HiddenExtesions
    {
        public static bool IsHidden<TEnum>(this Enum value)
        {
            return HiddenAttribute.IsHidden<TEnum>(value);
        }

        public static bool IsHidden<TEnum>(this TEnum value)
            where TEnum : struct
        {
            return HiddenAttribute.IsHidden<TEnum>(value);
        }
    }
}
