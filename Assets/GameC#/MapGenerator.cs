using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject buildingPrefab; // Unityエディタからビル1のPrefabを割り当ててください

    // Start is called before the first frame update
    void Start()
    {
        if (buildingPrefab == null)
        {
            Debug.LogError("buildingPrefabが割り当てられていません。Inspectorから設定してください。");
            return;
        }

        // 最初の5つをZ=30ラインに生成
        for (int i = 0; i < 5; i++)
        {
            float xPosition = (-2 + i) * 50f;
            Vector3 position = new Vector3(xPosition, 0, 30);
            GameObject building = Instantiate(buildingPrefab, position, Quaternion.identity);
            RandomizeBuildingColor(building);
        }

        // 残りの5つをZ=-30ラインに生成
        for (int i = 0; i < 5; i++)
        {
            float xPosition = (-2 + i) * 50f;
            Vector3 position = new Vector3(xPosition, 0, -30);
            GameObject building = Instantiate(buildingPrefab, position, Quaternion.identity);
            RandomizeBuildingColor(building);
        }
    }

    void RandomizeBuildingColor(GameObject building)
    {
        Renderer renderer = building.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propBlock);

            // 元の色を基準に、明度をランダムに変化させる
            Color baseColor = renderer.material.color;
            float h, s, v;
            Color.RGBToHSV(baseColor, out h, out s, out v);
            v *= Random.Range(0.7f, 1.3f); // 明るさを70%から130%の範囲で変更

            propBlock.SetColor("_Color", Color.HSVToRGB(h, s, v));
            renderer.SetPropertyBlock(propBlock);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}