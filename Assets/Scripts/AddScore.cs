using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    private ScoreController scoreController;
    public GameObject explosion;
    void Start()
    {
        scoreController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreController>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.tag == "Spider")
        {
            scoreController.Score++;
        }
        GameObject CreatedExplosion = Instantiate(explosion, transform.position, transform.rotation);
        CreatedExplosion.transform.localScale = transform.localScale;
        Destroy(gameObject);
        Destroy(CreatedExplosion, 2f);
    }
}
