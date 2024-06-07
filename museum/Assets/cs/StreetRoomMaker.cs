using UnityEngine;

public class StreetRoomMaker : MonoBehaviour
{
    public GameObject Streetprefab; // 廊下プレハブ
    
    public GameObject Roomprefab; // 部屋のプレハブ
    public int StreetNum = 5; // 生成するインスタンスの数
    public Vector3 startPosition = new Vector3(0, 0, 50); // 開始位置
    public Vector3 positionOffset = new Vector3(0, 0, 50); // 各インスタンスの位置オフセット

    void Start()
    {
        for (int i = 0; i < StreetNum; i++)
        {
            Vector3 position = startPosition + i * positionOffset;
            if (i % 2 == 0)
            {
                Instantiate(Streetprefab, position, Quaternion.identity);
            }
            else
            {
                Instantiate(Roomprefab, position, Quaternion.identity);
            }
        }
    }
}