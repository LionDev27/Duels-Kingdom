using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundChecker : MonoBehaviour
{
    [SerializeField] EnemyAI eAI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si el collider del enemigo entra en contacto con el suelo.
        if (collision.CompareTag("Ground"))
        {
            eAI.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            eAI.isGrounded = false;
        }
    }
}
