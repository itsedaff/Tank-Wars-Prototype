﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    [SerializeField] GameObject projectile;
    [SerializeField] GameObject gun;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            Fire();
        }
    }


    public void Fire()
    {
        Instantiate(projectile, gun.transform.position, Quaternion.identity);
    }

    
}
