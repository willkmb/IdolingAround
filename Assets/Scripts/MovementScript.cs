using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MovementScript : MonoBehaviour
{
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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxSpeed;

    }

    private void FixedUpdate()
    {
        rb.centerOfMass = COM;

        float move = Input.GetAxis("Vertical");
        float turning = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.W)) flipDir = Random.Range(0, 2);

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
            if (move != 0) cube.transform.Rotate(Vector3.up, turning * turnSpeed * Time.deltaTime, Space.World);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            if(jumpVel < 200)
            {
                jumpVel++;
            }
            charge.fillAmount += 1.25f * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) && canJump)
        {
            jump();
            jumpVel = 125f;
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
    }
    void jump()
    {
        forwardDir = rb.linearVelocity.normalized;
        rb.AddForce(Vector3.up * jumpVel, ForceMode.Impulse);
        rb.AddForce(forwardDir * jumpVelFor, ForceMode.Impulse);
        drain = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) canJump = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) canJump = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 worldCOM = transform.TransformPoint(rb.centerOfMass);
        Gizmos.DrawSphere(worldCOM, 0.1f);
        Debug.DrawRay(transform.position, forwardDir * 3f, Color.green);
    }
}
