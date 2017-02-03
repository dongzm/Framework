using Framework.Core.Attributes.Support;
using Framework.Core.Model;
using Framework.Core.Proc;
using Framework.Core.Query;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace Framework.Core.Attributes
{
    public class DataSchema
    {
        private Dictionary<Type, ModelAttributes> _modelReflectDictionary;
        private Dictionary<Type, ProcModelAttributes> _procModelDictionary;
        public static readonly DataSchema Instance = new DataSchema();

        private DataSchema()
        {
        }

        private void AddOther(Type type, ModelType modelType)
        {
            ModelAttributes attributes = new ModelAttributes();
            TableReflectAttribute tableReflectAttribute = GetCustomAttributes.GetTableReflectAttribute(type);
            if ((modelType == ModelType.Model) && (null != tableReflectAttribute))
            {
                attributes.TableReflect = tableReflectAttribute;
                attributes.PropertyCloumnReflectAttr = GetCustomAttributes.GetAllCloumnReflectAttrs(type);
            }
            else
            {
                attributes.PropertyCloumnReflectAttr = GetCustomAttributes.GetAllCloumnReflectAttrs(type);
            }
            this._modelReflectDictionary.Add(type, attributes);
        }

        private void AddProc(Type type)
        {
            ProcModelAttributes attributes = new ProcModelAttributes();
            OutCursorAttribute outCursorAttribute = GetCustomAttributes.GetOutCursorAttribute(type);
            attributes.OutCursor = outCursorAttribute;
            attributes.PropertyCloumnReflectAttr = GetCustomAttributes.GetAllCloumnReflectAttrs(type);
            this._procModelDictionary.Add(type, attributes);
        }

        private ModelType GetContainType(Type type)
        {
            Type baseType = type.BaseType;
            if (baseType == typeof(BaseModel))
            {
                return ModelType.Model;
            }
            if (baseType == typeof(BaseQueryResult))
            {
                return ModelType.Query;
            }
            if (baseType == typeof(BaseProcedureResult))
            {
                return ModelType.Proc;
            }
            if (baseType == typeof(object))
            {
                return ModelType.Unknow;
            }
            return this.GetContainType(baseType);
        }

        public void LoadEntityInfo(string asemblyName)
        {
            if (null == this._modelReflectDictionary)
            {
                this._modelReflectDictionary = new Dictionary<Type, ModelAttributes>();
            }
            if (this._procModelDictionary == null)
            {
                this._procModelDictionary = new Dictionary<Type, ProcModelAttributes>();
            }
            Assembly assembly = Assembly.Load(asemblyName);
            ModelType unknow = ModelType.Unknow;
            foreach (Type type2 in assembly.GetTypes())
            {
                unknow = this.GetContainType(type2);
                if (unknow != ModelType.Unknow)
                {
                    if (unknow == ModelType.Proc)
                    {
                        this.AddProc(type2);
                    }
                    else
                    {
                        this.AddOther(type2, unknow);
                    }
                }
            }
        }

        public Dictionary<Type, ModelAttributes> ModelReflectDictionary
        {
            get
            {
                return this._modelReflectDictionary;
            }
            set
            {
                this._modelReflectDictionary = value;
            }
        }

        public Dictionary<Type, ProcModelAttributes> ProcModelDictionary
        {
            get
            {
                return this._procModelDictionary;
            }
            set
            {
                this._procModelDictionary = value;
            }
        }

        private enum ModelType
        {
            Model,
            Query,
            Proc,
            Unknow
        }
    }
}

