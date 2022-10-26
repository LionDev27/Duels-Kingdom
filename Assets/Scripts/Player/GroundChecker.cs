using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public PlayerMovement pM;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si el collider del jugador entra en contacto con el suelo.
        if (collision.CompareTag("Ground") || collision.CompareTag("Destroyable"))
        {
            //Indicamos que se está tocando el suelo.
            pM.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Si el collider del jugador deja de colisionar con el suelo.
        if (collision.CompareTag("Ground") || collision.CompareTag("Destroyable"))
        {
            //Indicamos que no está tocando el suelo.
            pM.isGrounded = false;
        }
    }
}
