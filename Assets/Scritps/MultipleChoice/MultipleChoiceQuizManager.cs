using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scritps.MultipleChoice;
using TMPro;
using Lean.Gui;
using System.IO;
using UnityEngine.SceneManagement;

public class MultipleChoiceQuizManager : MonoBehaviour
{
    public QuizDataContainer easyQuestionsContainer;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI[] optionTexts;
    public LeanButton[] optionButtons;
    public TextMeshProUGUI timerText;
    public Text modalWindowText;
    public Image modalIcon;
    public TextMeshProUGUI scoreText;
    private List<QuizData> remainingQuestions;
    private QuizData currentQuestion;
    private int correctAnswers;
    private int totalQuestions;

    private float startTime;
    private bool isGameOver = false;  // ελεγχος εαν ο γυρος εχει ολοκληρωθει
    private string sceneName;
    private float elapsedTime;
    private int countriesGuessed = 0;
    void Update()
    {
        if (!isGameOver)
        {
            float elapsedTime = Time.time - startTime;
            UpdateTimerText(elapsedTime);

            if (remainingQuestions.Count <= 0)
            {
                // τελος παιχνιδιου οταν τελειωσουν ο ιερωτησεις
                DisableOptionButtons();
                float completionTime = Time.time - startTime;
                isGameOver = true;
            }
        }

        // και μονο αφου τελειωσει επιτρεπεται να εγκαταλειψει ο παικτης 
        if (isGameOver && Input.GetKeyUp(KeyCode.Escape) || Input.GetKey(KeyCode.Mouse1))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void Start()
    {
        startTime = Time.time;

        sceneName = SceneManager.GetActiveScene().name;
        
        //φορτωση των δεδομενων αναλογα με την σκηνη που εχει φορτωθει
        if (sceneName == "MultipleChoiceEasy")
        {
            LoadQuestions("Assets/Resources/Multiple1QuizData.json");
        }
        else if (sceneName == "MultipleChoiceHard")
        {
            LoadQuestions("Assets/Resources/Multiple2QuizData.json");
        }

        UpdateScoreText();
    }

    void LoadQuestions(string jsonPath)
    {
        string jsonText = File.ReadAllText(jsonPath);

        easyQuestionsContainer = JsonUtility.FromJson<QuizDataContainer>(jsonText);
        remainingQuestions = new List<QuizData>(easyQuestionsContainer.quizQuestions);
        correctAnswers = 0;
        totalQuestions = easyQuestionsContainer.quizQuestions.Count;

        LoadNextQuestion();
    }

    void LoadNextQuestion()
    {
        if (remainingQuestions.Count > 0)
        {
            int randomIndex = Random.Range(0, remainingQuestions.Count);
            currentQuestion = remainingQuestions[randomIndex];
            DisplayQuestion();
            remainingQuestions.RemoveAt(randomIndex);
        }
        else
        {
            EndGame();
        }
    }

    void EndGame()
    {
        int questionsPresented = Mathf.Min(countriesGuessed, totalQuestions);
        isGameOver = true;
        float elapsedTime = Time.time - startTime;
        int timeBonus = CalculateTimeBonus(elapsedTime);
        int correctAnswersScore = correctAnswers * 10;
        int totalScore = timeBonus + correctAnswersScore;

        // εαν το σκορ ειναι μεγαλυτερο απο το αποθηκευμενο γινεται overwrite
        int previousScore = PlayerPrefs.GetInt("MultipleChoiceEasyScore");
        if (totalScore > previousScore)
        {
            if (sceneName == "MultipleChoiceEasy")
            {
                PlayerPrefs.SetInt("MultipleChoiceEasyScore", totalScore);
                PlayerPrefs.Save();
            }
            else if (sceneName == "MultipleChoiceHard")
            {
                PlayerPrefs.SetInt("MultipleChoiceHardScore", totalScore);
                PlayerPrefs.Save();
            }     
        }

        scoreText.text = correctAnswers + " / " + questionsPresented + "Σωστές απαντήσεις";
        modalWindowText.text = "<size=30>Κουιζ ολοκληρώθηκε!</size>\r\n\r\n\r\n"
            + "Με " + correctAnswers + " σωστές απαντήσεις\r\n"
            + "Συνολικό χρόνος: " + elapsedTime.ToString("F2") + " δευτερόλεπτα\r\n"
            + "Το σκορ είναι: " + totalScore.ToString("F2") + ". Συγχαρητήρια!\r\n"
            + "Πιέστε 'Esc' για την επιστροφή στο μενού";
        modalIcon.color = Color.blue;
    }

    int CalculateTimeBonus(float elapsedTime)
    {
        float maxTime = 60.0f;
        float timePercentage = Mathf.Clamp01(1.0f - (elapsedTime / maxTime));
        int maxTimeBonus = 50;
        int timeBonus = Mathf.RoundToInt(timePercentage * maxTimeBonus);
        return timeBonus;
    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.question;
        for (int i = 0; i < currentQuestion.options.Length; i++)
        {
            optionTexts[i].text = currentQuestion.options[i];
        }
    }

    public void OnOptionSelected(int selectedOptionIndex)
    {
        if (!isGameOver)
        {
            countriesGuessed++;
            if (selectedOptionIndex == currentQuestion.correctOptionIndex)
            {
                modalWindowText.text = "<size=60>Σωστά!</size>\r\n\r\nΗ σωστή απάντηση είναι: \r\n" + currentQuestion.options[currentQuestion.correctOptionIndex];
                modalIcon.color = Color.green;
                correctAnswers++;
            }
            else
            {
                modalWindowText.text = "<size=60>Λάθος απάντηση!</size>\r\n\r\nΗ σωστή απάντηση είναι: \r\n" + currentQuestion.options[currentQuestion.correctOptionIndex];
                modalIcon.color = Color.red;
            }

            UpdateScoreText();

            if (countriesGuessed < 20)
            {
                LoadNextQuestion();
            }
            else
            {
                EndGame();
            }
        }
    }

    private void UpdateScoreText()
    {
        int questionsPresented = Mathf.Min(countriesGuessed, totalQuestions);
        float correctRatio = (float)correctAnswers / questionsPresented;
        scoreText.text = correctAnswers + " / " + questionsPresented + " Σωστές απαντήσεις (" + (correctRatio * 100f).ToString("F2") + "%)";
    }

    private void UpdateTimerText(float elapsedTime)
    {
        timerText.text = "Time: " + elapsedTime.ToString("F0") + "'";
    }

    void DisableOptionButtons()
    {
        foreach (var button in optionButtons)
        {
            button.interactable = false;
        }
    }

    public void GoBackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
