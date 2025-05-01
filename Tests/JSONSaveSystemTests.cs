using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class JSONSaveSystemTests : MonoBehaviour
{
    public JSONSaveSystem saveSystem;
    public Text testResultsText;
    
    private string testSavePath;
    
    void Awake()
    {
        testSavePath = Path.Combine(Application.persistentDataPath, "savedata.json");
    }
    
    public void RunTests()
    {
        if (saveSystem == null || testResultsText == null)
        {
            Debug.LogError("Missing references for testing!");
            return;
        }
        
        testResultsText.text = "Running JSON Save System Tests...\n";
        int passedTests = 0;
        int totalTests = 4;
        
        // Test 1: Save and Load JSON Data
        testResultsText.text += "Test 1: Save and Load JSON Data\n";
        
        // Borrar archivo existente para asegurar consistencia
        if (File.Exists(testSavePath))
        {
            File.Delete(testSavePath);
        }
        
        // Añadir datos de prueba
        saveSystem.playerNameInput.text = "TestHero";
        saveSystem.AddExperience(250);
        saveSystem.AddItem("potion_02", "Mana Potion", 5);
        saveSystem.CompleteQuest("quest_village_rats");
        saveSystem.ChangeLevel("dungeon_01");
        
        // Guardar juego
        saveSystem.SaveGame();
        
        // Verificar que el archivo existe
        bool fileExists = File.Exists(testSavePath);
        testResultsText.text += fileExists ? "✓ Save file created\n" : "✗ Save file not created\n";
        
        if (fileExists)
        {
            // Borrar datos y cargar
            saveSystem.DeleteSaveFile();
            saveSystem.LoadGame();
            
            // Verificar que los datos se cargaron correctamente
            if (saveSystem.playerStatsText.text.Contains("TestHero") && 
                saveSystem.inventoryText.text.Contains("Mana Potion") &&
                saveSystem.questsText.text.Contains("quest_village_rats"))
            {
                testResultsText.text += "✓ Data loaded correctly\n";
                passedTests++;
            }
            else
            {
                testResultsText.text += "✗ Data did not load correctly\n";
            }
        }
        
        // Test 2: Inventory Management
        testResultsText.text += "\nTest 2: Inventory Management\n";
        
        saveSystem.DeleteSaveFile(); // Reset state
        
        // Añadir ítem nuevo
        saveSystem.AddItem("sword_02", "Iron Sword", 1);
        
        // Añadir al mismo ítem (aumentar cantidad)
        saveSystem.AddItem("potion_01", "Health Potion", 2);
        
        if (saveSystem.inventoryText.text.Contains("Iron Sword x1") &&
            saveSystem.inventoryText.text.Contains("Health Potion x5")) // 3 iniciales + 2 nuevas
        {
            testResultsText.text += "✓ Inventory management works correctly\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Inventory management failed\n";
        }
        
        // Test 3: Level Up System
        testResultsText.text += "\nTest 3: Level Up System\n";
        
        saveSystem.DeleteSaveFile(); // Reset state
        
        // Añadir suficiente experiencia para subir de nivel (>100)
        saveSystem.AddExperience(150);
        
        if (saveSystem.playerStatsText.text.Contains("Level: 2"))
        {
            testResultsText.text += "✓ Level up system works correctly\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Level up system failed\n";
        }
        
        // Test 4: Delete Save File
        testResultsText.text += "\nTest 4: Delete Save File\n";
        
        // Guardar juego para asegurar que existe el archivo
        saveSystem.SaveGame();
        
        // Borrar archivo
        saveSystem.DeleteSaveFile();
        
        bool fileDeleted = !File.Exists(testSavePath);
        
        if (fileDeleted && saveSystem.playerStatsText.text.Contains("Level: 1"))
        {
            testResultsText.text += "✓ Delete save file works correctly\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Delete save file failed\n";
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