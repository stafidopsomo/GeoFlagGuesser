using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI easyScore;
    public TextMeshProUGUI hardScore;
    public TextMeshProUGUI continentsScore;
    public TextMeshProUGUI flagEasyScore;
    public TextMeshProUGUI flagHardScore;

    public Animator QuizesManager;
    public Animator LearningManager;

    public GameObject SettingsMenu;
    public AudioSource BGM;
    public Toggle checkbox;

    public void OpenMultipleChoiceEasy()
    {
        SceneManager.LoadScene("MultipleChoiceEasy");
    }

    public void OpenMultipleChoiceHard()
    {
        SceneManager.LoadScene("MultipleChoiceHard");
    }

    public void OpenContinents()
    {
        SceneManager.LoadScene("Continents");
    }

    public void Start()
    {
        easyScore.text = "Καλύτερο Σκόρ: " + PlayerPrefs.GetInt("MultipleChoiceEasyScore").ToString();
        hardScore.text = "Καλύτερο Σκόρ: " + PlayerPrefs.GetInt("MultipleChoiceHardScore").ToString();
        continentsScore.text = "Καλύτερο Σκόρ: " + PlayerPrefs.GetInt("DragContinentsHighScore").ToString();
        flagEasyScore.text = "Καλύτερο Σκόρ: " + PlayerPrefs.GetInt("QuizFlagEasyScore").ToString();
        flagHardScore.text = "Καλύτερο Σκόρ: " + PlayerPrefs.GetInt("QuizFlagHardScore").ToString();
        checkbox.isOn = PlayerPrefs.GetInt("MuteBGM") == 1 ? true : false;
    }
    public void Update()
    {
        if (PlayerPrefs.GetInt("MuteBGM") == 1)
        {
            BGM.mute = true;
        }
        else
        {
            BGM.mute = false;
        }
    }

    public void MuteBGM()
    {
        if (checkbox.isOn)
        {
            PlayerPrefs.SetInt("MuteBGM", 1);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("MuteBGM", 0);
            PlayerPrefs.Save();
        }
    }

    public void resetAllScores()
    {
        PlayerPrefs.SetInt("MultipleChoiceEasyScore", 0);
        PlayerPrefs.SetInt("MultipleChoiceHardScore", 0);
        PlayerPrefs.SetInt("DragContinentsHighScore", 0);
        PlayerPrefs.SetInt("QuizFlagEasyScore", 0);
        PlayerPrefs.SetInt("QuizFlagHardScore", 0);
        PlayerPrefs.Save();

        easyScore.text = "Καλύτερο Σκόρ: " + PlayerPrefs.GetInt("MultipleChoiceEasyScore").ToString();
        hardScore.text = "Καλύτερο Σκόρ: " + PlayerPrefs.GetInt("MultipleChoiceHardScore").ToString();
        continentsScore.text = "Καλύτερο Σκόρ: " + PlayerPrefs.GetInt("DragContinentsHighScore").ToString();
        flagEasyScore.text = "Καλύτερο Σκόρ: " + PlayerPrefs.GetInt("QuizFlagEasyScore").ToString();
        flagHardScore.text = "Καλύτερο Σκόρ: " + PlayerPrefs.GetInt("QuizFlagHardScore").ToString();
    }

    public void ShowQuizes()
    {
        QuizesManager.SetBool("ShowQuizes", true);
    }

    public void HideQuizes()
    {
        QuizesManager.SetBool("ShowQuizes", false);
    }

    public void ShowLearning()
    {
        LearningManager.SetBool("OpenLearning", true);
    }

    public void HideLearning()
    {
        LearningManager.SetBool("OpenLearning", false);
    }

    public void OpenLearningContinents()
    {
        SceneManager.LoadScene("LearningContinents");
    }

    public void OpenLearningFlags()
    {
        SceneManager.LoadScene("LearningFlags");
    }
    public void OpenQuizFlagsEasy()
    {
        SceneManager.LoadScene("QuizFlagsEasy");
    }
    public void OpenQuizFlagsHard()
    {
        SceneManager.LoadScene("QuizFlagsHard");
    }
    public void OpenLearningCountries()
    {
        SceneManager.LoadScene("LearningCountries");
    }

    public void OpenSettings()
    {
        SettingsMenu.SetActive(true);
    }
    public void CloseSettings()
    {
        SettingsMenu.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
