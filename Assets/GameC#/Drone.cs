using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.ProBuilder;

public class DroneController : MonoBehaviour
{
    float rotationSpeed = 55f;
    float speed = 40f;
    public float tiltAngle = 5f; // 最大傾斜角
    public float tiltSmooth = 100f; // 傾きの補間速度
    private Rigidbody rb;
    
    private List<string> LuggagesList = new List<string>();
    private List<string> CollectedLuggagesList = new List<string>();

    public PropellerController controller1;
    public PropellerController controller2;
    public PropellerController controller3;
    public PropellerController controller4;
    bool wasMoving = false;
    float transitionTimer = 0f;
    public float takeOffDuration = 1.5f; // ゆっくり加速する時間
    public Transform cargoAttachPoint; //ドローンの荷物つける場所
    private Global globalScript;
    public  float RemainingBattery = 100;
    

    

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject globalObj = GameObject.Find("Global");
        if (globalObj != null)
        {
            globalScript = globalObj.GetComponent<Global>();
        }
        else
        {
            Debug.LogError("Global オブジェクトが見つかりません");
        }

    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * vertical;
        rb.velocity = moveDirection * speed;

        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f || Input.GetKey(KeyCode.Space) ||
        Input.GetKey(KeyCode.LeftShift);
        if (this.transform.position.y > -6.8){
            RemainingBattery -= 0.008f;
        }
        else
        {
            controller1.SetState(PropellerController.State.Idle);
            controller2.SetState(PropellerController.State.Idle);
            controller3.SetState(PropellerController.State.Idle);
            controller4.SetState(PropellerController.State.Idle);
        }

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
        if(RemainingBattery>0){
            RemainingBattery -= 0.01f;
        }
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
        Debug.Log("当たった");
        if (other.gameObject.CompareTag("luggage"))
        {
            string boxName = other.gameObject.name;
            Debug.Log("取得したもの: " + boxName);

            // すでにリストに入ってなければ追加する
            if (LuggagesList.Count < 1)
            {
                if (!LuggagesList.Contains(boxName) && !CollectedLuggagesList.Contains(boxName))
                {
                    LuggagesList.Add(boxName);
                    Luggage luggage = other.gameObject.GetComponent<Luggage>();
                    if (luggage != null)
                    {
                        luggage.GetLugggage(boxName);
                    }
                    Global player = GameObject.Find("Global").GetComponent<Global>();
                    player.audioSource.Play();



                    // 荷物をドローンの下にくっつける処理

                    other.transform.position = cargoAttachPoint.position;
                    other.transform.rotation = cargoAttachPoint.rotation;
                    other.transform.SetParent(cargoAttachPoint);
                    Dictionary<string, Vector3> scaleMap = new Dictionary<string, Vector3>()
                    {
                        { "New荷物(Clone)", new Vector3(0.07f, 0.18f, 0.5f) },
                        { "New財布(Clone)", new Vector3(0.4f, 0.09f, 0.25f) },
                        { "Newスマホ(Clone)", new Vector3(0.4f, 0.09f, 0.25f) }
                    };
                    if (scaleMap.ContainsKey(boxName))
                    {
                        other.transform.localScale = scaleMap[boxName];
                    }


                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true; // 物理挙動を止める（ぶら下げるだけなら）
                    }
                    foreach (Transform child in other.GetComponentsInChildren<Transform>(true))
                    {
                        if (child.CompareTag("LuggageEffect"))
                        {
                            Destroy(child.gameObject);
                        }
                    }
                    


                }
            }

        }
        if (globalScript != null && LuggagesList.Count > 0)
        {
            
            Package currentPackage = globalScript.packages[Global.phase - 1]; 
            if (other.gameObject.CompareTag("Spot"))
            {
                Debug.Log("配達場所来た");

                if (LuggagesList.Count > 0)
                {
                    string targetName = LuggagesList[0]; // 先頭の荷物
                    GameObject cargo = GameObject.Find(targetName);
                    if (cargo != null)
                    {
                        cargo.transform.position = other.transform.position + new Vector3(1, 5f, 2); // 位置調整
                        cargo.transform.SetParent(null); // ドローンから切り離す
                        Rigidbody rb = cargo.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = false;
                        }
                        Dictionary<string, Vector3> scaleMap = new Dictionary<string, Vector3>
                    {
                        { "New荷物(Clone)", new Vector3(0.14f, 0.36f, 1f) },
                        { "New財布(Clone)",   new Vector3(0.72f, 0.05f, 0.6f) },
                        { "Newスマホ(Clone)", new Vector3(1f, 1f, 1f) }
                    };
                        if (scaleMap.ContainsKey(targetName))
                        {
                            cargo.transform.localScale = scaleMap[targetName];
                        }



                        LuggagesList.Remove(targetName);
                        CollectedLuggagesList.Add(targetName);
                        Debug.Log("荷物を置いた: " + targetName);
                        globalScript.SpawnNextPackageDelayed(3f);
                    }
                }
            }

            
           

        }
        if (other.gameObject.CompareTag("Battery"))
        {
            if (RemainingBattery <= 50)
                RemainingBattery += 50;
            else
                RemainingBattery = 100;
                Global.DeleteBattery();
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