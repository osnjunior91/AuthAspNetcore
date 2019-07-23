using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProductCatalog.Api.Mock.User
{
    public class UserRepositoryMock
    {
        public static User GetUser(string username, string password)
        {
            var JSONString = System.IO.File.ReadAllText("./Mock/User/Users.json");
            var user = JsonConvert.DeserializeObject<List<User>>(JSONString)
                .Find(x => x.Username == username && x.Password == password);

            return user;
        }
    }
}
