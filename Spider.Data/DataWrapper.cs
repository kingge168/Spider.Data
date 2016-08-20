namespace Spider.Data
{
    using System;
    using System.Collections.Generic;

    public class DataWrapper
    {
        public string CommandText { get; set; }

        public object Data { get; set; }

        public Func<object, IEnumerable<KeyValuePair<string, object>>> ParameterProvider { get; set; }
    }
}

