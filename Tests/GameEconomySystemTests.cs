using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class GameEconomySystemTests : MonoBehaviour
{
    public GameEconomySystem economySystem;
    public Text testResultsText;
    
    private string economyDataPath;
    private string checksumPath;
    
    void Awake()
    {
        economyDataPath = Path.Combine(Application.persistentDataPath, "economy_data.dat");
        checksumPath = Path.Combine(Application.persistentDataPath, "economy_checksum.dat");
    }
    
    public void RunTests()
    {
        if (economySystem == null || testResultsText == null)
        {
            Debug.LogError("Missing references for testing!");
            return;
        }
        
        testResultsText.text = "Running Game Economy System Tests...\n";
        StartCoroutine(RunTestSequence());
    }
    
    private IEnumerator RunTestSequence()
    {
        int passedTests = 0;
        int totalTests = 5;
        
        // Reset para empezar con estado conocido
        economySystem.ResetEconomyData();
        yield return new WaitForSeconds(0.5f);
        
        // Test 1: Transacciones de Soft Currency (Gold)
        testResultsText.text += "Test 1: Soft Currency Transactions\n";
        
        // Conseguir valores iniciales
        int initialGold = int.Parse(economySystem.goldText.text.Replace("Gold: ", ""));
        
        // Añadir oro
        economySystem.AddGold(50);
        yield return new WaitForSeconds(0.2f);
        
        // Verificar que se añadió
        int updatedGold = int.Parse(economySystem.goldText.text.Replace("Gold: ", ""));
        
        if (updatedGold == initialGold + 50)
        {
            testResultsText.text += "✓ Adding gold works correctly\n";
            
            // Gastar oro
            economySystem.SpendGold(30, "test_item");
            yield return new WaitForSeconds(0.2f);
            
            // Verificar que se gastó
            int finalGold = int.Parse(economySystem.goldText.text.Replace("Gold: ", ""));
            
            if (finalGold == updatedGold - 30)
            {
                testResultsText.text += "✓ Spending gold works correctly\n";
                passedTests++;
            }
            else
            {
                testResultsText.text += "✗ Spending gold failed\n";
            }
        }
        else
        {
            testResultsText.text += "✗ Adding gold failed\n";
        }
        
        // Test 2: Transacciones de Hard Currency (Gems)
        testResultsText.text += "\nTest 2: Hard Currency Transactions\n";
        
        // Conseguir valores iniciales
        int initialGems = int.Parse(economySystem.gemsText.text.Replace("Gems: ", ""));
        
        // Añadir gemas
        economySystem.AddGems(20);
        yield return new WaitForSeconds(1.5f); // Dar tiempo para la "verificación del servidor"
        
        // Verificar que se añadió
        int updatedGems = int.Parse(economySystem.gemsText.text.Replace("Gems: ", ""));
        
        if (updatedGems == initialGems + 20)
        {
            testResultsText.text += "✓ Adding gems works correctly\n";
            
            // Gastar gemas
            economySystem.SpendGems(5, "premium_item");
            yield return new WaitForSeconds(1.5f); // Dar tiempo para la "verificación del servidor"
            
            // Verificar que se gastó
            int finalGems = int.Parse(economySystem.gemsText.text.Replace("Gems: ", ""));
            
            if (finalGems == updatedGems - 5)
            {
                testResultsText.text += "✓ Spending gems works correctly\n";
                passedTests++;
            }
            else
            {
                testResultsText.text += "✗ Spending gems failed\n";
            }
        }
        else
        {
            testResultsText.text += "✗ Adding gems failed\n";
        }
        
        // Test 3: Validación de Fondos Insuficientes
        testResultsText.text += "\nTest 3: Insufficient Funds Validation\n";
        
        // Intentar gastar más oro del disponible
        int currentGold = int.Parse(economySystem.goldText.text.Replace("Gold: ", ""));
        economySystem.SpendGold(currentGold + 1000); // Mucho más de lo que tenemos
        yield return new WaitForSeconds(0.2f);
        
        // Verificar que no cambió el balance
        int afterAttemptGold = int.Parse(economySystem.goldText.text.Replace("Gold: ", ""));
        
        if (afterAttemptGold == currentGold && economySystem.statusText.text.Contains("Not enough"))
        {
            testResultsText.text += "✓ Gold insufficient funds validation works\n";
            
            // Intentar gastar más gemas de las disponibles
            int currentGems = int.Parse(economySystem.gemsText.text.Replace("Gems: ", ""));
            economySystem.SpendGems(currentGems + 1000); // Mucho más de lo que tenemos
            yield return new WaitForSeconds(0.2f);
            
            // Verificar que no cambió el balance
            int afterAttemptGems = int.Parse(economySystem.gemsText.text.Replace("Gems: ", ""));
            
            if (afterAttemptGems == currentGems && economySystem.statusText.text.Contains("Not enough"))
            {
                testResultsText.text += "✓ Gems insufficient funds validation works\n";
                passedTests++;
            }
            else
            {
                testResultsText.text += "✗ Gems insufficient funds validation failed\n";
            }
        }
        else
        {
            testResultsText.text += "✗ Gold insufficient funds validation failed\n";
        }
        
        // Test 4: Persistencia de Datos
        testResultsText.text += "\nTest 4: Data Persistence\n";
        
        // Establecer valores conocidos
        economySystem.ResetEconomyData();
        yield return new WaitForSeconds(0.5f);
        
        // Añadir cantidades específicas
        economySystem.AddGold(123);
        economySystem.AddGems(45);
        yield return new WaitForSeconds(1.5f);
        
        // Guardar (automático al hacer transacciones)
        
        // Verificar que existen los archivos
        bool filesExist = File.Exists(economyDataPath) && File.Exists(checksumPath);
        
        if (filesExist)
        {
            testResultsText.text += "✓ Save files created successfully\n";
            
            // Simular reinicio (resetear datos sin borrar archivos)
            economySystem.SendMessage("SimulateRestart");
            yield return new WaitForSeconds(1.0f);
            
            // Cargar datos
            economySystem.SendMessage("LoadEconomyDataForTest");
            yield return new WaitForSeconds(0.5f);
            
            // Verificar que se cargaron correctamente
            int loadedGold = int.Parse(economySystem.goldText.text.Replace("Gold: ", ""));
            int loadedGems = int.Parse(economySystem.gemsText.text.Replace("Gems: ", ""));
            
            if (loadedGold == 123 && loadedGems == 45)
            {
                testResultsText.text += "✓ Data loaded correctly after restart\n";
                passedTests++;
            }
            else
            {
                testResultsText.text += "✗ Data loading failed after restart\n";
            }
        }
        else
        {
            testResultsText.text += "✗ Save files creation failed\n";
        }
        
        // Test 5: Integridad de Datos
        testResultsText.text += "\nTest 5: Data Integrity\n";
        
        // Guardar datos
        economySystem.ResetEconomyData();
        yield return new WaitForSeconds(0.5f);
        economySystem.AddGold(100);
        yield return new WaitForSeconds(0.2f);
        
        // Corromper archivo (modificar unos bytes)
        if (File.Exists(economyDataPath))
        {
            byte[] data = File.ReadAllBytes(economyDataPath);
            
            // Modificar algunos bytes para corromper el archivo
            if (data.Length > 20)
            {
                for (int i = 15; i < 20; i++)
                {
                    data[i] = 0xFF;
                }
                
                // Escribir datos corrompidos
                File.WriteAllBytes(economyDataPath, data);
                
                // Intentar cargar datos corrompidos
                economySystem.SendMessage("LoadEconomyDataForTest");
                yield return new WaitForSeconds(0.5f);
                
                // Verificar que se detectó la corrupción
                if (economySystem.statusText.text.Contains("Error loading") || 
                    economySystem.statusText.text.Contains("integrity") || 
                    economySystem.statusText.text.Contains("decrypt"))
                {
                    testResultsText.text += "✓ Data corruption detected successfully\n";
                    passedTests++;
                }
                else
                {
                    testResultsText.text += "✗ Failed to detect data corruption\n";
                }
            }
            else
            {
                testResultsText.text += "✗ Save file too small to test corruption\n";
            }
        }
        else
        {
            testResultsText.text += "✗ Save file not found for corruption test\n";
        }
        
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
    }
}