using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Levels to Load")]
    public string newGameLevel;
    private string _levelToLoad;
    [SerializeField] private Button modBtn;

    [Header("Diary")]
    [SerializeField] private Button diaryBtn;

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text _volumeTextValue = null;
    [SerializeField] private Slider _volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Confirmation")]
    [SerializeField] private GameObject _comfirmationPrompt = null;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text mouseSenTextValue = null;
    [SerializeField] private Slider mouseSenSlider = null;
    [SerializeField] private int defaultMouseSen = 4;
    public int mainMouseSen = 4;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private int defaultBrightness = 0;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;


    private int _qualityLevel;
    private float _brightnessLevel;

    [Header("Resolution Settings")]
    public TMP_Text resolutionDropdownText;
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [SerializeField] private GameObject _canvas;
    [SerializeField] private AudioSource _backgroundAudio;
    public VideoPlayer VideoPlayer;
    public VideoClip antCinematic;
    [SerializeField] private bool speeded = false;


    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        if (PlayerPrefs.HasKey("gameFinished"))
            if (PlayerPrefs.GetInt("gameFinished") == 1)
                modBtn.interactable = true;

        if (PlayerPrefs.HasKey("diary"))
            if (PlayerPrefs.GetInt("diary") == 1)
                diaryBtn.interactable = true;

        LoadVolumeInfo();
    } 

    void Update()
    {
        if(VideoPlayer.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if(!speeded)
                VideoPlayer.playbackSpeed = 5;
            else
                VideoPlayer.playbackSpeed = 1;

            speeded = !speeded;
        }
    }   

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    void LoadCinematic(VideoPlayer vp)
    {
        vp.clip = antCinematic;
        vp.Play();         
        VideoPlayer.loopPointReached += LoadScene;
    }


    void LoadScene(VideoPlayer vp)
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void NewGameDialogYes()
    {
        _canvas.SetActive(false);
        _backgroundAudio.Stop();
        VideoPlayer.gameObject.SetActive(true);
        VideoPlayer.Play();
        //VideoPlayer.loopPointReached += LoadScene;
        VideoPlayer.loopPointReached += LoadCinematic;        
    }

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("gameFinished"))
        {
            _levelToLoad = PlayerPrefs.GetString("gameFinished");
            SceneManager.LoadScene(_levelToLoad);
        }

    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void LoadVolumeInfo()
    {
        _volumeSlider.value = PlayerPrefs.GetFloat("masterVolume", AudioListener.volume);
        _volumeTextValue.text = PlayerPrefs.GetFloat("masterVolume", AudioListener.volume).ToString("0.0"); ;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        _volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetMouseSen(float sensitivity)
    {
        mainMouseSen = Mathf.RoundToInt(sensitivity);
        mouseSenTextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        PlayerPrefs.SetFloat("masterSen", mainMouseSen);
        StartCoroutine(ConfirmationBox());
    }

    public void LoadGameplayInfo()
    {
        mouseSenSlider.value = PlayerPrefs.GetFloat("masterSen", mainMouseSen);
        mouseSenTextValue.text = mouseSenSlider.value.ToString("0");
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness * 10f;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        FindObjectOfType<BrightnessController>().updateBrightness(_brightnessLevel);
        //change brightness post processing

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        StartCoroutine(ConfirmationBox());
    }


    public void ResetButton(string MenuType)
    {
        if (MenuType == "Graphics")
        {
            //reset brightness
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;

            GraphicApply();
        }


        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            _volumeSlider.value = defaultVolume;
            _volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if (MenuType == "Gameplay")
        {
            mouseSenTextValue.text = defaultMouseSen.ToString("0");
            mouseSenSlider.value = defaultMouseSen;
            mainMouseSen = defaultMouseSen;
            GameplayApply();
        }
    }

    public void LoadDiary() => SceneManager.LoadScene("Diary");

    public void LoadSpiderFight() => SceneManager.LoadScene("SpiderFight");

    public IEnumerator ConfirmationBox()
    {
        _comfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2f);
        _comfirmationPrompt.SetActive(false);
    }
}
