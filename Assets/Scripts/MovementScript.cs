using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class MovementScript : MonoBehaviour
{
    public Transform respawnPoint;

    public CinemachineVirtualCamera cam;
    public GameObject cube;
    public GameObject part;
    public Image charge;
    [SerializeField] float rollTorque = 20f;
    [SerializeField] float turnSpeed = 120f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] float jumpVel = 8f;
    [SerializeField] float jumpVelFor = 8f;
    [SerializeField] float coyote = 0.2f;
    private Vector3 COM = new Vector3(0, 0.5f, 0);
    private Vector3 forwardDir;
    private Rigidbody rb;
    private int flipDir;
    private bool flipped;
    private bool canJump;
    private bool drain;
    private float coyoteTimer;
    private bool pending = false;


    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timerText;
    private float timer;

    [Header("voiceLines")]
    [SerializeField] AudioClip[] voiceLines;
    [SerializeField] AudioSource source;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxSpeed;
        Animation transAnim = GameObject.Find("IdolTransition").GetComponent<Animation>();
        if (transAnim != null) transAnim.Play();
        StartCoroutine("voices");
    }

    private void FixedUpdate()
    {
        rb.centerOfMass = COM;

        float move = Input.GetAxis("Vertical");
        float turning = Input.GetAxis("Horizontal");

        if (move != 0)
        {
            float uprightAmount = Vector3.Dot(transform.up, Vector3.up);
            if (uprightAmount > 0.7f)
            {
                if (move > 0)
                {
                    Vector3 targetCOM = new Vector3(0, -0.3f, 0);
                    COM = Vector3.Lerp(COM, targetCOM, 50f * Time.deltaTime);
                    rb.AddTorque(transform.forward * -rollTorque);
                    flipped = false;
                }
            }
            else
            {
                rb.AddTorque(transform.up * move * rollTorque, ForceMode.Acceleration);

                Vector3 targetCOM = new Vector3(0, 0, 0);
                COM = Vector3.Lerp(COM, targetCOM, 12f * Time.deltaTime);
                rb.angularDamping = 4;
                flipped = true;

                cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, 40, 1.65f * Time.deltaTime);
            }
        }
        else if (Mathf.Abs(move) < 0.01f && rb.angularVelocity.magnitude < 1f)
        {
            COM = new Vector3(0, -1f, 0);
            rb.angularDamping = 2.25f;
        }

        if (move == 0) cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, 35.41f, 2 * Time.deltaTime);

        if (turning != 0 && flipped)
        {
            if (move != 0) transform.Rotate(Vector3.up, turning * turnSpeed * Time.deltaTime, Space.World);
        }
    }

    private void Update()
    {
        CubeChecks();
        if (canJump) coyoteTimer = coyote;
        else coyoteTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && coyoteTimer > 0f)
        {
            if (jumpVel < 195f)
            {
                jumpVel++;
            }
            charge.fillAmount += 1.25f * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) && coyoteTimer > 0f)
        {
            jump(1f);
            jumpVel = 85f;
        }

        cube.transform.position = transform.position;
        part.transform.position = transform.position;

        timer += Time.deltaTime;
        int mins = Mathf.FloorToInt(timer / 60f);
        int secs = Mathf.FloorToInt(timer % 60f);
        timerText.text = $"{mins:00}:{secs:00}";

        if (drain)
        {
            charge.fillAmount -= 1.75f * Time.deltaTime;
            if (charge.fillAmount < 0.42f) drain = false;
        }

        Vector3 vel = Vector3.ProjectOnPlane(rb.linearVelocity, Vector3.up); // gets velocity of the idol on the ground
        float speed = vel.magnitude;
        Vector3 velDir = vel.normalized;
        float dot = Vector3.Dot(cube.transform.forward, velDir); // dot product to check if player is moving forward relative to direction
        Quaternion targetRotation;
        if (dot > 0) targetRotation = Quaternion.LookRotation(velDir, transform.up); //sets rotation direction of forward
        else targetRotation = Quaternion.LookRotation(-velDir, transform.up); // sets rotation direction of backwards to be the opposite of forwards to prevent camera pivoting
        cube.transform.rotation = Quaternion.Slerp(cube.transform.rotation, targetRotation, 10f * Time.deltaTime); // smoothly adjust cubes rotation values
    }
    public void jump(float mult)
    {
        forwardDir = rb.linearVelocity.normalized;
        rb.AddForce(Vector3.up * jumpVel * mult, ForceMode.Impulse);
        rb.AddForce(forwardDir * jumpVelFor, ForceMode.Impulse);
        drain = true;
    }

    IEnumerator voices()
    {
        yield return new WaitForSeconds(20f);
        while (true)
        {
            int lineVal = Random.Range(0, voiceLines.Length);
            int waitTime = Random.Range(30, 60);
            AudioClip current = voiceLines[lineVal];
            source.PlayOneShot(current);

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) canJump = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) canJump = false;
    }

    void CubeChecks()
    {
        Vector3 vel = Vector3.ProjectOnPlane(rb.linearVelocity, Vector3.up);
        float move = Input.GetAxisRaw("Vertical");

        if (vel.magnitude > 0.01f && move > 0 && !Input.GetKey(KeyCode.S))
        {
            float dot = Vector3.Dot(cube.transform.forward.normalized, vel.normalized);

            if (dot > 0.7f)
            {
                Debug.Log("ForwardMatch");
            }
            else if (dot < -0.7f)
            {
                Debug.Log("Opposite!");
                
                pending = true;
            }
            else
            {
                Debug.Log("sideways");
            }
        }

        if(pending && vel.magnitude < 0.05f)
        {
            Debug.Log("Change Now!!!");
            Quaternion targetRotation = Quaternion.LookRotation(vel.normalized, Vector3.up);
            cube.transform.rotation = targetRotation;
            pending = false;
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
}
