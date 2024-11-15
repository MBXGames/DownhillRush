using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI inputScore;
    [SerializeField]
    private TextMeshProUGUI inputTime;
    [SerializeField]
    private TMP_InputField inputName;

    public UnityEvent<string, int, string> submitScoreEvent;

    public void SubmitScore()
    {
        submitScoreEvent.Invoke(inputName.text, int.Parse(inputScore.text.Replace("Puntuación: ", "")), inputTime.text.Replace("Tiempo: ", ""));
    }
}
