using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour
{
    public Transform respawnPoint;

    public CinemachineVirtualCamera cam;
    public GameObject cube;
    public Image charge;
    [SerializeField] float rollTorque = 20f;
    [SerializeField] float turnSpeed = 120f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] float jumpVel = 8f;
    [SerializeField] float jumpVelFor = 8f;
    private Vector3 COM = new Vector3 (0, 0.5f, 0);
    private Vector3 forwardDir;
    private Rigidbody rb;
    private int flipDir;
    private bool flipped;
    private bool canJump;
    private bool drain;

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
        if(transAnim != null ) transAnim.Play();
        StartCoroutine("voices");
    }

    private void FixedUpdate()
    {
        rb.centerOfMass = COM;

        float move = Input.GetAxis("Vertical");
        float turning = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.W)) { flipDir = Random.Range(0, 2);}

        if (move != 0)
        {
            float uprightAmount = Vector3.Dot(transform.up, Vector3.up);
            if (uprightAmount > 0.7f)
            {
                Vector3 targetCOM = new Vector3(0, -0.3f, 0);
                COM = Vector3.Lerp(COM, targetCOM, 50f * Time.deltaTime);
                if(flipDir == 0) rb.AddTorque(transform.forward * -move * rollTorque);
                else rb.AddTorque(-transform.forward * -move * rollTorque);
                flipped = false;
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
        else if(Mathf.Abs(move) < 0.01f && rb.angularVelocity.magnitude < 1f)
        {
            COM = new Vector3(0, -1f, 0);
            rb.angularDamping = 1.25f;
        }

        if(move == 0) cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, 35.41f, 2 * Time.deltaTime);

        if (turning != 0 && flipped)
        {
            if(move != 0) transform.Rotate(Vector3.up, turning * turnSpeed * Time.deltaTime, Space.World);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            if(jumpVel < 125f)
            {
                jumpVel++;
            }
            charge.fillAmount += 1.25f * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) && canJump)
        {
            jump(1f);
            jumpVel = 85f;
        }

        cube.transform.position = transform.position;

        timer += Time.deltaTime;
        int mins = Mathf.FloorToInt(timer / 60f);
        int secs = Mathf.FloorToInt(timer % 60f);
        timerText.text = $"{mins:00}:{secs:00}";

        if (drain)
        {
            charge.fillAmount -= 1.75f * Time.deltaTime;
            if(charge.fillAmount < 0.42f) drain = false;
        }

        Vector3 vel = Vector3.ProjectOnPlane(rb.linearVelocity, Vector3.up); // gets velocity of the idol on the ground
        float speed = vel.magnitude;
        if (speed > 0.1f) //check if moving
        {
            Vector3 velDir = vel.normalized;
            float dot = Vector3.Dot(cube.transform.forward, velDir); // dot product to check if player is moving forward relative to direction
            Quaternion targetRotation;
            if (dot > 0) targetRotation = Quaternion.LookRotation(velDir, transform.up); //sets rotation direction of forward
            else targetRotation = Quaternion.LookRotation(-velDir, transform.up); // sets rotation direction of backwards to be the opposite of forwards to prevent camera pivoting
            cube.transform.rotation = Quaternion.Slerp(cube.transform.rotation, targetRotation, 10f * Time.deltaTime); // smoothly adjust cubes rotation values
        }
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
}
