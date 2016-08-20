# Spider.Data
A very easy and fast ORM framework.
dynamic d=new SmartObject（）;
d["a"]=1;
d.B="abc";
Console.Write(d.ToJson());//{"a":1,B:"abc"}
d=SmartObject.Parse(d.ToJson（）);
IList<KeyValuePair<string,object>>list=d.AsList();
