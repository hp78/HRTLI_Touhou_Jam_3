

using UnityEngine;
using System.Collections;

// Game logic for enemy bullets
public class BulletBehaviour : MonoBehaviour {

    // The bullet's current sprite
    Sprite bulletSprite;

    BoolVar bvInLoading;
    BoolVar bvGamePaused;
    BoolVar bvGameInCutscene;
    BoolVar bvGameOver;

    // Basic properties for the bullet
    bool  isAlive   = true;      // If the bullet is active
    bool  isPlayers = true;      // If the bullet is player's
    float bulletLife  = 5f;      // The lifetime of a bullet  
    float bulletSpeed = 1f;      // Velocity of bullet     -> The 'v' value for v' = v + at
    float bulletAccel = 0f;      // Acceleration of bullet -> The 'a' value for v' = v + at
    float bulletAngle = 0f;      // Bullet direction in degrees

    // Curve properties for the bullet
    float curveHeight = 0f;      // Amptitude of function -> The 'a' for f(t) = a sin(bt)
    float curveLength = 0f;      // Frequency of function -> The 'b' for f(t) = a sin(bt)
    float elapsedTime = 0f;      // Start time of funciton ->The starting 't' in f(t)

    // Homing curve limit
    const float homingUppLimit = 45f;
    const float homingLowLimit = 10f;
    const float homingAdjust = 0.5f;

    // Target to home onto
    Transform homeTarget = null;

    // The trajectory function which takes in 'elapsedTime' and plots the position of the bullet
    //ENEMY_BULLET_TRAJECTORY trajectoryType = ENEMY_BULLET_TRAJECTORY.STRAIGHT;
    TrajectoryFunction trajectoryFunction;
    Vector3 nextPos;

    // Called on the first frame
    void Start()
    {
        // Sets the bullet's current sprite
        bulletSprite = GetComponent<SpriteRenderer>().sprite;
        homeTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        nextPos = new Vector3(0,0);

        //
        bvInLoading = BulletFactory.instance.bvInLoading;
        bvGamePaused = BulletFactory.instance.bvGamePaused;
        bvGameInCutscene = BulletFactory.instance.bvGameInCutscene;
        bvGameOver = BulletFactory.instance.bvGameOver;
    }

    /// <summary>
    ///     Sets the property of a bullet by taking in a BulletProperty scriptable object class
    /// </summary>
    /// <param name="bulletProperty">BulletProperty contains pre-set data for a type of bullet</param>
    public void SetBulletProperty(BulletProperty bulletProperty, bool isplayer )
    {
        // If the bullet is set to a different sprite, change it
        if (bulletSprite != bulletProperty.bulletSprite)
            bulletSprite = bulletProperty.bulletSprite;

        // Sets all of the bullet's property
        isAlive = bulletProperty.isAlive;
        bulletLife  = bulletProperty.bulletLife;
        bulletSpeed = bulletProperty.bulletSpeed;
        bulletAccel = bulletProperty.bulletAccel;
        bulletAngle = bulletProperty.bulletAngle;
        curveHeight = bulletProperty.curveHeight;
        curveLength = bulletProperty.curveLength;
        elapsedTime = bulletProperty.elapsedTime;

        isPlayers = isplayer;

        // Sets the bullet's trajectory function
        switch (bulletProperty.trajectoryType)
        {
            case BULLET_TRAJECTORY.STRAIGHT:
                trajectoryFunction = TrajectoryStraight;
                break;

            case BULLET_TRAJECTORY.STRAIGHT_SINE:
                trajectoryFunction = TrajectoryStraightSine;
                break;

            case BULLET_TRAJECTORY.HOME_SNAP:
                trajectoryFunction = TrajectoryHomeSnap;
                break;

            case BULLET_TRAJECTORY.HOMING:
                trajectoryFunction = TrajectoryHoming;
                break;

            case BULLET_TRAJECTORY.STILL:
                trajectoryFunction = TrajectoryStill;
                break;

            case BULLET_TRAJECTORY.TIMECUBED:
                trajectoryFunction = TimeCubed;
                break;

            default:
                isAlive = false;
                break;
        }
    }

    /// <summary>
    ///     Sets the property of a bullet by taking in a BulletProperty scriptable object class 
    ///     after the given time
    /// </summary>
    /// <param name="bulletProperty">BulletProperty contains pre-set data for a type of bullet</param>
    /// <param name="delay">The amount of time before the property is set</param>
    /// <returns></returns>
    public IEnumerator SetBulletPropertyDelayed(BulletProperty bulletProperty, float delay, bool isplayers)
    {
        // Delay before setting bullet properties
        yield return new WaitForSeconds(delay);

        // Sets the bullet property
        SetBulletProperty(bulletProperty, isplayers);
    }

    /// <summary>
    ///     Set the bullet angle directioin in degrees
    /// </summary>
    /// <param name="nAngle"></param>
    public void SetBulletAngle ( float nAngle )
    {
        bulletAngle = nAngle;
    }

    public IEnumerator SetBulletAngleDelayed(float nAngle, float delay)
    { 
        // Delay before setting bullet properties
        yield return new WaitForSeconds(delay);

        //
        bulletAngle = nAngle;
    }

