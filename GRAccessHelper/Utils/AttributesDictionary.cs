using System;
using System.Collections.Generic;
using ArchestrA.GRAccess;
namespace GRAccessHelper.Utils
{
    public class AttributesDictionary : Dictionary<string, MxValueClass>
    {
        #region Constructors

        public AttributesDictionary() : base()
        {
        }

        public AttributesDictionary(string name, MxValueClass value) : base()
        {
            Add(name, value);
        }

        #endregion

        #region Methods

        public void AddInterString(string name, string str)
        {
            MxValueClass temp = new MxValueClass();
            temp.PutInternationalString(1049, str);
            Add(name, temp);
        }

      
        public void AddShortDescription(string desc)
        {
            AddInterString("ShortDesc", desc);
        }


        public void AddValues(AttributesDictionary values)
        {
            foreach (string name in values.Keys)
                this.Add(name, values[name]);
        }

        public void AddSimpleValue(IAttribute attribute, string value)
        {
            MxValueClass temp = null;
            switch (attribute.DataType)
            {
                case MxDataType.MxBigString:
                    temp = new MxValueClass();
                    temp.PutString(value);
                    break;
                case MxDataType.MxBoolean:
                    temp = new MxValueClass();
                    if (value.Length > 1)
                        temp.PutBoolean(Convert.ToBoolean(value));
                    else temp.PutBoolean(value.Equals("1") ? true : false);
                    break;

                case MxDataType.MxDouble:
                    temp = new MxValueClass();
                    temp.PutDouble(Convert.ToDouble(value));
                    break;

                case MxDataType.MxFloat:
                    temp = new MxValueClass();
                    temp.PutFloat(Convert.ToSingle(value));
                    break;
                case MxDataType.MxInteger:
                    temp = new MxValueClass();
                    temp.PutInteger(Convert.ToInt32(value));
                    break;
                case MxDataType.MxInternationalizedString:
                    temp = new MxValueClass();
                    temp.PutInternationalString(1049, value);
                    break;

                case MxDataType.MxString:
                    temp = new MxValueClass();
                    temp.PutString(value);
                    break;
                default:
                    break;
            }

            if (temp != null)
                Add(attribute.Name, temp);
        }

        #endregion
    }
}
