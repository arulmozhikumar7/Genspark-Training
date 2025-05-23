using fileAccessProxy.Interfaces;

namespace fileAccessProxy.Services
{
    public class RoleBasedAccessControl : IAccessControl
    {
        public string GetAccessLevel(string role)
        {
            return role switch
            {
                "admin" => "full",
                "user" => "limited",
                "guest" => "none",
                _ => "none"
            };
        }
    }
}
