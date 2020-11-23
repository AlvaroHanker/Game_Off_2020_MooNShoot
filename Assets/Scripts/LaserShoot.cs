using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShoot : MonoBehaviour
{
    public GameObject hitParticles;
    private GameObject particlesReference;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        if (other.collider.CompareTag("InventoryBox")) {
            //other.gameObject.GetComponent<caja>().soltarRecursos();
        }
        particlesReference = Instantiate(hitParticles, transform.position, Quaternion.identity);
        Destroy(particlesReference, 1f);
        Destroy(this.gameObject);
    }
}
