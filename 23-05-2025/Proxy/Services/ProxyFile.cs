using System;
using fileAccessProxy.Interfaces;
using fileAccessProxy.Models;

namespace fileAccessProxy.Services
{
    public class ProxyFile : IFile
    {
        private readonly IAccessControl _accessControl;
        private readonly string _filePath;

        public ProxyFile(IAccessControl accessControl, string filePath)
        {
            _accessControl = accessControl;
            _filePath = filePath;
        }

        public void Read(User user)
        {
            string accessLevel = _accessControl.GetAccessLevel(user.Role.ToLower());

            switch (accessLevel)
            {
                case "full":
                    Console.WriteLine($"[Access Granted]\n{FileReader.Instance.ReadSensitiveContent(_filePath)}");
                    Console.WriteLine($"[METADATA]\n{FileReader.Instance.ReadMetadata(_filePath)}");
                    break;
                case "limited":
                    Console.WriteLine($"[Limited Access]\n{FileReader.Instance.ReadMetadata(_filePath)}");
                    break;
                case "none":
                default:
                    Console.WriteLine("[Access Denied] You do not have permission to read this file.");
                    break;
            }
        }
    }
}
