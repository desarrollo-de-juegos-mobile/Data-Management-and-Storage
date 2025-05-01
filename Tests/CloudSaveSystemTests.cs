using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloudSaveSystemTests : MonoBehaviour
{
    public CloudSaveSystem cloudSaveSystem;
    public Text testResultsText;
    
    public void RunTests()
    {
        if (cloudSaveSystem == null || testResultsText == null)
        {
            Debug.LogError("¡Faltan referencias para las pruebas!");
            return;
        }
        
        testResultsText.text = "Ejecutando pruebas de Unity Cloud Save...\n";
        StartCoroutine(RunTestSequence());
    }
    
    private IEnumerator RunTestSequence()
    {
        int passedTests = 0;
        int totalTests = 3;
        
        // Test 1: Guardar datos básicos
        testResultsText.text += "Test 1: Guardar datos básicos\n";
        
        // Establecer datos
        cloudSaveSystem.playerNameInput.text = "TestPlayer";
        cloudSaveSystem.IncrementScore(); // Aumenta a 10
        cloudSaveSystem.IncrementLevel(); // Aumenta a 2
        
        // Guardar datos
        cloudSaveSystem.SaveGameDataButton();
        yield return new WaitForSeconds(2f); // Esperar a que termine la operación asincrónica
        
        // Verificar estado
        if (cloudSaveSystem.syncStatusText.text.Contains("guardados correctamente"))
        {
            testResultsText.text += "✓ Guardado exitoso\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Error al guardar datos\n";
        }
        
        // Test 2: Cargar datos guardados
        testResultsText.text += "\nTest 2: Cargar datos guardados\n";
        
        // Restablecer valores
        cloudSaveSystem.playerNameInput.text = "";
        // Reiniciar UI directamente
        cloudSaveSystem.GetComponent<CloudSaveSystem>().scoreText.text = "Puntuación: 0";
        cloudSaveSystem.GetComponent<CloudSaveSystem>().levelText.text = "Nivel: 1";
        
        // Cargar datos
        cloudSaveSystem.LoadGameDataButton();
        yield return new WaitForSeconds(2f); // Esperar a que termine la operación asincrónica
        
        // Verificar que se cargaron correctamente
        if (cloudSaveSystem.playerNameInput.text == "TestPlayer" &&
            cloudSaveSystem.GetComponent<CloudSaveSystem>().scoreText.text.Contains("10") &&
            cloudSaveSystem.GetComponent<CloudSaveSystem>().levelText.text.Contains("2"))
        {
            testResultsText.text += "✓ Carga exitosa\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Error al cargar datos\n";
        }
        
        // Test 3: Eliminar datos
        testResultsText.text += "\nTest 3: Eliminar datos\n";
        
        // Eliminar datos
        cloudSaveSystem.DeleteCloudData();
        yield return new WaitForSeconds(2f); // Esperar a que termine la operación asincrónica
        
        // Verificar que se eliminaron
        if (cloudSaveSystem.syncStatusText.text.Contains("eliminados"))
        {
            // Intentar cargar después de eliminar
            cloudSaveSystem.LoadGameDataButton();
            yield return new WaitForSeconds(2f);
            
            // Si no hay datos, los valores deberían ser los predeterminados
            // O podría haber un mensaje de error/advertencia
            testResultsText.text += "✓ Eliminación exitosa\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Error al eliminar datos\n";
        }
        
        // Test Summary
        testResultsText.text += $"\nPruebas aprobadas: {passedTests}/{totalTests}\n";
        if (passedTests == totalTests)
        {
            testResultsText.text += "¡Todas las pruebas aprobadas! ✓";
        }
        else
        {
            testResultsText.text += "Algunas pruebas fallaron. ✗";
        }
    }
}