using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [Header("Variables globales")]
    //Prefab que usaremos para instanciar los ítems de forma aleatoria, o cuando los lancemos.
    public GameObject itemPrefab;
    //ID del objeto. Cada objeto tendrá una ID distinta.
    public int id;
    //Nombre del ítem para diferenciarlo.
    public string itemName;
    //Descripción de la función del ítem.
    [TextArea(3, 5)]
    public string description;
    //Tipo de ataque que dará al jugador este item.
    public AttackTypes attackType;
    //Booleana que indicará si el ataque del arma es letal o no.
    public bool isLethal;
    //Usos que tendrá este objeto.
    public float usages;
    //Cooldown que tendrá entre uso y uso del objeto.
    public float cooldown;

    [Header("Variables melee")]
    public float knockbackMultiplier;

    [Header("Variables a distancia")]
    public GameObject bulletPrefab;
    public float bulletSpeed;
}

public class ItemDB : MonoBehaviour
{
    //Array que contendrá todos los items que existen en el juego.
    public Item[] items;

    //SINGLETON.
    public static ItemDB instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
}
