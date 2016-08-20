namespace Spider.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Web.Script.Serialization;

    public class SmartJosnSerializer
    {
        private static JavaScriptSerializer _jss = new JavaScriptSerializer();

        static SmartJosnSerializer()
        {
            _jss.RegisterConverters(new JavaScriptConverter[] { new SmartObjectJsonConverter() });
        }
        public static dynamic Deserialize<T>(string json)
        {
            return _jss.Deserialize(json, typeof(T));
        }

        public static string Serialize<T>(T obj)
        {
            return _jss.Serialize(obj);
        }

        private class SmartObjectJsonConverter : JavaScriptConverter
        {
            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
            {
                if (dictionary == null) throw new ArgumentNullException("dictionary");
                if (type == typeof(SmartObject)) return new SmartObject(dictionary);
                return null;
            }

            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                SmartObject obj2 = obj as SmartObject;
                if (obj2 != null)
                return obj2.AsDictionary();
                return obj as IDictionary<string, object>;
            }

            public override IEnumerable<Type> SupportedTypes
            {
                get
                {
                    return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(SmartObject) }));
                }
            }
        }
    }
}

