using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private Vector3 targetPoint;
    public float shootDistance = 50f;
    public float shootSpeed = 50f;
    private RaycastHit rayHit;
    private Ray ray;
    public GameObject canon;
    private GameObject laserShoot;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.ShootGunButton()) {
            ShootLaser();
        }
    }

    private void ShootLaser() {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 direction = new Vector3();
        if (Physics.Raycast(ray, out rayHit, shootDistance)) {
            direction = (rayHit.point - canon.transform.position).normalized;
            //laserShoot.GetComponent<Rigidbody>().velocity = direction*shootSpeed;
        } else {
            direction = ray.GetPoint(1000).normalized;
        }
        laserShoot = Instantiate(bulletPrefab, canon.transform.position, Quaternion.identity);
        laserShoot.GetComponent<Rigidbody>().AddForce(direction * shootSpeed, ForceMode.Impulse);
        Destroy(laserShoot, 5f);
    }
}
