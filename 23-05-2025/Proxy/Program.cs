using System;
using fileAccessProxy.Models;
using fileAccessProxy.Services;
using fileAccessProxy.Interfaces;

namespace fileAccessProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "secure-data.txt";

            IAccessControl accessControl = new RoleBasedAccessControl();
            IFile file = new ProxyFile(accessControl, filePath);

            Console.Write("Enter your username: ");
           string username = Console.ReadLine() ?? string.Empty;

            string? role;
            while (true)
            {
                Console.Write("Enter your role (Admin/User/Guest): ");
                role = Console.ReadLine()?.Trim();

                if (role != null &&
                    (role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                     role.Equals("User", StringComparison.OrdinalIgnoreCase) ||
                     role.Equals("Guest", StringComparison.OrdinalIgnoreCase)))
                {
                    
                    role = char.ToUpper(role[0]) + role.Substring(1).ToLower();
                    break;
                }

                Console.WriteLine("[Invalid Role] Please enter only Admin, User, or Guest.\n");
            }

            var user = new User(username, role);

            Console.WriteLine($"\nUser: {user.Username} | Role: {user.Role}");
            file.Read(user);
        }
    }
}
