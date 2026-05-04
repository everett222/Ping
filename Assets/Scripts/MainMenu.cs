using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "GameScene"; 

    [Header("Menu Panels")]
    public GameObject modePanel;   // The panel with 2 Playa, Easy, etc.
    public GameObject pointsPanel; // The new panel asking 12 or 21

    private void Start()
    {
        // Whenever the menu loads, make sure we see the Mode choices first
        if (modePanel != null) modePanel.SetActive(true);
        if (pointsPanel != null) pointsPanel.SetActive(false);
    }

    // --- STEP 1: CHOOSE THE MODE ---
    
    public void PlayEasyAI()
    {
        PlayerPrefs.SetInt("GameMode", 0); // 0 = Easy AI
        ShowPointsPanel();
    }

    public void PlayMediumAI()
    {
        PlayerPrefs.SetInt("GameMode", 2); // 2 = Medium AI
        ShowPointsPanel();
    }

    public void PlayHardAI()
    {
        PlayerPrefs.SetInt("GameMode", 3); // 3 = Hard AI
        ShowPointsPanel();
    }

    public void PlayTwoPlayer()
    {
        PlayerPrefs.SetInt("GameMode", 1); // 1 = Player 2
        ShowPointsPanel();
    }

    // This custom function swaps the screens!
    private void ShowPointsPanel()
    {
        modePanel.SetActive(false);
        pointsPanel.SetActive(true);
    }

    // --- STEP 2: CHOOSE THE POINTS & START ---

    public void PlayTo12()
    {
        PlayerPrefs.SetInt("PointCap", 12); // Saves 12 as the winning score
        SceneManager.LoadScene(gameSceneName);
    }

    public void PlayTo21()
    {
        PlayerPrefs.SetInt("PointCap", 21); // Saves 21 as the winning score
        SceneManager.LoadScene(gameSceneName);
    }
}