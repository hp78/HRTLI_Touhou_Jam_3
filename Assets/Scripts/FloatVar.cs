using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableVariable/Float")]
public class FloatVar : ScriptableObject
{
    [SerializeField]
    float mData = 0.0f;

    public float data
    {
        set { mData = value; }
        get { return mData; }
    }
}