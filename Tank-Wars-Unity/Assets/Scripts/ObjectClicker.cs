﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectClicker : MonoBehaviour
{

    public GameObject tankClicked = null;
    public GameObject tankClicked2 = null;
    public GameObject tileClicked = null;
    public GameObject redTank;
    public GameObject blueTank;
    public float yVal = 1f;
    public int[,] terrainMapOne;
    public Grid grid;
    public int playerTurn;
    public int round;
    public bool gamblePressed = false;
    public Transform MapTarget;
    public Transform RedTarget;
    public Transform BlueTarget;
    public bool tankCamToggle = false;
    public bool worldCamToggle = false;
    public bool freeCamToggle = false;
    public string endGameText = "";
    public bool inWater1;
    public bool inWater2;
    AudioSource cannonFire;
    BlueShooter blueFire;
    RedShooter redFire;

    //the object that will become playerGUI
    PlayerGUI theGUI;

    private void Start()
    {
        terrainMapOne = new int[,]
          {
            {0,2,0,2,1,1,0,2,0,2},
            {2,4,2,4,1,1,2,4,2,4},
            {3,2,3,2,1,1,3,2,3,2},
            {2,4,2,0,1,1,2,4,2,3},
            {3,2,4,2,1,1,0,2,0,2},
            {2,0,2,0,1,1,2,0,2,4},
            {4,2,3,2,1,1,3,2,3,2},
            {2,0,2,4,1,1,2,4,2,4},
            {3,2,3,2,1,1,0,2,0,2},
            {2,1,2,4,1,1,2,3,2,4}
          };

        MapTarget = GameObject.Find("Cube(0,0) (44)").transform;
        RedTarget = GameObject.Find("Tank Variant").transform;
        BlueTarget = GameObject.Find("Tank (1) Variant").transform;

        redTank = GameObject.Find("redTank");
        blueTank = GameObject.Find("blueTank");

        grid = new Grid(terrainMapOne, 4, 0, 5, 9);

        playerTurn = 1;
        round = 1;

        theGUI = FindObjectOfType<PlayerGUI>();
        redFire = FindObjectOfType<RedShooter>();
        blueFire = FindObjectOfType<BlueShooter>();
        cannonFire = GameObject.Find("Tank Variant").GetComponent<AudioSource>();
    }

    public void GambleButton()
    {
        if (round == 3)
        {
            gamblePressed = true;
            string powerUp = grid.gamble(playerTurn);
            Debug.Log(powerUp);
        }
        
    }

    public void NoGambleButton()
    {
        if (round == 3)
        {
            gamblePressed = true;
            Debug.Log("Player " + playerTurn + " chose not to gamble");
        }
    }

    private void Update()
    {
        // Check if it's game over
        int victory = grid.isGameOver();
        if (victory != 0)
        {
            endGameText = "Player " + (victory == 1 ? "one" : "two") + " has won the game!";
            Debug.Log(endGameText);

            // Goto Victory Screen
            SceneManager.LoadScene("GameOver");
        }

        if(grid.getPlayerAttack((int)Players.Player1) > 2 && theGUI.p1Powerup) {
            theGUI.Player1Powerup();
        }
        else if (grid.getPlayerAttack((int)Players.Player1) <= 2 && !theGUI.p1Powerup) {
            theGUI.Player1Powerup();
        }
        else if(grid.getPlayerAttack((int)Players.Player2) > 2 && theGUI.p2Powerup) {
            theGUI.Player2Powerup();
        }
        else if (grid.getPlayerAttack((int)Players.Player2) <= 2 && !theGUI.p2Powerup) {
            theGUI.Player2Powerup();
        }

        if(round == 1) {
            // Display GUI for Player Movement
            if (playerTurn == (int)Players.Player1 && theGUI.p1Move) {
                if (!theGUI.p2Gamble) {
                    theGUI.Player2Gamble();
                }
                theGUI.Player1Movement();
            }
            else if(playerTurn == (int)Players.Player2 && theGUI.p2Move){
                theGUI.Player1Gamble();
                theGUI.Player2Movement();
            }
        }
        else if(round == 2) {
            // Display GUI for player attack
            if (playerTurn == (int)Players.Player1 && theGUI.p1Attack) {
                theGUI.Player1Movement();
                theGUI.Player1Attack();
            }
            else if(playerTurn == (int)Players.Player2 && theGUI.p2Attack) {
                theGUI.Player2Movement();
                theGUI.Player2Attack();
            }
        }
        else {
            // Display gui for player gamble
            if (playerTurn == (int)Players.Player1 && theGUI.p1Gamble) {
                theGUI.Player1Attack();
                theGUI.Player1Gamble();
            }
            else if(playerTurn == (int)Players.Player2 && theGUI.p2Gamble){
                theGUI.Player2Attack();
                theGUI.Player2Gamble();
            }
        }

        // Press R to reset camera position
        if (Input.GetKeyDown(KeyCode.R))
        {
            resetCamera();
        }

        // Press 1 to turn on/off tanks camera
        if (Input.GetKeyDown(KeyCode.Alpha1) && tankCamToggle == false && worldCamToggle == false && freeCamToggle == false)
        {
            tankCamToggle = true;
            resetCamera();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && tankCamToggle == true)
        {
            tankCamToggle = false;
        }

        // Press 2 to turn on/off center map camera 
        if (Input.GetKeyDown(KeyCode.C) && worldCamToggle == false && tankCamToggle == false && freeCamToggle == false)
        {
            worldCamToggle = true;
            resetCamera();
            transform.LookAt(MapTarget.position, MapTarget.up);
        }
        else if (Input.GetKeyDown(KeyCode.C) && worldCamToggle == true)
        {
            worldCamToggle = false;
        }

        // Press 3 to turn on/off free cam 
        if (Input.GetKeyDown(KeyCode.Alpha3) && freeCamToggle == false && tankCamToggle == false && worldCamToggle == false)
        {
            freeCamToggle = true;
            resetCamera();
            transform.LookAt(MapTarget.position, MapTarget.up);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) && freeCamToggle == true)
        {
            freeCamToggle = false;
        }

        if (playerTurn == 1 && tankCamToggle)
        {
            transform.RotateAround(RedTarget.position, RedTarget.up, -Input.GetAxis("Mouse X") * 50);
        }
        else if(playerTurn == 2 && tankCamToggle)
        {
            transform.RotateAround(BlueTarget.position, BlueTarget.up, -Input.GetAxis("Mouse X") * 50);
        }

        if (worldCamToggle)
        {
            transform.RotateAround(MapTarget.position, MapTarget.up, -Input.GetAxis("Mouse X") * 50);
        }

        if (freeCamToggle)
        {
            transform.RotateAround(MapTarget.position, MapTarget.up, -Input.GetAxis("Mouse X") * 50);
            transform.RotateAround(MapTarget.position, MapTarget.right, -Input.GetAxis("Mouse Y") * 50);
        }

        if (Input.GetMouseButtonDown(0) || round == 3)
        {
            if (round == 1)
            {

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (hit.transform != null)
                    {
                        Rigidbody rb;
                        if (rb = hit.transform.GetComponent<Rigidbody>())
                        {
                            print(hit.transform.gameObject);
                            if (hit.transform.gameObject.tag == "Red Tank" || hit.transform.gameObject.tag == "Blue Tank")
                            {
                                resetColors();

                                tankClicked = hit.transform.gameObject;

                                if (hit.transform.gameObject.tag == "Red Tank")
                                {
                                    highlightValidMovementTiles((int)Players.Player1);
                                }
                                else
                                {
                                    highlightValidMovementTiles((int)Players.Player2);
                                }
                            }
                            else if (hit.transform.gameObject.tag == "Tile" && tankClicked != null)
                            {
                                resetColors();

                                tileClicked = hit.transform.gameObject;
                                if (grid.canMove(playerTurn, (int)tileClicked.transform.position.x, (int)tileClicked.transform.position.z))
                                {
                                    tankClicked.transform.position = new Vector3(tileClicked.transform.position.x, yVal, tileClicked.transform.position.z);
                                    //if(targetNode.terrain != (int)Terrains.Mountains) {
                                    //public GridNode getNode(int x, int y) {
                                    if (grid.getNode((int)tileClicked.transform.position.x, (int)tileClicked.transform.position.z).terrain == (int)Terrains.Water)
                                    {
                                        if (playerTurn == 1)
                                        {
                                            inWater1 = true;
                                        }
                                        if (playerTurn == 2)
                                        {
                                            inWater2 = true;
                                        }
                                    }
                                    else
                                    {
                                        if (playerTurn == 1)
                                        {
                                            inWater1 = false;
                                        }
                                        if (playerTurn == 2)
                                        {
                                            inWater2 = false;
                                        }
                                    }
                                    if (inWater1 || inWater2)
                                    {
                                        if (playerTurn == 1)
                                        {
                                            grid.setPlayerHealth(playerTurn, grid.getPlayerHealth(playerTurn) - 1);
                                        }
                                        if (playerTurn == 2)
                                        {
                                            grid.setPlayerHealth(playerTurn, grid.getPlayerHealth(playerTurn) - 1);
                                        }
                                    }
                                    tileClicked = null;
                                    tankClicked = null;
                                    inWater1 = false;
                                    inWater2 = false;
                                    round++;
                                    //print(grid.getPlayerHealth(1) + " " + grid.getPlayerHealth(2));
                                }
                            }
                        }
                    }
                }
            }
            else if (round == 2)
            {
                resetColors();

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (hit.transform != null)
                    {
                        Rigidbody rb;
                        if (rb = hit.transform.GetComponent<Rigidbody>())
                        {
                            if (hit.transform.gameObject.tag == "Red Tank")
                            {
                                resetColors();

                                if (playerTurn == (int)Players.Player1) {
                                    tankClicked = hit.transform.gameObject;
                                    highlightValidAttackTiles((int)Players.Player1);
                                }
                                else {
                                    tankClicked2 = hit.transform.gameObject;
                                }
                                
                            }
                            else if (hit.transform.gameObject.tag == "Blue Tank")
                            {
                                resetColors();

                                if (playerTurn == (int)Players.Player2) {
                                    tankClicked = hit.transform.gameObject;
                                    highlightValidAttackTiles((int)Players.Player2);
                                }
                                else {
                                    tankClicked2 = hit.transform.gameObject;
                                }
                            }
                            if (tankClicked != null && tankClicked2 != null)
                            {
                                resetColors();
                                cannonFire.Play();

                                //TEST DELETE AFTER******************
                                if(playerTurn == 1)
                                {
                                    redFire.Fire();
                                    //shoot red bullet
                                }
                                if(playerTurn == 2)
                                {
                                    blueFire.Fire();
                                    //shoot blue bullet
                                }
                                //TEST DELETE AFTER******************

                                if (grid.canAttack(playerTurn, (int)tankClicked2.transform.position.x, (int) tankClicked2.transform.position.z))
                                {
                                    print("Good attack");
                                }
                                else
                                {
                                    print("Bad attack");
                                }
                                tankClicked = null;
                                tankClicked2 = null;
                                round++;
                            }
                        }
                    }
                }
            }
            else if (round == 3)
            {
                if (gamblePressed)
                {
                    gamblePressed = false;
                    round = 1;
                    if (++playerTurn > 2)
                    {
                        playerTurn = 1;
                    }
                    resetCamera();
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (round == 1)
            {
                if (playerTurn == 1)
                {
                    print("Red tank move phase");
                }
                else if (playerTurn == 2)
                {
                    print("Blue tank move phase");
                }
            }
            if (round == 2)
            {
                if (playerTurn == 1)
                {
                    print("Red tank attack phase");
                }
                else if (playerTurn == 2)
                {
                    print("Blue tank attack phase");
                }
            }
            if (round == 3)
            {
                if (playerTurn == 1)
                {
                    print("Red tank gamble phase");
                }
                else if (playerTurn == 2)
                {
                    print("Blue tank gamble phase");
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (++round > 3)
            {
                round = 1;
                if (++playerTurn > 2)
                {
                    playerTurn = 1;
                }
                resetCamera();
            }
        }
    }

    public void highlightValidMovementTiles(int player)
    {
        ArrayList movements = grid.getAllValidMovementTiles(player);
        object[] arrayOfTiles = GameObject.FindGameObjectsWithTag("Tile");
        Material movementMat = Resources.Load("Materials/MovementHighlight", typeof(Material)) as Material;
        Material playerMat;
        int currentPlayerX, currentPlayerY;
        if (player == (int)Players.Player1)
        {
            playerMat = Resources.Load("Materials/RedTankColor", typeof(Material)) as Material;
            currentPlayerX = grid.player1X;
            currentPlayerY = grid.player1Y;
        }
        else
        {
            playerMat = Resources.Load("Materials/BlueTankColor", typeof(Material)) as Material;
            currentPlayerX = grid.player2X;
            currentPlayerY = grid.player2Y;
        }
        Coord currentCoordinates;

        foreach (object t in arrayOfTiles)
        {
            GameObject tile = (GameObject)t;
            MeshRenderer meshRenderer = tile.GetComponent<MeshRenderer>();

            for (int i = 0; i < movements.Count; i++)
            {
                currentCoordinates = (Coord)movements[i];
                if (((int)tile.transform.position.x == currentCoordinates.x) && ((int)tile.transform.position.z == currentCoordinates.y))
                {
                    meshRenderer.material = movementMat;
                }
                else if ((int)tile.transform.position.x == currentPlayerX && (int)tile.transform.position.z == currentPlayerY)
                {
                    meshRenderer.material = playerMat;
                }
            }
        }
    }

    public void highlightValidAttackTiles(int player)
    {
        ArrayList attacks = grid.getAllValidAttackTiles(player);
        object[] arrayOfTiles = GameObject.FindGameObjectsWithTag("Tile");
        Material attackMat = Resources.Load("Materials/AttackHighlight", typeof(Material)) as Material;
        Material playerMat;
        int currentPlayerX, currentPlayerY;
        if (player == (int)Players.Player1)
        {
            playerMat = Resources.Load("Materials/RedTankColor", typeof(Material)) as Material;
            currentPlayerX = grid.player1X;
            currentPlayerY = grid.player1Y;
        }
        else
        {
            playerMat = Resources.Load("Materials/BlueTankColor", typeof(Material)) as Material;
            currentPlayerX = grid.player2X;
            currentPlayerY = grid.player2Y;
        }
        Coord currentCoordinates;

        foreach (object t in arrayOfTiles)
        {
            GameObject tile = (GameObject)t;
            MeshRenderer meshRenderer = tile.GetComponent<MeshRenderer>();

            for (int i = 0; i < attacks.Count; i++)
            {
                currentCoordinates = (Coord)attacks[i];
                if (((int)tile.transform.position.x == currentCoordinates.x) && ((int)tile.transform.position.z == currentCoordinates.y))
                {
                    meshRenderer.material = attackMat;
                }
                else if ((int)tile.transform.position.x == currentPlayerX && (int)tile.transform.position.z == currentPlayerY)
                {
                    meshRenderer.material = playerMat;
                }
            }
        }
    }

    public void resetColors()
    {
        object[] arrayOfTiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (object t in arrayOfTiles)
        {
            GameObject tile = (GameObject)t;
            int terrain = grid.grid[(int)tile.transform.position.x, (int)tile.transform.position.z].terrain;
            MeshRenderer meshRenderer = tile.GetComponent<MeshRenderer>();
            Material mat;

            switch (terrain)
            {
                case 0:
                    mat = Resources.Load<Material>("Materials/Desert");
                    meshRenderer.material = mat;
                    break;
                case 1:
                    mat = Resources.Load<Material>("Materials/GrassColor");
                    meshRenderer.material = mat;
                    break;
                case 2:
                    mat = Resources.Load<Material>("Materials/WaterColor");
                    meshRenderer.material = mat;
                    break;
                case 3:
                    mat = Resources.Load<Material>("Materials/LavaColor");
                    meshRenderer.material = mat;
                    break;
                case 4:
                    mat = Resources.Load<Material>("Materials/MountainColor");
                    meshRenderer.material = mat;
                    break;
            }
        }

        print("Tile Colors Reset");

    }

    public Grid getObjectClickerGrid()
    {
        return grid;
    }

    public void resetCamera()
    {
        if(playerTurn == 1)
        {
            GameObject.Find("Main Camera").transform.position = new Vector3(4.29f, 4.63f, -5.88f);
            transform.LookAt(RedTarget.position, RedTarget.up);
        }
        else
        {
            GameObject.Find("Main Camera").transform.position = new Vector3(4.29f, 4.63f, 14.88f);
            transform.LookAt(BlueTarget.position, BlueTarget.up);
        }
    }

}