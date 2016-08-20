using Spider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
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
        }
    }
}
