using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicalPins : MonoBehaviour
{
    [SerializeField] private Rigidbody myRb;
    [SerializeField] private float extraForce;
    [SerializeField] private float randomTorqueAmount;
    private void OnCollisionEnter(Collision collision)
    {
        //the rigidbody of the thing that hit us
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            AddFakeForce(rb.velocity * rb.mass);
            AddRandomTorque();
        }
    }

    private void AddFakeForce(Vector3 v)
    {
        // Apply an impulse force in the dircetion of v. the magnitute of the force
        // should be |v| * the mass * extraForce
        myRb.AddForce(v * myRb.mass * extraForce, ForceMode.Impulse);
    }

    private void AddRandomTorque()
    {
        float x = UnityEngine.Random.Range(0.1f, 1f);
        float y = UnityEngine.Random.Range(0.1f, .5f);
        float z = UnityEngine.Random.Range(0.1f, 1f);
        // Make a vector from the X, Y, and Z components, and add an impulse torque
        // in the direction of the random vector, with magnitude mass * randomTorqueAmount
        Vector3 v = new Vector3(x, y, z);
        myRb.AddTorque(v.normalized * randomTorqueAmount * myRb.mass, ForceMode.Impulse);
    }
}
