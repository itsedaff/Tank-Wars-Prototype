﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] bool escButtonIsClicked = false;
    [SerializeField] GameObject escMenu;
    [SerializeField] GameObject optionMenu;

    // Start is called before the first frame update
    void Start()
    {
        escMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //write a function for this later so its not so potato

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space key was pressed");
            escMenu.SetActive(false);
        }
        */
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escButtonIsClicked == false)
            {
                //Debug.Log("esc key was pressed");
                escMenu.SetActive(true);
                escButtonIsClicked = true;
            }
            else
            {
                //Debug.Log("esc key was pressed");
                escMenu.SetActive(false);
                escButtonIsClicked = false;
                optionMenu.SetActive(false);
            }
        }
    }

}