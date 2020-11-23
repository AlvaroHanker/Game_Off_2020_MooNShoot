using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviors : MonoBehaviour, IEffectHandler
{
    public GameObject myLunarModule;
    private GameObject ownedInventoryBox;
    public bool boxInPossession = true;

    /// <summary>
    /// PROVISIONAL HASTA SABER DONDE PONERLO
    /// </summary>
    public float attractionSpeed = 15f;
    /// <summary>
    /// Objeto dentro de area de deteccion del player al que se esta mirando para interactuar
    /// </summary>
    public Transform interactionItem = null;
    private Ray cameraRay;
    private RaycastHit rayHit;
    [SerializeField]
    private float INTERACT_DISTANCE = 2f;
    [SerializeField]
    private float DISTANCE_WEIGHT = 0.6f;
    [SerializeField]
    private float ANGLE_WEIGHT = 0.4f;
    [SerializeField]
    private float INTERACTION_ANGLE = 30f;
    private float INTERACT_ANGLE;
    private bool preventInteraction = false;
    public Material enabledMat;
    public Material disabledMat;
    [SerializeField]
    private int resourcesCapacity = 10;
    private int currentResources = 0;
    public int currentMines = 0;
    public GameObject blackHoleMinePrefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            if (child.CompareTag("InventoryBox")) {
                ownedInventoryBox = child.gameObject;
            }
        }
        //ownedInventoryBox = GetComponentInChildren<BoxInventoryScript>().gameObject;
        INTERACT_ANGLE = Mathf.Cos(Mathf.Rad2Deg*INTERACTION_ANGLE);
    }

    // Update is called once per frame
    void Update()
    {
        DetectionArea();
        //!preventInteraction && 
        if (InputManager.InteractionButton()) {
            if (boxInPossession) {
                DropInventoryBox();
            } else {
                if (interactionItem != null) {
                    if (interactionItem.CompareTag("InventoryBox")) {
                        PickInventoryBox();
                    } else {
                        // interaccion con el objeto en cuestion
                        if (interactionItem.GetComponent<LunarModule>() && interactionItem.gameObject == myLunarModule) {
                            interactionItem.GetComponent<LunarModule>().Interaction();
                        }
                        /*if () { // veta de mineral

                        }*/
                        /*if () { // mineral
                            if (currentResources < 10) {
                                PickAlomoonite();
                                //¿hace falta hacer interactionItem = null?
                                Destroy (interactionItem);
                            }
                        }*/
                    }
                }
            }
        }
        if (currentMines > 0 && InputManager.TrapButton()) {
            PlantBlackHoleMine();
        }
    }

    public void OnEffectEnter(IEffectSource source) {
        if (source.owner != gameObject) {// La idea es saber si soy o no el owner para que no me afecte mi propia mina o efecto
            switch (source.type) {
                case EffectType.MOO_MINE:
                    if (boxInPossession) {
                        // tirar aloomonite
                    } else {
                        // incapacitar para actuar
                    }
                    break;
                case EffectType.BLACK_HOLE:
                    //if (source.owner == gameObject) { // La idea es saber si soy o no el owner para que no me afecte mi propia mina
                    //Debug.Log("En blackhole");
                        if (boxInPossession) {
                            DropInventoryBox();
                        }
                        //preventInteraction = true;
                        transform.position = Vector3.MoveTowards(transform.position, source.GetGameObject().transform.position, attractionSpeed * Time.deltaTime);
                    //}
                    break;
                case EffectType.STUN:
                    // animacion de stun
                    // bloquear movimiento
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// CREO QUE NUNCA SE TRIGGEREA PORQUE SE DESTRUYEN LOS COLLIDERS Y OBJETOS DE MINA, ETC ANTES DE DETECTAR EL EXIT
    /// </summary>
    /// <param name="source"></param>
    public void OnEffectExit(IEffectSource source) {
        if (source.owner != gameObject) {
            switch (source.type) {
                case EffectType.MOO_MINE:
                    if (boxInPossession) {
                        // tirar aloomonite 2 segundos mas
                    } else {
                        // incapacitar para actuar 2 segundos mas
                    }
                    break;
                case EffectType.BLACK_HOLE:

                    break;
                case EffectType.STUN:
                    // ¿nada?
                    break;
                default:
                    break;
            }
        }
    }    
    
    public void OnEffectStay(IEffectSource source) {
        if (source.owner != gameObject) {
            switch (source.type) {
                case EffectType.MOO_MINE:
                    if (boxInPossession) {
                        // tirar aloomonite
                    } else {
                        // incapacitar para actuar
                    }
                    break;
                case EffectType.BLACK_HOLE:
                    //if (source.owner == gameObject) { // La idea es saber si soy o no el owner para que no me afecte mi propia mina
                        if (boxInPossession) {
                            DropInventoryBox();
                        }
                        //preventInteraction = true;
                        transform.position = Vector3.MoveTowards(transform.position, source.GetGameObject().transform.position, attractionSpeed * Time.deltaTime);
                    //}
                    break;
                case EffectType.STUN:
                    // ¿nada?
                    break;
                default:
                    break;
            }
        }

    }
    
    /// <summary>
    /// Metodo que recoge la caja del jugador y se la coloca en la cabeza.
    /// Relacion padre-hijo
    /// </summary>
    public void PickInventoryBox() {
        // animacion coger caja
        ownedInventoryBox = interactionItem.gameObject;
        ownedInventoryBox.transform.parent = gameObject.transform;
        ownedInventoryBox.transform.forward = transform.forward;
        ownedInventoryBox.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        boxInPossession = true;
    }

    /// <summary>
    /// Metodo que suelta la caja del jugador en el suelo delante de el.
    /// Relacion padre-hijo
    /// </summary>
    public void DropInventoryBox() {
        ownedInventoryBox.transform.parent = null;
        Vector3 aux = transform.position + transform.forward * 2;
        ownedInventoryBox.transform.position = aux;
        //ownedInventoryBox.transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 2);
        boxInPossession = false;
    }

    /// <summary>
    /// Metodo para llamar cuando el player entra en rango de su caja(?)
    /// </summary>
    /// <returns></returns>
    public int TransferResourcesToInventoryBox() {
        int aux = currentResources;
        currentResources = 0;
        return aux;
    }

    /// <summary>
    /// Metodo que genera un area esferica de X diametro y detecta todos los objetos dentro de ella que tengan la layer 9 - Interactable
    /// Comprobará los items que esten en un rango [-30º, 30º] con respecto al forward del personaje para determinar el objeto
    /// a ser interactuado
    /// </summary>
    private void DetectionArea() {
        Collider[] interactablesInRange = Physics.OverlapSphere(transform.position, INTERACT_DISTANCE, 1<<9);
        //Debug.Log(interactablesInRange.Length);
        INTERACT_ANGLE = Mathf.Cos(Mathf.Rad2Deg*INTERACTION_ANGLE);
        Vector3 direction = new Vector3();
        Vector3 distanceVector;
        float distance;
        float maxDistance = INTERACT_DISTANCE * INTERACT_DISTANCE;
        float previousDistance = maxDistance;
        float elegibility;
        float previousElegibility = 0f;
        if (interactionItem != null && interactionItem.GetComponent<MeshRenderer>() != null) {
            //interactionItem.GetComponent<MeshRenderer>().material = disabledMat;
        }
        interactionItem = null;
        for (int i = 0; i < interactablesInRange.Length; ++i) {
            //Debug.Log(interactablesInRange[i].gameObject.layer);
            if (interactablesInRange[i].gameObject.layer == 9) {
                distanceVector = interactablesInRange[i].transform.position - transform.position;
                direction = distanceVector.normalized;
                float angle = Vector3.Dot(transform.forward, direction);
                if (-angle < INTERACT_ANGLE) {
                    distance = distanceVector.sqrMagnitude;
                    elegibility = ((maxDistance - distance) / maxDistance) * DISTANCE_WEIGHT + ((1f - Mathf.Abs(angle)) / (1 - INTERACT_ANGLE)) * ANGLE_WEIGHT;
                    if (elegibility > previousElegibility) {
                        previousElegibility = elegibility;
                        interactionItem = interactablesInRange[i].gameObject.transform;
                    }
                }
            }
        }
        if (interactionItem != null) {
            //interactionItem.GetComponent<MeshRenderer>().material = enabledMat;
            //Debug.Log(interactionItem.name);
        }
    }

    private void PlantBlackHoleMine() {
        Vector3 aux = transform.position + transform.forward * 2;
        GameObject mineInstance = Instantiate(blackHoleMinePrefab, aux, Quaternion.identity);
        currentMines--;
        mineInstance.GetComponent<BlackHoleActivation>().owner = this.gameObject;
    }
}
