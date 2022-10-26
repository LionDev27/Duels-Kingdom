using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    //ID del item.
    public int itemID;
    //Booleana que indicar� si estamos lanzando el item.
    public bool throwingItem;
    //Velocidad que determinar� si estamos lanzando el objeto o no.
    public Vector2 minVelocity;
    //Jugador que ha lanzado el �tem.
    public GameObject parentPlayer;

    //REFERENCIAS.
    Rigidbody2D rB;
    KnockbackApplier kA;

    private void Start()
    {
        rB = GetComponent<Rigidbody2D>();
        kA = GetComponent<KnockbackApplier>();
    }

    private void Update()
    {
        ToggleIgnoreLayerCollision();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si estamos lanzando el objeto, el objeto tiene el tag Player y puede recibir da�o.
        if (throwingItem && collision.collider.CompareTag("Player") && collision.gameObject.GetComponent<Damageable>().canTakeDamage)
        {
            Debug.Log("Colliding with Player");
            Vector2 knockback = collision.transform.position - transform.position;
            kA.ApplyKnockback(knockback, collision.transform, 1f, parentPlayer);
            throwingItem = false;
        }
        else if (collision.collider.CompareTag("Destroyable"))
        {
            collision.gameObject.GetComponent<DestroyableBox>().DestroyAndInstantiateObject();
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            //Comprobamos la velocidad del �tem
            InvokeRepeating("CheckItemVelocity", 0f, 0.5f);
        }
    }

    void CheckItemVelocity()
    {
        //Si la velocidad del objeto es menor que la velocidad m�nima.
        if (rB.velocity.magnitude < minVelocity.magnitude)
        {
            //Indicamos que no se est� lanzando el objeto.
            throwingItem = false;
            //Eliminamos del objeto parenPlayer la referencia al player que hab�a lanzado el objeto.
            parentPlayer = null;
            CancelInvoke();
        }
    }

    void ToggleIgnoreLayerCollision()
    {
        //Si estamos lanzando un item.
        if (throwingItem)
        {
            //Hacemos que no se ignore la colisi�n entre los Layers Player e Item.
            Physics2D.IgnoreLayerCollision(6, 3, false);
        }
        //Si no lo estamos lanzando.
        else
        {
            //Ignoramos la colisi�n del player. Esto hace que el jugador pueda pasar por delante de los �tems.
            Physics2D.IgnoreLayerCollision(6, 3, true);
        }
    }

    //void ApplyRebound(Transform t)
    //{
    //    Vector2 reboundDir = transform.position - t.position;
    //    rB.AddForce(reboundDir * rebound);
    //}
}
