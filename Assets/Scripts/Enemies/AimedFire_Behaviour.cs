using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimedFire_Behaviour : Enemy_base
{
    protected GameObject Player;

    // Start is called before the first frame update
    protected override void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if(players.Length > 1)
        {
            int playerID = Random.Range(0, players.Length);

            //Check if dead
            //if playerID is alive, Player = playerID.
            //else other player;

            Player = players[playerID];

            return;
        }

            Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Shoot()
    {
        BulletFactory.instance.ShootAt(transform, Player.transform, pattern, false);
    }
}
