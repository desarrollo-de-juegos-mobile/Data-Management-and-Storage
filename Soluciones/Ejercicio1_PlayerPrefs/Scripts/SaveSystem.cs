using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    // Referencias UI
    public InputField playerNameInput;
    public Text scoreText;
    public Text levelText;
    public Text itemsText;
    
    // Datos del jugador
    private string playerName = "Player";
    private int score = 0;
    private int level = 1;
    private int itemsCollected = 0;
    
    // Para la demostración
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }
    
    public void LevelUp()
    {
        level++;
        UpdateUI();
    }
    
    public void CollectItem()
    {
        itemsCollected++;
        UpdateUI();
    }
    
    // Sistema de guardado
    public void SaveGame()
    {
        // Obtener nombre del input si existe
        if (playerNameInput != null && !string.IsNullOrEmpty(playerNameInput.text))
        {
            playerName = playerNameInput.text;
        }
        
        // Guardar datos en PlayerPrefs
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("ItemsCollected", itemsCollected);
        
        // Guardar cambios inmediatamente
        PlayerPrefs.Save();
        
        Debug.Log("Game Saved!");
    }
    
    public void LoadGame()
    {
        // Cargar datos de PlayerPrefs con valores por defecto si no existen
        playerName = PlayerPrefs.GetString("PlayerName", "Player");
        score = PlayerPrefs.GetInt("Score", 0);
        level = PlayerPrefs.GetInt("Level", 1);
        itemsCollected = PlayerPrefs.GetInt("ItemsCollected", 0);
        
        // Actualizar input de nombre si existe
        if (playerNameInput != null)
        {
            playerNameInput.text = playerName;
        }
        
        UpdateUI();
        Debug.Log("Game Loaded!");
    }
    
    public void ResetGame()
    {
        // Reiniciar variables
        playerName = "Player";
        score = 0;
        level = 1;
        itemsCollected = 0;
        
        // Actualizar input de nombre si existe
        if (playerNameInput != null)
        {
            playerNameInput.text = playerName;
        }
        
        UpdateUI();
        Debug.Log("Game Reset!");
    }
    
    private void UpdateUI()
    {
        // Actualizar UI con valores actuales
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (levelText != null) levelText.text = "Level: " + level;
        if (itemsText != null) itemsText.text = "Items: " + itemsCollected;
    }
    
    // Inicializar UI al empezar
    private void Start()
    {
        UpdateUI();
    }
}