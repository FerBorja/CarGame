using UnityEngine;
using UnityEngine.InputSystem;

public class Drive : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public float torque = 200;
    public float maxSteerAngle = 30;
    public GameObject[] wheels;

    void Go(float accel, float steer)
    {
        //Setting de torque y giro
        accel = Mathf.Clamp(accel, -1f, 1f);
        steer = Mathf.Clamp(steer, -1f, 1f) * maxSteerAngle;
        float thrustTorque = accel * torque;

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = thrustTorque;

            if (i < 2)
            {
                wheelColliders[i].steerAngle = steer;
            }

            //Setting de rotación de llantas
            Quaternion quat;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out quat);
            wheels[i].transform.position = position;
            wheels[i].transform.rotation = quat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float a = 0f;
        float s = 0f;

        //Adelante
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            a += 1f;
        }

        //Atras
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            a -= 1f;
        }

        //Izquierda
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            s -= 1f;
        }

        //Derecha
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            s += 1f;
        }

        Go(a, s);
    }
}
