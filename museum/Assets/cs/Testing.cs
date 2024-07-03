using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;

public class Testing : MonoBehaviour
{
    private List<string> v = new List<string>{};//s1,s2,...は廊下でに1,r1,r1,r2,r2,...は部屋に画像を配置する判別リスト
        
    public GameObject streetPrefab;
    public GameObject roomPrefab;
    private Vector3 startPosition = new Vector3(0, 0, 50); // 開始位置
    private Vector3 positionOffset = new Vector3(0, 0, 50); // 各インスタンスの位置オフセット
    
    //写真関連
    public static LinkedList photoList = new LinkedList();//双方向リストの用意
    public GameObject exhibitPrefab;  // スペルミス修正
    Quaternion rot =Quaternion.Euler(0,90,180);//exhibitPrefabを回転すさせる
    float padding = 5;

    //部屋のタイトルの受け渡し系
    private IsEnterRoom isEnterRoom;
    
    private Vector3 exhibitStart = new Vector3(10, 10, -15); // 開始位置
    private Vector3 exhibitOffset = new Vector3(0, 0, 10); // 各インスタンスの位置オフセット
    private Vector3 leftsidePosition = new Vector3(-10, 10, -12);
    private Vector3 leftsideRotation = new Vector3(0, 180, 0);
    
    public static int streetNum = 0;

    private List<Vector3> roomPhotoPos = new List<Vector3>
    {
        new Vector3(-60, 10 , 0),//1
        new Vector3(-40, 10 , -20),//2
        new Vector3(-40, 10 , 20),//3
        new Vector3(-60, 10 , 10),//4
        new Vector3(-60, 10 , -10),//5
        new Vector3(-30, 10 , -20),//6
        new Vector3(-30, 10 , 20),//7
        new Vector3(-50, 10 , -20),//8
        new Vector3(-50, 10 , 20)//9
    };
    
    private List<Vector3> roomPhotoRote = new List<Vector3>
    {
        new Vector3(0, -90 ,180),//1
        new Vector3(0, 180 ,180),//2
        new Vector3(0, 0 ,180),//3
        new Vector3(0, -90 ,180),//4
        new Vector3(0, -90 ,180),//5
        new Vector3(0, 180 ,180),//6
        new Vector3(0, 0 ,180),//7
        new Vector3(0, 180 ,180),//8
        new Vector3(0, 0 ,180)//9
    };

    void Start()
    {
       extractionDB();//データを抽出して画像をLinkedListに挿入
       photoList.SorR(v);//廊下か部屋かの判別用リストに情報を入れる
       MuseumMaker();//内装づくり&配置
    }

    private void extractionDB(){
        Vector3 position = Vector3.zero;//Prefabテスト

        for(int i = 0; i < 5; i++){
            float width = 2044;
            float height = 1526;

            //PrefabによるexhibitPrefabのインスタンス生成
            GameObject exhibitPrefabInstance = Instantiate(exhibitPrefab, position, Quaternion.identity);
            exhibitPrefabInstance.transform.localScale = new Vector3((width/(width+height))*10, (height/(width+height))*10, (float)0.05);
            
            position.x += padding;

            //画像を設定する
            String title = "Title";
            String detailed_title = $"More detailed{i}";
            String time = "2024/6/12";
            String tag = "s";
            int photo_num = i + 1;

            exhibitPrefabInstance.SetActive(false);//オブジェクトの非表示
            photoList.Append(title, detailed_title, time, exhibitPrefabInstance, height, width, tag, photo_num);
        }

        for(int i = 0; i < 10; i++){
            float width = 2044;
            float height = 1526;

            //PrefabによるexhibitPrefabのインスタンス生成
            GameObject exhibitPrefabInstance = Instantiate(exhibitPrefab, position, Quaternion.identity);
            exhibitPrefabInstance.transform.localScale = new Vector3((width/(width+height))*10, (height/(width+height))*10, (float)0.05);
            
            position.x += padding;

            //画像を設定する
            String title = "Title";
            String detailed_title = $"More detailed{i}";
            String time = "2024/6/12";
            String tag = "r1";
            int photo_num = i + 6;

            exhibitPrefabInstance.SetActive(false);//オブジェクトの非表示
            photoList.Append(title, detailed_title, time, exhibitPrefabInstance, height, width, tag, photo_num);
        }

        for(int i = 0; i < 5; i++){
            float width = 2044;
            float height = 1526;

            //PrefabによるexhibitPrefabのインスタンス生成
            GameObject exhibitPrefabInstance = Instantiate(exhibitPrefab, position, Quaternion.identity);
            exhibitPrefabInstance.transform.localScale = new Vector3((width/(width+height))*10, (height/(width+height))*10, (float)0.05);
            
            position.x += padding;

            //画像を設定する
            String title = "Title";
            String detailed_title = $"More detailed{i}";
            String time = "2024/6/12";
            String tag = "r2";
            int photo_num = i + 6;

            exhibitPrefabInstance.SetActive(false);//オブジェクトの非表示
            photoList.Append(title, detailed_title, time, exhibitPrefabInstance, height, width, tag, photo_num);
        }
        

    }

    private void MuseumMaker()
    {
        int exhibitNum = 0;
        string roomName;
        LinkedPhoto current = photoList.First();
        //Debug.Log(photoList.Last().photoNum_);



        for (int i = 0; i < v.Count;)
        {
            
            if (v[i].Contains("s"))
            {
                // 通路
                Vector3 position = startPosition + streetNum * positionOffset;
                GameObject parentInstance = Instantiate(streetPrefab, position, Quaternion.identity);
                streetNum++;
                // 写真
                current.SetUp(position + exhibitStart,rot);
                i++;
                exhibitNum++;
                //current = current.NextPhoto;
                
                while (i < v.Count && v[i].Contains("s"))
                {
                    current = current.NextPhoto;

                    // 写真
                    Vector3 exhibitPosition = position + exhibitStart + exhibitNum * exhibitOffset;
                    current.SetUp(exhibitPosition, rot);
            
                    i++;
                    exhibitNum++;

                    if (exhibitNum > 3)
                    {
                        exhibitNum = 0;
                        break;
                    }
                }
                exhibitNum = 0;
                current = current.NextPhoto;
            }
            else
            {
                roomName = v[i];
                // 通路
                Vector3 position = startPosition + streetNum * positionOffset;
                GameObject parentInstance = Instantiate(roomPrefab, position, Quaternion.identity);
                streetNum++;

                // 写真
                Quaternion rotation = Quaternion.Euler(roomPhotoRote[exhibitNum]);
                current.SetUp(roomPhotoPos[exhibitNum] + position, rotation);

                exhibitNum++;
                i++;

                while (i < v.Count && v[i] == roomName)
                {
                    current = current.NextPhoto;
                    //Debug.Log(current.photoNum_);
                    
                    if (exhibitNum < roomPhotoPos.Count && exhibitNum < roomPhotoRote.Count)
                    {
                        // 写真
                        rotation = Quaternion.Euler(roomPhotoRote[exhibitNum]);
                        current.SetUp(roomPhotoPos[exhibitNum] + position, rotation);

                    }

                    i++;
                    exhibitNum++;
                }

                exhibitNum = 0;
                current = current.NextPhoto;
            }
        }
    }
}
