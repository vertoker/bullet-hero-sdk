using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BHSDK.Services
{
    public class CryptographyService
    {
        public enum Algorithm
        {
            None = 0,
            AES = 1,
        }

        private readonly Random _random;

        public CryptographyService(int seed = 1337)
        {
            _random = new Random(seed);
        }

        public string Encrypt(string data, string password, Algorithm algorithm = Algorithm.AES)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            var result = Encrypt(bytes, passwordBytes, algorithm);
            return Convert.ToBase64String(result);
        }
        public string Decrypt(string data, string password, Algorithm algorithm = Algorithm.AES)
        {
            var bytes = Convert.FromBase64String(data);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            var result = Decrypt(bytes, passwordBytes, algorithm);
            return Encoding.UTF8.GetString(result);
        }
        public byte[] Encrypt(byte[] data, byte[] password, Algorithm algorithm = Algorithm.AES)
        {
            return algorithm switch
            {
                Algorithm.None => data,
                Algorithm.AES => EncryptAES(data, password),
                _ => throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null)
            };
        }
        public byte[] Decrypt(byte[] data, byte[] password, Algorithm algorithm)
        {
            return algorithm switch
            {
                Algorithm.None => data,
                Algorithm.AES => DecryptAES(data, password),
                _ => throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null)
            };
        }
        
        // Game Encryption
        
        public const int KeySize = 32; // 256 bit
        public const int IvSize = 16; // 128 bit
        public const int SaltSize = 32; // 256 bit
        public const int Iterations = 100_000; // Rfc2898DeriveBytes iterations

        public byte[] EncryptAES(byte[] bytes, byte[] passwordBytes)
        {
            var salt = GetRandomBytes(SaltSize);
            var iv = GetRandomBytes(IvSize);
            
            using var keyDerivation = new Rfc2898DeriveBytes(passwordBytes, 
                salt, Iterations, HashAlgorithmName.SHA256);
            var key = keyDerivation.GetBytes(KeySize);
            
            using var aes = CreateAES();
            using var encryptor = aes.CreateEncryptor(key, iv);
            
            using var memoryStream = new MemoryStream();
            memoryStream.Write(salt, 0, salt.Length);
            memoryStream.Write(iv, 0, iv.Length);
            
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            
            return memoryStream.ToArray();
        }
        public byte[] DecryptAES(byte[] bytes, byte[] passwordBytes)
        {
            var salt = new byte[SaltSize];
            var iv = new byte[IvSize];
            var cipherBytes = new byte[bytes.Length - SaltSize - IvSize];
            
            // take salt and iv from beginning
            Buffer.BlockCopy(bytes, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(bytes, SaltSize, iv, 0, IvSize);
            Buffer.BlockCopy(bytes, SaltSize + IvSize, cipherBytes, 0, cipherBytes.Length);
            
            using var keyDerivation = new Rfc2898DeriveBytes(passwordBytes, 
                salt, Iterations, HashAlgorithmName.SHA256);
            var key = keyDerivation.GetBytes(KeySize);
            
            using var aes = CreateAES();
            using var decryptor = aes.CreateDecryptor(key, iv);
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write);
            
            cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
            cryptoStream.FlushFinalBlock();
            
            return memoryStream.ToArray();
        }

        private static Aes CreateAES()
        {
            var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }

        private byte[] GetRandomBytes(int length)
        {
            var bytes = new byte[length];
            _random.NextBytes(bytes);
            return bytes;
        }
    }
}