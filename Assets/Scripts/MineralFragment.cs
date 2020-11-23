using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralFragment : MonoBehaviour
{
    public float cooldownTime = 2.0f;
    public float currentcooldownTime = 0.0f;
    public bool coolingdown = false;

    private void Awake()
    {
        coolingdown = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //coolingdown = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldown();
    }

    public bool Activated()
    {
        if (!coolingdown)
        {
            return true;
        }
        return false;
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
}
