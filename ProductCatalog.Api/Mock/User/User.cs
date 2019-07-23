using System;
using System.Collections.Generic;

namespace ProductCatalog.Api.Mock.User
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
            Permissions = new List<Permission>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
