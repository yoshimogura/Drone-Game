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
    public PropellerController controller1;
    public PropellerController controller2;
    public PropellerController controller3;
    public PropellerController controller4;
    bool wasMoving = false;
    float transitionTimer = 0f;
    public float takeOffDuration = 1.5f; // ゆっくり加速する時間


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
       float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");
    Vector3 moveDirection = transform.forward * vertical;
    rb.velocity = moveDirection * speed;

    bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

    // 状態管理
    if (isMoving && !wasMoving)
    {
        // 移動開始（TakeOffへ）
        transitionTimer = 0f;
        controller1.SetState(PropellerController.State.TakingOff);
        controller2.SetState(PropellerController.State.TakingOff);
        controller3.SetState(PropellerController.State.TakingOff);
        controller4.SetState(PropellerController.State.TakingOff);
    }

    if (isMoving)
    {
        transitionTimer += Time.deltaTime;
        if (transitionTimer > takeOffDuration)
        {
            // 規定時間経過後、Flyingへ
            controller1.SetState(PropellerController.State.Flying);
            controller2.SetState(PropellerController.State.Flying);
            controller3.SetState(PropellerController.State.Flying);
            controller4.SetState(PropellerController.State.Flying);
        }
    }

    if (!isMoving && wasMoving)
    {
        controller1.SetState(PropellerController.State.Landing);
        controller2.SetState(PropellerController.State.Landing);
        controller3.SetState(PropellerController.State.Landing);
        controller4.SetState(PropellerController.State.Landing);
    }

    // プロペラが完全停止してからIdleへ
    if (!isMoving && controller1.currentSpeed < 10f)
    {
        controller1.SetState(PropellerController.State.Idle);
        controller2.SetState(PropellerController.State.Idle);
        controller3.SetState(PropellerController.State.Idle);
        controller4.SetState(PropellerController.State.Idle);
    }


    wasMoving = isMoving;

        

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