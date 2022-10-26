using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableBox : MonoBehaviour
{
    public void DestroyAndInstantiateObject()
    {
        //Accedemos al array de Items que hay en el Item DataBase
        Item[] i = ItemDB.instance.items;
        //Instanciamos un ítem aleatorio de los que hay en la misma posición que la caja.
        Instantiate(i[Random.Range(0, i.Length)].itemPrefab, transform.position, transform.rotation);
        //Destruimos la caja.
        Destroy(gameObject);
    }
}
