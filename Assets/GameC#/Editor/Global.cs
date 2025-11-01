using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
[System.Serializable]
public class Package
{
    public GameObject item;
    public Vector3 position;
    public GameObject place;
    public Vector3 position2;
    public GameObject SetBattery;
    public Vector3 SetBatteryPosition;

    public Package(GameObject item, Vector3 position, GameObject place, Vector3 position2,GameObject SetBattery,Vector3 SetBatteryPosition)
    {
        this.item = item;
        this.place = place;
        this.position = position;
        this.position2 = position2;
        this.SetBattery = SetBattery;
        this.SetBatteryPosition = SetBatteryPosition;
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
    public TextMeshProUGUI batteryText;

    public Vector3 spawnPosition = new Vector3(36, 0, 21);
    public static int phase = 0;
    public GameObject battery;
    public Package[] packages;
    private DroneController drone;
    public Slider meterSlider; // メーターUI
    public int currentValue = 100; // 現在の値
    public int maxValue = 150;    // 最大値
    public Outline sliderOutline;



    void Start()
    {
        packages = new Package[3];
        packages[0] = new Package(lugagge1, new Vector3(-10, 20, 5), Spot, new Vector3(10, -10, 5),battery,new Vector3(12, 10, 6));
        packages[1] = new Package(lugagge2, new Vector3(20, 20, -3), Spot, new Vector3(-67, -10, -219),battery,new Vector3(12, 10, 6));
        packages[2] = new Package(lugagge3, new Vector3(-5, 20, 8), Spot, new Vector3(251, -10, -91), battery, new Vector3(12, 10, 6));


        drone = GameObject.Find("drone 2").GetComponent<DroneController>();

        meterSlider.maxValue = maxValue;
        meterSlider.value = drone.RemainingBattery;


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
    void Update()
    {
        batteryText.text = Mathf.Ceil(drone.RemainingBattery).ToString() + "%";//切り上げｔテキストに表示
        meterSlider.value = drone.RemainingBattery;//メーター
        batteryText.text = Mathf.Ceil(drone.RemainingBattery).ToString() + "%";
        meterSlider.value = drone.RemainingBattery;

        // 色変更
        if (drone.RemainingBattery > 60)
        {
            meterSlider.fillRect.GetComponent<Image>().color = Color.green;
            sliderOutline.effectColor = new Color(0, 0, 0, 0); // 完全に透明（RGBA）
        }
        else if (drone.RemainingBattery > 30)
        {
            sliderOutline.effectColor = new Color(0, 0, 0, 0); // 完全に透明（RGBA）
            meterSlider.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            sliderOutline.effectColor = Color.red;
            meterSlider.fillRect.GetComponent<Image>().color = Color.red;
        }
           

        if (drone.RemainingBattery <= 0)
        {
            Debug.Log("バッテリー切れだよ")
;       }
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
        DeleteBattery();


        if (phase >= packages.Length)
        {
            Debug.Log("すべての物たちを配置済み");
            yield break;
        }

        GameObject prefab = packages[phase].item;
        GameObject prefab2 = packages[phase].place;
        GameObject prefab3 = packages[phase].SetBattery;

        if (prefab == null)
        {
            Debug.LogError($"packages[{phase}].item が null です");
            yield break;
        }

        // プレハブからインスタンスを生成
        GameObject obj = Instantiate(prefab, packages[phase].position, Quaternion.identity);
        GameObject spot = Instantiate(prefab2, packages[phase].position2, Quaternion.identity);
        GameObject battery =Instantiate(prefab3, packages[phase].SetBatteryPosition, Quaternion.identity);
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
        phase++;

    }

    public static void DeleteBattery()
    {
        foreach (GameObject DeleteBatteryObj in GameObject.FindGameObjectsWithTag("Battery"))
        {
            Destroy(DeleteBatteryObj);
        }
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
