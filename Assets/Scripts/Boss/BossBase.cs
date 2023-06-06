using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;


public class BossBase : MonoBehaviour
{

    public enum BossState { ENTRY,SPELL, DECLARE, DEAD, INVUL};
    public BossState bossState;

    public BoxCollider2D boxCol;
    public Animator animator;
    public int maxHealth;
    [ReadOnly]
    public int currentHealth;

    public int noOfLives;
    [ReadOnly]
    public int currLives;

    public GameObject spawnTarget;


    protected Color dmgedColor = new Color(1.0f, 0.0f, 0.0f);
    protected Color realColor = new Color(1.0f, 1.0f, 1.0f);
    protected float damagedFrame = 0.0f;
    public SpriteRenderer sprite;

    float invulTime;
    public Transform player;
    public CircleCollider2D playerCol;

    public int phaseNo;

    public ParticleSystem chargeUp;
    public ParticleSystem death;

    public AudioSource hitsound;
    public AudioSource spellDeclare;
    public AudioSource deadSound;

    public Image health;
    public Text spellcardText;

    float declareTimer;
    bool declared;
    protected bool dead;
    public string[] spell;

    public GameObject CineTriggerStart;
    public GameObject CineTriggerEnd;

    public void StartCine()
    {
        CineTriggerStart.SetActive(true);
    }

    // Start is called before the first frame update
    protected void Start()
    {
        currentHealth = maxHealth;
        currLives = noOfLives;
        boxCol.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        invulTime = 3.0f;
        declareTimer = 3.5f;

    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void StartFight()
    {
        bossState = BossState.DECLARE;

        GameController.instance.bossHUDController.DoCutIn();
        spellcardText.text = "fug em good";
    }

    public void Declare()
    {
        declareTimer -= Time.deltaTime;
        if(!declared)
        {
            GameController.instance.bossHUDController.DoCutIn();
            spellcardText.text = spell[phaseNo -1];
            spellDeclare.Play();
            declared = true;

        }
        if(declareTimer< 0.0f)
        {
            declareTimer = 3.5f;
            declared = false;
            boxCol.enabled = true;
            bossState = BossState.SPELL;

        }
    }

    public void BossInvulMode()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2 (5f,0f) , 3.0f * Time.deltaTime);
        invulTime -= Time.deltaTime;
        sprite.enabled = !sprite.enabled;
        if (invulTime < 0.0f)
        {
            invulTime = 3.0f;
            bossState = BossState.DECLARE;
            ++phaseNo;
            sprite.enabled = true;
        }

    }


    public void EnemyFeedback()
    {
        // if damageframe is over, set back default speed and color
        if (sprite.color != realColor && damagedFrame <= 0.0f)
        {
            sprite.color = realColor;
        }
    }

    public IEnumerator DeathSeq(int level)
    {
        boxCol.enabled = false;
        GameController.instance.bossHUDController.animator.Play("empty");
        CineTriggerEnd.SetActive(true);
        yield return new WaitForSeconds(.5f);

        death.Play();
        playerCol.enabled = false;
        yield return new WaitForSeconds(2.5f);
        deadSound.Play();
        yield return new WaitForSeconds(1f);

        dead = true;
        sprite.enabled = false;

        yield return new WaitForSeconds(2f);
        if (level == 0)
        {
            GameController.instance.ChangeLevel("Ending");
            SoundManager.instance.PlayMenu();
        }
            
        else
        {
            GameController.instance.ChangeLevel(("Level" + level).ToString());
            SoundManager.instance.PlayStageTheme();
        }
        yield return 0;
    }

    public void PlayHit()
    {
        hitsound.Play();
    }


}
