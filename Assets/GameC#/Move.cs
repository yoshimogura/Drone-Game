using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
            float rotationSpeed = 50f;
    float speed = 15f;
        void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal"); // A,D
        float vertical = Input.GetAxis("Vertical"); // W,Sキ

        // 向いている方向を基準に移動する
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;

        // 移動処理
        transform.position += moveDirection * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            this.transform.position += new Vector3(0, 0.1f, 0);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            this.transform.position += new Vector3(0, -0.1f, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.right * -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {

            transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        }
    


    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arch"))
        {
            Debug.Log("アーチをくぐった！");
            // ここにスコア加算、エフェクト再生などの処理を追加
        }
    }
 
 
 }

