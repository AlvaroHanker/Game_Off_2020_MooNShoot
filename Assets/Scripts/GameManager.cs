using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary> Instancia del GameManager para acceder desde cualquier parte del juego </summary>
    public static GameManager instance = null;

    public float gameDuration = 600f;
    public Text timerText;
    public bool lastMinute = false;
    private GameObject winner;

    public GameObject alarmHUD;
    [SerializeField]
    private GameObject[] lunarModules;
    [SerializeField]
    private GameObject[] players;
    private bool finishedGame = false;

    // Awake con patron singleton
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } 
        else if(instance != this) 
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CalculateTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!finishedGame) {
            if (gameDuration > 0) {
                if (winner != null) {
                    ShowWinner();
                } else {
                    if (gameDuration <= 60 && !lastMinute) {
                        lastMinute = true;
                        TimeAlarm();
                    }
                }
                CalculateTimer();
            } else {
                finishedGame = true;
                timerText.text = "0:00";
                CheckWinner();
            }
        }
    }

    private void CalculateTimer() {
        gameDuration -= Time.deltaTime;
        string minutes = Mathf.Floor(gameDuration / 60).ToString();
        string seconds = Mathf.Clamp(gameDuration % 60, 0, 59).ToString("00");
        timerText.text = minutes + ":" + seconds;
    }

    private void TimeAlarm() {
        alarmHUD.SetActive(true);
        Invoke("HideAlarmMessage", 5f);
    }

    public void SetWinner(GameObject player) {
        winner = player;
    }

    public void CheckWinner() {
        int winnerResources = 0;
        foreach (GameObject module in lunarModules)
        {
            if (module.GetComponent<LunarModule>().getCurrentResources() > winnerResources) {
                SetWinner(module.GetComponent<LunarModule>().myPlayer);
                winnerResources = module.GetComponent<LunarModule>().getCurrentResources();
            }
        }
        ShowWinner();
    }

    private void HideAlarmMessage() {
        alarmHUD.SetActive(false);
    }

    private void ShowWinner() {
        // Mostrar ganador
        foreach(GameObject player in players) {
            player.GetComponent<HUDManager>().ShowWinner(winner);
        }
        Invoke("ShowEndGameHUD", 5f);
    }

    private void ShowEndGameHUD() {
        foreach(GameObject player in players) {
            player.GetComponent<HUDManager>().ShowEndGameHUD();
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    /*public void EndGame() {
        Application.Quit(); // A cascoporro por ahora en build
    }*/
}
