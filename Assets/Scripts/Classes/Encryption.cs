//================================================================================
//
//  Encryption
//
//  暗号化・複合化のクラス
//
//================================================================================

using System.IO;
using System.Security.Cryptography;

public class Encryption{

    /// <summary>
    /// 初期化ベクトル
    /// </summary>
    private const string initializationVector = @"jCddaOybW3zEh0Kl";
    /// <summary>
    /// 共有鍵
    /// </summary>
    private const string sharedKey = @"giVJrbHRlWBDIggF";
 
    /// <summary>
    /// 暗号化処理
    /// </summary>
    public static string Encrypt(string text){
        RijndaelManaged aes = new RijndaelManaged();
        aes.BlockSize = 128;
        aes.KeySize = 128;
        aes.Padding = PaddingMode.Zeros;
        aes.Mode = CipherMode.CBC;
        aes.Key = System.Text.Encoding.UTF8.GetBytes(sharedKey);
        aes.IV = System.Text.Encoding.UTF8.GetBytes(initializationVector);
 
        ICryptoTransform encrypt = aes.CreateEncryptor();
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write);
 
        byte[] text_bytes = System.Text.Encoding.UTF8.GetBytes(text);
 
        cryptStream.Write(text_bytes, 0, text_bytes.Length);
        cryptStream.FlushFinalBlock();
 
        byte[] encrypted = memoryStream.ToArray();
 
        return System.Convert.ToBase64String(encrypted);
    }
 
    /// <summary>
    /// 複合化処理
    /// </summary>
    public static string Decrypt(string cryptText){
        RijndaelManaged aes = new RijndaelManaged();
        aes.BlockSize = 128;
        aes.KeySize = 128;
        aes.Padding = PaddingMode.Zeros;
        aes.Mode = CipherMode.CBC;
        aes.Key = System.Text.Encoding.UTF8.GetBytes(sharedKey);
        aes.IV = System.Text.Encoding.UTF8.GetBytes(initializationVector);
 
        ICryptoTransform decryptor = aes.CreateDecryptor();
 
        byte[] encrypted = System.Convert.FromBase64String(cryptText);
        byte[] planeText = new byte[encrypted.Length];
 
        MemoryStream memoryStream = new MemoryStream(encrypted);
        CryptoStream cryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
 
        cryptStream.Read(planeText, 0, planeText.Length);
 
        return System.Text.Encoding.UTF8.GetString(planeText);
    }

}
