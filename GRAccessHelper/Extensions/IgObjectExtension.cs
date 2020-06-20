using System;

namespace GRAccessHelper.Extensions
{
    using ArchestrA.GRAccess;
    using Exceptions.Galaxy;
    using Exceptions.IAttribute;
    public static class IgObjectExtension
    {
        #region Базовые методы
        // Проверяем статус редактирования и берем на редактирвоание в противном случае
        public static void CheckOutWithCheckStatus(this IgObject gobj)
        {
            if (gobj.CheckoutStatus != ECheckoutStatus.notCheckedOut)
                throw new GalaxyObjectAlreadyCheckOutedException(gobj.Tagname);
            gobj.CheckOut();
        }
        public static void SaveAndCheckIn(this IgObject obj, string comment = null)
        {
            obj.Save();
            if (comment == null)
                obj.CheckIn(comment);
            else 
                obj.CheckIn();
        } 
        #endregion

        #region Работа с атрибутами
        //Получить коллекцию атрибутов в IgObject( IsConfigurable - конфигурируемые или все) 
        public static IAttributes GetAttributesAny(this IgObject gobj, bool IsConfigurable = false)
        {
            if (gobj == null) throw new ArgumentNullException($" Объект {gobj.Tagname} не существует или NULL");
            if (IsConfigurable)
                return gobj.ConfigurableAttributes;
            else 
                return gobj.Attributes;
        }

        //Проверить, существует ли такой атрибут.
        public static bool IsExistAttribute(this IgObject gobj, string attrname)
        {
            if (gobj == null) throw new ArgumentNullException($" Объект {gobj.Tagname} не существует или NULL");
            if (string.IsNullOrWhiteSpace(attrname)) throw new Exception($"Имя атрибутта {nameof(attrname)} не может быть пустым");
            try
            {
                var attrs = gobj.GetAttributesAny();// TODO: проверить нужно ли брать на редактирвоание
                var attr = attrs[attrname];
                if (attr == null)
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
                throw; //TODO: может и не надо прокидывать дальше
            }
        }
        //Получить тип
        // если вернулось MxNoData - значит атрибута не существует
        public static MxDataType GetAttributeMxDataType(this IgObject gobj, string attrname)
        {
            if (gobj == null) 
                throw new ArgumentNullException($" Объект {gobj.Tagname} не существует или NULL");
            if (string.IsNullOrWhiteSpace(attrname)) 
                throw new Exception($"Имя атрибута {nameof(attrname)} не может быть пустым");
            MxDataType type = MxDataType.MxNoData;
            gobj.CheckOutWithCheckStatus();
            if (gobj.IsExistAttribute(attrname))
            {
                type = gobj.ConfigurableAttributes[attrname].DataType;
                gobj.SaveAndCheckIn($"Получение типа атрибута {attrname} для объекта {gobj}.");
            }
            else
                throw new AttributeNullReferenceException();
            return type;
        }
        // значение, имя, создать, удалить, записать свойства атрибута
        //Полчучаем значение атрибута
        public static string GetAttributeValue(this IgObject gobj, string attrname)
        {
            if (gobj == null)
                throw new ArgumentNullException($" Объект {gobj.Tagname} не существует или NULL");
            if (string.IsNullOrWhiteSpace(attrname))
                throw new Exception($"Имя атрибута {nameof(attrname)} не может быть пустым");
            if (gobj.IsExistAttribute(attrname))
            {
                var attrs = gobj.GetAttributesAny();
                return attrs[attrname]?.value?.GetString();
            }
            else
                return null;
        }
        #endregion
    }
}
