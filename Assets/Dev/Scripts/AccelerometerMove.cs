using UnityEngine;

public class AccelerometerMove : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10f;

    public float baseGravityScale = 1.0f;  
    public float maxGravityScale = 5.0f;   


    void Start()
    {
       
    }

    void Update()
    {
        if (SystemInfo.supportsAccelerometer)
        {
            Vector3 tilt = Input.acceleration;
            Vector2 tilt2D = new Vector2(tilt.x, tilt.y);
            float gravityScale = baseGravityScale + (tilt2D.magnitude * (maxGravityScale - baseGravityScale));
            rb.gravityScale = gravityScale;

            Vector3 dir = Vector3.zero;
            dir.x = -Input.acceleration.y;
            dir.z = Input.acceleration.x;

            if (dir.sqrMagnitude > 1)
                dir.Normalize();
            dir *= Time.deltaTime;
            transform.Translate(dir * speed);
        }    
    }

    void FixedUpdate()
    {
        
    }
}
