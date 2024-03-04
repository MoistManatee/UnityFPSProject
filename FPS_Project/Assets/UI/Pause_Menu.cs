using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Save_Manager;

public class Pause_Menu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused=false;

    private Save_Manager.PlayerData playerData;
    private Save_Manager.EnemyData enemyData;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);

        playerData = new Save_Manager.PlayerData();
        enemyData = new Save_Manager.EnemyData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ResumeGame()
    {
        //Debug.Log("ResumeGame called");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale=1; 
    }
    public void SaveGame()
    {
        Save_Manager.SaveGame(playerData, enemyData);

    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UI_Main_Menu");
    }
    public void QuitMenu()
    {
        Application.Quit();
    }
}
