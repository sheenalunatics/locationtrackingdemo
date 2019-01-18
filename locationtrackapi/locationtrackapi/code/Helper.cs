using System;
using System.Security.Cryptography;
using System.Text;

namespace locationtrackapi
{
    public static class Helper
    {
      public static string EncodePassword(string password, string passwordSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(passwordSalt);
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] bytesToHash = new byte[saltBytes.Length + passwordBytes.Length];
            saltBytes.CopyTo(bytesToHash, 0);
            passwordBytes.CopyTo(bytesToHash, saltBytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] hash = algorithm.ComputeHash(bytesToHash);
            return hash.Length > 0 ? Convert.ToBase64String(hash) : "";
        }

    }
}