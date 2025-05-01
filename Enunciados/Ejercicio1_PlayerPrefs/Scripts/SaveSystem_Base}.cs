using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sistema básico de guardado usando PlayerPrefs
/// Este script debe ser completado como parte del Ejercicio 1
/// </summary>
public class SaveSystem_Base : MonoBehaviour
{
    // Referencias a elementos de UI
    [Header("Referencias UI")]
    public InputField playerNameInput;
    public Text scoreText;
    public Text levelText;
    public Text statusText;
    
    // Datos del jugador (deben ser persistentes)
    private string playerName = "Player";
    private int score = 0;
    private int level = 1;
    
    void Start()
    {
        // TODO: Implementar carga de datos guardados al iniciar
        UpdateUI();
    }

    /// <summary>
    /// Guarda los datos del jugador utilizando PlayerPrefs
    /// </summary>
    public void SaveGame()
    {
        // TODO: Implementar guardado de datos utilizando PlayerPrefs
        // 1. Obtener el nombre del inputField (si existe)
        // 2. Guardar nombre, puntuación y nivel en PlayerPrefs
        // 3. Llamar a PlayerPrefs.Save() para asegurar que se guarden los datos
        // 4. Actualizar el texto de estado
        
        Debug.Log("SaveGame() debe ser implementado");
    }
    
    /// <summary>
    /// Carga los datos del jugador desde PlayerPrefs
    /// </summary>
    public void LoadGame()
    {
        // TODO: Implementar carga de datos desde PlayerPrefs
        // 1. Cargar nombre, puntuación y nivel desde PlayerPrefs
        // 2. Actualizar las variables con los valores cargados
        // 3. Actualizar la UI para reflejar los datos cargados
        // 4. Actualizar el texto de estado
        
        Debug.Log("LoadGame() debe ser implementado");
    }
    
    /// <summary>
    /// Reinicia los datos del jugador a valores por defecto
    /// </summary>
    public void ResetGame()
    {
        // TODO: Implementar reinicio de datos
        // 1. Restablecer las variables a sus valores por defecto
        // 2. Actualizar la UI para reflejar los cambios
        // 3. Opcionalmente, eliminar las claves de PlayerPrefs
        // 4. Actualizar el texto de estado
        
        Debug.Log("ResetGame() debe ser implementado");
    }
    
    /// <summary>
    /// Actualiza los elementos de UI con los valores actuales
    /// </summary>
    private void UpdateUI()
    {
        // TODO: Actualizar todos los elementos de UI con los valores actuales
        // 1. Actualizar el InputField del nombre
        // 2. Actualizar el texto de puntuación
        // 3. Actualizar el texto de nivel
        
        Debug.Log("UpdateUI() debe ser implementado");
    }
    
    // Métodos para las pruebas y demostración
    
    /// <summary>
    /// Incrementa la puntuación en 10 puntos
    /// </summary>
    public void AddScore()
    {
        score += 10;
        UpdateUI();
    }
    
    /// <summary>
    /// Incrementa el nivel en 1
    /// </summary>
    public void LevelUp()
    {
        level++;
        UpdateUI();
    }
}