using UnityEngine;

public class ExihibitMaker : MonoBehaviour
{
    public GameObject Exihibit; // インスペクターから設定するプレハブ
    public int ExihibitNum = 5; // 生成するインスタンスの数
    private Vector3 startPosition = new Vector3(10, 5, -15); // 開始位置
    private Vector3 positionOffset = new Vector3(0, 0, 10); // 各インスタンスの位置オフセット
    private Vector3 leftsidePosition = new Vector3(-10, 5, -12);
    private Vector3 leftsideRotation = new Vector3(0, 180, 0);
    
    
    void Start()
    {
        for (int i = 0; i < ExihibitNum; i++)
        {
            if (i < 4)
            {
                // 最初の4つのインスタンスは通常の位置に配置
                Vector3 position = startPosition + i * positionOffset;
                Instantiate(Exihibit, position, Quaternion.identity);
            }
            else
            {
                // 5番目以降のインスタンスは左側に配置し、180度回転させる
                Vector3 position = leftsidePosition + (i - 4) * positionOffset;
                Quaternion rotation = Quaternion.Euler(leftsideRotation);
                Instantiate(Exihibit, position, rotation);
            }
        }
    }
}