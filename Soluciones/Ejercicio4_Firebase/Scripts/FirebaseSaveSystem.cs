using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

// Nota: Necesita el paquete Firebase instalado en Unity
// Window > Package Manager > + > Add package from git URL > https://github.com/firebase/firebase-unity-sdk.git

public class FirebaseSaveSystem : MonoBehaviour
{
    // Referencias UI
    public InputField playerNameInput;
    public Text scoreText;
    public Text statusText;
    public Text syncStatusText;
    public InputField playerIdInput;
    
    // Datos del juego
    private string playerName = "Player";
    private int score = 0;
    private string playerId = "";
    
    // Estado de sincronización
    private bool isOnline = false;
    private bool needsSync = false;
    private DateTime lastLocalSaveTime;
    
    // Objetos Firebase
    private Firebase.FirebaseApp app;
    private Firebase.Database.FirebaseDatabase database;
    private Firebase.Database.DatabaseReference dbReference;
    
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
    
    private void Start()
    {
        // Inicializar Firebase
        InitializeFirebase();
        
        // Generar un ID de jugador aleatorio si no se proporciona uno
        if (string.IsNullOrEmpty(playerId))
        {
            playerId = Guid.NewGuid().ToString();
        }
        
        if (playerIdInput != null)
        {
            playerIdInput.text = playerId;
        }
        
        UpdateUI();
        UpdateSyncStatus();
        
        // Comprobar conectividad cada 5 segundos
        InvokeRepeating("CheckConnectivity", 0, 5f);
    }
    
    private void CheckConnectivity()
    {
        StartCoroutine(CheckInternetConnection((isConnected) => {
            if (isOnline != isConnected)
            {
                isOnline = isConnected;
                UpdateSyncStatus();
                
                // Si recuperamos conexión y hay datos para sincronizar
                if (isOnline && needsSync)
                {
                    SyncToCloud();
                }
            }
        }));
    }
    
