using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LunarModule : MonoBehaviour
{
    public GameObject myPlayer;
    [SerializeField]
    private int resourcesNeededToWin = 100;
    [SerializeField]
    private int currentResources = 0;
    private GameObject playerInventoryBox;
    private bool winner = false;
    [SerializeField]
    private GameObject menuLunarModule;
    public Image alomooniteBarFilling;
    private bool selectedItem = false;
    [SerializeField]
    private Text mineAmountText;
    public int blackHoleCost = 10;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("AddAlomoonite", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentResources >= resourcesNeededToWin && !winner) {
            winner = true;
            GameManager.instance.SetWinner(myPlayer);
        }
    }

    /// <summary>
    /// Si estas en rango de tu nave, automaticamente debería guardar los recursos que lleves encima
    /// Debería comprobar si has llegado o superado los 100 para terminar la partida como ganador
    /// </summary>
    /// <param name="player"> objeto del jugador para coger el contenido de su caja </param>
    private void StoreResources(GameObject player) {
        foreach (Transform child in player.transform) {
            if (child.CompareTag("InventoryBox")) {
                playerInventoryBox = child.gameObject;
                //currentResources += playerInventoryBox.TransferResources(); // Mi idea es que este metodo en la caja devuelva un int con los recursos actuales en la caja, 
                                                                                // y se los ponga a 0 ya que pasan a la nave
            }
        }
        UpdateAlomooniteBar();
    }

    /// <summary>
    /// Se lanza al presionar la F sobre tu modulo lunar. Abriria el menu de la nave para craftear cosas
    /// </summary>
    public void Interaction() {
        menuLunarModule.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        mineAmountText.text = myPlayer.GetComponent<PlayerBehaviors>().currentMines.ToString() + "/3";
    }

    public void ExitLunarModuleHUD() {
        Cursor.visible = false;
        selectedItem = false;
    }

    private void UpdateAlomooniteBar() {
        float current_fill = (float) currentResources / (float) resourcesNeededToWin;

        alomooniteBarFilling.fillAmount = Mathf.Clamp(current_fill, 0, resourcesNeededToWin);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && other.gameObject == myPlayer) {
            StoreResources(other.gameObject);
        }
    }

    private void AddAlomoonite() {
        currentResources += 4;
        UpdateAlomooniteBar();
    }

    public int getCurrentResources() {
        return currentResources;
    }

    // Pensar bien como organizar la logica de crafteo y seleccion de item a la vez que controlar la cantidad de minas/modificadores que lleva el jugador y cuales
    public void SelectItem() {
        selectedItem = true;
    }

    public void Craft() {
        if (selectedItem && currentResources >= blackHoleCost && myPlayer.GetComponent<PlayerBehaviors>().currentMines < 3) {
            currentResources -= blackHoleCost;
            myPlayer.GetComponent<PlayerBehaviors>().currentMines++;
        }
        mineAmountText.text = myPlayer.GetComponent<PlayerBehaviors>().currentMines.ToString() + "/3";
    }
}
