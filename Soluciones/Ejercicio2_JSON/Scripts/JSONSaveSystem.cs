using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

// Clases para almacenar datos
[System.Serializable]
public class GameItem
{
    public string id;
    public string name;
    public int quantity;
    
    public GameItem(string id, string name, int quantity)
    {
        this.id = id;
        this.name = name;
        this.quantity = quantity;
    }
}

[System.Serializable]
public class PlayerProfile
{
    public string name;
    public int level;
    public int experience;
    
    public PlayerProfile(string name, int level, int experience)
    {
        this.name = name;
        this.level = level;
        this.experience = experience;
    }
}

[System.Serializable]
public class GameState
{
    public string currentLevel;
    public List<string> completedQuests;
    
    public GameState(string currentLevel, List<string> completedQuests)
    {
        this.currentLevel = currentLevel;
        this.completedQuests = completedQuests;
    }
}

[System.Serializable]
public class SaveData
{
    public PlayerProfile player;
    public List<GameItem> inventory;
    public GameState gameState;
    public long savedTimestamp;
    
    public SaveData(PlayerProfile player, List<GameItem> inventory, GameState gameState)
    {
        this.player = player;
        this.inventory = inventory;
        this.gameState = gameState;
        this.savedTimestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}

public class JSONSaveSystem : MonoBehaviour
{
    // Referencias UI
    public InputField playerNameInput;
    public Text playerStatsText;
    public Text inventoryText;
    public Text questsText;
    
    // Datos del juego
    private SaveData currentSaveData;
    private string savePath;
    
    void Start()
    {
        // Establecer ruta del archivo guardado
        savePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        Debug.Log("Save path: " + savePath);
        
        // Inicializar con datos por defecto
        InitializeDefaultData();
        UpdateUI();
    }
    
    void InitializeDefaultData()
    {
        // Crear perfil de jugador
        PlayerProfile profile = new PlayerProfile("Adventurer", 1, 0);
        
        // Crear inventario
        List<GameItem> inventory = new List<GameItem>
        {
            new GameItem("potion_01", "Health Potion", 3),
            new GameItem("sword_01", "Wooden Sword", 1)
        };
        
        // Crear estado del juego
        GameState gameState = new GameState("town_01", new List<string>());
        
        // Crear datos de guardado
        currentSaveData = new SaveData(profile, inventory, gameState);
    }
    
    // Métodos de demostración para modificar datos
    public void AddExperience(int amount)
    {
        currentSaveData.player.experience += amount;
        
        // Subir de nivel cada 100 exp
        if (currentSaveData.player.experience >= currentSaveData.player.level * e 100)
        {
            currentSaveData.player.level++;
        }
        
        UpdateUI();
    }
    
    public void AddItem(string itemId, string itemName, int quantity)
    {
        // Buscar si el ítem ya existe
        GameItem existingItem = currentSaveData.inventory.Find(i => i.id == itemId);
        
        if (existingItem != null)
        {
            existingItem.quantity += quantity;
        }
        else
        {
            currentSaveData.inventory.Add(new GameItem(itemId, itemName, quantity));
        }
        
        UpdateUI();
    }
    
    public void CompleteQuest(string questId)
    {
        if (!currentSaveData.gameState.completedQuests.Contains(questId))
        {
            currentSaveData.gameState.completedQuests.Add(questId);
            UpdateUI();
        }
    }
    
    public void ChangeLevel(string levelId)
    {
        currentSaveData.gameState.currentLevel = levelId;
        UpdateUI();
    }
    
    // Sistema de guardado
    public void SaveGame()
    {
        // Actualizar nombre del jugador si existe input
        if (playerNameInput != null && !string.IsNullOrEmpty(playerNameInput.text))
        {
            currentSaveData.player.name = playerNameInput.text;
        }
        
        // Actualizar timestamp
        currentSaveData.savedTimestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        // Convertir a JSON
        string jsonData = JsonUtility.ToJson(currentSaveData, true);
        
        // Guardar archivo
        try
        {
            File.WriteAllText(savePath, jsonData);
            Debug.Log("Game saved to: " + savePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving game: " + e.Message);
        }
    }
    
    public void LoadGame()
    {
        try
        {
            if (File.Exists(savePath))
            {
                // Leer archivo
                string jsonData = File.ReadAllText(savePath);
                
                // Deserializar JSON
                currentSaveData = JsonUtility.FromJson<SaveData>(jsonData);
                
                // Actualizar input de nombre si existe
                if (playerNameInput != null)
                {
                    playerNameInput.text = currentSaveData.player.name;
                }
                
                UpdateUI();
                Debug.Log("Game loaded from: " + savePath);
            }
            else
            {
                Debug.LogWarning("Save file not found at: " + savePath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading game: " + e.Message);
        }
    }
    
    public void DeleteSaveFile()
    {
        try
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log("Save file deleted: " + savePath);
            }
            
            // Reiniciar a valores por defecto
            InitializeDefaultData();
            
            // Actualizar input de nombre si existe
            if (playerNameInput != null)
            {
                playerNameInput.text = currentSaveData.player.name;
            }
            
            UpdateUI();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error deleting save file: " + e.Message);
        }
    }
    
    private void UpdateUI()
    {
        // Actualizar UI con información actual
        if (playerStatsText != null)
        {
            string statsText = "Player: " + currentSaveData.player.name +
                "\nLevel: " + currentSaveData.player.level +
                "\nExp: " + currentSaveData.player.experience;
            playerStatsText.text = statsText;
        }
        
        if (inventoryText != null)
        {
            string invText = "Inventory:\n";
            foreach (GameItem item in currentSaveData.inventory)
            {
                invText += item.name + " x" + item.quantity + "\n";
            }
            inventoryText.text = invText;
        }
        
        if (questsText != null)
        {
            string questText = "Current Level: " + currentSaveData.gameState.currentLevel + "\n\nCompleted Quests:\n";
            if (currentSaveData.gameState.completedQuests.Count > 0)
            {
                foreach (string quest in currentSaveData.gameState.completedQuests)
                {
                    questText += "- " + quest + "\n";
                }
            }
            else
            {
                questText += "None";
            }
            questsText.text = questText;
        }
    }
}