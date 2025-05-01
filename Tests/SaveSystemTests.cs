using UnityEngine;
using UnityEngine.UI;

public class SaveSystemTests : MonoBehaviour
{
    public SaveSystem saveSystem;
    public Text testResultsText;
    
    public void RunTests()
    {
        if (saveSystem == null || testResultsText == null)
        {
            Debug.LogError("Missing references for testing!");
            return;
        }
        
        testResultsText.text = "Running tests...\n";
        int passedTests = 0;
        int totalTests = 3;
        
        // Test 1: Save and Load Basic Data
        testResultsText.text += "Test 1: Save and Load Basic Data\n";
        
        // Set initial values
        saveSystem.ResetGame();
        saveSystem.AddScore(100);
        saveSystem.LevelUp();
        saveSystem.CollectItem();
        saveSystem.CollectItem();
        
        // Save game
        saveSystem.SaveGame();
        
        // Reset and verify reset worked
        saveSystem.ResetGame();
        
        // Load and verify data was restored
        saveSystem.LoadGame();
        
        // Check if score text contains "Score: 100"
        if (saveSystem.GetComponent<SaveSystem>().scoreText.text.Contains("Score: 100"))
        {
            testResultsText.text += "✓ Score loaded correctly\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Score did not load correctly\n";
        }
        
        // Test 2: Player Name Persistence
        testResultsText.text += "\nTest 2: Player Name Persistence\n";
        
        saveSystem.ResetGame();
        saveSystem.playerNameInput.text = "TestPlayer123";
        saveSystem.SaveGame();
        saveSystem.playerNameInput.text = "";
        saveSystem.LoadGame();
        
        if (saveSystem.playerNameInput.text == "TestPlayer123")
        {
            testResultsText.text += "✓ Player name persisted correctly\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Player name did not persist correctly\n";
        }
        
        // Test 3: Reset Functionality
        testResultsText.text += "\nTest 3: Reset Functionality\n";
        
        saveSystem.AddScore(500);
        saveSystem.ResetGame();
        
        if (saveSystem.GetComponent<SaveSystem>().scoreText.text.Contains("Score: 0"))
        {
            testResultsText.text += "✓ Reset functionality works correctly\n";
            passedTests++;
        }
        else
        {
            testResultsText.text += "✗ Reset functionality failed\n";
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