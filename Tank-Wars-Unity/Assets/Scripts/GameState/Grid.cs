﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    // Player1: Red
    // Player2: Blue
    // 1 = Player1, 2 = Player2

    public GridNode[,] grid;
    public int gridSize;

    // Player 1 Attributes
    public int player1X = 4;
    public int player1Y = 0;
    int player1Movement = 2;
    int player1Attack = 2;
    int player1Health = 5;

    public int player2X = 5;
    public int player2Y = 9;
    int player2Movement = 2;
    int player2Attack = 2;
    int player2Health = 5;

    // Constructors
    public Grid() {
        grid = new GridNode[10, 10];
        gridSize = 10;
    }

    public Grid(int[,] terrainMap, int player1X, int player1Y, int player2X, int player2Y) {
        grid = new GridNode[10, 10];
        gridSize = 10;

        this.player1X = player1X;
        this.player1Y = player1Y;
        this.player2X = player2X;
        this.player2Y = player2Y;

        for(int i = 0; i < gridSize; i++) {
            for(int j = 0; j < gridSize; j++) {

                grid[i, j] = new GridNode(i, j, terrainMap[i,j]);

                if(player1X == i && player1Y == j) {
                    grid[i, j].player1OnNode = true;
                }

                if(player2X == i && player2Y == j) {
                    grid[i, j].player2OnNode = true;
                }
            }
        }
    }

    // Member functions
    public GridNode getNode(int x, int y) {
        return grid[x, y];
    }

    // TODO: Eric Implement
    public bool canMove(int player, int x, int y) {
        return true;
    }

    // TODO: Joshua Implement
    // Check if valid attack
    // If valid, decrement other player's health by player's attack value
    // Return true
    // Otherwise just return false
    public bool canAttack(int player, int x, int y) {
        return true;
    }
}

enum Players {
    Player1 = 1,
    Player2 = 2
}