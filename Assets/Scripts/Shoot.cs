using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private GameObject arCamera;
    public GameObject explosion;
    RaycastHit hit;

    void Start()
    {
        arCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        // user tapped on the screen
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit))
            {
                if (hit.transform.tag == "Spider")
                {
                    // destroy spider
                    Destroy(hit.transform.gameObject);
                    // create explosion at position spider at rotation spider
                    GameObject createdExplosion = Instantiate(explosion, hit.transform.position, hit.transform.rotation);
                    // destroy explosion after 2s
                    // ERROR Destroy(explosion.gameObject);
                    Destroy(createdExplosion, 2);
                }
            }
        }
    }
}
