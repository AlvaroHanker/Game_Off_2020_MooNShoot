using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public int mineralAmount = 0;
    public int maxMineralAmount = 50;
    public GameObject owner = null;

    public GameObject[] mineralPrefabs; // Objects to be shown randomly when dropping mineral from box

    public SphereCollider mineralCollider;
    public float mineralRadius = 15.0f;
    public float playerRadius = 25.0f;

    public float cooldownTime = 2.0f;
    public float currentcooldownTime = 0.0f;
    public bool coolingdown = false;

    public Material boxMaterial;
    public List<Texture2D> texturasIndex;
    private int texInd = 0;

    private void Awake()
    {
        InitializeBox();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldown();
        CheckOwnerDistance();
    }

    // Quiza no es necesario al tener un cooldown el mineral cuando se mina o se saca de la caja
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "MineralFragment")
    //    {
    //        MineralFragment mineralDetected = other.gameObject.GetComponent<MineralFragment>();
    //        AcquireMineral(mineralDetected);
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "MineralFragment")
        {
            MineralFragment mineralDetected = other.gameObject.GetComponent<MineralFragment>();
            AcquireMineral(mineralDetected);

        }
    }


    public void InitializeBox()
    {
        mineralCollider = GetComponentInChildren<SphereCollider>();
        mineralCollider.radius = mineralRadius;
    }

    public void AcquireMineral(MineralFragment mineral)
    {
        if (mineral != null && mineral.Activated())
        {
            mineralAmount = Mathf.Clamp((mineralAmount + 1), 0, maxMineralAmount);
            Destroy(mineral.gameObject);
            Debug.Log("Mineral recogido!");
            texInd = CheckAmount();
            boxMaterial.SetTexture("MineralFeedBack", texturasIndex[texInd]);
        }
    }

    public void DropMineral()
    {
        if (!coolingdown && mineralAmount > 0)
        {
            GameObject mineralFragment = Instantiate(mineralPrefabs[Random.Range(0, mineralPrefabs.Length)]) as GameObject;
            mineralFragment.transform.position = transform.position + new Vector3(0.0f, 2.0f, 0.0f);

            Rigidbody rb = mineralFragment.GetComponent<Rigidbody>();
            Vector3 trajectory = new Vector3(0.0f, 0.0f, 0.0f);
            trajectory.x = Random.Range(-1.0f, 1.0f);
            trajectory.y = Random.Range(1.0f, 5.0f);
            trajectory.z = Random.Range(-1.0f, 1.0f);
            //float force = Random.Range(5.0f, 10.0f);
            float force = 5.0f;
            
            //rb.velocity = trajectory * force;

            rb.AddForce(trajectory * force, ForceMode.Impulse);

            // Cooldown
            coolingdown = true;

            mineralAmount = Mathf.Clamp((mineralAmount - 1), 0, maxMineralAmount);

            Debug.Log("Mineral soltado!");
        }
    }
       
    public void UpdateCooldown()
    {
        if (coolingdown)
        {
            currentcooldownTime += Time.deltaTime;

            if (currentcooldownTime >= cooldownTime)
            {
                currentcooldownTime = 0.0f;
                coolingdown = false;
            }
        }
    }

    public int TransferResources()
    {
        int mineralTransfered = mineralAmount;
        mineralAmount = 0;
        return mineralTransfered;
    }

    public void UnattendedBox()
    {
        // CAMBIAR MATERIAL
        MeshRenderer[] boxChildren = this.gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer child in boxChildren)
        {
            child.material = owner.GetComponentInChildren<InputTest>().defaultMaterial; // ESTO ES PROVISIONAL
            //child.material.SetColor("Color_80348FC8", owner.GetComponentInChildren<InputTest>().defaultColor); // ESTO ES PROVISIONAL
        }

        owner = null;

        // EFECTO PARA RESALTAR LA CAJA
        Debug.Log("Caja desatendida!");

    }

    public void CheckOwnerDistance()
    {
        if (owner != null)
        {
            //float distance = Vector3.Distance(owner.transform.position, transform.position);
            Vector3 distance2player = owner.transform.position - transform.position;
            float distance = distance2player.sqrMagnitude;

            //if (distance > playerRadius)
            if(distance > (playerRadius * playerRadius))
            {
                UnattendedBox();
            }
        }
    }

    public GameObject GetBoxOwner()
    {
        return owner;
    }

    public void ChangeBoxOwner(GameObject newOwner)
    {
        owner = newOwner;
        MeshRenderer[] boxChildren = this.gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer child in boxChildren)
        {
            child.material = newOwner.GetComponentInChildren<InputTest>().playerMaterial; // ESTO ES PROVISIONAL
            //child.material.SetColor("Color_80348FC8", newOwner.GetComponentInChildren<InputTest>().playerColor); // ESTO ES PROVISIONAL
        }
    }

    private int CheckAmount()
    {
        int textureIndex = 0;
        if (mineralAmount > 7)
            textureIndex += 1;
        if (mineralAmount>21)
            textureIndex += 1;
        if (mineralAmount > 28)
            textureIndex += 1;
        if (mineralAmount > 35)
            textureIndex += 1;
        if (mineralAmount > 42)
            textureIndex += 1;
        if (mineralAmount > 49)
            textureIndex += 1;
        return textureIndex;
    }


    //ESTO A BORRAR, SOLO ES PARA PRUEBAS EN INPUTTEST
    public void MineralAcquired()
    {
        mineralAmount = Mathf.Clamp((mineralAmount + 1), 0, maxMineralAmount);
    }
    
}
