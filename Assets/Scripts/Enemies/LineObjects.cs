using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObjects : MonoBehaviour
{
    GameObject Player;
    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 1)
        {
            int playerID = Random.Range(0, players.Length + 1);

            //Check if dead
            //if playerID is alive, Player = playerID.
            //else other player;

            Player = players[playerID];

            return;
        }

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Pew(BulletPattern pattern)
    {
        BulletFactory.instance.Shoot(transform, 180, pattern, false);
    }
}
