using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableVariable/Int")]
public class IntVar : ScriptableObject
{
    [SerializeField]
    int mData = 0;

    public int data
    {
        set { mData = value; }
        get { return mData; }
    }
}