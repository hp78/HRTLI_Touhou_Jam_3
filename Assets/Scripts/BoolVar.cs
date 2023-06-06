using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableVariable/Bool")]
public class BoolVar : ScriptableObject
{
    [SerializeField]
    bool mData;

    public bool data
    {
        set { mData = value; }
        get { return mData; }
    }
}