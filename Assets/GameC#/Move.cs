using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
  public float moveSpeed = 50f;       // 前後移動速度
    public float rotationSpeed = 80f;  // 回転速度
    public float tiltAngle = 3f;      // 傾きの角度
    public float verticalSpeed = 3f;   // 上下移動の速度
    private Rigidbody rb;
    private List<string> LuggagesList = new List<string>();


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Vertical"); // W/Sキー（前後移動）
        float rotationInput = Input.GetAxis("Horizontal"); // A/Dキー（回転）
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.Space)) verticalInput = 1f;  // スペースキーで上昇
        if (Input.GetKey(KeyCode.LeftShift)) verticalInput = -1f; // シフトキーで下降

        // 前後移動
        Vector3 moveDirection = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + moveDirection);

        // 左右回転
        transform.Rotate(Vector3.up * rotationInput * rotationSpeed * Time.deltaTime);

        // 上下移動
        Vector3 verticalMovement = Vector3.up * verticalInput * verticalSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + verticalMovement);

        // 傾き（前後移動時のみ）
        float targetTilt = moveInput * tiltAngle;
        transform.rotation = Quaternion.Euler(targetTilt, transform.rotation.eulerAngles.y, 0);
    }


    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arch"))
        {
            Debug.Log("アーチをくぐった！");
            if (LuggagesList.Count >= 3)
            {
                Debug.Log("判定完了");
                Global.ChangeScene("Result");
            }

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