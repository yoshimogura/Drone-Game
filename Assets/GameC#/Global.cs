using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Package
{
    public GameObject item;
    public Vector3 position;
    public GameObject place;

    public Package(GameObject item, Vector3 position, GameObject place)
    {
        this.item = item;
        this.place = place;
        this.position = position;
    }
}



public class Global : MonoBehaviour
{
    // Start is called before the first frame update


    public AudioSource audioSource;
    public GameObject targetParent;
    public GameObject lugagge1;
    public GameObject lugagge2;
    public GameObject lugagge3;
    public GameObject Spot1;
    public GameObject Spot2;
    public GameObject Spot3;

    public Vector3 spawnPosition = new Vector3(36, 0, 21);
    public static int fase = 0;

    public Package[] packages;

    void Start()
    {
        packages = new Package[3];
        packages[0] = new Package(lugagge1, new Vector3(10, 20, 5),Spot1);
        packages[1] = new Package(lugagge2, new Vector3(20, 20, -3),Spot2);
        packages[2] = new Package(lugagge3, new Vector3(-5, 20, 8),Spot3);

        audioSource = GetComponent<AudioSource>();

        foreach (Transform child in targetParent.transform)
        {
            if (child.GetComponent<MeshFilter>() != null)
            {
                MeshCollider collider = child.gameObject.AddComponent<MeshCollider>();
                collider.convex = false;
            }
        }

        SpawnNextPackage(); // 最初の1つを配置
    }

    public void SpawnNextPackage()
    {
        if (fase >= packages.Length)
        {
            Debug.Log("すべての荷物を配置済み");
            return;
        }

        GameObject prefab = packages[fase].item;

        if (prefab == null)
        {
            Debug.LogError($"packages[{fase}].item が null です");
            return;
        }

        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab, packages[fase].position, Quaternion.identity);
        
        // RigidbodyとBoxColliderを追加
        if (obj.GetComponent<Rigidbody>() == null)
            obj.AddComponent<Rigidbody>();
        if (obj.GetComponent<BoxCollider>() == null)
            obj.AddComponent<BoxCollider>();
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        BoxCollider boxCollider = obj.GetComponent<BoxCollider>();

        rb.useGravity = false; // ← ここで重力をオフにする
        rb.isKinematic = true;
        boxCollider.isTrigger = true;
        obj.tag = "luggage";

        fase++;

    }

    public static void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void Sound()
    {
        audioSource.PlayOneShot(audioSource.clip);

    }

}