    private IEnumerator CheckInternetConnection(Action<bool> callback)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        
        if (www.error != null)
        {
            callback(false);
        }
        else
        {
            callback(true);
        }
    }
    
    private void InitializeFirebase()
    {
        try
        {
            // Comprobar dependencias de Firebase
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    app = Firebase.FirebaseApp.DefaultInstance;
                    
                    // Obtener instancia de la base de datos e inicializar
                    database = Firebase.Database.FirebaseDatabase.DefaultInstance;
                    dbReference = database.GetReference("players");
                    
                    Debug.Log("Firebase initialized successfully!");
                    
                    // Configurar persistencia offline
                    database.SetPersistenceEnabled(true);
                }
                else
                {
                    Debug.LogError("Could not resolve Firebase dependencies: " + dependencyStatus);
                }
            });
        }
        catch (Exception e)
        {
            // En el editor de Unity, esto puede fallar si no está configurado Firebase
            Debug.LogError("Firebase initialization failed: " + e.Message);
            UpdateStatus("Firebase initialization failed. Testing in Editor?", Color.yellow);
        }
    }
    
    // Para demostración
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
        needsSync = true;
        UpdateSyncStatus();
    }
    
    // Sistema de guardado
    public void SaveGame()
    {
        // Actualizar player ID si se proporciona uno nuevo
        if (playerIdInput != null && !string.IsNullOrEmpty(playerIdInput.text))
        {
            playerId = playerIdInput.text;
        }
        
        // Verificar que tenemos un ID válido
        if (string.IsNullOrEmpty(playerId))
        {
            UpdateStatus("PlayerID is required!", Color.red);
            return;
        }
        
        // Obtener nombre del input si existe
        if (playerNameInput != null && !string.IsNullOrEmpty(playerNameInput.text))
        {
            playerName = playerNameInput.text;
        }
        
        // Guardar localmente
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetString("PlayerId", playerId);
        PlayerPrefs.Save();
        
        // Registrar tiempo de guardado local
        lastLocalSaveTime = DateTime.Now;
        
        UpdateStatus("Game saved locally!", Color.green);
        
        // Sincronizar con la nube si hay conexión
        if (isOnline)
        {
            SyncToCloud();
        }
        else
        {
            needsSync = true;
            UpdateSyncStatus();
        }
    }
    
    private void SyncToCloud()
    {
        try
        {
            if (database == null || dbReference == null)
            {
                UpdateStatus("Firebase not initialized yet", Color.yellow);
                return;
            }
            
            // Crear datos para guardar
            SaveData saveData = new SaveData(playerName, score);
            Dictionary<string, object> dataToSave = new Dictionary<string, object>
            {
                { "playerName", saveData.playerName },
                { "score", saveData.score },
                { "timestamp", saveData.timestamp }
            };
            
            // Guardar en Firebase
            dbReference.Child(playerId).UpdateChildrenAsync(dataToSave).ContinueWith(task => {
                UnityMainThreadDispatcher.Instance().Enqueue(() => {
                    if (task.IsFaulted)
                    {
                        UpdateStatus("Error syncing to cloud: " + task.Exception, Color.red);
                        needsSync = true;
                    }
                    else if (task.IsCompleted)
                    {
                        UpdateStatus("Synced to cloud successfully!", Color.green);
                        needsSync = false;
                    }
                    
                    UpdateSyncStatus();
                });
            });
        }
        catch (Exception e)
        {
            Debug.LogError("Error syncing to cloud: " + e.Message);
            UpdateStatus("Error syncing to cloud: " + e.Message, Color.red);
            needsSync = true;
            UpdateSyncStatus();
        }
    }
    
    public void LoadGame()
    {
        // Cargar datos locales primero
        LoadLocalData();
        
        // Si estamos online, intentar cargar desde la nube
        if (isOnline)
        {
            LoadFromCloud();
        }
        else
        {
            UpdateStatus("Loaded local data. Offline mode.", Color.yellow);
        }
    }
    
    private void LoadLocalData()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Player");
        score = PlayerPrefs.GetInt("Score", 0);
        playerId = PlayerPrefs.GetString("PlayerId", playerId);
        
        // Actualizar UI
        if (playerNameInput != null)
        {
            playerNameInput.text = playerName;
        }
        
        if (playerIdInput != null)
        {
            playerIdInput.text = playerId;
        }
        
        UpdateUI();
    }
    
    private void LoadFromCloud()
    {
        try
        {
            if (database == null || dbReference == null)
            {
                UpdateStatus("Firebase not initialized yet", Color.yellow);
                return;
            }
            
            // Verificar que tenemos un ID válido
            if (string.IsNullOrEmpty(playerId))
            {
                UpdateStatus("PlayerID is required to load from cloud!", Color.red);
                return;
            }
            
            // Leer datos de Firebase
            dbReference.Child(playerId).GetValueAsync().ContinueWith(task => {
                UnityMainThreadDispatcher.Instance().Enqueue(() => {
                    if (task.IsFaulted)
                    {
                        UpdateStatus("Error loading from cloud: " + task.Exception, Color.red);
                    }
                    else if (task.IsCompleted)
                    {
                        Firebase.Database.DataSnapshot snapshot = task.Result;
                        
                        if (snapshot.Exists)
                        {
                            // Obtener valores
                            string cloudPlayerName = snapshot.Child("playerName").Value?.ToString();
                            int cloudScore = Convert.ToInt32(snapshot.Child("score").Value);
                            long cloudTimestamp = Convert.ToInt64(snapshot.Child("timestamp").Value);
                            
                            // Decidir qué datos usar (local vs. cloud)
                            long localTimestamp = PlayerPrefs.GetInt("LastSaveTimestamp", 0);
                            
                            if (cloudTimestamp > localTimestamp)
                            {
                                // Los datos de la nube son más recientes
                                playerName = cloudPlayerName;
                                score = cloudScore;
                                
                                // Actualizar datos locales con los de la nube
                                PlayerPrefs.SetString("PlayerName", playerName);
                                PlayerPrefs.SetInt("Score", score);
                                PlayerPrefs.SetInt("LastSaveTimestamp", (int)cloudTimestamp);
                                PlayerPrefs.Save();
                                
                                // Actualizar UI
                                if (playerNameInput != null)
                                {
                                    playerNameInput.text = playerName;
                                }
                                
                                UpdateUI();
                                UpdateStatus("Loaded newer data from cloud!", Color.green);
                            }
                            else
                            {
                                UpdateStatus("Local data is newer than cloud data", Color.yellow);
                            }
                        }
                        else
                        {
                            UpdateStatus("No data found in cloud for this player ID", Color.yellow);
                        }
                    }
                });
            });
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading from cloud: " + e.Message);
            UpdateStatus("Error loading from cloud: " + e.Message, Color.red);
        }
    }
    
    public void DeleteSaveData()
    {
        try
        {
            // Borrar datos locales
            PlayerPrefs.DeleteKey("PlayerName");
            PlayerPrefs.DeleteKey("Score");
            PlayerPrefs.DeleteKey("LastSaveTimestamp");
            PlayerPrefs.Save();
            
            // Reiniciar variables
            playerName = "Player";
            score = 0;
            needsSync = false;
            
            // Actualizar UI
            if (playerNameInput != null)
            {
                playerNameInput.text = playerName;
            }
            
            UpdateUI();
            UpdateSyncStatus();
            UpdateStatus("Local save data deleted!", Color.yellow);
            
            // Borrar datos en la nube si estamos conectados
            if (isOnline && database != null && dbReference != null && !string.IsNullOrEmpty(playerId))
            {
                dbReference.Child(playerId).RemoveValueAsync().ContinueWith(task => {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => {
                        if (task.IsFaulted)
                        {
                            UpdateStatus("Error deleting cloud data: " + task.Exception, Color.red);
                        }
                        else if (task.IsCompleted)
                        {
                            UpdateStatus("Cloud data deleted successfully!", Color.green);
                        }
                    });
                });
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error deleting save data: " + e.Message);
            UpdateStatus("Error deleting save data: " + e.Message, Color.red);
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
    
    private void UpdateSyncStatus()
    {
        if (syncStatusText != null)
        {
            string statusMessage = isOnline ? "ONLINE" : "OFFLINE";
            Color statusColor = isOnline ? Color.green : Color.yellow;
            
            if (needsSync && !isOnline)
            {
                statusMessage += " (Pending Sync)";
                statusColor = new Color(1f, 0.5f, 0f); // Naranja
            }
            
            syncStatusText.text = "Status: " + statusMessage;
            syncStatusText.color = statusColor;
        }
    }
}

// Clase auxiliar para ejecutar código en el hilo principal de Unity
// (necesario para actualizar la UI desde callbacks de Firebase)
public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance;
    private readonly Queue<Action> executionQueue = new Queue<Action>();
    
    public static UnityMainThreadDispatcher Instance()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("UnityMainThreadDispatcher");
            instance = go.AddComponent<UnityMainThreadDispatcher>();
            DontDestroyOnLoad(go);
        }
        return instance;
    }
    
    public void Enqueue(Action action)
    {
        lock (executionQueue)
        {
            executionQueue.Enqueue(action);
        }
    }
    
    void Update()
    {
        lock (executionQueue)
        {
            while (executionQueue.Count > 0)
            {
                Action action = executionQueue.Dequeue();
                action.Invoke();
            }
        }
    }
}