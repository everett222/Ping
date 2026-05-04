using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "GameScene"; 

    [Header("Menu Panels")]
    public GameObject modePanel;   
    public GameObject pointsPanel; 

    private void Start()
    {
        if (modePanel != null) modePanel.SetActive(true);
        if (pointsPanel != null) pointsPanel.SetActive(false);
    }

    // --- STEP 1: CHOOSE THE MODE ---
    
    public void PlayEasyAI()
    {
        PlayerPrefs.SetInt("GameMode", 0); 
        ShowPointsPanel();
    }

    public void PlayMediumAI()
    {
        PlayerPrefs.SetInt("GameMode", 2); 
        ShowPointsPanel();
    }

    public void PlayHardAI()
    {
        PlayerPrefs.SetInt("GameMode", 3); 
        ShowPointsPanel();
    }

    public void PlayTwoPlayer()
    {
        PlayerPrefs.SetInt("GameMode", 1); 
        ShowPointsPanel();
    }

    private void ShowPointsPanel()
    {
        modePanel.SetActive(false);
        pointsPanel.SetActive(true);
    }

    // --- STEP 2: CHOOSE THE POINTS & START ---

    // UPDATED: Now plays to 11
    public void PlayTo11()
    {
        PlayerPrefs.SetInt("PointCap", 11); // Saves 11 as the cap
        SceneManager.LoadScene(gameSceneName);
    }

    // UPDATED: Now plays to 22
    public void PlayTo22()
    {
        PlayerPrefs.SetInt("PointCap", 22); // Saves 22 as the cap
        SceneManager.LoadScene(gameSceneName);
    }
}