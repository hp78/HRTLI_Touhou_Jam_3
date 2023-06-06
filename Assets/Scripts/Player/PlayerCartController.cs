using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCartController : MonoBehaviour
{
    float attachedAngle = 0.0f;
    float currInertia = 0.0f;
    int cartCount = 0;
    public BoolVar bvPlayerAlive;
    public CartCorpseBehaviour corpseChild;
    Vector3 corpseOffset = new Vector3(0.0f,0.25f,-0.1f);

    const float bombInterval = 0.05f;
    float lastBombTime = 0.0f;
    const float bombDuration = 1.0f;
    float currBombDuration = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        corpseChild = GetComponentInChildren<CartCorpseBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCorpseChild();
        UpdateCartBomber();
    }

    void UpdateCorpseChild()
    {
        if (corpseChild == null) return;

        Vector3 currEuRot = corpseChild.transform.localEulerAngles;
        corpseChild.transform.localEulerAngles = new Vector3(0,0,
                                                 Mathf.LerpAngle(currEuRot.z, attachedAngle + currInertia, Time.deltaTime));
    }

    void UpdateCartBomber()
    {
        if (currBombDuration > 0.0f)
        {
            if (corpseChild == null)
            {
                currBombDuration = 0.0f;
                return;
            }

            currBombDuration -= Time.deltaTime;
            lastBombTime -= Time.deltaTime;

            if(lastBombTime < 0.0f)
            {
                ShootCorpse();
                lastBombTime = bombInterval;
            }
        }
    }

    public void SetInertia(float val, float speed)
    {
        if (val > 0.1f)
            currInertia = 0.5f + speed;
        else if (val < -0.1f)
            currInertia = -0.5f - speed;
        else
            currInertia = 0.0f;

        if (corpseChild != null)
        {
            corpseChild.SetInertia(currInertia);
        }
    }

    public bool EjectCorpses()
    {
        if (corpseChild == null)
        {
            return false;
        }

        cartCount = 0;

        corpseChild.EjectCorpses();
        corpseChild = null;
        return true;
    }

    public bool ShootCorpse()
    {
        //
        if (corpseChild == null)
        {
            return false;
        }

        corpseChild = corpseChild.ShootCorpse();

        //
        if (corpseChild != null)
        {
            corpseChild.transform.localPosition = corpseOffset;
        }

        --cartCount;

        return true;
    }

    public bool BombCorpses()
    {
        if (currBombDuration <= 0.0f)
        {
            currBombDuration = bombDuration;
        }

        return true;
    }

    public bool AttachCorpse(CartCorpseBehaviour corpse)
    {
        if (!bvPlayerAlive.data)
            return false;

        if(corpseChild == null)
        {
            attachedAngle = corpse.transform.eulerAngles.z;
            corpse.transform.parent = transform;
            corpseChild = corpse;
            corpseChild.transform.position = transform.position + corpseOffset;

            return true;
        }
        else if(cartCount < 15)
        {
            corpseChild.AttachCorpse(corpse);

            ++cartCount;

            return true;
        }
        return false;
    }
}
