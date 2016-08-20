Spider.Data
==============

##A very easy and fast ORM framework.

SmartObject
------------
    dynamic d=new SmartObject（）;
    d["a"]=1;
    d.B="abc"
    Console.Write(d.ToJson());//{"a":1,B:"abc"}
    Console.Write(d[0]);//1
    Console.Write(d["a"]);//1
    Console.Write(d.a);//1
    d=SmartObject.Parse(d.ToJson（）);
    IList<KeyValuePair<string,object>>list=d.AsList();
    IDictionary<string,object> dict=d.AsDictionary();

ORM
-------------

    BatchDataHelper helper = new BatchDataHelper("MysqlConnection");
    IList<dynamic> lst1=helper.Query("select * from User",new List<KeyValuePair<string,object>>());
    IList<User> lst2 = helper.Query("select * from User", new List<KeyValuePair<string, object>>(),r=> { return new User() {Name=r["Name"].ToString(), Code=r["Code"].ToString() }; });
    DataWrapper wrapper = new DataWrapper();
    wrapper.CommandText = "update User set Name=@Name where ID=@ID;";
    wrapper.Data = new User() { Name = "xxx" };
    wrapper.ParameterProvider = d => { User user = d as User; return new List<KeyValuePair<string, object>>(){ new KeyValuePair<string, object>("Name",user.Name), new KeyValuePair<string, object> ("ID",2)}; };
    DataWrapper wrapper2 = new DataWrapper();
    wrapper2.CommandText = "delete from User where ID=@ID;";
    wrapper2.ParameterProvider = d => { return new List<KeyValuePair<string, object>>() {  new KeyValuePair<string, object>("ID", 1) }; };
    helper.ProcessData(new List<DataWrapper>() { wrapper,wrapper2});

App.Config
-------------
    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
      <connectionStrings>
        <add name="MysqlConnection" providerName="MySql.Data.MySqlClient" connectionString="server=localhost;database=test;Uid=root;Pwd=xxx;port=3306;" />
      </connectionStrings>
    <system.data>
      <DbProviderFactories>
         <remove invariant="MySql.Data.MySqlClient" />
          <add name="MySQL Data Provider" invariant="MySql.Data.MySqlClient" description=".Net Framework Data Provider for MySQL" type="MySql.Data.MySqlClient.MySqlClientFactory, MySql.Data, Version=6.7.4.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d" />
      </DbProviderFactories>
    </system.data>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" />
    </startup>
    </configuration>
