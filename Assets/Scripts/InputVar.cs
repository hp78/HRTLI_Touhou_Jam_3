using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableVariable/InputVar")]
public class InputVar : ScriptableObject
{
    [SerializeField]
    KeyCode mData = KeyCode.None;

    public KeyCode data
    {
        set { mData = value; }
        get { return mData; }
    }
}