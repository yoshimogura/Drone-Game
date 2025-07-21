using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    float rotationSpeed = 50f;
    float speed = 30f;
    public float tiltAngle = 5f; // 最大傾斜角
    public float tiltSmooth = 100f; // 傾きの補間速度
    private Rigidbody rb;
    
    private List<string> LuggagesList = new List<string>();
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A,D（左右回転）
        float vertical = Input.GetAxis("Vertical");   // W,S（前後移動）

        // **W/Sで前後移動**
        Vector3 moveDirection = transform.forward * vertical;
        rb.velocity = moveDirection * speed;

        // **A/Dで左右回転**
        transform.Rotate(Vector3.up * horizontal * rotationSpeed * Time.deltaTime);


        if (Input.GetKey(KeyCode.Space))
        {
            this.transform.position += new Vector3(0, 0.1f, 0);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            this.transform.position += new Vector3(0, -0.1f, 0);
        }
        // **傾きを計算**
        float targetTiltX = Mathf.Clamp(vertical * tiltAngle, -tiltAngle, tiltAngle);
        float targetTiltZ = Mathf.Clamp(-horizontal * tiltAngle, -tiltAngle, tiltAngle);

        // **傾きの補間を適用**
        Quaternion targetRotation = Quaternion.Euler(targetTiltX, transform.eulerAngles.y, targetTiltZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSmooth);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arch"))
        {
            Debug.Log("アーチをくぐった！");
        }
        if (other.gameObject.CompareTag("luggage"))
        {
            string boxName = other.gameObject.name;
            Debug.Log("触れた箱: " + boxName);

            // すでにリストに入ってなければ追加する
            if (!LuggagesList.Contains(boxName))
            {
                LuggagesList.Add(boxName);
                Debug.Log("触れた箱: " + boxName);
                Luggage luggage = other.gameObject.GetComponent<Luggage>();
                if (luggage != null)
                {
                    luggage.GetLugggage(boxName);
                }

            }

        }
        if (other.gameObject.name == "Spot")
        {
            string targetName = "荷物";
            LuggagesList.RemoveAll(obj => obj == targetName);
            foreach (string name in LuggagesList)
                {
                    Debug.Log(name);
                }

            
           

        }

    
    }
    void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        rb.angularVelocity = Vector3.zero; // 接地時に回転の揺れを抑える
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // 地面にぶつかったときの縦方向の速度をゼロにする
    }
}

}
// float horizontal = Input.GetAxis("Horizontal"); // A,D
//         float vertical = Input.GetAxis("Vertical"); // W,S

//         // **移動処理：現在の向きに基づく**
//         Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
//         rb.velocity = moveDirection * speed;

//         if (Input.GetKey(KeyCode.D))
//         {
//             transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); 
//         }
//         if (Input.GetKey(KeyCode.A))
//         {
//             transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime); 
//         }
//         if (Input.GetKey(KeyCode.W))
//         {
//             transform.Rotate(Vector3.right * -rotationSpeed * Time.deltaTime); 
//         }
//         if (Input.GetKey(KeyCode.S))
//         {
//             transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime); 
        // }