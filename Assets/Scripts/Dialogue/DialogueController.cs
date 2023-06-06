using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Luminosity.IO;

public class DialogueController : MonoBehaviour {

    GameController gameController;

    public DialogueSet[] sets;
    int currSet = 0;
    bool isSetActive = false;

    [Header("Left Panel")]
    public Transform left;
    public Image leftBack;
    public Image leftBackPortrait;
    public Image leftTopPortrait;
    public Image leftPortrait;
    public Image leftEmote;
    public Text leftName;
    public Text leftDialogue;
    
    [Header("Right Panel")]
    public Transform right;
    public Image rightBack;
    public Image rightBackPortrait;
    public Image rightTopPortrait;
    public Image rightPortrait;
    public Image rightEmote;
    public Text rightName;
    public Text rightDialogue;

    [Header("Character Sprites")]
    public Sprite orin;
    public Sprite okuu;
    public Sprite cirno;
    public Sprite seiga;
    public Sprite yoshika;
    public Sprite shikieki;
    public Sprite satori;
    public Sprite trolley;

    [Header("Character Color")]
    public Color orinColor;
    public Color okuuColor;
    public Color cirnoColor;
    public Color seigaColor;
    public Color yoshikaColor;
    public Color shikiekiColor;
    public Color satoriColor;
    public Color greyColor;
    public Color trolleyColor;

    [Header("Character Sprites")]
    public Sprite shock;
    public Sprite confused;
    public Sprite sweat;

    // Use this for initialization
    void Start () {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        if (gameController == null)
            Debug.LogAssertion("DialogueController/Start() - GameController ref not set");
    }
	
	// Update is called once per frame
	void LateUpdate () {

		if(isSetActive && !gameController.bvGamePaused.data)
        {
            if (InputManager.GetButtonDown("Shoot") || InputManager.GetButtonDown("ShootGP") || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                NextDialogue();
            }
        }
	}

    public void NextDialogue()
    {
        if(sets[currSet].NextDialogue())
        {
            DisableDialogue();
        }
        else
        {
            UpdateDialogue();
        }
    }

    public void EnableDialogue()
    {
        if (currSet == sets.Length) return;

        isSetActive = true;
        Time.timeScale = 0.0f;
        GameController.instance.bvGameInCutscene.data = true;
        UpdateDialogue();
    }

    void DisableDialogue()
    {
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        isSetActive = false;
        Time.timeScale = 1.0f;
        GameController.instance.bvGameInCutscene.data = false;

        ++currSet;
    }

    public void UpdateDialogue()
    {
        //
        DialogueSet thisSet = sets[currSet];
        bool isLeft = thisSet.set[thisSet.currSet].isLeftChar;

        //
        if (isLeft)
        {
            // Set the left Character
            left.gameObject.SetActive(true);
            left.SetAsLastSibling();

            //leftBackPortrait.color = SetCharColor(thisSet.set[thisSet.currSet].character);
            leftPortrait.sprite = SetCharPortrait(thisSet.set[thisSet.currSet].character);
            leftBackPortrait.sprite = leftPortrait.sprite;
            leftEmote.sprite = SetEmote(thisSet.set[thisSet.currSet].emote);
            leftName.text = SetCharName(thisSet.set[thisSet.currSet].character);
            leftDialogue.text = thisSet.set[thisSet.currSet].sentence;

            leftBack.color = SetCharColor(thisSet.set[thisSet.currSet].character);


            leftTopPortrait.color = Color.white;
            //leftBackPortrait.color = greyColor;
            leftPortrait.color = Color.white;
            leftEmote.color = Color.white;


            // Grey out the right Character
            rightBack.color = greyColor;


            rightTopPortrait.color = greyColor;
            //rightBackPortrait.color = greyColor;
            rightPortrait.color = greyColor;
            rightEmote.color = greyColor;
        }
        else
        {
            // Set the right Character
            right.gameObject.SetActive(true);
            right.SetAsLastSibling();

            //rightBackPortrait.color = SetCharColor(thisSet.set[thisSet.currSet].character);
            rightPortrait.sprite = SetCharPortrait(thisSet.set[thisSet.currSet].character);
            rightBackPortrait.sprite = rightPortrait.sprite;
            rightEmote.sprite = SetEmote(thisSet.set[thisSet.currSet].emote);
            rightName.text = SetCharName(thisSet.set[thisSet.currSet].character);
            rightDialogue.text = thisSet.set[thisSet.currSet].sentence;

            rightBack.color = SetCharColor(thisSet.set[thisSet.currSet].character);


            rightTopPortrait.color = Color.white;
            //rightBackPortrait.color = greyColor;
            rightPortrait.color = Color.white;
            rightEmote.color = Color.white;


            // Grey out the left Character
            leftBack.color = greyColor;


            leftTopPortrait.color = greyColor;
            //leftBackPortrait.color = greyColor;
            leftPortrait.color = greyColor;
            leftEmote.color = greyColor;
        }
    }

    public Color SetCharColor(Dialogue.DialogueChar character)
    {
        switch (character)
        {
            case Dialogue.DialogueChar.ORIN:
                return orinColor;

            case Dialogue.DialogueChar.OKUU:
                return okuuColor;

            case Dialogue.DialogueChar.CIRNO:
                return cirnoColor;

            case Dialogue.DialogueChar.SEIGA:
                return seigaColor;

            case Dialogue.DialogueChar.YOSHIKA:
                return yoshikaColor;

            case Dialogue.DialogueChar.SHIKIEKI:
                return shikiekiColor;

            case Dialogue.DialogueChar.SATORI:
                return satoriColor;

            case Dialogue.DialogueChar.TROLLEY:
                return trolleyColor;

            default:
                return orinColor;
        }
    }

    public Sprite SetCharPortrait(Dialogue.DialogueChar character)
    {
        switch (character)
        {
            case Dialogue.DialogueChar.ORIN:
                return orin;

            case Dialogue.DialogueChar.OKUU:
                return okuu;

            case Dialogue.DialogueChar.CIRNO:
                return cirno;

            case Dialogue.DialogueChar.SEIGA:
                return seiga;

            case Dialogue.DialogueChar.YOSHIKA:
                return yoshika;

            case Dialogue.DialogueChar.SHIKIEKI:
                return shikieki;

            case Dialogue.DialogueChar.SATORI:
                return satori;

            case Dialogue.DialogueChar.TROLLEY:
                return trolley;

            default:
                return null;
        }
    }

    public string SetCharName(Dialogue.DialogueChar character)
    {
        switch (character)
        {
            case Dialogue.DialogueChar.ORIN:
                return "Orin";

            case Dialogue.DialogueChar.OKUU:
                return "Okuu";

            case Dialogue.DialogueChar.CIRNO:
                return "Cirno";

            case Dialogue.DialogueChar.SEIGA:
                return "Seiga";

            case Dialogue.DialogueChar.YOSHIKA:
                return "Yoshika";

            case Dialogue.DialogueChar.SHIKIEKI:
                return "Shikieiki";

            case Dialogue.DialogueChar.SATORI:
                return "Satori";

            case Dialogue.DialogueChar.TROLLEY:
                return "???";

            default:
                return "";
        }
    }

    public Sprite SetEmote(Dialogue.DialogueEmote emote)
    {
        switch(emote)
        {
            case Dialogue.DialogueEmote.SHOCK:
                return shock;

            case Dialogue.DialogueEmote.CONFUSED:
                return confused;

            case Dialogue.DialogueEmote.SWEAT:
                return sweat;

            default:
                return null;
        }
    }
}
