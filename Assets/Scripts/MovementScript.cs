using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour
{
    #region Inspector Fields

    [Header("References")]
    public Transform respawnPoint;
    public CinemachineVirtualCamera cam;
    public CinemachineVirtualCamera orbit;
    public CinemachineVirtualCamera follow;
    public GameObject cube;
    public GameObject part;
    public Image charge;
    public bool isSpawning;

    [Header("Movement Settings")]
    [SerializeField] float rollTorque = 20f;
    [SerializeField] float turnSpeed = 120f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] float jumpVel = 8f;
    [SerializeField] float jumpVelFor = 8f;
    [SerializeField] float coyote = 0.2f;

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI currentTimeText;
    [SerializeField] TextMeshProUGUI highScoreText;

    [Header("Voice Lines")]
    [SerializeField] AudioClip[] voiceLinesMove;
    [SerializeField] AudioClip[] voiceLinesIdle;
    [SerializeField] AudioClip[] voiceLinesHit;
    [SerializeField] AudioClip colClip;
    [SerializeField] AudioSource source;
    public AudioSource sourceJump;
    [SerializeField] AudioSource sourceCol;
    [SerializeField] AudioSource rolling;

    #endregion

    #region Private Fields

    private Rigidbody rb;
    private Scene activeScene;

    private Vector3 COM = new Vector3(0, 0.5f, 0);
    private Vector3 forwardDir;

    private float coyoteTimer;
    private float timer;
    private float highScore = Mathf.Infinity;
    private float lastVoice = -Mathf.Infinity;
    private float camTimer = 0f;

    private bool flipped;
    private bool canJump;
    private bool drain;
    private bool timerRunning = true;
    private bool following = true;
    private bool hasJumped;

    #endregion

    #region Unity Methods

    private void Start()
    {
        activeScene = SceneManager.GetActiveScene();

        Application.targetFrameRate = 200;

        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxSpeed;

        if (activeScene.buildIndex == 1)
        {
            LoadHighScore();
        }

        StartCoroutine(voices());
    }

    private void FixedUpdate()
    {
        rb.centerOfMass = COM;

        Vector3 camForward = cam.transform.forward;
        camForward = Vector3.ProjectOnPlane(camForward, Vector3.up).normalized;

        float move = Input.GetAxis("Vertical");
        float turning = Input.GetAxis("Horizontal");

        HandleMovement(move);
        HandleTurning(move, turning);
        HandleCameraFOV(move);

        if (Input.GetAxis("Vertical") < 0f)
        {
            Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (horizontalVel.magnitude > 1.45f)
            {
                Vector3 clamped = horizontalVel.normalized * 1.45f;
                rb.linearVelocity = new Vector3(clamped.x, rb.linearVelocity.y, clamped.z);
            }
        }
    }

    private void Update()
    {
        Debug.Log(hasJumped);
        HandleCoyoteTimer();
        HandleJumpInput();
        UpdateCubePosition();
        UpdateTimer();
        HandleChargeDrain();
        RotateCubeToVelocity();
        UpdateHighScoreCache();
        HandleRollingSound();

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 camDir = follow.transform.position - transform.position;
            float yaw = Mathf.Atan2(camDir.x, camDir.z) * Mathf.Rad2Deg + 180f;
            float pitch = Mathf.Clamp(Vector3.Angle(Vector3.up, camDir.normalized), 0f, 180f);
            CinemachineFreeLook freeLook = orbit.gameObject.GetComponent<CinemachineFreeLook>();
            freeLook.m_XAxis.Value = yaw;
            freeLook.m_YAxis.Value = pitch / 180f;
        }


        if (Input.GetMouseButton(1))
        {
            orbit.gameObject.GetComponent<CinemachineFreeLook>().Priority = 11;
            follow.Priority = 10;
            cam = orbit;
        }
        else
        {
            orbit.gameObject.GetComponent<CinemachineFreeLook>().Priority = 10;
            follow.Priority = 11;
            cam = follow;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 vel = Vector3.ProjectOnPlane(rb.linearVelocity, Vector3.up);

        if (vel.magnitude > 2.75f)
        {
            sourceCol.pitch = Random.Range(0.4f, 0.6f);
            sourceCol.Play();

            if (vel.magnitude > 3.2f && Time.time - lastVoice >= 2f)
            {
                int lineVal = Random.Range(0, voiceLinesHit.Length);
                source.PlayOneShot(voiceLinesHit[lineVal]);
                lastVoice = Time.time;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(cube.transform.position, cube.transform.position + cube.transform.forward * 2f);
        Vector3 vel = Vector3.ProjectOnPlane(rb != null ? rb.linearVelocity : Vector3.zero, Vector3.up);

        if (vel.magnitude > 0.01f)
        {
            Vector3 camDir = (cam.transform.position - cube.transform.position).normalized;

            float dot = Vector3.Dot(vel.normalized, camDir);
            Gizmos.color = (dot > 0) ? Color.green : Color.red;
            Gizmos.DrawLine(cube.transform.position, cube.transform.position + vel.normalized * 2f);
        }
    }

    #endregion

    #region Movement Logic

    private void HandleMovement(float move)
    {
        if (move != 0)
        {
            float uprightAmount = Vector3.Dot(transform.up, Vector3.up);

            if (uprightAmount > 0.7f)
            {
                if (move > 0)
                {
                    COM = Vector3.Lerp(COM, new Vector3(0, -0.3f, 0), 46f * Time.deltaTime); //wide FOV
                    rb.AddTorque(transform.forward * -rollTorque);
                    flipped = false;
                }
            }
            else
            {
                rb.AddTorque(transform.up * move * rollTorque, ForceMode.Acceleration);
                COM = Vector3.Lerp(COM, Vector3.zero, 12f * Time.deltaTime);
                rb.angularDamping = 4;
                flipped = true;
                cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, 53, 1.65f * Time.deltaTime);
            }
        }
        else if (rb.angularVelocity.magnitude < 1f)
        {
            COM = new Vector3(0, -1f, 0);
            rb.angularDamping = 2.25f;
        }
    }

    private void HandleTurning(float move, float turning)
    {
        if (turning != 0 && flipped && move != 0)
        {
            transform.Rotate(Vector3.up, turning * turnSpeed * Time.deltaTime, Space.World);
        }
    }

    private void HandleCameraFOV(float move)
    {
        if (move == 0)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, 41f, 2 * Time.deltaTime);
        }
    }

    #endregion

    #region Jump Logic

    private void HandleCoyoteTimer()
    {
        if (canJump)
        {
            coyoteTimer = coyote;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    private void HandleJumpInput()
    {
        if (Input.GetKey(KeyCode.Space) && coyoteTimer > 0f && !hasJumped)
        {
            if (jumpVel < 220f)
            {
                jumpVel++;
            }

            charge.fillAmount += 1.25f * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) && coyoteTimer > 0f & !hasJumped)
        {
            jump(1f);
            jumpVel = 85f;
        }
    }

    public void jump(float mult)
    {
        if (hasJumped) return;
        forwardDir = rb.linearVelocity.normalized;
        rb.AddForce(Vector3.up * jumpVel * mult, ForceMode.Impulse);
        rb.AddForce(forwardDir * jumpVelFor, ForceMode.Impulse);
        drain = true;
        sourceJump.pitch = Random.Range(0.80f, 1f);
        sourceJump.Play();
        coyoteTimer = 0f;
        canJump = false;
        hasJumped = true;
        StartCoroutine(ResetJump());
    }

    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(coyote);
        hasJumped = false;
    }

    #endregion

    #region Timer & Score

    private void UpdateTimer()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
        }

        int mins = Mathf.FloorToInt(timer / 60f);
        int secs = Mathf.FloorToInt(timer % 60f);
        timerText.text = $"{mins:00}:{secs:00}";
    }

    private void LoadHighScore()
    {
        Animation transAnim = GameObject.Find("IdolTransition").GetComponent<Animation>();
        AudioSource transSound = GameObject.Find("IdolTransition").GetComponent<AudioSource>();

        if (transAnim != null)
        {
            transAnim.Play();
            transSound.Play();
        }

        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetFloat("HighScore");
            highScoreText.text = "Highscore: " + FormatTime(highScore);
        }
        else
        {
            highScoreText.text = "Highscore: 00:00";
        }
    }

    private void UpdateHighScoreCache()
    {
        highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetFloat("HighScore") : Mathf.Infinity;
    }

    public void CheckScore()
    {
        if (!timerRunning) return;

        timerRunning = false;
        currentTimeText.text = FormatTime(timer);

        if (timer < highScore)
        {
            highScore = timer;
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
            highScoreText.text = "Highscore: " + FormatTime(highScore);
        }
    }

    private string FormatTime(float t)
    {
        int mins = Mathf.FloorToInt(t / 60f);
        int secs = Mathf.FloorToInt(t % 60f);
        return $"{mins:00}:{secs:00}";
    }

    #endregion

    #region Misc Logic

    private void UpdateCubePosition()
    {
        cube.transform.position = transform.position;
        if (activeScene.buildIndex == 1) part.transform.position = transform.position;
    }

    private void RotateCubeToVelocity()
    {
        Vector3 vel = Vector3.ProjectOnPlane(rb.linearVelocity, Vector3.up);
        float speed = vel.magnitude;

        if (speed < 0.01f) return;

        Vector3 velDir = vel.normalized;
        float dot = Vector3.Dot(cube.transform.forward, velDir);
        Quaternion targetRotation = dot > 0 ? Quaternion.LookRotation(velDir, Vector3.up) : Quaternion.LookRotation(-velDir, Vector3.up);
        cube.transform.rotation = Quaternion.Slerp(cube.transform.rotation, targetRotation, 10f * Time.deltaTime);

        float alignDot = Vector3.Dot(cube.transform.forward, velDir);
        Debug.DrawRay(cube.transform.position, cube.transform.forward * 3f, Color.blue);
        Debug.DrawRay(cube.transform.position, velDir * 3f, Color.red);

        if (alignDot < 0f && Input.GetAxis("Vertical") > 0f && speed > 1.6f && canJump) cube.transform.Rotate(Vector3.up, 180f, Space.World);
    }

    private void HandleChargeDrain()
    {
        if (!drain) return;

        charge.fillAmount -= 1.75f * Time.deltaTime;

        if (charge.fillAmount < 0.42f) drain = false;
    }

    private void HandleRollingSound()
    {
        if (activeScene.buildIndex != 1) return;

        float target = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) ? 0.075f : 0f;
        rolling.volume = Mathf.MoveTowards(rolling.volume, target, 0.12f * Time.deltaTime);
    }

    #endregion

    #region Voice Lines

    IEnumerator voices()
    {
        yield return new WaitForSeconds(20f);

        while (true)
        {
            if (activeScene.buildIndex != 1) yield break;

            Vector3 vel = Vector3.ProjectOnPlane(rb.linearVelocity, Vector3.up);
            bool isMoving = vel.magnitude > 2.75f;
            AudioClip[] cur = isMoving ? voiceLinesMove : voiceLinesIdle;

            if (!source.isPlaying && cur.Length > 0)
            {
                int lineVal = Random.Range(0, cur.Length);
                source.clip = cur[lineVal];
                source.Play();
            }

            yield return new WaitForSeconds(Random.Range(30f, 60f));
        }
    }

    #endregion
}