using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableVariable/String")]
public class StringVar : ScriptableObject
{
    [SerializeField]
    string mData = "";

    public string data
    {
        set { mData = value; }
        get { return mData; }
    }
}