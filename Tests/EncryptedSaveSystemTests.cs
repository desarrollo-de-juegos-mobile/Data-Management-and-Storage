using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class EncryptedSaveSystemTests : MonoBehaviour
{
    public EncryptedSaveSystem saveSystem;
    public Text testResultsText;
    
    private string savePath;
    private string checksumPath;
    
    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "encrypted_save.dat");
        checksumPath = Path.Combine(Application.persistentDataPath, "checksum.dat");
    }
    
    public void RunTests()
    {
        if (saveSystem == null || testResultsText == null)
        {
            Debug.LogError("Missing references for testing!");
            return;
        }
        
        testResultsText.text = "Running Encrypted Save System Tests...\n";
        int passedTests = 0;
        int totalTests = 5;
        
        // Borrar archivos de guardado para empezar limpio
        if (File.Exists(savePath)) File.Delete(savePath);
        if (File.Exists(checksumPath)) File.Delete(checksumPath);
        
        // Test 1: Guardar y Cargar Datos Encriptados
        testResultsText.text += "Test 1: Save and Load Encrypted Data\n";
        
        // Preparar datos
        saveSystem.ResetGame();
        saveSystem.playerNameInput.text = "CryptoPlayer";
        saveSystem.AddScore(777);
        
        // Guardar juego encriptado
        saveSystem.SaveGame();
        
        // Verificar que los archivos existen
        bool saveFileExists = File.Exists(savePath);
        bool checksumExists = File.Exists(checksumPath);
        
        if (saveFileExists && checksumExists)
        {
            testResultsText.text += "✓ Encrypted save file and checksum created\n";
            
            // Resetear datos
            saveSystem.ResetGame();
            
            // Cargar datos encriptados
            saveSystem.LoadGame();
            
            // Verificar que se cargaron correctamente
            if (saveSystem.playerNameInput.text == "CryptoPlayer" && 
                saveSystem.scoreText.text.Contains("777"))
            {
                testResultsText.text += "✓ Encrypted data loaded correctly\n";
                passedTests++;
            }
            else
            {
                testResultsText.text += "✗ Encrypted data did not load correctly\n";
            }
        }
        else
        {
            testResultsText.text += "✗ Failed to create encrypted files\n";
        }
        
        // Test 2: Verificación de Integridad
        testResultsText.text += "\nTest 2: Data Integrity Check\n";
        
        // Guardar datos para tener un archivo válido
        saveSystem.ResetGame();
        saveSystem.AddScore(123);
        saveSystem.SaveGame();
        
        // Corromper el archivo
        saveSystem.CorruptSaveFile();
        
        // Intentar cargar datos corrompidos
        saveSystem.LoadGame();
        
        // Verificar si el sistema detectó la corrupción
        if (saveSystem.statusText.text.Contains("integrity") || 
            saveSystem.statusText.text.Contains("Decryption failed"))
        {
            testResultsText.text += "✓ Data corruption detected successfully\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Failed to detect data corruption\n";
        }
        
        // Test 3: Cambio de Clave de Encriptación
        testResultsText.text += "\nTest 3: Change Encryption Key\n";
        
        // Guardar con clave predeterminada
        saveSystem.ResetGame();
        saveSystem.AddScore(456);
        saveSystem.customSecretKeyInput.text = "DefaultKey123"; 
        saveSystem.SaveGame();
        
        // Intentar cargar con clave incorrecta
        saveSystem.ResetGame();
        saveSystem.customSecretKeyInput.text = "WrongKey456";
        saveSystem.LoadGame();
        
        // Verificar que la carga falló por clave incorrecta
        if (saveSystem.statusText.text.Contains("Decryption failed") &&
            !saveSystem.scoreText.text.Contains("456"))
        {
            testResultsText.text += "✓ Rejected load with incorrect key\n";
            
            // Ahora cargar con la clave correcta
            saveSystem.customSecretKeyInput.text = "DefaultKey123";
            saveSystem.LoadGame();
            
            if (saveSystem.scoreText.text.Contains("456"))
            {
                testResultsText.text += "✓ Loaded with correct key\n";
                passedTests++;
            }
            else
            {
                testResultsText.text += "✗ Failed to load with correct key\n";
            }
        }
        else
        {
            testResultsText.text += "✗ Failed encryption key test\n";
        }
        
        // Test 4: Borrado de Archivos de Guardado
        testResultsText.text += "\nTest 4: Delete Save Files\n";
        
        // Asegurarse de que hay archivos para borrar
        saveSystem.SaveGame();
        
        // Borrar archivos
        saveSystem.DeleteSaveFile();
        
        // Verificar que se borraron
        bool filesDeleted = !File.Exists(savePath) && !File.Exists(checksumPath);
        
        if (filesDeleted)
        {
            testResultsText.text += "✓ Save files deleted successfully\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Failed to delete save files\n";
        }
        
        // Test 5: Manejo de Archivos Inexistentes
        testResultsText.text += "\nTest 5: Handle Missing Save Files\n";
        
        // Asegurarse de que no hay archivos
        if (File.Exists(savePath)) File.Delete(savePath);
        if (File.Exists(checksumPath)) File.Delete(checksumPath);
        
        // Intentar cargar archivos inexistentes
        saveSystem.LoadGame();
        
        // Verificar que se manejó adecuadamente
        if (saveSystem.statusText.text.Contains("not found"))
        {
            testResultsText.text += "✓ Missing files handled properly\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Failed to handle missing files\n";
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