using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sistema de guardado en la nube con Unity Cloud Save
/// Este script debe ser completado como parte del Ejercicio 6
/// Requiere Unity Gaming Services (Cloud Save y Authentication)
/// </summary>
public class CloudSaveSystem_Base : MonoBehaviour
{
    [Header("Referencias UI")]
    public InputField playerNameInput;
    public Text scoreText;
    public Text levelText;
    public Text syncStatusText;
    public Button saveButton;
    public Button loadButton;
    
    // Datos del jugador
    private string playerName = "Player";
    private int score = 0;
    private int level = 1;
    
    // Estado de la nube
    private bool isInitialized = false;
    private bool isSaving = false;
    private bool isLoading = false;
    
    // Espacio para las directivas using necesarias
    // using Unity.Services.Core;
    // using Unity.Services.Authentication;
    // using Unity.Services.CloudSave;
    
    async void Start()
    {
        // Deshabilitar botones hasta que se inicialice
        saveButton.interactable = false;
        loadButton.interactable = false;
        
        // TODO: Inicializar Unity Gaming Services
        // 1. Llamar a InitializeUnityServices()
    }
    
    /// <summary>
    /// Inicializa Unity Gaming Services (Core, Authentication, CloudSave)
    /// </summary>
    private async Task InitializeUnityServices()
    {
        // TODO: Implementar inicialización de servicios
        // 1. Mostrar estado "Inicializando..."
        // 2. Inicializar UnityServices.InitializeAsync()
        // 3. Iniciar sesión con autenticación anónima si no está conectado
        // 4. Mostrar ID del jugador conectado
        // 5. Habilitar botones
        // 6. Actualizar estado isInitialized
        // 7. Cargar datos automáticamente
        // 8. Manejar errores
        
        Debug.Log("InitializeUnityServices() debe ser implementado");
        return;
    }
    
    /// <summary>
    /// Guarda datos en Unity Cloud Save (botón UI)
    /// </summary>
    public async void SaveGameDataButton()
    {
        // TODO: Implementar guardado mediante botón
        // 1. Verificar que hay un nombre de jugador
        // 2. Actualizar nombre desde input
        // 3. Llamar a SaveGameData()
        
        Debug.Log("SaveGameDataButton() debe ser implementado");
    }
    
    /// <summary>
    /// Implementación del guardado en Unity Cloud Save
    /// </summary>
    private async Task SaveGameData()
    {
        // TODO: Implementar guardado en Cloud Save
        // 1. Verificar inicialización y que no está guardando
        // 2. Actualizar estado a guardando y mostrar mensaje
        // 3. Crear diccionario con datos a guardar
        // 4. Llamar a CloudSaveService para guardar datos
        // 5. Actualizar estado y mostrar mensaje de éxito
        // 6. Manejar errores
        // 7. Restablecer estado de guardado
        
        Debug.Log("SaveGameData() debe ser implementado");
        return;
    }
    
    /// <summary>
    /// Carga datos desde Unity Cloud Save (botón UI)
    /// </summary>
    public async void LoadGameDataButton()
    {
        // TODO: Implementar carga mediante botón
        // 1. Llamar a LoadGameData()
        
        Debug.Log("LoadGameDataButton() debe ser implementado");
    }
    
    /// <summary>
    /// Implementación de la carga desde Unity Cloud Save
    /// </summary>
    private async Task LoadGameData()
    {
        // TODO: Implementar carga desde Cloud Save
        // 1. Verificar inicialización y que no está cargando
        // 2. Actualizar estado a cargando y mostrar mensaje
        // 3. Definir las claves que queremos cargar
        // 4. Llamar a CloudSaveService para cargar datos
        // 5. Procesar los datos cargados (playerName, score, level)
        // 6. Actualizar UI
        // 7. Mostrar mensaje de éxito con timestamp
        // 8. Manejar errores
        // 9. Restablecer estado de carga
        
        Debug.Log("LoadGameData() debe ser implementado");
        return;
    }
    
    /// <summary>
    /// Elimina datos de Unity Cloud Save
    /// </summary>
    public async void DeleteCloudData()
    {
        // TODO: Implementar eliminación de datos
        // 1. Verificar inicialización
        // 2. Mostrar estado "Eliminando datos..."
        // 3. Definir las claves que queremos eliminar
        // 4. Llamar a CloudSaveService para eliminar datos
        // 5. Mostrar mensaje de éxito
        // 6. Manejar errores
        
        Debug.Log("DeleteCloudData() debe ser implementado");
    }
    
    // Métodos para las pruebas y demostración
    
    /// <summary>
    /// Incrementa la puntuación en 10 puntos
    /// </summary>
    public void IncrementScore()
    {
        score += 10;
        scoreText.text = "Puntuación: " + score;
    }
    
    /// <summary>
    /// Incrementa el nivel en 1
    /// </summary>
    public void IncrementLevel()
    {
        level++;
        levelText.text = "Nivel: " + level;
    }
    
    /// <summary>
    /// Actualiza la UI con los valores actuales
    /// </summary>
    private void UpdateUI()
    {
        // Actualizar UI con valores actuales
        if (playerNameInput != null)
        {
            playerNameInput.text = playerName;
        }
        
        if (scoreText != null)
        {
            scoreText.text = "Puntuación: " + score;
        }
        
        if (levelText != null)
        {
            levelText.text = "Nivel: " + level;
        }
    }
}