using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ArchestrA.GRAccess;
using Galaxy.Model.Attributes;

namespace GRAccessHelper.Extensions
{
    using Exceptions.IAttribute;
    using Exceptions.IgObject;
    using Exceptions.Galaxy;
    using System.IO;
    using System.Linq;
    using System.CodeDom;
    using System.Security;
    using System.Data.Common;

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
        // TODO: проверить нужен ли check out
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
        // делаем словарь готовый для атрмбутов, определяющих положение в модели (имя атрибута)(делегат <igobj><value>)
        public static Dictionary<string, Action<IgObject, string>> SystemAttributes =
                                   new Dictionary<string, Action<IgObject, string>>(StringComparer.InvariantCultureIgnoreCase)
                                   {
                                       {"Area",(gobj,value) => gobj.Area = value },
                                       {"ContainedName",(gobj,value) => gobj.ContainedName = value },
                                       {"Host",(gobj,value) => gobj.Host = value },
                                       {"Container",(gobj,value) => gobj.Container = value },
                                       {"TagName",(gobj,value) => gobj.Tagname = value }
                                   };
        // Получаем имена польовательских атрибутов( атрибуты созданные в шаблоне так же вляются пользовательскими)
        public static string[] GetUDANames(this IgObject gobj)
        {
            if (gobj == null)
                throw new IgObjectsNullReferenceExceptions();
            var attrStringXML = gobj.Attributes["_InheritedUDAs"].value.GetString();
            XmlSerializer makeXML = new XmlSerializer(typeof(UDAInfo));
            var attrString = new StringReader(attrStringXML);
            var attrUDeserialize = (UDAInfo)makeXML.Deserialize(attrString);
            return attrUDeserialize.Attribute.Select(x => x.Name).ToArray();
        }

        public static Dictionary<string,string> GetUDAValues(this IgObject gobj)
        {
            if (gobj == null)
                throw new IgObjectsNullReferenceExceptions();
            Dictionary<string, string> udas = new Dictionary<string, string>();
            var attrs = gobj?.GetAttributesAny();
            var uda_names = gobj?.GetUDANames();
            foreach (var name in uda_names)
                udas.Add(name, attrs[name].value.GetString());
            return udas;
        }

        #endregion
    }
}
