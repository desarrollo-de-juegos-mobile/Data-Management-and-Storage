using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FirebaseSaveSystemTests : MonoBehaviour
{
    public FirebaseSaveSystem saveSystem;
    public Text testResultsText;
    
    // Simulación de estados de conexión
    public void SimulateOfflineMode()
    {
        saveSystem.SendMessage("SetOfflineMode", true);
    }
    
    public void SimulateOnlineMode()
    {
        saveSystem.SendMessage("SetOfflineMode", false);
    }
    
    public void RunTests()
    {
        if (saveSystem == null || testResultsText == null)
        {
            Debug.LogError("Missing references for testing!");
            return;
        }
        
        testResultsText.text = "Running Firebase Save System Tests...\n";
        testResultsText.text += "Note: These tests simulate online/offline behavior.\n\n";
        
        // Comenzar secuencia de pruebas
        StartCoroutine(RunTestSequence());
    }
    
    private IEnumerator RunTestSequence()
    {
        int passedTests = 0;
        int totalTests = 4;
        
        // Test 1: Guardar datos localmente (modo offline)
        testResultsText.text += "Test 1: Save Data Locally (Offline Mode)\n";
        
        // Simular modo offline
        SimulateOfflineMode();
        yield return new WaitForSeconds(1f);
        
        // Resetear datos
        saveSystem.DeleteSaveData();
        yield return new WaitForSeconds(0.5f);
        
        // Establecer datos
        saveSystem.playerNameInput.text = "OfflinePlayer";
        saveSystem.AddScore(50);
        
        // Guardar juego (localmente)
        saveSystem.SaveGame();
        yield return new WaitForSeconds(0.5f);
        
        // Verificar que indica modo offline y pendiente de sincronización
        if (saveSystem.syncStatusText.text.Contains("OFFLINE") && 
            saveSystem.syncStatusText.text.Contains("Pending"))
        {
            // Resetear datos
            saveSystem.playerNameInput.text = "";
            saveSystem.AddScore(-50); // Volver a 0
            
            // Cargar
            saveSystem.LoadGame();
            yield return new WaitForSeconds(0.5f);
            
            // Verificar carga local
            if (saveSystem.playerNameInput.text == "OfflinePlayer" && 
                saveSystem.scoreText.text.Contains("50"))
            {
                testResultsText.text += "✓ Local save/load works in offline mode\n";
                passedTests++;
            }
            else
            {
                testResultsText.text += "✗ Local save/load failed in offline mode\n";
            }
        }
        else
        {
            testResultsText.text += "✗ Offline status not detected correctly\n";
        }
        
        // Test 2: Sincronización al recuperar conexión
        testResultsText.text += "\nTest 2: Auto-Sync When Going Online\n";
        
        // Asegurarse de que estamos en modo offline y tenemos datos para sincronizar
        SimulateOfflineMode();
        yield return new WaitForSeconds(0.5f);
        
        saveSystem.playerNameInput.text = "WillSyncPlayer";
        saveSystem.AddScore(100); // Ahora debería ser 150
        saveSystem.SaveGame();
        yield return new WaitForSeconds(0.5f);
        
        // Verificar que hay sincronización pendiente
        bool hasPendingSync = saveSystem.syncStatusText.text.Contains("Pending");
        
        // Cambiar a modo online
        SimulateOnlineMode();
        yield return new WaitForSeconds(2f); // Dar tiempo para sincronizar
        
        // Verificar si ya no hay sincronización pendiente
        bool syncComplete = !saveSystem.syncStatusText.text.Contains("Pending") && 
                           saveSystem.syncStatusText.text.Contains("ONLINE");
        
        if (hasPendingSync && syncComplete)
        {
            testResultsText.text += "✓ Auto-sync when going online works\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Auto-sync when going online failed\n";
        }
        
        // Test 3: Carga desde la nube
        testResultsText.text += "\nTest 3: Load From Cloud\n";
        
        // Asegurarse de que estamos online
        SimulateOnlineMode();
        yield return new WaitForSeconds(0.5f);
        
        // Guardar datos en la nube para asegurarnos de que hay algo que cargar
        saveSystem.playerNameInput.text = "CloudPlayer";
        saveSystem.AddScore(50); // Ahora debería ser 200
        saveSystem.SaveGame();
        yield return new WaitForSeconds(1f);
        
        // Borrar datos locales
        PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.DeleteKey("Score");
        PlayerPrefs.Save();
        
        // Actualizar UI para reflejar que no hay datos locales
        saveSystem.playerNameInput.text = "";
        saveSystem.SendMessage("ResetLocalScore");
        yield return new WaitForSeconds(0.5f);
        
        // Cargar desde la nube
        saveSystem.LoadGame();
        yield return new WaitForSeconds(2f);
        
        // Verificar que se cargaron los datos de la nube
        if (saveSystem.playerNameInput.text == "CloudPlayer" && 
            saveSystem.scoreText.text.Contains("200"))
        {
            testResultsText.text += "✓ Cloud load works correctly\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Cloud load failed\n";
        }
        
        // Test 4: Borrado de datos en la nube
        testResultsText.text += "\nTest 4: Delete Cloud Data\n";
        
        // Asegurarse de que estamos online
        SimulateOnlineMode();
        yield return new WaitForSeconds(0.5f);
        
        // Borrar datos
        saveSystem.DeleteSaveData();
        yield return new WaitForSeconds(2f);
        
        // Intentar cargar datos después de borrar
        saveSystem.LoadGame();
        yield return new WaitForSeconds(2f);
        
        // Verificar que se borraron los datos
        if (saveSystem.statusText.text.Contains("No data found") || 
            (saveSystem.playerNameInput.text == "Player" && 
             saveSystem.scoreText.text.Contains("0")))
        {
            testResultsText.text += "✓ Cloud data deletion works\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Cloud data deletion failed\n";
        }
        
        // Test Summary
        testResultsText.text += $"\nTests passed: {passedTests}/{totalTests}\n";
        if (passedTests == totalTests)
        {
            testResultsText.text += "All tests passed! ✓";

            // Test Summary
        testResultsText.text += $"\nTests passed: {passedTests}/{totalTests}\n";
        if (passedTests == totalTests)
        {
            testResultsText.text += "All tests passed! ✓";
        }
        else
        {
            testResultsText.text += "Some tests failed. ✗";
        }
        
        // Nota sobre pruebas en un entorno real
        testResultsText.text += "\n\nNota: Estas pruebas simulan comportamiento online/offline. " +
                               "En un entorno real, el comportamiento puede variar dependiendo " +
                               "de la conexión a Internet y la configuración de Firebase.";
    }
}