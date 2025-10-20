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
    public Vector3 position2;

    public Package(GameObject item, Vector3 position, GameObject place, Vector3 position2) 
    {
        this.item = item;
        this.place = place;
        this.position = position;
        this.position2 = position2;
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
    public GameObject Spot;

    public Vector3 spawnPosition = new Vector3(36, 0, 21);
    public static int fase = 0;
    public Package[] packages;

    void Start()
    {
        packages = new Package[3];
        packages[0] = new Package(lugagge1, new Vector3(10, 20, 5),Spot,new Vector3(10, -10, 5));
        packages[1] = new Package(lugagge2, new Vector3(20, 20, -3),Spot,new Vector3(-67, -10, -219));
        packages[2] = new Package(lugagge3, new Vector3(-5, 20, 8),Spot,new Vector3(251, -10, -91));

        audioSource = GetComponent<AudioSource>();

        foreach (Transform child in targetParent.transform)
        {
            if (child.GetComponent<MeshFilter>() != null)
            {
                MeshCollider collider = child.gameObject.AddComponent<MeshCollider>();
                collider.convex = false;
            }
        }

        StartCoroutine(SpawnNextPackage(0f)); // 最初の1つを配置
    }
    public void SpawnNextPackageDelayed(float delaySeconds)
    {
        StartCoroutine(SpawnNextPackage(delaySeconds));
    }

    public IEnumerator  SpawnNextPackage(float delay)

    {
        yield return new WaitForSeconds(delay);

       foreach (GameObject DeleteObj in GameObject.FindGameObjectsWithTag("luggage"))
        {
            Destroy(DeleteObj);
        }

        foreach (GameObject DeleteSpotObj in GameObject.FindGameObjectsWithTag("Spot"))
        {
            Destroy(DeleteSpotObj);
        }


        if (fase >= packages.Length)
        {
            Debug.Log("すべての荷物と配達地点を配置済み");
            yield break;
        }

        GameObject prefab = packages[fase].item;
        GameObject prefab2 = packages[fase].place;

        if (prefab == null)
        {
            Debug.LogError($"packages[{fase}].item が null です");
            yield break;
        }

        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab, packages[fase].position, Quaternion.identity);
        GameObject spot =Instantiate(prefab2, packages[fase].position2, Quaternion.identity);
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
        spot.tag = "Spot";

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
