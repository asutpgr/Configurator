using System;
using System.Collections.Generic;

namespace GRAccessHelper.Extensions
{
    using ArchestrA.GRAccess;
    using Exceptions.IAttribute;

    public static class IAttributeExtension
    {
        // Преобразуем в список string
        public static List<string> ToList(this IAttributes attrs)
        {
            if (attrs == null) throw new AttributeNullReferenceException();
            List<string> list = new List<string>();
            foreach (IAttribute attr in attrs)
                list.Add(attr.Name);
            return list ?? null;
        }
        // Ошибка последней команды
        public static string GetFailReason(this IAttribute attr)
        {
            if (attr == null) throw new ArgumentNullException(nameof(attr));
            if (attr.CommandResult == null || attr.CommandResult.Successful)
                return null;
            else
                return $"{attr.CommandResult.Text}: {attr.CommandResult.CustomMessage}";
        }
    }
}
