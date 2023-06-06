using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSet : MonoBehaviour {

    public Dialogue[] set;
    public int currSet = 0;

    public bool NextDialogue()
    {
        ++currSet;

        if (currSet >= set.Length)
            return true;

        return false;
    }
}
