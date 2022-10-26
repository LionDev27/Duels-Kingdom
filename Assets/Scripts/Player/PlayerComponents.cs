using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    [HideInInspector] public Controls c;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rB;
    [HideInInspector] public PlayerMovement pM;
    [HideInInspector] public PlayerAttack pA;
    [HideInInspector] public PlayerInventory pI;
    [HideInInspector] public DamageDealer dD;
    [HideInInspector] public SpriteRenderer sR;

    private void Awake()
    {
        rB = GetComponent<Rigidbody2D>();
        pM = GetComponent<PlayerMovement>();
        pA = GetComponent<PlayerAttack>();
        pI = GetComponent<PlayerInventory>();
        anim = GetComponent<Animator>();
        sR = GetComponentInChildren<SpriteRenderer>();
        dD = transform.GetChild(0).GetComponent<DamageDealer>();
    }
}
