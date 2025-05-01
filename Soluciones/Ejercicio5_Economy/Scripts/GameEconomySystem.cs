using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class GameEconomySystem : MonoBehaviour
{
    // Referencias UI
    public Text goldText;          // Soft currency
    public Text gemsText;          // Hard currency
    public Text statusText;
    public Text transactionLogText;
    
    // Balances de monedas
    private int gold = 0;
    private int gems = 0;
    
    // Datos del juego
    private string playerId;
    private bool isInitialized = false;
    
    // Ruta de guardado
    private string economyDataPath;
    private string checksumPath;
    
    // Clave para encriptación (en producción debería ser más segura)
    private string encryptionKey = "3c0n0mY$yS8em2023K3y!";
    
    // Datos económicos
    [System.Serializable]
    private class EconomyData
    {
        public int gold;
        public int gems;
        public List<Transaction> transactions;
        public string playerId;
        public long lastModified;
        
        public EconomyData(int gold, int gems, string playerId)
        {
            this.gold = gold;
            this.gems = gems;
            this.playerId = playerId;
            this.transactions = new List<Transaction>();
            this.lastModified = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
    
    [System.Serializable]
    private class Transaction
    {
        public string id;
        public string type;        // "gold_earned", "gold_spent", "gems_purchased", "gems_spent"
        public int amount;
        public string itemId;      // Opcional, para compras de items
        public long timestamp;
        
        public Transaction(string type, int amount, string itemId = "")
        {
            this.id = Guid.NewGuid().ToString();
            this.type = type;
            this.amount = amount;
            this.itemId = itemId;
            this.timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
    
    // Clases para la tienda
    [System.Serializable]
    public class StoreItem
    {
        public string id;
        public string name;
        public string description;
        public string currencyType;  // "gold" o "gems"
        public int price;
        public string category;
        
        public StoreItem(string id, string name, string description, string currencyType, int price, string category)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.currencyType = currencyType;
            this.price = price;
            this.category = category;
        }
    }
    
    // Lista de items disponibles en la tienda
    private List<StoreItem> storeItems = new List<StoreItem>();
    
    // Datos de economía actuales
    private EconomyData economyData;
    
    private void Start()
    {
        // Generar ID de jugador si no existe
        playerId = PlayerPrefs.GetString("PlayerId", "");
        if (string.IsNullOrEmpty(playerId))
        {
            playerId = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("PlayerId", playerId);
            PlayerPrefs.Save();
        }
        
        // Establecer rutas de archivos
        economyDataPath = Path.Combine(Application.persistentDataPath, "economy_data.dat");
        checksumPath = Path.Combine(Application.persistentDataPath, "economy_checksum.dat");
        
        // Inicializar tienda con algunos items
        InitializeStoreItems();
        
        // Cargar datos económicos
        LoadEconomyData();
        
        isInitialized = true;
    }
    
    private void InitializeStoreItems()
    {
        // Items comprables con moneda soft (oro)
        storeItems.Add(new StoreItem("health_potion", "Health Potion", "Restores 50 health points", "gold", 100, "consumable"));
        storeItems.Add(new StoreItem("mana_potion", "Mana Potion", "Restores 30 mana points", "gold", 80, "consumable"));
        storeItems.Add(new StoreItem("basic_sword", "Basic Sword", "+5 Attack", "gold", 500, "weapon"));
        
        // Items comprables con moneda hard (gemas)
        storeItems.Add(new StoreItem("premium_skin", "Premium Character Skin", "Exclusive skin for your character", "gems", 50, "cosmetic"));
        storeItems.Add(new StoreItem("epic_sword", "Epic Sword", "+20 Attack, +5% Critical", "gems", 100, "weapon"));
        storeItems.Add(new StoreItem("xp_boost", "XP Boost", "Double XP for 1 hour", "gems", 30, "booster"));
    }
    
    public void AddGold(int amount)
    {
        if (!isInitialized) return;
        
        if (amount <= 0)
        {
            Debug.LogWarning("AddGold: amount must be positive");
            return;
        }
        
        // Añadir oro
        gold += amount;
        
        // Registrar transacción
        Transaction transaction = new Transaction("gold_earned", amount);
        economyData.transactions.Add(transaction);
        
        // Actualizar datos y UI
        economyData.gold = gold;
        economyData.lastModified = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        SaveEconomyData();
        UpdateUI();
        
        // Mostrar mensaje
        UpdateStatus($"Earned {amount} gold!", Color.green);
        AddToTransactionLog($"+ {amount} gold");
    }
    
    public void SpendGold(int amount, string itemId = "")
    {
        if (!isInitialized) return;
        
        if (amount <= 0)
        {
            Debug.LogWarning("SpendGold: amount must be positive");
            return;
        }
        
        // Verificar si hay suficiente oro
        if (gold >= amount)
        {
            // Restar oro
            gold -= amount;
            
            // Registrar transacción
            Transaction transaction = new Transaction("gold_spent", amount, itemId);
            economyData.transactions.Add(transaction);
            
            // Actualizar datos y UI
            economyData.gold = gold;
            economyData.lastModified = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            
            SaveEconomyData();
            UpdateUI();
            
            // Mostrar mensaje
            UpdateStatus($"Spent {amount} gold" + (itemId != "" ? $" on {itemId}" : ""), Color.yellow);
            AddToTransactionLog($"- {amount} gold" + (itemId != "" ? $" ({itemId})" : ""));
            
            return;
        }
        
        // No hay suficiente oro
        UpdateStatus("Not enough gold!", Color.red);
    }
    
    public void AddGems(int amount)
    {
        if (!isInitialized) return;
        
        if (amount <= 0)
        {
            Debug.LogWarning("AddGems: amount must be positive");
            return;
        }
        
        // Añadir gemas
        gems += amount;
        
        // Registrar transacción
        Transaction transaction = new Transaction("gems_purchased", amount);
        economyData.transactions.Add(transaction);
        
        // Actualizar datos y UI
        economyData.gems = gems;
        economyData.lastModified = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        SaveEconomyData();
        UpdateUI();
        
        // Simular verificación con servidor para compras premium
        StartCoroutine(SimulateServerVerification("add_gems", amount));
        
        // Mostrar mensaje
        UpdateStatus($"Purchased {amount} gems!", Color.green);
        AddToTransactionLog($"+ {amount} gems (purchased)");
    }
    
    public void SpendGems(int amount, string itemId = "")
    {
        if (!isInitialized) return;
        
        if (amount <= 0)
        {
            Debug.LogWarning("SpendGems: amount must be positive");
            return;
        }
        
        // Verificar si hay suficientes gemas
        if (gems >= amount)
        {
            // Verificar con "servidor" antes de completar la transacción
            StartCoroutine(SimulateServerVerification("spend_gems", amount, itemId, () => {
                // Callback después de verificación exitosa
                // Restar gemas
                gems -= amount;
                
                // Registrar transacción
                Transaction transaction = new Transaction("gems_spent", amount, itemId);
                economyData.transactions.Add(transaction);
                
                // Actualizar datos y UI
                economyData.gems = gems;
                economyData.lastModified = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                
                SaveEconomyData();
                UpdateUI();
                
                // Mostrar mensaje
                UpdateStatus($"Spent {amount} gems" + (itemId != "" ? $" on {itemId}" : ""), Color.yellow);
                AddToTransactionLog($"- {amount} gems" + (itemId != "" ? $" ({itemId})" : ""));
            }));
            
            return;
        }
        
        // No hay suficientes gemas
        UpdateStatus("Not enough gems!", Color.red);
    }
    
    // Métodos para la tienda
    public void BuyItem(string itemId)
    {
        if (!isInitialized) return;
        
        // Buscar el item en la tienda
        StoreItem item = storeItems.Find(i => i.id == itemId);
        
        if (item == null)
        {
            UpdateStatus("Item not found!", Color.red);
            return;
        }
        
        // Comprar según el tipo de moneda
        if (item.currencyType == "gold")
        {
            SpendGold(item.price, item.id);
        }
        else if (item.currencyType == "gems")
        {
            SpendGems(item.price, item.id);
        }
    }
    
    // Simulación de verificación con servidor
    private IEnumerator SimulateServerVerification(string action, int amount, string itemId = "", Action onSuccess = null)
    {
        UpdateStatus("Verifying transaction with server...", Color.yellow);
        
        // Simular latencia de red
        yield return new WaitForSeconds(1.0f);
        
        // Simular verificación del servidor (en un juego real, esto sería una llamada a un backend)
        bool verificationSuccessful = true;
        
        // Falsificar una falla ocasional para probar el manejo de errores (10% de probabilidad)
        if (UnityEngine.Random.value < 0.1f)
        {
            verificationSuccessful = false;
        }
        
        if (verificationSuccessful)
        {
            UpdateStatus("Transaction verified!", Color.green);
            
            // Ejecutar callback de éxito si existe
            if (onSuccess != null)
            {
                onSuccess.Invoke();
            }
        }
        else
        {
            UpdateStatus("Server verification failed! Transaction cancelled.", Color.red);
            
            // En caso de fallo, podríamos revertir cambios locales o intentar de nuevo
        }
    }
    
    // Métodos de encriptación
    private byte[] EncryptData(string data)
    {
        byte[] encrypted;
        
        using (Aes aes = Aes.Create())
        {
            // Derivar clave
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
            using (SHA256 sha256 = SHA256.Create())
            {
                keyBytes = sha256.ComputeHash(keyBytes);
            }
            
            aes.Key = keyBytes;
            aes.GenerateIV();
            
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            
            using (MemoryStream ms = new MemoryStream())
            {
                // Guardar IV al inicio del stream
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
    
    private string DecryptData(byte[] data)
    {
        string decrypted = null;
        
        using (Aes aes = Aes.Create())
        {
            // Derivar clave
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
            using (SHA256 sha256 = SHA256.Create())
            {
                keyBytes = sha256.ComputeHash(keyBytes);
            }
            
            aes.Key = keyBytes;
            
            // Leer IV del inicio de los datos
            byte[] iv = new byte[aes.BlockSize / 8];
            Array.Copy(data, 0, iv, 0, iv.Length);
            aes.IV = iv;
            
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Copiar datos sin el IV
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
    
    // Guardar y cargar datos
    private void SaveEconomyData()
    {
        try
        {
            // Actualizar datos antes de guardar
            economyData.gold = gold;
            economyData.gems = gems;
            economyData.playerId = playerId;
            economyData.lastModified = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            
            // Convertir a JSON
            string jsonData = JsonUtility.ToJson(economyData);
            
            // Generar checksum
            string checksum = GenerateChecksum(jsonData);
            
            // Encriptar datos
            byte[] encryptedData = EncryptData(jsonData);
            
            // Guardar datos
            File.WriteAllBytes(economyDataPath, encryptedData);
            File.WriteAllText(checksumPath, checksum);
            
            Debug.Log("Economy data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving economy data: " + e.Message);
            UpdateStatus("Error saving economy data!", Color.red);
        }
    }
    
    private void LoadEconomyData()
    {
        try
        {
            // Verificar si existen los archivos
            if (File.Exists(economyDataPath) && File.Exists(checksumPath))
            {
                // Leer datos y checksum
                byte[] encryptedData = File.ReadAllBytes(economyDataPath);
                string savedChecksum = File.ReadAllText(checksumPath);
                
                // Descifrar datos
                string jsonData = DecryptData(encryptedData);
                
                if (jsonData == null)
                {
                    throw new Exception("Failed to decrypt data. Possible tampering.");
                }
                
                // Verificar integridad con checksum
                string calculatedChecksum = GenerateChecksum(jsonData);
                if (savedChecksum != calculatedChecksum)
                {
                    throw new Exception("Data integrity check failed. File may be corrupted or tampered with.");
                }
                
                // Deserializar datos
                economyData = JsonUtility.FromJson<EconomyData>(jsonData);
                
                // Verificar que el ID del jugador coincide
                if (economyData.playerId != playerId)
                {
                    Debug.LogWarning("Player ID mismatch. Using saved data but with current player ID.");
                    economyData.playerId = playerId;
                }
                
                // Actualizar variables
                gold = economyData.gold;
                gems = economyData.gems;
                
                // Actualizar UI
                UpdateUI();
                
                Debug.Log("Economy data loaded successfully.");
                UpdateStatus("Economy data loaded.", Color.green);
            }
            else
            {
                // Inicializar con valores por defecto
                economyData = new EconomyData(100, 10, playerId); // Dar algo inicial
                gold = economyData.gold;
                gems = economyData.gems;
                
                // Guardar datos iniciales
                SaveEconomyData();
                
                // Actualizar UI
                UpdateUI();
                
                Debug.Log("Initialized new economy data.");
                UpdateStatus("New player economy initialized.", Color.green);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading economy data: " + e.Message);
            UpdateStatus("Error loading economy data: " + e.Message, Color.red);
            
            // Inicializar con valores por defecto en caso de error
            economyData = new EconomyData(100, 10, playerId);
            gold = economyData.gold;
            gems = economyData.gems;
            
            // Actualizar UI
            UpdateUI();
        }
    }
    
    // Reiniciar datos (para testing)
    public void ResetEconomyData()
    {
        try
        {
            // Borrar archivos si existen
            if (File.Exists(economyDataPath))
            {
                File.Delete(economyDataPath);
            }
            
            if (File.Exists(checksumPath))
            {
                File.Delete(checksumPath);
            }
            
            // Inicializar con valores por defecto
            economyData = new EconomyData(100, 10, playerId);
            gold = economyData.gold;
            gems = economyData.gems;
            
            // Guardar datos iniciales
            SaveEconomyData();
            
            // Actualizar UI
            UpdateUI();
            
            Debug.Log("Economy data reset.");
            UpdateStatus("Economy data reset to default.", Color.yellow);
            ClearTransactionLog();
        }
        catch (Exception e)
        {
            Debug.LogError("Error resetting economy data: " + e.Message);
            UpdateStatus("Error resetting economy data!", Color.red);
        }
    }
    
    // UI
    private void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + gold;
        }
        
        if (gemsText != null)
        {
            gemsText.text = "Gems: " + gems;
        }
    }
    
    private void UpdateStatus(string message, Color color)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = color;
        }
    }
    
    private void AddToTransactionLog(string message)
    {
        if (transactionLogText != null)
        {
            // Añadir timestamp al mensaje
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            string logEntry = $"[{timestamp}] {message}\n";
            
            // Añadir al inicio del log (más recientes arriba)
            transactionLogText.text = logEntry + transactionLogText.text;
            
            // Limitar el tamaño del log (opcional)
            if (transactionLogText.text.Length > 2000)
            {
                transactionLogText.text = transactionLogText.text.Substring(0, 2000) + "...";
            }
        }
    }
    
    private void ClearTransactionLog()
    {
        if (transactionLogText != null)
        {
            transactionLogText.text = "";
        }
    }
}