using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startInstructions;
    [SerializeField]
    private Text winnerText;
    [SerializeField]
    private GameObject endGameHUD;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startInstructions.activeSelf && InputManager.Exit()) {
            HideInstructions();
        }
    }

    public void HideInstructions() {
        startInstructions.SetActive(false);    
    }

    public void ShowWinner(GameObject winner) {
        winnerText.transform.parent.gameObject.SetActive(true);
        winnerText.text = winner.name + " WINS!!!";
    }

    public void ShowEndGameHUD() {
        endGameHUD.SetActive(true);
    }

    public void ExitGame() {
        Application.Quit();
    }

    /*public void Craft() {
        
    }*/
}
