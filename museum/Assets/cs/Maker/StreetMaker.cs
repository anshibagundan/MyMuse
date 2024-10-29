using UnityEngine;

public class StreetMaker : MonoBehaviour
{
    public GameObject prefabToSpawn; // インスペクターから設定するプレハブ
    public int StreetNum = 5; // 生成するインスタンスの数
    public Vector3 startPosition = new Vector3(0, 0, 50); // 開始位置
    public Vector3 positionOffset = new Vector3(0, 0, 50); // 各インスタンスの位置オフセット

    void Start()
    {
        for (int i = 0; i < StreetNum; i++)
        {
            Vector3 position = startPosition + i * positionOffset;
            Instantiate(prefabToSpawn, position, Quaternion.identity);
        }
    }
}