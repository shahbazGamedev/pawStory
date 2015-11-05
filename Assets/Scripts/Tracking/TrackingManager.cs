/**
Script Author : Vaikash
Description   : Game Manager - Tracking
**/

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class TrackingManager : MonoBehaviour
{
    #region Variables

    //public static TrackingManager instanceRef;
    public Text instructions;
    public Text score;
    public GameObject marker;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject touchMat;
    public Slider dogCapacity;
    public GameObject lineRenderer;
    public GameObject dogRef;
    public GameObject[] life;
    public Text roundInfoDisplay;
    public GameObject markerPrefab;
    public GameObject dragStartRange;
    GameObject trackingMarker;

    public int round;
    public int resetChances;
    public int points;
    public int maxRounds;
    public int scoreIncrement;

    public float minSqrDistance;
    public float interpolationScale;
    public float maxTrackingCapacity;
    public List<Vector3> dragData;
    public float sliderUpdateSpeed;
    public bool isGameOn;
    public bool roundComplete;
    public GameObject[] obstacleCollection;
    public bool lastChance;

    DogPathMovement pathMove;
    Animator dogAnim;
    LineRenderer line;
    string layerName;

    Vector3 worldPoint;
    Vector3 potentialSamplePoint;
    bool isFirstRun;
    bool needToPop;
    bool swipeFinished;
    bool gameOver;
    bool startTracking;
    bool pathEnable;
    bool reset; // Prevents the user from drawing path when dog capacity slider is filling up
    bool lineRendererActive;
    bool canReset; // Allows the user to reset path if true
    bool justStarted;
    float distanceCovered;
    float remainingCapacity;

    Vector3 touchStartPosition;
    Vector3 previousPosition;
    Vector3 startPosition;
    Quaternion startRotation;
    Rigidbody markerRB;

    BezierCurve bezierPath;
    List<Vector3> drawingPoints;

    public delegate void TrackingEvent();
    public static TrackingEvent SpawnMarker;

    #endregion Variables

    public void Awake()
    {
        dogRef = GameObject.FindGameObjectWithTag("Player");
        pathMove = dogRef.GetComponent<DogPathMovement>();
        dogAnim = dogRef.GetComponent<Animator>();
        line = lineRenderer.GetComponent<LineRenderer>();
        trackingMarker = (GameObject) Instantiate(markerPrefab, Vector3.one, Quaternion.AngleAxis(90f, new Vector3(1, 0f, 0f)));
        trackingMarker.SetActive(false);
        markerRB = trackingMarker.GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // sets the dog path data and enables path movement
        if (startTracking)
        {
            RenderBezier();
            drawingPoints = bezierPath.GetDrawingPoints1();
            pathMove.SetPathData(drawingPoints);
            pathMove.EnableDogPathMovement(true, false);
            startTracking = false;
            swipeFinished = false;
        }

        // initiates gameOver function
        if (gameOver)
        {
            GameOver();
        }

        // initiates nextRound function
        if (roundComplete)
        {
            StartNextRound();
            roundComplete = false;
        }

        // updates gameover bool based on maxRounds
        if (round > maxRounds && !gameOver)
        {
            gameOver = true;
            points += resetChances * 10;
        }
    }

    // Initialize game
    void Init()
    {
        justStarted = true;
        dogCapacity.maxValue = maxTrackingCapacity;
        bezierPath = new BezierCurve();
        isFirstRun = true;
        roundComplete = true;

        StartCoroutine(UpdateSlider());
        startPosition = dogRef.transform.position;
        startRotation = dogRef.transform.rotation;
        SpawnMarker();
        obstacleCollection[Random.Range(0, 5)].SetActive(true);

        // Add Event Listeners
        EventMgr.GameRestart += PlayAgain;
        DogPathMovement.PathEnd += ReachedPathEnd;
        DogPathMovement.TargetReached += ReachedTarget;
        DogPathMovement.DogReturned += DogReturned;
    }

    // Decouple Event Listeners on disable
    public void OnDisable()
    {
        EventMgr.GameRestart -= PlayAgain;
        DogPathMovement.PathEnd -= ReachedPathEnd;
        DogPathMovement.TargetReached -= ReachedTarget;
        DogPathMovement.DogReturned -= DogReturned;
    }

    #region BezierInterface

    // smooth and render curve each frame as player swipes/drags on screen
    void RenderBezier()
    {
        bezierPath.Interpolate(dragData, interpolationScale);
        drawingPoints = bezierPath.GetDrawingPoints1();
        SetLinePoints(drawingPoints);
        lineRendererActive = true;
        if (trackingMarker == null)
        {
            trackingMarker = (GameObject) Instantiate(markerPrefab, drawingPoints.LastOrDefault(), Quaternion.AngleAxis(90f, new Vector3(1, 0f, 0f)));
        }
        else
        {
            trackingMarker.SetActive(true);
            markerRB.MovePosition(drawingPoints.LastOrDefault());
            //trackingMarker.transform.position = drawingPoints.LastOrDefault();

        }

    }

    // update line renderer with fresh count and positions
    void SetLinePoints(List<Vector3> drawingPoint)
    {
        line.SetVertexCount(drawingPoint.Count);

        for (int i = 0; i < drawingPoint.Count; i++)
        {
            line.SetPosition(i, drawingPoint[i]);
        }
    }

    // Add end point OnDragEnd
    void addEndPoint(BaseEventData Data)
    {
        var data=(PointerEventData)Data;
        Vector3 screenPoint = new Vector3(data.position.x, data.position.y, 0f);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out hit, 200f))
        {
            layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            if (layerName == "Floor")
            {
                worldPoint = hit.point + (Vector3.up * 0.02f);
                dragData.Add(worldPoint);
            }
        }
    }

    #endregion BezierInterface

    // Activates the gameOver Panel
    void GameOver()
    {
        instructions.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        gameOverText.text = "Score: " + points + " Points.";
        foreach (var live in life) // Destroys remaining resets / lives
        {
            live.SetActive(false);
        }
        touchMat.SetActive(false);
    }

    // Event handler for dog returned
    void DogReturned()
    {
        line.SetVertexCount(0);
        lineRendererActive = false;
        round += 1;
        StartCoroutine(FillDogCapacity());
        StartCoroutine(UpdateRoundInfoDisplay());
        if (round > maxRounds)
            return;
        pathEnable = true;
        isFirstRun = true;
        SpawnMarker();
        if (trackingMarker != null)
            trackingMarker.SetActive(false);
    }

    // Starts next round and updates the corresponding variables
    void StartNextRound()
    {
        dragData.Clear();
        if (round >= 1)
            pathMove.EnableDogPathMovement(false, true);
        if (justStarted)
        {
            justStarted = false;
            DogReturned();
        }
        //StartCoroutine(dogRef.GetComponent<DogManager>().MoveToPosition(startPosition, startRotation));

    }

    #region Coroutines

    // Fills the dog capacity bar over time
    IEnumerator FillDogCapacity()
    {
        reset = true;
        while (dogCapacity.value < dogCapacity.maxValue)
        {
            yield return new WaitForFixedUpdate();
            distanceCovered -= Time.deltaTime * sliderUpdateSpeed;
        }
        distanceCovered = 0;
        dogCapacity.value = dogCapacity.maxValue;
        reset = false;
        yield return null;
    }

    // Depletes the dog capacity bar as player swipe the path
    IEnumerator UpdateSlider()
    {
        while (!gameOver)
        {
            yield return new WaitForFixedUpdate();
            remainingCapacity = maxTrackingCapacity - distanceCovered;
            dogCapacity.value = remainingCapacity;
            if (remainingCapacity <= 0)
                isGameOn = false;
        }
        yield return null;
    }

    // Displays round info at start of each round
    IEnumerator UpdateRoundInfoDisplay()
    {
        if (round <= maxRounds)
        {
            instructions.text = "Round " + round;
            yield return new WaitForSeconds(1.5f);
        }
    }

    // Dog has tracked the target successfully
    IEnumerator TargetFound()
    {
        TargetSpawner.instRef.KillMarkers();
        yield return new WaitForEndOfFrame();
        pathMove.reachedTarget = false;
        dogAnim.SetTrigger("Win");
        yield return new WaitForSeconds(3);
        roundComplete = true;
    }

    // Dog has failed to track
    IEnumerator TargetNotFound()
    {
        yield return new WaitForEndOfFrame();
        pathMove.reachedPathEnd = false;
        dogAnim.SetTrigger("Lose");
        yield return new WaitForSeconds(3);
        roundComplete = true;
    }

    // Reload Level
    IEnumerator Reload()
    {
        Application.LoadLevel(Application.loadedLevel);
        yield return null;
    }

    #endregion Coroutines

    #region EventTriggers

    // Event handler for target not found
    void ReachedPathEnd()
    {
        StartCoroutine(TargetNotFound());
    }

    // Event handler for target found
    private void ReachedTarget()
    {
        points += scoreIncrement + (int) dogCapacity.value;
        StartCoroutine(TargetFound());
    }

    // Clear previous Swipe on Drag Begin
    public void OnBeginDrag(BaseEventData Data)
    {
        var data=(PointerEventData)Data;
        if (resetChances >= 0)
        {
            if (!isGameOn && pathEnable)
            {
                if (!reset)
                {
                    Vector3 screenPoint = new Vector3(data.position.x, data.position.y, 0f);
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(screenPoint);
                    if (Physics.Raycast(ray, out hit, 200f))
                    {
                        //Debug.Log("Hit");
                        layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                        if (layerName == "Toys")
                        {
                            touchStartPosition = dragStartRange.transform.position;
                            marker.gameObject.SetActive(false);
                            pathEnable = false;
                            isGameOn = true;
                            dragData.Clear();
                            isFirstRun = true;
                        }
                    }
                }
            }
        }
    }

    // Add end point
    public void OnEndDrag(BaseEventData data)
    {
        swipeFinished = true;
        if (isGameOn)
        {
            addEndPoint(data);
            isGameOn = false;
            if (dragData.Count > 1)
            {
                // Render Line
                RenderBezier();
            }
        }
    }

    // Check dist between drag points and add points that are min dist apart
    public void OnDrag(BaseEventData Data)
    {
        var data=(PointerEventData)Data;
        var screenPoint = new Vector3(data.position.x, data.position.y, 0f);
        if (isGameOn)
        {
            // raycast to find hit point on plane
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out hit, 200f))
            {
                layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                if (layerName == "Floor" || layerName == "Toys")
                {
                    worldPoint = hit.point + (Vector3.up * 0.01f);

                    if (dragData.Count < 1) // dragData is empty
                    {
                        dragData.Add(touchStartPosition);
                        dragData.Add(worldPoint);
                        previousPosition = worldPoint;
                    }
                    else if (dragData.Count == 2 && isFirstRun) // dragData has drag start point
                    {
                        potentialSamplePoint = worldPoint;
                        isFirstRun = false;
                    }
                    else // checks for minimum distance between points and adds them if true
                    {
                        if (needToPop)
                        {
                            dragData.RemoveAt(dragData.Count - 1);
                            needToPop = false;
                        }
                        if ((dragData.LastOrDefault() - potentialSamplePoint).sqrMagnitude >= minSqrDistance)
                        {
                            dragData.Add(worldPoint);
                        }
                        else // added to prevent visual lag - removed @ next frame
                        {
                            dragData.Add(worldPoint);
                            needToPop = true;
                        }
                        potentialSamplePoint = worldPoint;
                    }
                }
                else if(layerName == "Back")
                {
                    // code for disrupting tracking
                    isGameOn = false;
                    if (!lastChance)
                        ResetPathBtn();
                    else
                        gameOver = true;
                }
                distanceCovered += Vector3.Distance(worldPoint, previousPosition);
                previousPosition = worldPoint;
            }
            if (dragData.Count > 1)
            {
                // Render Line
                RenderBezier();
                canReset = true;
            }
        }
    }

    #endregion EventTriggers

    #region ButtonCallbacks

    // Back button
    public void GoBack()
    {
        Application.LoadLevel(GlobalConst.Scene_MainMenu);
    }

    // Replay button
    public void PlayAgain()
    {
        StartCoroutine(Reload());
    }

    // Reset Path button
    public void ResetPathBtn()
    {
        if (lineRendererActive && canReset)
        {
            resetChances -= 1;
            if (resetChances >= 0)
            {
                line.SetVertexCount(0);
                lineRendererActive = false;
                StartCoroutine(FillDogCapacity()); // Fills the dog capacity
                life[resetChances].SetActive(false);
                dragData.Clear();
                isFirstRun = true;
                pathEnable = true;
                trackingMarker.SetActive(false);
                if (resetChances == 0)
                    lastChance = true;
            }
            else
                resetChances = 0;
        }
    }

    // Signal Dog to track
    public void StartTracking()
    {

        if (dragData.Count < 2)
        {
            Debug.Log("Swipe data empty");
        }
        else
        {
            if (swipeFinished)
            {
                canReset = false; // Disable reset button once tracking is started
                startTracking = true;

            }
        }
    }

    #endregion ButtonCallbacks
}