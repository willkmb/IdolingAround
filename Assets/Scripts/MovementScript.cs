using Cinemachine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

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
    public Image chargeHolder;
    public Image chargeBlur;
    public bool isSpawning;

    [Header("Movement Settings")]
    [SerializeField] float rollTorque = 20f;
    [SerializeField] float turnSpeed = 120f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] float jumpVel = 8f;
    [SerializeField] float jumpVelFor = 8f;
    [SerializeField] float coyote = 0.2f;
    [SerializeField] float inAirControl = 5f;
    [SerializeField] float inAirControlSide = 5f;

    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timerText;


    [Header("Jump Meter Colors")]
    [SerializeField] Color minChargeColor;
    [SerializeField] Color maxChargeColor;
    [SerializeField] Color holderMinChargeColor;
    [SerializeField] Color holderMaxChargeColor;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI currentTimeText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI gemText;
    [SerializeField] TextMeshProUGUI deathText;

    [Header("Voice Lines")]
    [SerializeField] AudioClip[] voiceLinesMove;
    [SerializeField] AudioClip[] voiceLinesIdle;
    [SerializeField] AudioClip[] voiceLinesHit;
    [SerializeField] AudioClip colClip;
    [SerializeField] AudioSource source;
    public AudioSource sourceJump;
    [SerializeField] AudioSource sourceCol;
    [SerializeField] AudioSource rolling;

    [Header("Decals")]
    [SerializeField] Material[] grassMats;
    [SerializeField] Material[] mudMats;
    [SerializeField] ParticleSystem grassPart;
    [SerializeField] ParticleSystem mudPart;

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
    private float collisionCooldown = 0f;
    private Vector3 storedFlipDirection = Vector3.zero;
    private float flipDirTimer = 0f;
    private bool stoodUp = true;
    private bool onMud = false;

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

        if (Input.GetAxis("Vertical") < 0f && !onMud)
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
        HandleCoyoteTimer();
        HandleJumpInput();
        UpdateCubePosition();
        UpdateTimer();
        HandleChargeDrain();
        RotateCubeToVelocity();
        UpdateHighScoreCache();
        HandleRollingSound();
        CheckDecals();

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

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("MenuScene");
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && rb.linearVelocity.magnitude < 0.25f)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            transform.position = respawnPoint.position;
            Debug.Log("resetting checkpoint");
            GameObject.Find("CameraTarget").transform.rotation = Quaternion.identity;
            rb.isKinematic = false;
        }

        stoodUp = Vector3.Dot(transform.up, Vector3.up) > 0.9f;
        if (collisionCooldown > 0f) collisionCooldown -= Time.deltaTime;
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
        Vector3 normal = collision.contacts[0].normal;
        bool isWall = Vector3.Dot(normal, Vector3.up) < 0.5f;
        if (isWall) collisionCooldown = 1.5f;
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.8f, 3.5f, 0.8f));
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
                    COM = Vector3.Lerp(COM, new Vector3(0, -0.3f, 0), 46f * Time.deltaTime);
                    flipDirTimer -= Time.deltaTime;

                    if (flipDirTimer <= 0f)
                    {
                        float closestDistance = Mathf.Infinity;
                        float secondClosestDistance = Mathf.Infinity;
                        Vector3 closestDirection = Vector3.zero;

                        for (int i = 0; i < 8; i++)
                        {
                            float angle = i * 45f;
                            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 15f))
                            {
                                if (hit.distance < closestDistance)
                                {
                                    secondClosestDistance = closestDistance;
                                    closestDistance = hit.distance;
                                    closestDirection = direction;
                                }
                                else if (hit.distance < secondClosestDistance) secondClosestDistance = hit.distance;
                            }
                        }

                        if ((secondClosestDistance - closestDistance) < 1.5f || closestDirection == Vector3.zero)
                        {
                            storedFlipDirection = transform.right;
                            flipDirTimer = 0.5f;
                        }
                        else
                        {
                            storedFlipDirection = closestDirection;
                            flipDirTimer = 0.2f;
                        }
                    }

                    Vector3 torqueAxis = Vector3.Cross(Vector3.up, storedFlipDirection).normalized;
                    rb.AddTorque(torqueAxis * rollTorque);
                    flipped = false;
                }
            }
            else
            {
                rb.AddTorque(transform.up * move * rollTorque, ForceMode.Acceleration);
                COM = Vector3.Lerp(COM, Vector3.zero, 12f * Time.deltaTime);
                rb.angularDamping = 4;
                flipped = true;
                cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, 63, 0.55f * Time.deltaTime);
            }

            if (!canJump)
            {
                Vector3 airForward = Vector3.ProjectOnPlane(cube.transform.forward, Vector3.up).normalized;
                Vector3 airSideways = Vector3.ProjectOnPlane(cube.transform.right, Vector3.up).normalized;
                rb.AddForce(airForward * move * inAirControl, ForceMode.Acceleration);
                rb.AddForce(airSideways * Input.GetAxis("Horizontal") * inAirControlSide, ForceMode.Acceleration);
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
            cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, 41f, 1.15f * Time.deltaTime);
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
                jumpVel += 100f * Time.deltaTime;
            }

            charge.fillAmount += 1.25f * Time.deltaTime;
            float third = Mathf.InverseLerp(0.33f, 1f, charge.fillAmount);
            charge.color = Color.Lerp(minChargeColor, maxChargeColor, third);
            chargeHolder.color = Color.Lerp(holderMinChargeColor, holderMaxChargeColor, charge.fillAmount);
            chargeBlur.color = Color.Lerp(holderMinChargeColor, holderMaxChargeColor, charge.fillAmount);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if(coyoteTimer > 0f && !hasJumped) jump(1f);
            drain = true;
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
        gemText.text = this.GetComponent<GemCounter>().gems.ToString();
        deathText.text = this.GetComponent<DeathCounter>().deaths.ToString();

        if (timer < highScore)
        {
            highScore = timer;
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
            highScoreText.text = "Highscore: " + FormatTime(highScore);
        }
        else
        {
            highScore = PlayerPrefs.GetFloat("HighScore");
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
        if (stoodUp)
        {
            Quaternion target = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
            cube.transform.rotation = Quaternion.Slerp(cube.transform.rotation, target, 15f * Time.deltaTime);
            return;
        }

        Vector3 vel = Vector3.ProjectOnPlane(rb.linearVelocity, Vector3.up);
        float speed = vel.magnitude;

        if (speed < 0.01f) return;

        Vector3 velDir = vel.normalized;
        float dot = Vector3.Dot(cube.transform.forward, velDir);
        Quaternion targetRotation = dot > 0? Quaternion.LookRotation(velDir, Vector3.up): Quaternion.LookRotation(-velDir, Vector3.up);
        cube.transform.rotation = Quaternion.Slerp(cube.transform.rotation, targetRotation, 10f * Time.deltaTime);

        float alignDot = Vector3.Dot(cube.transform.forward, velDir);
        Debug.DrawRay(cube.transform.position, cube.transform.forward * 3f, Color.blue);
        Debug.DrawRay(cube.transform.position, velDir * 3f, Color.red);

        if (alignDot < 0f && Input.GetAxis("Vertical") > 0f && speed > 1.6f && collisionCooldown <= 0f)cube.transform.Rotate(Vector3.up, 180f, Space.World);
    }

    private void HandleChargeDrain()
    {
        if (!drain) return;
        charge.fillAmount -= 1.75f * Time.deltaTime;
        charge.color = Color.Lerp(minChargeColor, maxChargeColor, charge.fillAmount);
        chargeHolder.color = Color.Lerp(holderMinChargeColor, holderMaxChargeColor, charge.fillAmount);
        chargeBlur.color = Color.Lerp(holderMinChargeColor, holderMaxChargeColor, charge.fillAmount);
        if (charge.fillAmount <= 0) drain = false;
    }

    private void HandleRollingSound()
    {
        if (activeScene.buildIndex != 1) return;

        float target = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) ? 0.075f : 0f;
        rolling.volume = Mathf.MoveTowards(rolling.volume, target, 0.12f * Time.deltaTime);
    }

    private void CheckDecals()
    {
        if (!canJump || stoodUp)
        {
            grassPart.Stop();
            mudPart.Stop();
            onMud = false;
            return;
        }
        Collider[] colliders = Physics.OverlapBox(transform.position,new Vector3(0.8f, 3.5f, 0.8f),Quaternion.identity,~0,QueryTriggerInteraction.Collide);

        foreach (Collider col in colliders)
        {
            List<Material> curMats = new List<Material>();
            UnityEngine.Rendering.Universal.DecalProjector decal = col.GetComponent<UnityEngine.Rendering.Universal.DecalProjector>();
            if(decal != null) curMats.Add(decal.material);
            Renderer rend = col.GetComponent<Renderer>();
            if (rend != null) curMats.AddRange(rend.sharedMaterials);

            foreach(Material mat in curMats)
            {
                foreach (Material grass in grassMats)
                {
                    if(mat == grass)
                    {
                        if (!grassPart.isPlaying) grassPart.Play();
                        if (mudPart.isPlaying) mudPart.Stop();
                        return;
                    }
                }
                foreach(Material mud in mudMats)
                {
                    if(mat == mud)
                    {
                        if (!mudPart.isPlaying) mudPart.Play();
                        if (grassPart.isPlaying) grassPart.Stop();
                        onMud = true;
                        return;
                    }
                }
            }
        }

        grassPart.Stop();
        mudPart.Stop();
        onMud = false;
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