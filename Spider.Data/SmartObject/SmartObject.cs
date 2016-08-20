namespace Spider.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    public sealed class SmartObject : DynamicObject
    {
        private NamedCollection<NameValueObject> _dict;

        public SmartObject()
        {
            this.Dictionary = new NamedCollection<NameValueObject>();
        }

        internal SmartObject(IDictionary<string, object> dict) : this()
        {
            if (dict != null)
            {
                foreach (KeyValuePair<string, object> pair in dict)
                {
                    this.TrySet(pair.Key, pair.Value);
                }
            }
        }

        public IDictionary<string, object> AsDictionary()
        {
            return this.Dictionary.ToDictionary<NameValueObject, string, object>(o => o.Name, o => DataUtil.RecoverData(o.Value));
        }

        public IList<KeyValuePair<string, object>> AsList()
        {
            return (from t in this.Dictionary select new KeyValuePair<string, object>(t.Name, DataUtil.RecoverData(t.Value))).ToList<KeyValuePair<string, object>>();
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return (from item in this.Dictionary.ToList<NameValueObject>() select item.Name);
        }

        public bool IsDefined(string name)
        {
            return this.Dictionary.Contains(name);
        }

        public static SmartObject Parse(string json)
        {
            return (SmartObject) SmartJosnSerializer.Deserialize<SmartObject>(json);
        }

        public string ToJson()
        {
            return SmartJosnSerializer.Serialize<SmartObject>(this);
        }

        public bool TryGet(int index, out object result)
        {
            if (index < 0 || index > this.Dictionary.Count)
            {
                result = null;
                return false;
            }
            result = this.Dictionary[index].Value;
            return true;
        }

        public bool TryGet(string name, out object result)
        {
            if (this.Dictionary.Contains(name))
            {
                result = this.Dictionary[name].Value;
                return true;
            }
            result = null;
            return false;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            object obj2 = indexes[0];
            if (obj2 is int)
            {
                int index = (int) obj2;
                return this.TryGet(index, out result);
            }
            if (obj2 is string)
            {
                string name = obj2 as string;
                return this.TryGet(name, out result);
            }
            result = null;
            return false;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return this.TryGet(binder.Name, out result);
        }

        public bool TrySet(int index, object value)
        {
            if (index < 0 || index > this.Dictionary.Count) return false;
            this.Dictionary[index].Value = value;
            return true;
        }

        public bool TrySet(string name, object value)
        {
            NameValueObject item = null;
            if (this.Dictionary.TryGet(name, out item))
                item.Value = value;
            else
            {
                try
                {
                    NameValueObject obj3 = new NameValueObject {
                        Name = name,
                        Value = value
                    };
                    this.Dictionary.Add(obj3);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            object obj2 = indexes[0];
            if (obj2 is int)
            {
                int index = (int) obj2;
                return this.TrySet(index, value);
            }
            if (obj2 is string)
            {
                string name = obj2 as string;
                return this.TrySet(name, value);
            }
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return this.TrySet(binder.Name, value);
        }

        private NamedCollection<NameValueObject> Dictionary
        {
            get
            {
                return this._dict;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("Dictionary");
                this._dict = value;
            }
        }

        private class DataUtil
        {
            public static object RecoverData(object value)
            {
                if (value is string) return value;
                if (value is SmartObject) return (value as SmartObject).AsDictionary();
                if (!(value is IEnumerable)) return value;
                IEnumerable enumerable = value as IEnumerable;
                IList<object> list = new LineList<object>();
                foreach (object obj2 in enumerable)
                {
                    if (obj2 is SmartObject)
                        list.Add((obj2 as SmartObject).AsDictionary());
                    else
                    {
                        if (obj2 is IEnumerable)
                        {
                            list.Add(RecoverData(obj2 as IEnumerable));
                            continue;
                        }
                        list.Add(obj2);
                    }
                }
                return list;
            }

            public static object WrapperData(object value)
            {
                if (value is string) return value;
                if (value is IDictionary<string, object>) return new SmartObject(value as IDictionary<string, object>);
                if (!(value is IEnumerable)) return value;
                IEnumerable enumerable = value as IEnumerable;
                IList<object> list = new LineList<object>();
                foreach (object obj2 in enumerable)
                {
                    if (obj2 is IDictionary<string, object>)
                        list.Add(new SmartObject(obj2 as IDictionary<string, object>));
                    else
                        list.Add(WrapperData(obj2));
                }
                return list;
            }
        }

        private class NameValueObject : INamedObject
        {
            private string _name;
            private object _value;

            public string Name
            {
                get
                {
                    return this._name;
                }
                set
                {
                    if (string.IsNullOrEmpty(value)) throw new ArgumentException("name");
                    this._name = value;
                }
            }

            public object Value
            {
                get
                {
                    return this._value;
                }
                set
                {
                    if (value != null)
                    {
                        switch (Type.GetTypeCode(value.GetType()))
                        {
                            case TypeCode.Empty:
                            case TypeCode.DBNull:
                            case (TypeCode.DateTime | TypeCode.Object):
                                return;

                            case TypeCode.Object:
                                this._value = SmartObject.DataUtil.WrapperData(value);
                                return;

                            case TypeCode.Boolean:
                            case TypeCode.Char:
                            case TypeCode.SByte:
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                            case TypeCode.DateTime:
                            case TypeCode.String:
                                this._value = value;
                                return;
                        }
                    }
                    else
                        this._value = null;
                }
            }
        }
    }
}

