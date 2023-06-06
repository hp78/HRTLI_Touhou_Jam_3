using UnityEngine;

// Publicly accessable enum for enemy bullet types
//public enum ENEMY_BULLET_TYPE { BASIC1, CURVING1, HOMING1, DEAD };
public enum BULLET_TYPE { BASIC1, CURVING1, HOMING1, DEAD };

// Publicly accessable enum for enemy bullet trajectories
//public enum ENEMY_BULLET_TRAJECTORY { STRAIGHT, STRAIGHT_SINE, HOMING, STILL };
public enum BULLET_TRAJECTORY { STRAIGHT, STRAIGHT_SINE, HOME_SNAP, HOMING, STILL, TIMECUBED };

// The trajectory function which takes in 'elapsedTime' 
public delegate void TrajectoryFunction(ref Vector3 nextPos);

[CreateAssetMenu(menuName = "Bullet/BulletProperty")]
public class BulletProperty : ScriptableObject
{
    // Bullet's Sprite
    public Sprite bulletSprite;

    // Basic properties for the bullet
    public bool isAlive = true;         // If the bullet is active
    public float bulletLife  = 5f;      // The lifetime of a bullet 
    public float bulletSpeed = 1f;      // Velocity of bullet     -> The 'v' value for v' = v + at
    public float bulletAccel = 0f;      // Acceleration of bullet -> The 'a' value for v' = v + at
    public float bulletAngle = 0f;      // Bullet direction in degrees

    // Curve properties for the bullet
    public float curveHeight = 0f;      // Amptitude of function -> The 'a' for f(t) = a sin(bt)
    public float curveLength = 0f;      // Frequency of function -> The 'b' for f(t) = a sin(bt)
    public float elapsedTime = 0f;      // Start time of funciton ->The starting 't' in f(t)

    // The current trajactory type of the bullet
    public BULLET_TRAJECTORY trajectoryType =
        BULLET_TRAJECTORY.STRAIGHT;
    public TrajectoryFunction trajectoryFunction;
}
