using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.UI;

public class EncryptedSaveSystem : MonoBehaviour
{
    // Referencias UI
    public InputField playerNameInput;
    public Text scoreText;
    public Text statusText;
    public InputField customSecretKeyInput;
    
    // Datos del juego
    private string playerName = "Player";
    private int score = 0;
    
    // Configuración de encriptación
    private string secretKey = "yjd7HnM90!xpQw54"; // Clave por defecto, mejor práctica es generarla y almacenarla de forma segura
    private string savePath;
    private string checksumPath;
    
    [System.Serializable]
    private class SaveData
    {
        public string playerName;
        public int score;
        public long timestamp;
        
        public SaveData(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
            this.timestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
    
    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "encrypted_save.dat");
        checksumPath = Path.Combine(Application.persistentDataPath, "checksum.dat");
        
        UpdateUI();
        
        if (customSecretKeyInput != null)
        {
            customSecretKeyInput.text = secretKey;
        }
    }
    
    // Para demostración
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }
    
    // Encrypt/Decrypt Functions
    private byte[] EncryptData(string data, string key)
    {
        byte[] encrypted;
        
        using (Aes aes = Aes.Create())
        {
            // Obtener bytes de clave y crear derivada de tamaño adecuado
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (SHA256 sha256 = SHA256.Create())
            {
                keyBytes = sha256.ComputeHash(keyBytes);
            }
            
            aes.Key = keyBytes;
            aes.GenerateIV();
            
            // Crear encriptador y encriptar datos
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            
            using (MemoryStream ms = new MemoryStream())
            {
                // Primero escribir el IV (vector de inicialización)
                ms.Write(aes.IV, 0, aes.IV.Length);
                
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(data);
                    }
                }
                
                encrypted = ms.ToArray();
            }
        }
        
        return encrypted;
    }
    
    private string DecryptData(byte[] data, string key)
    {
        string decrypted = null;
        
        using (Aes aes = Aes.Create())
        {
            // Obtener bytes de clave y crear derivada de tamaño adecuado
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (SHA256 sha256 = SHA256.Create())
            {
                keyBytes = sha256.ComputeHash(keyBytes);
            }
            
            aes.Key = keyBytes;
            
            // El IV se guarda al principio de los datos encriptados
            byte[] iv = new byte[aes.BlockSize / 8];
            Array.Copy(data, 0, iv, 0, iv.Length);
            aes.IV = iv;
            
            // Crear descifrador
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Copiar datos encriptados a MemoryStream, saltando el IV
                    ms.Write(data, iv.Length, data.Length - iv.Length);
                    ms.Position = 0;
                    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (CryptographicException e)
            {
                Debug.LogError("Decryption failed: " + e.Message);
                return null;
            }
        }
        
        return decrypted;
    }
    
    // Generar checksum para verificar integridad
    private string GenerateChecksum(string data)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
    
    // Sistema de guardado
    public void SaveGame()
    {
        try
        {
            // Actualizar clave si se proporcionó una personalizada
            if (customSecretKeyInput != null && !string.IsNullOrEmpty(customSecretKeyInput.text))
            {
                secretKey = customSecretKeyInput.text;
            }
            
            // Obtener nombre del input si existe
            if (playerNameInput != null && !string.IsNullOrEmpty(playerNameInput.text))
            {
                playerName = playerNameInput.text;
            }
            
            // Crear objeto para guardar
            SaveData saveData = new SaveData(playerName, score);
            
            // Convertir a JSON
            string jsonData = JsonUtility.ToJson(saveData);
            
            // Generar checksum para verificar integridad
            string checksum = GenerateChecksum(jsonData);
            
            // Encriptar datos
            byte[] encryptedData = EncryptData(jsonData, secretKey);
            
            // Guardar datos encriptados
            File.WriteAllBytes(savePath, encryptedData);
            
            // Guardar checksum (también podría encriptarse)
            File.WriteAllText(checksumPath, checksum);
            
            UpdateStatus("Game saved successfully!", Color.green);
            Debug.Log("Game saved to: " + savePath);
        }
        catch (Exception e)
        {
            UpdateStatus("Error saving game: " + e.Message, Color.red);
            Debug.LogError("Error saving game: " + e.Message);
        }
    }
    
    public void LoadGame()
    {
        try
        {
            // Verificar si el archivo existe
            if (!File.Exists(savePath) || !File.Exists(checksumPath))
            {
                UpdateStatus("Save file not found!", Color.yellow);
                return;
            }
            
            // Actualizar clave si se proporcionó una personalizada
            if (customSecretKeyInput != null && !string.IsNullOrEmpty(customSecretKeyInput.text))
            {
                secretKey = customSecretKeyInput.text;
            }
            
            // Leer datos encriptados
            byte[] encryptedData = File.ReadAllBytes(savePath);
            
            // Leer checksum guardado
            string savedChecksum = File.ReadAllText(checksumPath);
            
            // Descifrar datos
            string jsonData = DecryptData(encryptedData, secretKey);
            
            if (jsonData == null)
            {
                UpdateStatus("Decryption failed! Wrong key?", Color.red);
                return;
            }
            
            // Verificar integridad con checksum
            string calculatedChecksum = GenerateChecksum(jsonData);
            if (savedChecksum != calculatedChecksum)
            {
                UpdateStatus("Data integrity check failed! File may be corrupted or tampered with.", Color.red);
                return;
            }
            
            // Deserializar JSON
            SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);
            
            // Actualizar datos del juego
            playerName = saveData.playerName;
            score = saveData.score;
            
            // Actualizar input de nombre si existe
            if (playerNameInput != null)
            {
                playerNameInput.text = playerName;
            }
            
            UpdateUI();
            UpdateStatus("Game loaded successfully!", Color.green);
            Debug.Log("Game loaded from: " + savePath);
        }
        catch (Exception e)
        {
            UpdateStatus("Error loading game: " + e.Message, Color.red);
            Debug.LogError("Error loading game: " + e.Message);
        }
    }
    
    public void ResetGame()
    {
        // Reiniciar variables
        playerName = "Player";
        score = 0;
        
        // Actualizar input de nombre si existe
        if (playerNameInput != null)
        {
            playerNameInput.text = playerName;
        }
        
        UpdateUI();
        UpdateStatus("Game reset!", Color.yellow);
    }
    

    public void DeleteSaveFile()
    {
        try
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            
            if (File.Exists(checksumPath))
            {
                File.Delete(checksumPath);
            }
            
            UpdateStatus("Save files deleted!", Color.yellow);
            Debug.Log("Save files deleted");
        }
        catch (Exception e)
        {
            UpdateStatus("Error deleting save files: " + e.Message, Color.red);
            Debug.LogError("Error deleting save files: " + e.Message);
        }
    }
    
    // Método para corromper intencionadamente el archivo de guardado (para testing)
    public void CorruptSaveFile()
    {
        try
        {
            if (File.Exists(savePath))
            {
                // Leer el archivo
                byte[] data = File.ReadAllBytes(savePath);
                
                // Modificar algunos bytes para corromperlo
                if (data.Length > 20)
                {
                    for (int i = 15; i < 20; i++)
                    {
                        data[i] = 0xFF;
                    }
                    
                    // Sobrescribir con datos corrompidos
                    File.WriteAllBytes(savePath, data);
                    UpdateStatus("Save file corrupted intentionally for testing!", Color.yellow);
                }
            }
            else
            {
                UpdateStatus("No save file to corrupt!", Color.yellow);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error corrupting file: " + e.Message);
        }
    }
    
    private void UpdateUI()
    {
        // Actualizar UI con valores actuales
        if (scoreText != null) scoreText.text = "Score: " + score;
    }
    
    private void UpdateStatus(string message, Color color)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = color;
        }
    }
}