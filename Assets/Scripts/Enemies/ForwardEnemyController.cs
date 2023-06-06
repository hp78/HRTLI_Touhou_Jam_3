using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardEnemyController : MonoBehaviour
{

    public Enemy_base script;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        script.Shoot();
    }

    public void Kill()
    {
        script.Kill();
    }
}
