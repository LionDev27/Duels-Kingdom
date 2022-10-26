using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnController : PlayerComponents
{
    [SerializeField] float defaultGravity;
    public Transform spawnPosition;

    private void Update()
    {
        CheckInputsAndInvinvibility();
        StayInSpawnPosition(spawnPosition.position);
    }

    void CheckInputsAndInvinvibility()
    {
        if (pM.movement.x != 0 || GetComponent<Damageable>().canTakeDamage)
        {
            rB.gravityScale = defaultGravity;
            enabled = false;
        }
    }

    void StayInSpawnPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    public void DeactivateGravity()
    {
        rB.gravityScale = 0f;
    }
}
