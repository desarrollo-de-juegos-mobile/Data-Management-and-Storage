using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Sistema de guardado usando serialización JSON
/// Este script debe ser completado como parte del Ejercicio 2
/// </summary>
public class JSONSaveSystem_Base : MonoBehaviour
{
    // Referencias UI
    [Header("Referencias UI")]
    public InputField playerNameInput;
    public Text playerStatsText;
    public Text inventoryText;
    public Text questsText;
    public Text statusText;
    
    // Ruta del archivo de guardado
    private string savePath;
    
    // Clases para almacenar datos (deben ser completadas)
    [System.Serializable]
    public class PlayerProfile
    {
        public string name;
        public int level;
        public int experience;
        
        // TODO: Implementar constructor y propiedades adicionales si es necesario
    }
    
    [System.Serializable]
    public class GameItem
    {
        public string id;
        public string name;
        public int quantity;
        
        // TODO: Implementar constructor y propiedades adicionales si es necesario
    }
    
    [System.Serializable]
    public class GameState
    {
        public string currentLevel;
        public List<string> completedQuests;
        
        // TODO: Implementar constructor y propiedades adicionales si es necesario
    }
    
    [System.Serializable]
    public class SaveData
    {
        public PlayerProfile player;
        public List<GameItem> inventory;
        public GameState gameState;
        public long savedTimestamp;
        
        // TODO: Implementar constructor y propiedades adicionales si es necesario
    }
    
    // Datos actuales del juego
    private SaveData currentSaveData;
    
    void Start()
    {
        // Establecer ruta del archivo guardado
        savePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        Debug.Log("Ruta de guardado: " + savePath);
        
        // TODO: Inicializar datos por defecto
        // TODO: Cargar datos guardados si existen
        
        // Actualizar UI
        UpdateUI();
    }
    
    /// <summary>
    /// Guarda los datos del juego en un archivo JSON
    /// </summary>
    public void SaveGame()
    {
        // TODO: Implementar guardado de datos en formato JSON
        // 1. Actualizar los datos actuales desde la UI
        // 2. Convertir los datos a formato JSON usando JsonUtility
        // 3. Guardar el JSON en un archivo en la ruta 'savePath'
        // 4. Manejar posibles excepciones
        // 5. Actualizar el texto de estado
        
        Debug.Log("SaveGame() debe ser implementado");
    }
    
    /// <summary>
    /// Carga los datos del juego desde un archivo JSON
    /// </summary>
    public void LoadGame()
    {
        // TODO: Implementar carga de datos desde archivo JSON
        // 1. Verificar si existe el archivo en 'savePath'
        // 2. Leer el contenido del archivo
        // 3. Deserializar el JSON a un objeto SaveData usando JsonUtility
        // 4. Actualizar los datos actuales con los cargados
        // 5. Actualizar la UI
        // 6. Manejar posibles excepciones
        // 7. Actualizar el texto de estado
        
        Debug.Log("LoadGame() debe ser implementado");
    }
    
    /// <summary>
    /// Elimina el archivo de guardado y reinicia los datos
    /// </summary>
    public void DeleteSaveFile()
    {
        // TODO: Implementar eliminación del archivo de guardado
        // 1. Verificar si existe el archivo en 'savePath'
        // 2. Eliminar el archivo si existe
        // 3. Reiniciar los datos a valores por defecto
        // 4. Actualizar la UI
        // 5. Manejar posibles excepciones
        // 6. Actualizar el texto de estado
        
        Debug.Log("DeleteSaveFile() debe ser implementado");
    }
    
    /// <summary>
    /// Inicializa los datos por defecto
    /// </summary>
    private void InitializeDefaultData()
    {
        // TODO: Implementar inicialización de datos por defecto
        // 1. Crear un nuevo perfil de jugador con valores iniciales
        // 2. Crear un inventario inicial con algunos items básicos
        // 3. Crear un estado de juego inicial
        // 4. Crear un nuevo objeto SaveData con todos los elementos anteriores
        
        Debug.Log("InitializeDefaultData() debe ser implementado");
    }
    
    /// <summary>
    /// Actualiza la UI con los datos actuales
    /// </summary>
    private void UpdateUI()
    {
        // TODO: Implementar actualización de UI
        // 1. Actualizar texto de estadísticas del jugador
        // 2. Actualizar texto del inventario
        // 3. Actualizar texto de misiones y nivel actual
        
        Debug.Log("UpdateUI() debe ser implementado");
    }
    
    // Métodos para pruebas y demostración
    
    /// <summary>
    /// Añade experiencia al jugador
    /// </summary>
    public void AddExperience(int amount)
    {
        // TODO: Implementar adición de experiencia
        // 1. Añadir experiencia al jugador
        // 2. Aumentar nivel si alcanza cierto umbral
        // 3. Actualizar UI
        
        Debug.Log("Se deben añadir " + amount + " puntos de experiencia");
    }
    
    /// <summary>
    /// Añade un item al inventario
    /// </summary>
    public void AddItem(string itemId, string itemName, int quantity = 1)
    {
        // TODO: Implementar adición de items
        // 1. Buscar si el item ya existe en el inventario
        // 2. Si existe, incrementar cantidad
        // 3. Si no existe, crear nuevo item y añadirlo
        // 4. Actualizar UI
        
        Debug.Log("Se debe añadir el item " + itemName);
    }
    
    /// <summary>
    /// Completa una misión
    /// </summary>
    public void CompleteQuest(string questId)
    {
        // TODO: Implementar completado de misiones
        // 1. Verificar si la misión ya está completada
        // 2. Si no está completada, añadirla a la lista
        // 3. Actualizar UI
        
        Debug.Log("Se debe completar la misión " + questId);
    }
    
    /// <summary>
    /// Cambia el nivel actual
    /// </summary>
    public void ChangeLevel(string levelId)
    {
        // TODO: Implementar cambio de nivel
        // 1. Actualizar el nivel actual
        // 2. Actualizar UI
        
        Debug.Log("Se debe cambiar al nivel " + levelId);
    }
}