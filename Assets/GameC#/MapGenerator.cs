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
        Building();
        StreetLight();
    }
    void Update()
    {

    }


    void Building()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                zPosition = 51f;
                
            }

            if (i == 1)
            {
                zPosition = -51.2f;
            }
            if( i== 2)
            {
                zPosition = 100f;
            }
            if( i== 3)
            {
                zPosition = -100.0f;
            }
            for (int j = 0; j < 5; j++)
            {
                if (j != 2)
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
    }
    void StreetLight()
    {
        
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
}