    /// <summary>
    ///     Updates the bullet trajectory every frame according to its type
    /// </summary>
    void FixedUpdate () {

        //
        if (bvGameInCutscene.data || bvGamePaused.data)
            return;

        // destroy itself when set to dead
        if (!isAlive)
            Destroy(gameObject);

        // update trajectory when game is moving
        if (Time.deltaTime > 0.0f)
        {
            // update elapsed time
            elapsedTime += Time.deltaTime;

            // set the next position the trajectory should move to
            //Vector3 nextPos = new Vector3(0f,0f);
            trajectoryFunction(ref nextPos);
            transform.position += nextPos;
            //transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(nextPos.y, nextPos.x));

            if (elapsedTime > bulletLife)
                SetDead();

            if(isPlayers)
            {
                if (transform.position.x > 8.5f || transform.position.x < -8.5f)
                    SetDead();

                //if (transform.position.y > 6.5f || transform.position.y < -6.5f)
                 //   SetDead();
            }

                if (transform.position.y > 6.5f || transform.position.y < -6.5f)
                    SetDead();


        }

        if (bvGameOver.data || bvGameInCutscene.data)
        {
            SetDead();
        }
	}

    /// <summary>
    ///     A straight line trajectory
    /// </summary>
    void TrajectoryStraight(ref Vector3 nNextPos)
    {
        // straightline velocity = vel0 + a(t)
        nNextPos = new Vector3(bulletSpeed + bulletAccel * elapsedTime , 0f) * 0.01f;

        // rotate velocity vector to the correct direction
        nNextPos = RotateVectorByAngle(nNextPos, bulletAngle);
        transform.rotation = Quaternion.Euler(0, 0, bulletAngle + 90.0f);
    }

    /// <summary>
    ///     A sine-curve trajectory
    /// </summary>
    void TrajectoryStraightSine(ref Vector3 nNextPos)
    {
        // do curvey stuff
        nNextPos = new Vector3(bulletSpeed + bulletAccel * elapsedTime, 
            curveHeight * curveLength * Mathf.Cos(curveLength * elapsedTime)) * 0.01f;

        // rotate velocity vector to the correct direction
        nNextPos = RotateVectorByAngle(nNextPos, bulletAngle);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(nNextPos.y, nNextPos.x) * 180 / Mathf.PI + 90.0f);
        //transform.rotation = Quaternion.Euler(0, 0, bulletAngle + 90.0f);
    }

    /// <summary>
    ///     Homing bullet trajectory
    /// </summary>
    void TrajectoryHomeSnap(ref Vector3 nNextPos)
    {
        // straightline velocity = vel0 + a(t)
        nNextPos = new Vector3(bulletSpeed + bulletAccel * elapsedTime, 0f) * 0.01f;

        // rotate velocity vector to the correct direction
        nNextPos = RotateVectorByAngle(nNextPos, bulletAngle);

        // Find the angle direction of the player relative to bullet
        float angleTowards = Vector3.Angle(transform.position, homeTarget.position);

        // Rotates the bullet direction towards player
        if (homingUppLimit > angleTowards && angleTowards > homingLowLimit)
        {
            bulletAngle += homingAdjust;
        }
        else if (-homingUppLimit < angleTowards && angleTowards < -homingLowLimit)
        {
            bulletAngle -= homingAdjust;
        }
        transform.rotation = Quaternion.Euler(0, 0, bulletAngle + 90.0f);
    }

    /// <summary>
    ///     Homing bullet trajectory
    /// </summary>
    void TrajectoryHoming(ref Vector3 nNextPos)
    {
        // straightline velocity = vel0 + a(t)
        nNextPos = new Vector3(bulletSpeed + bulletAccel * elapsedTime, 0f) * 0.01f;

        // rotate velocity vector to the correct direction
        nNextPos = RotateVectorByAngle(nNextPos, bulletAngle);

        // Find the angle direction of the player relative to bullet
        float angleTowards = Vector3.Angle(transform.position, homeTarget.position);

        // Rotates the bullet direction towards player
        if (homingUppLimit > angleTowards && angleTowards > homingLowLimit)
        {
            bulletAngle += homingAdjust;
        }
        else if (-homingUppLimit < angleTowards && angleTowards < -homingLowLimit)
        {
            bulletAngle -= homingAdjust;
        }
        transform.rotation = Quaternion.Euler(0, 0, bulletAngle + 90.0f);
    }

    void TrajectoryStill(ref Vector3 nNextPos)
    {
        nNextPos = Vector3.zero;
    }

    void TimeCubed(ref Vector3 nNextPos)
    {
        nNextPos = new Vector3(Mathf.Pow(elapsedTime,3) + bulletSpeed, 0f) * 0.01f;

        // rotate velocity vector to the correct direction
        nNextPos = RotateVectorByAngle(nNextPos, bulletAngle);
        transform.rotation = Quaternion.Euler(0, 0, bulletAngle + 90.0f);
    }

    /// <summary>
    ///     Dead bullets will destroy itself
    /// </summary>
    void DeadState()
    {
        Destroy(gameObject);
    }

    /// <summary>
    ///     Set the bullet to dead
    /// </summary>
    public void SetDead()
    {
        isAlive = false;
    }

    /// <summary>
    ///     Rotates a direction vector by degrees
    /// </summary>
    /// <param name="nVector">The direction vector to rotate</param>
    /// <param name="degAngle">Angle in degrees to rotate by</param>
    /// <returns>Rotated Direction Vector</returns>
    Vector3 RotateVectorByAngle(Vector3 nVector, float degAngle)
    {
        float sin = Mathf.Sin(degAngle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degAngle * Mathf.Deg2Rad);
        
        return new Vector3(cos * nVector.x - sin * nVector.y,
            sin * nVector.x + cos * nVector.y);
    }
}
