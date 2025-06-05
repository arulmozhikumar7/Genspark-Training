namespace HospitalManagementAPI.Interfaces
{
    public interface IEncryptionService
    {
        void CreatePasswordHash(string password, out byte[] hash, out byte[] salt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    }
}
