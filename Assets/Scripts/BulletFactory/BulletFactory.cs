
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class BulletFactory : MonoBehaviour
{
    // static to current bullet factory instance
    public static BulletFactory instance;

    public BoolVar bvInLoading;
    public BoolVar bvGamePaused;
    public BoolVar bvGameInCutscene;
    public BoolVar bvGameOver;

    // Set instance on awake
    void Awake () {
        // Destroy any duplicate instances created
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    /// <summary>
    ///     Spawns bullets in a set with the given property pre-sets
    ///     Coroutine for the Shoot function to utilize spawning pattern sets
    ///     Bullet direction is relative to the value of when it was called 
    ///     and not the when the bullet actually fires
    /// </summary>
    /// <param name="spawnTrans">Starting point of the shot</param>
    /// <param name="spawnDirection">Direction of the shot in degrees
    ///     with the interval offset factored in</param>
    /// <param name="property">Bullet property pre-set to use</param>
    /// <param name="delay">Delay before doing this set of bullets</param>
    /// <param name="spawnCount">Number of bullets for this set</param>
    /// <param name="addAngle">Angle added for each bullet spawn in set</param>
    IEnumerator SpawnBullets(GameObject pfBullet,
        Transform spawnTrans,float spawnOffset, float spawnDirection, BulletProperty property,
        float delay, int spawnCount, float addAngle, bool isPlayers = true)
    {
        // Delay before spawning 1 set of bullets
        yield return new WaitForSeconds(delay);

        if (spawnTrans && spawnTrans.gameObject.activeInHierarchy == true)
        {
            // Spawns all the bullets for 1 set
            for (int i = 0; i < spawnCount; ++i)
            // Instantiate an enemy bullet
            {
                GameObject bullet;
                BulletBehaviour bulletBehaviour;

                // Get the bullet's behaviour script
                bullet = (Instantiate(pfBullet.transform, spawnTrans.position, Quaternion.identity) as Transform).gameObject;
                bullet.tag = SetBulletTag(isPlayers);

                // Sets the bullet property according to pre-sets
                bulletBehaviour = bullet.GetComponent<BulletBehaviour>();
                bulletBehaviour.SetBulletProperty(property, isPlayers);

                // Adds offset depending on the bullet count of the set
                bulletBehaviour.SetBulletAngle(spawnDirection + spawnOffset + addAngle * i);
            }
        }
    }

    /// <summary>
    ///     Spawns bullets in a set with the given property pre-sets
    ///     Coroutine for the Shoot function to utilize spawning pattern sets
    ///     Bullet direction is relative to the value of when it was called 
    ///     and not the when the bullet actually fires
    /// </summary>
    /// <param name="spawnTrans">Starting point of the shot</param>
    /// <param name="targetTrans">Transform the bullet will travel towards</param>
    /// <param name="intervalAddAngle">Angle directio added to any subsequent bullet within an interval</param>
    /// <param name="property">Bullet property pre-set to use</param>
    /// <param name="delay">Delay before doing this set of bullets</param>
    /// <param name="spawnCount">Number of bullets for this set</param>
    /// <param name="addAngle">Angle added for each bullet spawn in set</param>
    /// <returns></returns>
    IEnumerator SpawnBulletsAt(GameObject pfBullet,
            Transform spawnTrans, Transform targetTrans, 
            float startOffsetAngle, float intervalAddAngle,
            BulletProperty property, float delay, 
            int spawnCount, float addAngle,
            bool isPlayers)
    {
        // Delay before spawning 1 set of bullets
        yield return new WaitForSeconds(delay);

        if (spawnTrans.gameObject.activeInHierarchy == true)
        {
            // Get the angle to the target relative from the spawn
            Vector3 targetVector = targetTrans.position - spawnTrans.position;
            float targetAngle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;
            targetAngle += startOffsetAngle;

            // Spawns all the bullets for 1 set
            for (int i = 0; i < spawnCount; ++i)
            {
                // Instantiate an enemy bullet
                GameObject bullet;
                BulletBehaviour bulletBehaviour;

                // Get the bullet's behaviour script
                bullet = (Instantiate(pfBullet.transform, spawnTrans.position, Quaternion.identity) as Transform).gameObject;
                bullet.tag = SetBulletTag(isPlayers);

                // Sets the bullet property according to pre-sets
                bulletBehaviour = bullet.GetComponent<BulletBehaviour>();
                 bulletBehaviour.SetBulletProperty(property, isPlayers);

                // Adds offset depending on the bullet count of the set
                bulletBehaviour.SetBulletAngle(targetAngle + intervalAddAngle + addAngle * i);
            }
        }
    }

    /// <summary>
    ///     Spawns complex bullets in a set with the given property pre-sets
    ///     Coroutine for the Shoot function to utilize spawning pattern sets
    ///     Bullet direction is relative to the value of when it was called 
    ///     and not the when the bullet actually fires
    /// </summary>
    /// <param name="spawnTrans">Starting point of the shot</param>
    /// <param name="spawnDirection">Direction of the shot in degrees
    /// <param name="propertyList">List of bullet property pre-sets to use</param>
    /// <param name="delay">Delay before doing this set of bullets</param>
    /// <param name="spawnCount">Number of bullets for this set</param>
    /// <param name="addAngle">Angle added for each bullet spawn in set</param>
    /// <returns></returns>
    IEnumerator SpawnBulletsComplex(GameObject pfBullet, Transform spawnTrans, 
                                    float spawnDirection, List<BulletComplexPattern.ComplexProperty> propertyList,
                                    float delay, int spawnCount, float addAngle,
                                    bool isPlayers)
    {
        // Delay before spawning 1 set of bullets
        yield return new WaitForSeconds(delay);

        if (spawnTrans.gameObject.activeInHierarchy == true)
        {

            // Spawns all the bullets for 1 set
            for (int i = 0; i < spawnCount; ++i)
            {
                // Instantiate an enemy bullet
                GameObject bullet;
                BulletBehaviour bulletBehaviour;

                // Get the bullet's behaviour script
                bullet = (Instantiate(pfBullet.transform, spawnTrans.position, Quaternion.identity) as Transform).gameObject;
                bullet.tag = SetBulletTag(isPlayers);

                // Sets the bullet property according to pre-sets
                bulletBehaviour = bullet.GetComponent<BulletBehaviour>();

                // Sets the initial bullet property
                bulletBehaviour.SetBulletProperty(propertyList[0].bulletProperty, isPlayers);
                bulletBehaviour.SetBulletAngle(spawnDirection + addAngle * i);

                float propertyStart = 0.0f;

                // Adds offset depending on the bullet count of the set
                foreach (BulletComplexPattern.ComplexProperty pattern in propertyList)
                {
                    //bulletBehaviour.SetBulletPropertyDelayed(pattern.bulletProperty, pattern.propertyLifetime);
                    StartCoroutine(bulletBehaviour.SetBulletPropertyDelayed(pattern.bulletProperty, propertyStart, isPlayers));
                    StartCoroutine(bulletBehaviour.SetBulletAngleDelayed(spawnDirection + addAngle * i, propertyStart));
                    propertyStart += pattern.propertyLifetime;
                }
            }
        }
    }

    /// <summary>
    ///     Shoots an enemy bullet from a predetermined set of bullet patterns and types
    /// </summary>
    /// <param name="spawnTrans">Origin of the shot</param>
    /// <param name="spawnDirection">Direction of the shot in degrees</param>
    /// <param name="pattern">BulletPattern to use</param>
    public void Shoot(Transform spawnTrans, 
                        float spawnDirection, BulletPattern pattern,
                        bool isPlayers)
    {
        // Repeat for total amount of intervals
        for (int i = 0; i < pattern.intervalCount; ++i)
        {
            // Delayed function call that spawns bullets
            StartCoroutine(SpawnBullets(pattern.bulletPrefab, spawnTrans, 
                pattern.spawnDirectionOffset, spawnDirection + pattern.intervalAngleAddition * i, 
                pattern.bulletType, pattern.initialDelay + pattern.intervalDelay * i,
                pattern.spawnCount, pattern.spawnAngleAddition, isPlayers));
        }
    }

    /// <summary>
    ///     Shoots an enemy bullet at a target from a predetermined set of bullet patterns and types
    /// </summary>
    /// <param name="spawnTrans">Origin of the shot</param>
    /// <param name="targetTrans">Target to shoot at</param>
    /// <param name="pattern">BulletPattern to use</param>
    public void ShootAt(Transform spawnTrans, 
                            Transform targetTrans, BulletPattern pattern,
                            bool isPlayers)
    {
        // Repeat for total amount of intervals
        for (int i = 0; i < pattern.intervalCount; ++i)
        {
            // Delayed function call that spawns bullets
            StartCoroutine(SpawnBulletsAt(pattern.bulletPrefab, spawnTrans, targetTrans, 
                pattern.spawnDirectionOffset, pattern.intervalAngleAddition * i,
                pattern.bulletType, pattern.initialDelay + pattern.intervalDelay * i,
                pattern.spawnCount, pattern.spawnAngleAddition, isPlayers));
        }
    }

    /// <summary>
    ///     Shoots a complex enemy bullet from a predetermined complex pattern set
    /// </summary>
    /// <param name="spawnTrans">Origin of the shot</param>
    /// <param name="spawnDirection">Direction of the shot in degrees</param>
    /// <param name="pattern">BulletPattern to use</param>
    public void ComplexShoot(Transform spawnTrans, 
                                float spawnDirection, BulletComplexPattern complexPattern,
                                bool isPlayers)
    {
        // Repeat for total amount of intervals
        for (int i = 0; i < complexPattern.intervalCount; ++i)
        {
            // Delayed function call that spawns bullets
            StartCoroutine(SpawnBulletsComplex(complexPattern.bulletPrefab, spawnTrans, spawnDirection + complexPattern.intervalAngleAddition * i,
                complexPattern.bulletPropertyList, complexPattern.initialDelay + complexPattern.intervalDelay * i,
                complexPattern.spawnCount, complexPattern.spawnAngleAddition, isPlayers));
        }
    }

    /// <summary>
    ///     Sets the tag depending if the bullet is player's or enemy's
    /// </summary>
    /// <param name="isPlayers">If bullet belongs to the player</param>
    /// <returns>The bullet tag</returns>
    string SetBulletTag(bool isPlayers)
    {
        if (isPlayers)
            return "PlayerBullet";
        else
            return "EnemyBullet";
    }
}
