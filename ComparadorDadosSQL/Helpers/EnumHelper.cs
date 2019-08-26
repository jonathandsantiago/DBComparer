using System.ComponentModel.DataAnnotations;

namespace ComparadorDadosSQL.Helpers
{
    public class EnumHelper
    {
        public static string GetDisplayName<TEnum>(TEnum enumVal)
        {
            var attr = GetAttribute<DisplayAttribute>(enumVal);

            if (attr != null)
            {
                return attr.Name;
            }

            return enumVal?.ToString() ?? string.Empty;
        }

        public static TAttr GetAttribute<TAttr>(object enumVal) where TAttr : System.Attribute
        {
            if (enumVal == null)
            {
                return default(TAttr);
            }

            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());

            if (memInfo.Length == 0)
            {
                return null;
            }

            var attributes = memInfo[0].GetCustomAttributes(typeof(TAttr), false);
            return attributes.Length > 0 ? (TAttr)attributes[0] : null;
        }
    }
}
