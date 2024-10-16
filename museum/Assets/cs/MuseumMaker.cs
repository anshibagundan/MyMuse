using System.Collections.Generic;
using UnityEngine;

public class MuseumMaker : MonoBehaviour
{
    public List<string> v = new List<string> { "S", "S", "S", "S", "S", "R", "R", "R", "R", "R", "R","R", "R", "R", "R1", "R1", "S", "S", "R2" };

    //部屋関連
    private Dictionary<string, GameObject> roomKind_PrefabDict = new Dictionary<string, GameObject>();
    private List<string> roomKindList = new List<string> { "normal","spring","summer","autumn","winter"};

    private Dictionary<string, string> roomName_KindDict = new Dictionary<string, string>{
        {"R1","normal"},
        {"R2","spring"},
        {"R3","summer"},
        {"R4","autumn"},
        {"R5","winter"}
    };

    public GameObject streetPrefab;
    private GameObject roomPrefab;
    public List<GameObject> roomList = new List<GameObject>();
    private Vector3 startPosition = new Vector3(0, 0, 50); // 開始位置
    private Vector3 positionOffset = new Vector3(0, 0, 50); // 各インスタンスの位置オフセット

    //写真関連
    public GameObject exhibitPrefab;

    private Vector3 exhibitStart = new Vector3(8, 10, -15); // 開始位置
    private Vector3 exhibitOffset = new Vector3(0, 0, 10); // 各インスタンスの位置オフセット
    private Vector3 leftsidePosition = new Vector3(-10, 10, -12);
    private Vector3 leftsideRotation = new Vector3(0, 180, 0);

    public static int streetNum = 0;

    private List<Vector3> roomPhotoPos = new List<Vector3>
    {
        new Vector3(-58, 10 , 0),//1
        new Vector3(-40, 10 , -18),//2
        new Vector3(-40, 10 , 18),//3
        new Vector3(-58, 10 , 10),//4
        new Vector3(-58, 10 , -10),//5
        new Vector3(-30, 10 , -18),//6
        new Vector3(-30, 10 , 18),//7
        new Vector3(-50, 10 , -18),//8
        new Vector3(-50, 10 , 18)//9
    };

    private List<Vector3> roomPhotoRote = new List<Vector3>
    {
        new Vector3(0, 180 ,0),//1
        new Vector3(0, 90 ,0),//2
        new Vector3(0, -90 ,0),//3
        new Vector3(0, 180 ,0),//4
        new Vector3(0, 180 ,0),//5
        new Vector3(0, 90 ,0),//6
        new Vector3(0, -90 ,0),//7
        new Vector3(0, 90 ,0),//8
        new Vector3(0, -90 ,0)//9
    };

    void Start()
    {
        // 部屋の名前とプレハブの辞書を作成
        for (int i = 0; i < roomKindList.Count; i++)
        {
            roomKind_PrefabDict.Add(roomKindList[i], roomList[i]);
        }

        // // 部屋の名前と種類の辞書を作成
        // for (int i = 0; i < roomKindList.Count; i++)
        // {
        //     roomName_KindDict.Add("R" + i+1, roomKindList[i]);

        //     //ここは本当は
        //     //roomName_KindDict.Add(v[i].name, v[i].kind);
        // }



        int exhibitNum = 0;
        string roomName;

        for (int i = 0; i < v.Count;)
        {
            if (v[i] == "S")
            {
                // 通路
                Vector3 position = startPosition + streetNum * positionOffset;
                GameObject parentInstance = Instantiate(streetPrefab, position, Quaternion.identity);
                streetNum++;
                // 写真
                Instantiate(exhibitPrefab, position + exhibitStart, Quaternion.identity, parentInstance.transform);
                i++;
                exhibitNum++;
                while (i < v.Count && v[i] == "S")
                {
                    // 写真
                    Vector3 exhibitPosition = position + exhibitStart + exhibitNum * exhibitOffset;
                    Instantiate(exhibitPrefab, exhibitPosition, Quaternion.identity, parentInstance.transform);
                    i++;
                    exhibitNum++;

                    if (exhibitNum > 3)
                    {
                        exhibitNum = 0;
                        break;
                    }
                }
                exhibitNum = 0;
            }
            else
            {
                roomName = v[i];
                if(!roomName_KindDict.ContainsKey(roomName))
                {
                    roomPrefab = roomList[0];
                }else{
                    roomPrefab = roomKind_PrefabDict[roomName_KindDict[roomName]];
                }
                // 通路
                Vector3 position = startPosition + streetNum * positionOffset;
                GameObject parentInstance = Instantiate(roomPrefab, position, Quaternion.identity);
                streetNum++;

                //roomNameの引き渡し
                RoomNameCenterShow roomNameCenterShow = parentInstance.GetComponentInChildren<RoomNameCenterShow>();
                roomNameCenterShow.TextChange(roomName);
                // 写真
                Quaternion rotation = Quaternion.Euler(roomPhotoRote[exhibitNum]);
                Instantiate(exhibitPrefab, roomPhotoPos[exhibitNum] + position, rotation, parentInstance.transform);
                exhibitNum++;
                i++;
                while (i < v.Count && v[i] == roomName)
                {
                    if (exhibitNum < roomPhotoPos.Count && exhibitNum < roomPhotoRote.Count)
                    {
                        // 写真
                        rotation = Quaternion.Euler(roomPhotoRote[exhibitNum]);
                        Instantiate(exhibitPrefab, roomPhotoPos[exhibitNum]+position, rotation, parentInstance.transform);
                    }
                    i++;
                    exhibitNum++;
                }
                exhibitNum = 0;
            }
        }
    }
}