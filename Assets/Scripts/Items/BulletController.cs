using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float lifeTime;

    private void Start()
    {
        //Cuando aparezca el objeto, lanzaremos el m�todo DestroyOverLifetime despu�s de su tiempo de vida.
        Invoke("DestroyOverLifetime", lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si choca con un jugador, un enemigo, el suelo o un �tem destru�ble.
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy") || collision.CompareTag("Ground") || collision.CompareTag("Destroyable"))
        {
            //Destruimos el objeto.
            Destroy(gameObject);
        }
    }

    void DestroyOverLifetime()
    {
        Destroy(gameObject);
    }
}
