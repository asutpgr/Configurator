using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ArchestrA.GRAccess;
using System.IO;
using System.Linq;

namespace GRAccessHelper.Extensions
{
    using Exceptions.IAttribute;
    using Exceptions.IgObject;
    using Exceptions.Galaxy;
    using Attribute;
    
   

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
            if (gobj == null) 
                throw new ArgumentNullException($" Объект {gobj.Tagname} не существует или NULL");
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
            var attrStringXML_inh = gobj.Attributes["_InheritedUDAs"].value.GetString();
            XmlSerializer makeXML_inh = new XmlSerializer(typeof(UDAInfo));
            var attrString_inh = new StringReader(attrStringXML_inh);
            var attrUDeserialize_inh = (UDAInfo)makeXML_inh.Deserialize(attrString_inh);

            var attrStringXML_mine = gobj.Attributes["UDAs"].value.GetString();
            XmlSerializer makeXML_mine = new XmlSerializer(typeof(UDAInfo));
            var attrString_mine = new StringReader(attrStringXML_mine);
            var attrUDeserialize_mine = (UDAInfo)makeXML_mine.Deserialize(attrString_mine);

            var res_inh = attrUDeserialize_inh.Attribute.Select(x => x.Name);
            var res_mine = attrUDeserialize_mine.Attribute.Select(x => x.Name);
            return res_inh.Concat(res_mine).ToArray();
        }
        // Получаем знаения UDA 
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
        // Установить значение любого объекта GR
        public static void SetAttributeValueRegular(this IgObject obj
                                                    , bool isUDA
                                                    , string name
                                                    , MxDataType type
                                                    , object value
                                                    , string description = ""
                                                    , string engUnits = ""
                                                    )
        {
            if (obj == null)
                throw new ArgumentNullException("IgObject is null");
            //Все типы MxString* сводятся только в тип MxInternationalizedString
            if (type == MxDataType.MxString || type == MxDataType.MxInternationalizedString || type == MxDataType.MxBigString)
                type = MxDataType.MxInternationalizedString;

            if (SystemAttributes.TryGetValue(name, out var act))
            {
                obj.CheckOutWithCheckStatus();
                try
                {
                    act(obj, value?.ToString());
                    obj.SaveAndCheckIn($"System attribute {name} = {(value == null ? "null" : $"\"{value}\"")}");
                }
                finally
                {
                    obj.UndoCheckOut();
                }
                return;
            }

            //если атрибут указан, что она UDA
            if (isUDA)
            {
                //если не существует, то создаём его
                if (!obj.IsExistAttribute(name))
                {
                    obj.CheckOutWithCheckStatus();
                    obj.AddUDA(name, type, MxAttributeCategory.MxCategoryWriteable_USC_Lockable, MxSecurityClassification.MxSecurityViewOnly);
                    obj.SaveAndCheckIn($"Add Attribute ({obj.Tagname}, {name})");

                }
                //если существует
                else
                {

                    var old_type = obj.GetAttributeMxDataType(name);
                    //проверяем изменился ли тип данных атрибута UDA
                    if (type != old_type)
                    {
                        obj.CheckOutWithCheckStatus();
                        var _attrs = obj.ConfigurableAttributes;

                        obj.UpdateUDA(name, type, MxAttributeCategory.MxCategoryWriteable_USC_Lockable, MxSecurityClassification.MxSecurityViewOnly);
                        obj.SaveAndCheckIn($"Change Attribute DataType ({obj.Tagname}, {name},{old_type}->{type})");
                    }
                }
            }
            obj.CheckOutWithCheckStatus();
            try
            {
                //получаем все атрибуты
                var attrs = obj.ConfigurableAttributes;
                //устанавливаем значение атрибута
                attrs[name].SetValue(GalaxyUtils.CreateMxValue(type, value));
                obj.SaveAndCheckIn($"Set value({obj.Tagname}, {name})");
            }
            catch
            {
                obj.SaveAndCheckIn($"Fail to set value({obj.Tagname}, {name})");
            }
        }
        #endregion
    }
}
