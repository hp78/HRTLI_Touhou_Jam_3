using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    //
    public enum DialogueChar { NONE, ORIN, OKUU, CIRNO, SEIGA, YOSHIKA, SHIKIEKI, SATORI, TROLLEY };
    public enum DialogueEmote { NONE, SHOCK, CONFUSED, SWEAT };

    //
    public bool isLeftChar;
    public DialogueChar character;
    public DialogueEmote emote;
    [TextArea(1, 5)]
    public string sentence;
}
