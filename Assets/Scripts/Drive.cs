using UnityEngine;
using UnityEngine.InputSystem;

public class Drive : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public float torque = 200;
    public float maxSteerAngle = 30;
    public float maxBrakeTorque = 500f;
    public GameObject[] wheels;
    public AudioSource skidSound;
    public Transform skidTrailPrefab;
    Transform[] skidTrails = new Transform[4];

    public void StartSkidTrail(int i)
    {
        if (skidTrails[i] == null)
        {
            skidTrails[i] = Instantiate(skidTrailPrefab);
        }
        skidTrails[i].parent = wheelColliders[i].transform;
        skidTrails[i].localPosition = -Vector3.up * wheelColliders[i].radius;
    }

    public void EndSkidTrail(int i)
    {
        if (skidTrails[i] == null)
        {
            return;
        }
        Transform holder = skidTrails[i];
        skidTrails[i] = null;
        holder.parent = null;
        Destroy(holder.gameObject, 30f);
    }

    void Go(float accel, float steer, float brake)
    {
        //Setting de aceleración, frenado y giro
        accel = Mathf.Clamp(accel, -1f, 1f);
        steer = Mathf.Clamp(steer, -1f, 1f) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0f, 1f) * maxBrakeTorque;
        float thrustTorque = accel * torque;

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = thrustTorque;

            if (i < 2)
            {
                wheelColliders[i].steerAngle = steer;
            }
            else
            {
                wheelColliders[i].brakeTorque = brake;
            }

            //Setting de rotación de llantas
            Quaternion quat;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out quat);
            wheels[i].transform.position = position;
            wheels[i].transform.rotation = quat;
        }
    }

    void CheckForSkid()
    {
        int numSkidding = 0;
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelHit;
            wheelColliders[i].GetGroundHit(out wheelHit);
            if (Mathf.Abs(wheelHit.forwardSlip) >= 0.4f || Mathf.Abs(wheelHit.sidewaysSlip) >= 0.4f)
            {
                numSkidding++;
                if (!skidSound.isPlaying)
                {
                    skidSound.Play();
                }
                StartSkidTrail(i);
            }
            else
            {
                EndSkidTrail(i);
            }
        }
        if (numSkidding == 0 && skidSound.isPlaying)
        {
            skidSound.Stop();
        }
    }    

    // Update is called once per frame
    void Update()
    {
        float a = 0f;
        float s = 0f;
        float b = 0f;

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

        //Frenado
        if (Keyboard.current.spaceKey.isPressed)
        {
            b += 1f;
        }

        Go(a, s, b);

        CheckForSkid();
    }
}
