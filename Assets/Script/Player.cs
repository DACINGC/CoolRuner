using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D playerCollider;

    public Text scoreText;
    public Text distanceText;
    public Text dieText;
    public Slider mySlider;

    [Space]
    [Header("得分和距离")]
    public float score = 0;
    public float distance = 0;
    public float awardScore = 0;

    private float highScore = 0; // 历史最高得分
    private const string HIGH_SCORE_KEY = "HighScore"; // 存储历史最高得分的键名

    private Vector2 originalPosion;
    private Vector2 originalSize;

    private float magnetTimer = 0;
    private float magnetTimerDuration = 5;

    private float sprintTimer = 0;
    private float sprintTimerDuration = 4;

    private float awardTimer;
    private float awardTimerDuration = 5;

    public bool isSprintinng { get; private set; }//冲刺

    [Space]
    public bool isAwarding;//奖励
    private bool isMagneted;
    private bool isGrounded;

    private bool isCollided = false;
    private bool canAirJump = true;
    public float airJumpNumber = 2;

    private bool canJump;

    [Space]
    [Header("音效")]
    public AudioClip jumpMusic;
    public AudioClip eatCornMusic;
    public AudioClip dieMusic;
    public AudioClip eatBonus;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();

        originalPosion = transform.position;
        originalSize = playerCollider.size;
    }

    // Update is called once per frame
    void Update()
    {
        sprintTimer -= Time.deltaTime;
        magnetTimer -= Time.deltaTime;
        awardTimer -= Time.deltaTime;

        //存储历史最高得分
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat(HIGH_SCORE_KEY, highScore);
        }

        InputLogic();
        TextChanger();
        FixedPosition();

        if (magnetTimer > 0 || isMagneted)
        {
            MagnetBuff();
        }

        if (sprintTimer > 0)
        {
            SprintBuff();
            
        }
        else if (sprintTimer < 0)
        {
            isMagneted = false;
            isSprintinng = false;
            playerCollider.isTrigger = false;
        }

        AwardMethod();

        SliderControl();
    }

    private void AwardMethod()
    {
        if (awardTimer > 0)
        {
            isAwarding = true;
            awardScore = 0;
        }
        else
            isAwarding = false;
    }

    private void SliderControl()
    {
        if (isAwarding)
        {
            mySlider.value = 0;
            return;
        }

        mySlider.value = awardScore;

        if (mySlider.value == mySlider.maxValue)
        {
            awardTimer = awardTimerDuration;
        }
    }

    private void FixedPosition()
    {
        if (isGrounded && !isCollided)
        {
            if (transform.position.x > originalPosion.x)
                transform.Translate(Vector2.left * Time.deltaTime * 1);
            if (transform.position.x < originalPosion.x)
                transform.Translate(Vector2.right * Time.deltaTime * 1);
        }
    }

    private void TextChanger()
    {
        scoreText.text = "得分：" + score.ToString();

        distance += Time.deltaTime * 20;
        distanceText.text = "距离：" + ((int)distance).ToString();
    }

    private void InputLogic()
    {

        if (airJumpNumber <= 0)
        {
            canJump = false;
            airJumpNumber = 2;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && canJump && airJumpNumber>0)
        {
            rb.velocity = Vector2.zero;
            //rb.isKinematic = true;
            rb.AddForce(Vector2.up * 600);
            anim.SetBool("Jump", true);
            AudioManager.Instance.PlayMusic(jumpMusic);


            if (airJumpNumber == 1)
            { 
                anim.SetBool("DoubleJump", true);
                
            }
            //////////二段跳
            if (canAirJump)
            {
                airJumpNumber--;
            }

        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            anim.SetBool("Slide", true);
            playerCollider.size = new Vector2(originalSize.y, originalSize.x );
            canJump = false;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            anim.SetBool("Slide", false);
            playerCollider.size = originalSize;
            canJump = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Thron")
        {
            anim.speed = 0;
            dieText.gameObject.SetActive(true);
            Invoke("Die", 2f);
            AudioManager.Instance.PlayMusic(dieMusic);
        }
        if (collision.transform.tag == "Spring_S")
        {
            score += 5;
            awardScore += 5;
            Destroy(collision.gameObject);
        }

        if (collision.transform.tag == "Ground")
        {
            airJumpNumber = 2;

            anim.SetBool("Jump", false);
            canJump = true;
            anim.SetBool("DoubleJump", false);
            isGrounded = true;
        }
        else
            isGrounded = false;

        if (collision.transform.tag == "Barrier")
            isCollided = true;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        isCollided = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSprintinng)
            Destroy(collision.gameObject);

        if (collision.tag == "Sprint")
        {
            sprintTimer = sprintTimerDuration;
            Destroy(collision.gameObject);
        }

        if (collision.transform.tag == "Magnet")
        {
            magnetTimer = magnetTimerDuration;
            Destroy(collision.gameObject);
        }

        if (collision.transform.tag == "Bonus1")
        {
            score++;
            awardScore++;
            AudioManager.Instance.PlayMusic(eatCornMusic);
            Destroy(collision.gameObject);
        }

        if (collision.transform.tag == "Bonus2")
        {
            awardScore += 5;
            score += 5;
            AudioManager.Instance.PlayMusic(eatBonus);

            Destroy(collision.gameObject);
        }

        if (collision.transform.tag == "EndCo")
        {
            dieText.gameObject.SetActive(true);
            Invoke("Die", 2f);
        }
    }

    //磁铁Buff
    public void MagnetBuff()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 8);

        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Bonus1" || collider.tag == "Bonus2")
            { 
                Vector2 moveDir = transform.position - collider.transform.position;

                collider.transform.Translate(moveDir * 15 * Time.deltaTime); 
            }
        }
    }

    public void SprintBuff()
    {
        canJump = false;

        playerCollider.isTrigger = true;   
        rb.velocity = Vector2.zero;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 2), 10 * Time.deltaTime);

        isSprintinng = true;
        isMagneted = true;
    }

    private void Die()
    {
        SceneManager.LoadScene(0);
    }

}
