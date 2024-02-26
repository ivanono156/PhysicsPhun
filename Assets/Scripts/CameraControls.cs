using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private GameObject bowlingBall;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Slider jumpBar;
    [Header ("Rolling")]
    [SerializeField] private float torqueMagnitude;
    [SerializeField] private float topAngularVelocity;
    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpAngle;
    [Header ("SizeChange")]
    [SerializeField] private float scaleChangeRate;
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    [Header ("Other")]
    [SerializeField] private float hardDropForce;

    private Vector3 torque;
    private float jumpMeter;
    private float massCoef;
    private Vector3 baseScale;
    private float scaleMult = 1;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        torque = Vector3.zero;
        // Note: Unity enforces a max angular velocity of 7 by default to all rigidbodies
        rb.maxAngularVelocity = topAngularVelocity;
        baseScale = bowlingBall.transform.localScale;
        massCoef = rb.mass / Mathf.Pow(bowlingBall.transform.localScale.x, 3);
    }

    void Update()
    {
        Quaternion q = bowlingBall.transform.rotation;
        transform.Rotate(new Vector3(0, 1, 0), Input.GetAxis("Mouse X") * sensitivity);
        bowlingBall.transform.rotation = q;
        DoRoll();
        DoJump();
        DoSizeChange();
        transform.position = bowlingBall.transform.position;
    }

    private void DoRoll()
    {
        Vector3 torqueDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            // add the unit vector corresponding to a forward rotation to torqueDir
            torqueDir += Vector3.zero;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // add the unit vector corresponding to a left rotation to torqueDir
            torqueDir += Vector3.zero;
        }
        if (Input.GetKey(KeyCode.S))
        {
            // add the unit vector corresponding to a backward rotation to torqueDir
            torqueDir += Vector3.zero;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // add the unit vector corresponding to a right rotation to torqueDir
            torqueDir += Vector3.zero;
        }
        torque = torqueDir.normalized * torqueMagnitude * SpeedCap() * rb.mass;
    }

    private void DoSizeChange()
    {
        if (Input.GetKey(KeyCode.E))
        {
            scaleMult = Mathf.Min(scaleMult + Time.deltaTime * scaleChangeRate, maxScale);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            scaleMult = Mathf.Max(scaleMult - Time.deltaTime * scaleChangeRate, minScale);
        }
        bowlingBall.transform.localScale = baseScale * scaleMult;
        rb.mass = Mathf.Pow(baseScale.x * scaleMult, 3) * massCoef;
    }

    private void DoJump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            jumpMeter = Mathf.Min(1f, jumpMeter + Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // Add an impulse force to the rigidbody in the direction of GetJumpVector
            // rb.AddForce(...);
            jumpMeter = 0f;
        }
        jumpBar.value = jumpMeter;
    }

    private Vector3 GetJumpVector()
    {
        // replace jumpDirection with the forward vector rotated up by [jumpAngle] degrees
        Vector3 jumpDirection = Vector3.up; // (Use Quaternion.AngleAxis())

        return jumpDirection * rb.mass * jumpMeter * jumpForce;
    }

    // Note that all non-impulse forces are handled in FixedUpdate
    private void FixedUpdate()
    {
        // Add the torque we calculate in Update to the rigidbody
        // Add torque on this line
        
        if (Falling())
        {
            rb.AddForce(Vector3.down * rb.mass * hardDropForce);
        }
    }

    private bool Falling()
    {
        // Check if our y velocity is less than zero
        return false;
    }

    //This function makes it much easier to change directions
    private float SpeedCap()
    {
        // use the dot product to figure out how how much of our angular velocity
        // is in the direction we are currently trying to rotate (the direction of torque)
        // and divide by out max angular velocity
        float fractionOfMax = 0;
        return Mathf.Pow(Mathf.Max(1 - fractionOfMax, 0), 3);
    }
}
