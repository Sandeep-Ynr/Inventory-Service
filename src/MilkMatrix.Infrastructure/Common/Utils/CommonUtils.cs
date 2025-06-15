using System.Security.Cryptography;
using System.Text;
using AutoMapper;

namespace MilkMatrix.Infrastructure.Common.Utils;

public static class CommonUtils
{
    #region Encryption
    public static string EncryptString(this string key, string plainText)
    {
        byte[] iv = new byte[16];
        byte[] array;
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new((Stream)cryptoStream))
            {
                streamWriter.Write(plainText);
            }
            array = memoryStream.ToArray();
        }
        return Convert.ToBase64String(array);
    }
    public static string DecryptString(this string key, string cipherText)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = iv;
        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using MemoryStream memoryStream = new MemoryStream(buffer);
        using CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
        using StreamReader streamReader = new StreamReader((Stream)cryptoStream);
        return streamReader.ReadToEnd();
    }
    public static string EncodeSHA512(this string data)
    {
        string hash = string.Empty;
        if (data != "")
        {
            using SHA512 sha512Hash = SHA512.Create();
            //From String to byte array
            byte[] sourceBytes = Encoding.UTF8.GetBytes(data);
            byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
            hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            return hash;
        }
        else
            return hash;
    }
    #endregion

    #region Generator
    public static string GetConcatenatedString(this string hostName, string userKey, int? businessId)
    {
        var tokenKey = $"{hostName.ToLower()}|{userKey.ToLower()}|{DateTime.Now.ToString("yyyyMMdd")}";
        if (businessId != null)
        {
            tokenKey += $"|{businessId}";
        }
        return tokenKey;
    }

    public static int GenerateRendomNumber(int length)
    {
        int rendomNumber = 0;
        const string chars = "0123456789";
        var random = new Random();
        var output = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        rendomNumber = Convert.ToInt32(output);
        return rendomNumber;
    }
    #endregion



    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        using var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            action(enumerator.Current);
        }
    }

    public static string MaskString(this string keyToMask, int startVisible = 2, int endVisible = 2, char maskChar = '*')
    {
        if (keyToMask.Length <= startVisible + endVisible)
        {
            // If the string is too short, return the original string
            return keyToMask;
        }

        int maskLength = keyToMask.Length - startVisible - endVisible;
        string maskedSubstring = new string(maskChar, maskLength);

        // Concatenate the visible portions with the masked substring
        string maskedString = keyToMask.Substring(0, startVisible) +
                              maskedSubstring +
                              keyToMask.Substring(keyToMask.Length - endVisible);

        return maskedString;
    }

    /// <summary>
    /// Format String message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="arg"></param>
    /// <returns>A formatted string</returns>
    public static string FormatString(this string message, string arg) => string.Format(message, arg);
}
public static class AutoMapperExtensions
{
    public static TResult MapWithOptions<TResult, T1>(this IMapper mapper, T1 model, Dictionary<string, object> options) =>
        mapper.Map<TResult>(model, opt => options.ForEach(x => opt.Items[x.Key] = x.Value))!;
}

