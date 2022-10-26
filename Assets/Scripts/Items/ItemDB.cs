using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [Header("Variables globales")]
    //Prefab que usaremos para instanciar los �tems de forma aleatoria, o cuando los lancemos.
    public GameObject itemPrefab;
    //ID del objeto. Cada objeto tendr� una ID distinta.
    public int id;
    //Nombre del �tem para diferenciarlo.
    public string itemName;
    //Descripci�n de la funci�n del �tem.
    [TextArea(3, 5)]
    public string description;
    //Tipo de ataque que dar� al jugador este item.
    public AttackTypes attackType;
    //Booleana que indicar� si el ataque del arma es letal o no.
    public bool isLethal;
    //Usos que tendr� este objeto.
    public float usages;
    //Cooldown que tendr� entre uso y uso del objeto.
    public float cooldown;

    [Header("Variables melee")]
    public float knockbackMultiplier;

    [Header("Variables a distancia")]
    public GameObject bulletPrefab;
    public float bulletSpeed;
}

public class ItemDB : MonoBehaviour
{
    //Array que contendr� todos los items que existen en el juego.
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
