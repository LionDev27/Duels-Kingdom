using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackApplier : MonoBehaviour
{
    //Knockback que se multiplicará al vector director.
    public Vector2 knockback;
    //La velocidad mínima y la velocidad máxima del objeto lanzado, siendo esta última la velocidad de lanzamiento.
    //Usaremos estas variables para hacer comprobaciones si el objeto es un item.
    public Vector2 maxVelocity;
    //Booleana que indicará si es un item para hacer unas comprobaciones extra.
    public bool isItem;

    Rigidbody2D rB;

    private void Start()
    {
        rB = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Método que aplicará knockback a un objeto.
    /// </summary>
    /// <param name="initKnockback">Vector del knockback inicial</param>
    /// <param name="target">Target al que se le aplicará el knockback</param>
    /// <param name="kBMultiplier">Multiplicador de knockback</param>
    /// <param name="kbApplier">Objeto que aplica el knockback</param>
    public void ApplyKnockback(Vector2 initKnockback, Transform target, float kBMultiplier, GameObject kbApplier = null)
    {
        //Accedemos al rigidbody del target.
        Rigidbody2D tRB = target.GetComponent<Rigidbody2D>();
        //Reseteamos su velocidad.
        tRB.velocity = Vector2.zero;
        //Si este objeto es un ítem, hacemos unas comprobaciones extra.
        if (isItem)
        {
            //Si la velocidad del objeto es menor que la mitad de la velocidad máxima, es decir, si el objeto va a la mitad de velocidad de la máxima.
            if (rB.velocity.magnitude < (maxVelocity/2).magnitude)
            {
                //Dividimos el knockback entre 2.
                knockback /= 2;
            }
        }
        //Si el knockback se aplica hacia abajo.
        if (initKnockback.y <= 0)
        {
            //Si el target es un jugador y no está en el suelo, salimos del método.
            if (target.CompareTag("Player") && !target.GetComponent<PlayerMovement>().isGrounded)
            {
                return;
            }
            //Hacemos que se aplique knockback hacia arriba en la dirección a la que se esté mirando.
            initKnockback.y = 0.5f;
        }
        //Normalizamos el vector director.
        initKnockback.Normalize();
        //Creamos un vector que será el Knockback inicial multiplicado por el Knockback adicional.
        Vector2 knockbackDir = initKnockback * knockback * kBMultiplier;
        //Añadimos la fuerza.
        tRB.AddForce(knockbackDir);
        //Accedemos a su Damageable
        Damageable dmg = target.GetComponent<Damageable>();
        //Paramos su corrutina de KnockbackApplied, por si se aplica otro knockback en el aire.
        dmg.StopAllCoroutines();
        //e iniciamos su corrutina KnockbackApplied.
        dmg.StartCoroutine(dmg.KnockbackApplied(kbApplier));
    }
}
