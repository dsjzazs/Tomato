using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net
{
    public class User : IEqualityComparer<User>
    {
        private User() { }
        public Guid Session { get; private set; }
        public bool IsLogin { get; }
        public static User Create()
        {
            return new User() { Session = Guid.NewGuid() };
        }
        public bool Equals(User x, User y)
        {
            return x.Session == y.Session;
        }
        public int GetHashCode(User obj)
        {
            return obj.GetHashCode();
        }
    }
}
