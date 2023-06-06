

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Bullet/EnemyComplexPattern")]
public class BulletComplexPattern : ScriptableObject
{
    [System.Serializable]
    public class ComplexProperty
    {
        public BulletProperty bulletProperty;
        public float propertyLifetime;
    }

    // the sprite when the pattern is in the player shot queue
    [Header("Player Shot Icon")]
    public Sprite shotIcon;

    // the sprite when the pattern is dropped as a shot pickup
    [Header("Drop Icon")]
    public Sprite dropIcon;

    // which kind of bullet to use
    [Header("Bullet Prefab & Type")]
    public GameObject bulletPrefab;

    //
    [Header("List of trajectory properties over the bullet's lifetime")]
    public List<ComplexProperty> bulletPropertyList;

    // Pattern Set
    public float spawnDirectionOffset = 0f;  // offsetted bullet direction
    public int spawnCount = 1;               // how many bullets to shoot
    public float spawnAngleAddition = 0f;    // angle added to each bullet

    // Multiple Pattern Sets Interval                 
    public int intervalCount = 1;            // number of pattern sets
    public float intervalAngleAddition = 0f; // angle added to each set
    public float initialDelay = 0f;          // starting delay at start
    public float intervalDelay = 0f;         // delay between sets
}

