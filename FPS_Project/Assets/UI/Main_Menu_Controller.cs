using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using static Save_Manager;

public class Main_Menu_Controller : MonoBehaviour
{
    public string newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSave = null;
    //private Save_Manager.PlayerData playerData;
    //private Save_Manager.EnemyData enemyData;

    //For Volume Setting
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSliderValue = null;
    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private float defaultVolume = 1.0f;

    //Game Play Setting
    [SerializeField] private TMP_Text controllerSensitivityValue;
    [SerializeField] private Slider controllerSensitivitySliderValue;
    [SerializeField] private int defaultSensitivity = 4;
    public int mainController = 4;
    [SerializeField] private Toggle invertYToggle = null;

    //Graphics Seting
    [SerializeField] private Slider brightnessSliger = null;
    [SerializeField] private TMP_Text brightnessText = null;
    [SerializeField] private float defaultBrightness = 1;

    private int qualityLevel;
    private bool isFullScreen;
    private float brightnessLevel;
    public TMP_Dropdown resolutionDropDown;
    private Resolution[] resolutions;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle; 
    private void Start()
    {
        resolutions=Screen.resolutions;
        resolutionDropDown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolution = 0;

        for(int i = 0;i< resolutions.Length; i++)
        {
            string option = resolutions[i].width+" x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height==Screen.height ) {
                         currentResolution= i;
            }
        }
        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolution;
        resolutionDropDown.RefreshShownValue();

    }
    public void SetResoltuion(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }
    public void NewGameDailogueYes()
    {
        SceneManager.LoadScene(newGameLevel);
    }
    /*public void LoadGame()
    {
        Save_Manager saveManager = Save_Manager.Instance;
        saveManager.LoadGame(ref playerData, ref enemyData);
    }*/
    public void LoadGameDailogueYes() 
    {
        if (PlayerPrefs.HasKey("SaveLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSave.SetActive(true);
        }
    }

    public void exit()
    {
        Application.Quit();
    }
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }
    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }
    public void SetControllerSensitivty(float sensitivty)
    {
        mainController = Mathf.RoundToInt(sensitivty);
        controllerSensitivityValue.text = sensitivty.ToString("0");
    }
    public void GameplayApply()
    {
        if(invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
            
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
        }
        PlayerPrefs.SetFloat("masterSensitivity", mainController);
        StartCoroutine (ConfirmationBox());
    }

    public void SetBrightness(float _brightness)
    {
        brightnessLevel = _brightness;
        brightnessText.text = _brightness.ToString("0.0");

    }
    public void SetFullScreen(bool _isFullScreen)
    {
        isFullScreen = _isFullScreen;
    }
    public void SetQuality(int _qualityLevel)
    {
        qualityLevel = _qualityLevel;
    }
    public void GraphicApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", brightnessLevel);
        QualitySettings.SetQualityLevel(qualityLevel);
        PlayerPrefs.SetInt("masterfullscreen",(isFullScreen? 1:0));
        Screen.fullScreen = isFullScreen;
        StartCoroutine (ConfirmationBox());


    }
    public void Reset_btn(string MenuType)
    {
        if (MenuType == "Grpahics")
        {
            brightnessSliger.value = defaultBrightness;
            brightnessText.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution=Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropDown.value = resolutions.Length;
            GraphicApply();

        }
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSliderValue.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
        if(MenuType == "Gameplay")
        {
            controllerSensitivityValue.text = defaultSensitivity.ToString("0");
            controllerSensitivitySliderValue.value = defaultSensitivity;
            mainController = defaultSensitivity;
            invertYToggle.isOn = false;
            GameplayApply();
        }
    }
    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
