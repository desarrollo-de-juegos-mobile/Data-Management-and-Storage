using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// Sistema de guardado con encriptación
/// Este script debe ser completado como parte del Ejercicio 3
/// </summary>
public class EncryptedSaveSystem_Base : MonoBehaviour
{
    // Referencias UI
    [Header("Referencias UI")]
    public InputField playerNameInput;
    public Text scoreText;
    public Text statusText;
    public InputField customSecretKeyInput;
    
    // Datos del juego
    private string playerName = "Player";
    private int score = 0;
    
    // Configuración de encriptación
    private string secretKey = "clave_secreta_por_defecto"; // Clave de encriptación (¡en un juego real no expondrías esto en el código!)
    private string savePath;
    private string checksumPath;
    
    // Clase para almacenar datos
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
            this.timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
    
    void Start()
    {
        // Establecer rutas de archivos
        savePath = Path.Combine(Application.persistentDataPath, "encrypted_save.dat");
        checksumPath = Path.Combine(Application.persistentDataPath, "checksum.dat");
        
        UpdateUI();
        
        // Mostrar clave por defecto en UI
        if (customSecretKeyInput != null)
        {
            customSecretKeyInput.text = secretKey;
        }
    }
    
    /// <summary>
    /// Encripta datos utilizando algoritmo AES
    /// </summary>
    private byte[] EncryptData(string data, string key)
    {
        // TODO: Implementar encriptación AES
        // 1. Crear instancia de Aes
        // 2. Generar clave derivada de la clave secreta
        // 3. Generar vector de inicialización (IV)
        // 4. Realizar encriptación
        // 5. Retornar datos encriptados
        
        Debug.Log("EncryptData() debe ser implementado");
        return null;
    }
    
    /// <summary>
    /// Desencripta datos utilizando algoritmo AES
    /// </summary>
    private string DecryptData(byte[] data, string key)
    {
        // TODO: Implementar desencriptación AES
        // 1. Crear instancia de Aes
        // 2. Generar clave derivada de la clave secreta
        // 3. Extraer vector de inicialización (IV) de los datos
        // 4. Realizar desencriptación
        // 5. Manejar errores de desencriptación
        // 6. Retornar datos desencriptados
        
        Debug.Log("DecryptData() debe ser implementado");
        return null;
    }
    
    /// <summary>
    /// Genera checksum para verificar integridad de datos
    /// </summary>
    private string GenerateChecksum(string data)
    {
        // TODO: Implementar generación de checksum usando SHA-256
        // 1. Crear instancia de SHA256
        // 2. Computar hash de los datos
        // 3. Convertir hash a string hexadecimal
        // 4. Retornar checksum
        
        Debug.Log("GenerateChecksum() debe ser implementado");
        return null;
    }
    
    /// <summary>
    /// Guarda datos encriptados
    /// </summary>
    public void SaveGame()
    {
        // TODO: Implementar guardado de datos encriptados
        // 1. Actualizar clave secreta desde UI si se proporcionó
        // 2. Crear objeto SaveData con datos actuales
        // 3. Serializar a JSON
        // 4. Generar checksum para verificación de integridad
        // 5. Encriptar datos JSON
        // 6. Guardar datos encriptados y checksum en archivos
        // 7. Manejar excepciones
        // 8. Actualizar status UI
        
        Debug.Log("SaveGame() debe ser implementado");
    }
    
    /// <summary>
    /// Carga datos encriptados
    /// </summary>
    public void LoadGame()
    {
        // TODO: Implementar carga de datos encriptados
        // 1. Verificar existencia de archivos
        // 2. Actualizar clave secreta desde UI si se proporcionó
        // 3. Leer datos encriptados y checksum
        // 4. Desencriptar datos
        // 5. Verificar integridad con checksum
        // 6. Deserializar JSON a objeto SaveData
        // 7. Actualizar datos del juego
        // 8. Actualizar UI
        // 9. Manejar excepciones y errores de integridad
        // 10. Actualizar status UI
        
        Debug.Log("LoadGame() debe ser implementado");
    }
    
    /// <summary>
    /// Reinicia datos a valores por defecto
    /// </summary>
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
        UpdateStatus("Datos reiniciados", Color.yellow);
    }
    
    /// <summary>
    /// Elimina archivos de guardado
    /// </summary>
    public void DeleteSaveFile()
    {
        // TODO: Implementar eliminación de archivos
        // 1. Verificar existencia de archivos
        // 2. Eliminar archivos si existen
        // 3. Manejar excepciones
        // 4. Actualizar status UI
        
        Debug.Log("DeleteSaveFile() debe ser implementado");
    }
    
    /// <summary>
    /// Corrompe intencionadamente el archivo para pruebas
    /// </summary>
    public void CorruptSaveFile()
    {
        // TODO: Implementar corrupción de archivo para pruebas
        // 1. Verificar existencia del archivo de guardado
        // 2. Leer datos
        // 3. Modificar algunos bytes para corromper el archivo
        // 4. Guardar archivo corrompido
        // 5. Actualizar status UI
        
        Debug.Log("CorruptSaveFile() debe ser implementado");
    }
    
    /// <summary>
    /// Actualiza elementos de UI
    /// </summary>
    private void UpdateUI()
    {
        // Actualizar UI con valores actuales
        if (scoreText != null) 
        {
            scoreText.text = "Puntuación: " + score;
        }
    }
    
    /// <summary>
    /// Actualiza texto de estado con color
    /// </summary>
    private void UpdateStatus(string message, Color color)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = color;
        }
    }
    
    // Métodos para pruebas y demostración
    
    /// <summary>
    /// Incrementa puntuación
    /// </summary>
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }
}