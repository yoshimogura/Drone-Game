using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject buildingPrefab1; // Unityエディタからビル1のPrefabを割り当ててください
    public GameObject buildingPrefab2;
    GameObject PutBuilding;
    Vector3 buildingSize;

    // Start is called before the first frame update
    void Start()
    {
        if (buildingPrefab1 == null)
        {
            Debug.LogError("buildingPrefabが割り当てられていません。Inspectorから設定してください。");
            return;
        }
        
        for (int i = 0; i < 5; i++)
        {
            if (Random.value < 0.5f)
            {
                PutBuilding = buildingPrefab1;
                buildingSize = new Vector3(10f, 25f, 10f);
            }
            else
            {
                PutBuilding = buildingPrefab2;
                buildingSize = new Vector3(3f,3f,3f);
            }           

            float xPosition = (-2 + i) * 50f;
            Vector3 position = new Vector3(xPosition, 25, 30);
            GameObject building = Instantiate(PutBuilding, position, Quaternion.identity);
            building.transform.localScale = buildingSize; //大きs調整
            RandomizeBuildingColor(building);



        }

        // 残りの5つをZ=-30ラインに生成
        for (int i = 0; i < 5; i++)
        {
            if (Random.value < 0.5f)
            {
                PutBuilding = buildingPrefab1;
                buildingSize = new Vector3(10f, 25f, 10f);
            }
            else
            {
                PutBuilding = buildingPrefab2;
                buildingSize = new Vector3(3f, 3f, 3f);
            }         
            float xPosition = (-2 + i) * 50f;
            Vector3 position = new Vector3(xPosition, 25, -30);
            GameObject building = Instantiate(PutBuilding, position, Quaternion.identity);
            building.transform.localScale =buildingSize; //大きさ調整
            RandomizeBuildingColor(building);
        }
        
    }

    void RandomizeBuildingColor(GameObject building)
{
    // Renderer renderer = building.GetComponentInChildren<Renderer>();
    // if (renderer != null)
    // {
    //     MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
    //     renderer.GetPropertyBlock(propBlock);

    //     Color baseColor = renderer.sharedMaterial.color; // ← sharedMaterial に変更
    //     float h, s, v;
    //     Color.RGBToHSV(baseColor, out h, out s, out v);
    //     v *= Random.Range(0.7f, 1.3f);

    //     propBlock.SetColor("_Color", Color.HSVToRGB(h, s, v));
    //     renderer.SetPropertyBlock(propBlock);
    // }
}


    // Update is called once per frame
    void Update()
    {

    }
}