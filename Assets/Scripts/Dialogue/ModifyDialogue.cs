using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyDialogue : MonoBehaviour
{
    public DialogueSet set;
    public IntVar ivTimesContinued;

    // Start is called before the first frame update
    void Start()
    {
        set.set[16].sentence = "they've died at least " + ivTimesContinued.data +" times playing it";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
