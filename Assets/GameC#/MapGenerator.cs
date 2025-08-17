using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject buildingPrefab1;
    public GameObject buildingPrefab2;
    public GameObject buildingPrefab3;
    public GameObject streetLight;

    GameObject PutBuilding;
    Vector3 buildingSize;
    float yPosition;
    float zPosition;
    float Randomcount;
    
        


    // Start is called before the first frame update



    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            if (i == 1)
            {
                zPosition = 30;
            }
            else
            {
                zPosition = -30;
            }
            for (int j = 0; j < 5; j++)
        {
           
            Randomcount = Random.value;

            if (Randomcount < 0.33f)
            {
                PutBuilding = buildingPrefab1;
                yPosition = 25f;
            }
            else if (Randomcount < 0.66f)
            {
                PutBuilding = buildingPrefab2;
                yPosition = 2.9f;
            }
            else
            {
                PutBuilding = buildingPrefab3;
                yPosition = 2.9f;
            }           

            float xPosition = (-2 + j) * 50f;
            Vector3 position = new Vector3(xPosition, yPosition, zPosition);
            GameObject building = Instantiate(PutBuilding, position, Quaternion.identity);
            if (building.GetComponent<Collider>() == null)
            {
                building.AddComponent<MeshCollider>();
            }

        }

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