using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using Utilities;

namespace AES256
{
	public static class EncriptadorAES
    {
		public static byte[] EncriptarConAES256(byte[] llave, String datos)
		{
			if (llave == null || llave.Length != 16)
				throw new ArgumentNullException("llave");
			if (datos == null || datos.Length > 100)
				throw new ArgumentNullException("datos");

			byte[] encrypted = null;
			try
			{
				encrypted = EncryptStringToBytes(datos,llave);
			}
			catch
			{
				//Captura excepcion y retorna null. Debe verificarse en el punto de llamada por nullidad de respuesta.
				return null;
			}

			return encrypted;
		}

		public static string DesencriptarConAES256(byte[] llave, byte[] datosEncriptados)
		{
			if (llave == null || llave.Length != 16)
				throw new ArgumentNullException("llave");
			if (datosEncriptados == null)
				throw new ArgumentNullException("datos");
	
			string resultado = null;
			try
			{
				resultado = DecryptStringFromBytes(datosEncriptados, llave);
			}
			catch
			{
				//Captura excepcion y retorna null. Debe verificarse en el punto de llamada por nullidad de respuesta.
				return null;
			}


			return resultado;
		}

		private static byte[] EncryptStringToBytes(string plainText, byte[] Key)
		{
			// Validacion
			if (plainText == null || plainText.Length <= 0)
				throw new ArgumentNullException("plainText");
			if (Key == null || Key.Length <= 0)
				throw new ArgumentNullException("Key");
			byte[] encrypted;
			
			// Create an RijndaelManaged object
			// with the specified key.
			using (RijndaelManaged rijAlg = new RijndaelManaged())
			{
				rijAlg.Key = Key;
				rijAlg.Mode = CipherMode.ECB;
				rijAlg.Padding = PaddingMode.PKCS7;

				// Create a decrytor to perform the stream transform.
				ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, null);

				// Create the streams used for encryption.
				using (MemoryStream msEncrypt = new MemoryStream())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
						{

							//Write all data to the stream.
							swEncrypt.Write(plainText);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}


			// Return the encrypted bytes from the memory stream.
			return encrypted;

		}

		private static string DecryptStringFromBytes(byte[] cipherText, byte[] Key)
		{
			// Validacion
			if (cipherText == null || cipherText.Length <= 0)
				throw new ArgumentNullException("cipherText");
			if (Key == null || Key.Length <= 0)
				throw new ArgumentNullException("Key");

			// Declare the string used to hold
			// the decrypted text.
			string plaintext = null;

			// Create an RijndaelManaged object
			// with the specified key.
			using (RijndaelManaged rijAlg = new RijndaelManaged())
			{
				rijAlg.Key = Key;
				rijAlg.Mode = CipherMode.ECB;
				rijAlg.Padding = PaddingMode.PKCS7;

				// Create a decrytor to perform the stream transform.
				ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, null);

				using (MemoryStream msDecrypt = new MemoryStream(cipherText))
				{
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader srDecrypt = new StreamReader(csDecrypt))
						{
							// Read the decrypted bytes from the decrypting stream
							// and place them in a string.
							plaintext = srDecrypt.ReadToEnd();
						}
					}
				}

			}

			return plaintext;

		}

    }
}
