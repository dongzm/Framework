using Framework.Core.Attributes;
using Framework.Core.Support;
using System;
using System.Collections.Generic;
using System.Reflection;



namespace Framework.Core.Attributes.Support
{

    public class GetCustomAttributes
    {
        /// <summary>
        /// 获取class的所有字段的
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<PropertyInfo, CloumnReflectAttribute> GetAllCloumnReflectAttrs(Type type)
        {
            Dictionary<PropertyInfo, CloumnReflectAttribute> dictionary = new Dictionary<PropertyInfo, CloumnReflectAttribute>();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                CloumnReflectAttribute cloumnReflectAttribute = GetCloumnReflectAttribute(info);
                if (null != cloumnReflectAttribute)
                {
                    dictionary.Add(info, cloumnReflectAttribute);
                }
            }
            return dictionary;
        }

        public static CloumnReflectAttribute GetCloumnReflectAttribute(PropertyInfo pi)
        {
            object[] customAttributes = pi.GetCustomAttributes(true);
            if (customAttributes.Length > 0)
            {
                foreach (object obj2 in customAttributes)
                {
                    CloumnReflectAttribute attribute = obj2 as CloumnReflectAttribute;
                    if (null != attribute)
                    {
                        attribute.DbType = GetDataBaseInfo.Instance.GetDbPermeterTypeName(pi.PropertyType.FullName);
                        return attribute;
                    }
                }
            }
            return null;
        }

        public static OutCursorAttribute GetOutCursorAttribute(Type type)
        {
            object[] customAttributes = type.GetCustomAttributes(true);
            if (customAttributes.Length > 0)
            {
                foreach (object obj2 in customAttributes)
                {
                    OutCursorAttribute attribute = obj2 as OutCursorAttribute;
                    if (null != attribute)
                    {
                        return attribute;
                    }
                }
            }
            return null;
        }

        public static TableReflectAttribute GetTableReflectAttribute(Type type)
        {
            object[] customAttributes = type.GetCustomAttributes(true);
            if (customAttributes.Length > 0)
            {
                foreach (object obj2 in customAttributes)
                {
                    TableReflectAttribute attribute = obj2 as TableReflectAttribute;
                    if (null != attribute)
                    {
                        return attribute;
                    }
                }
            }
            return null;
        }

        public static bool IsUnParamedConditionAttribute(PropertyInfo pi)
        {
            object[] customAttributes = pi.GetCustomAttributes(true);
            if (customAttributes.Length > 0)
            {
                foreach (object obj2 in customAttributes)
                {
                    IsUnParamedConditionAttribute attribute = obj2 as IsUnParamedConditionAttribute;
                    if (null != attribute)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

