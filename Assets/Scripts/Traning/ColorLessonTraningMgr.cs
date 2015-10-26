/**
Script Author : Srivatsan 
Description   : Color Lesson Training
**/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorLessonTraningMgr : MonoBehaviour
{



    public float Speed;
    public float speedDampTime;
    public GameObject PanelGameOver;
    public GameObject redBtn;
    public GameObject blueBtn;
    public GameObject greenBtn;
    public GameObject whiteBtn;
    public Transform Target;
    public Transform red;
    public Transform blue;
    public Transform green;
    public Transform yellow;
    public Text TxtTimer;
    float timer;
    float distance;
    bool isMoving;
    bool collectRed;
    bool collectBlue;
    bool collectGreen;
    bool collectWhite;
    bool canClick;
    bool gameStart;
    Vector3 startPos;
    Animator dogAnim;
    Rigidbody rb;
    string layerName;
    public Text teachingTxt;
    public GameObject TeachingPnl;
    public Text TxtGameOver;






    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        isMoving = false;
        dogAnim = GetComponent<Animator>();
        PanelGameOver.SetActive(false);
        canClick = true;
        teachingTxt.text = "Teach Green";
        blueBtn.SetActive(false);
        redBtn.SetActive(false);
        whiteBtn.SetActive(false);
    }


    void OnEnable()
    {
        EventMgr.GameRestart += OnRestartGame;
    }


    void OnDisable()
    {
        EventMgr.GameRestart -= OnRestartGame;
    }



    void Update()
    {

        distance = Vector3.Distance(Target.position, transform.position);
        if (distance > 2f && isMoving == true)
        {
            transform.LookAt(Target);
            dogAnim.SetFloat("Walk", 1f);
            float step = Speed * Time.deltaTime;
            rb.MovePosition(Vector3.MoveTowards(transform.position, Target.position, step));
        }
        if (distance < 2f)
        {
            isMoving = false;
            canClick = true;

            dogAnim.SetFloat("Walk", 0f);

        }
        TargetCreator();
        GameOver();

    }


    void TargetCreator()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 300f))
            {
                Debug.Log("Hit");
                layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                if (layerName == "Toys")
                {
                    collectGreen = true;
                    Debug.Log("S");
                    Target = green;
                    collectBlue = false;
                    collectRed = false;
                    collectWhite = false;
                }


                if (layerName == "Floor")
                {
                    collectBlue = true;
                    Debug.Log("t");
                    Target = blue;
                    collectGreen = false;
                    collectRed = false;
                    collectWhite = false;
                }


                if (layerName == "Dog")
                {
                    collectWhite = true;
                    Debug.Log("e");
                    Target = yellow;
                    collectBlue = false;
                    collectRed = false;
                    collectGreen = false;
                }  

                
                if (layerName == "Back")
                {
                    collectRed = true;
                    Debug.Log("v");
                    Target = red;
                    collectBlue = false;
                    collectGreen = false;
                    collectWhite = false;
                }
            }
        }
    }


    void FixedUpdate()
    {
        timer += Time.deltaTime;
        TxtTimer.text = "Time:" + (int)timer;
    }


    public void RedMove()
    {
        if (canClick && collectRed)
        {
            canClick = false;
            isMoving = true;
        }
        else
        {
            teachingTxt.text = "Wrong Color";
            timer += 2f;
        }
    }


    public void BlueMove()
    {
        if (canClick && collectBlue)
        {

            canClick = false;
            isMoving = true;
        }
        else
        {
            teachingTxt.text = "Wrong Color";
            timer += 2f;
        }
    }


    public void GreenMove()
    {
        if (canClick && collectGreen)
        {
            canClick = false;
            isMoving = true;
        }
        else
        {
            teachingTxt.text = "Wrong Color";
            timer += 2f;
        }
    }


    public void YellowMove()
    {
        if (canClick && collectWhite)
        {
            canClick = false;
            isMoving = true;


        }
        else
        {
            teachingTxt.text = "Wrong Color";
            timer += 2f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Toys")
        {
            greenBtn.SetActive(false);
            teachingTxt.text = "Teach Red";
            redBtn.SetActive(true);

        }
        if (other.gameObject.tag == "Basket")
        {
            redBtn.SetActive(false);
            teachingTxt.text = "Teach Blue";
            blueBtn.SetActive(true);

        }
        if (other.gameObject.tag == "MovePoints")
        {
            PanelGameOver.SetActive(true);
            TeachingPnl.SetActive(false);
            TxtGameOver.text = "Puppy Learned Well!!";
            timer = 0f;

        }

        if (other.gameObject.tag == "Ball")
        {
            blueBtn.SetActive(false);
            teachingTxt.text = "Teach white";
            whiteBtn.SetActive(true);

        }
    }


    void GameOver()
    {
        if (timer >= 20)
        {
            PanelGameOver.SetActive(true);
            TeachingPnl.SetActive(false);
            TxtGameOver.text = "Traning Session Failed!!";

        }
    }

    void OnRestartGame()
    {
        transform.position = startPos;
        timer = 0f;
        PanelGameOver.SetActive(false);
        TeachingPnl.SetActive(true);
        canClick = true;
        isMoving = false;
        teachingTxt.text = "Teach Green";
        greenBtn.SetActive(true);
        blueBtn.SetActive(false);
        redBtn.SetActive(false);
        whiteBtn.SetActive(false);
        transform.rotation = Quaternion.identity;
    }
}