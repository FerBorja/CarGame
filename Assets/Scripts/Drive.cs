using UnityEngine;
using UnityEngine.InputSystem;

public class Drive : MonoBehaviour
{
    public WheelCollider WC;
    public float torque = 200;
    public GameObject wheel;
    void Start()
    {
        if (WC == null)
        {
            WC = GetComponent<WheelCollider>();
        }
    }

    void Go(float accel)
    {
        //Setting de torque
        accel = Mathf.Clamp(accel, -1f, 1f);
        float thrustTorque = accel * torque;
        WC.motorTorque = thrustTorque;

        //Setting de rotación de llantas
        Quaternion quat;
        Vector3 position;
        WC.GetWorldPose(out position, out quat);
        wheel.transform.position = position;
        wheel.transform.rotation = quat;
    }

    // Update is called once per frame
    void Update()
    {
        float a = 0f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            a += 1f;
        }

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            a -= 1f;
        }

        Go(a);
    }
}
