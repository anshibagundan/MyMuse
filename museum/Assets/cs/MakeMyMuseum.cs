using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class MakeMyMuseum : MonoBehaviour
{
    private List<string> v = new List<string>{};//s1,s2,...は廊下でに1,r1,r1,r2,r2,...は部屋に画像を配置する判別リスト
        
    public GameObject streetPrefab; 
    public GameObject roomPrefab; 
    private Vector3 startPosition = new Vector3(0, 0, 50); // 開始位置
    private Vector3 positionOffset = new Vector3(0, 0, 50); // 各インスタンスの位置オフセット
    
    //写真関連
    public LinkedList photoList = new LinkedList();//双方向リストの用意
    public GameObject exhibitPrefab;  // スペルミス修正
    Quaternion rot =Quaternion.Euler(0,90,180);//exhibitPrefabを回転すさせる
    float padding = 5;
    
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

    async void Start()
    {
       await extractionDB();//データを抽出して画像をLinkedListに挿入
       photoList.SorR(v);//廊下か部屋かの判別用リストに情報を入れる
       MuseumMaker();//内装づくり&配置
       
        
    }

    async Task extractionDB(){
        //DBからデータを取得する
        string url = "https://vr-museum-6034ae04d19d.herokuapp.com/api/photo_model/";
        string rootUrl = "https://vr-museum-6034ae04d19d.herokuapp.com";//画像貼り付け用のurl

        List<MyData> myData = await FetchData(url);//DBから取得する

        

        //exhibitPrefabに画像を貼り付け、双方向リストに挿入する。
        if(myData != null){

            //LogIn login = new LogIn();
            string userFromU = "RCC";//login.userFromU;

            Vector3 position = Vector3.zero;//Prefabテスト

            foreach(MyData data in myData){
                if(userFromU == data.user){
                    
                }
                float width = (float)data.width;
                float height = (float)data.height;

                //PrefabによるexhibitPrefabのインスタンス生成
                GameObject exhibitPrefabInstance = Instantiate(exhibitPrefab, position, Quaternion.identity);
                exhibitPrefabInstance.transform.localScale = new Vector3((width/(width+height))*10, (height/(width+height))*10, (float)0.05);
                
                position.x += padding;

                //画像をテクスチャとして生成する
                string imageUrl = rootUrl + data.content;
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);//テクスチャを取得するためのオブジェクト
                var asyncOperation = www.SendWebRequest();//HTTPリクエストの送信
                asyncOperation.completed += (op) =>{//HTTPリクエストの完了したとき、テクスチャを取得する
        
                    if (www.result == UnityWebRequest.Result.Success){//リクエストが成功した時
                        Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;       
                        exhibitPrefabInstance.GetComponent<Renderer>().material.mainTexture = texture;//取得したテクスチャをexhibitPrefabのテクスチャとする
                    }
                    else{
                        Debug.Log("画像の読み込みに失敗しました: " + www.error);
                    }
                };

                exhibitPrefabInstance.SetActive(false);//オブジェクトの非表示

                photoList.Append(data.title, data.detailed_title, data.time, exhibitPrefabInstance, height, width, data.tag, data.photo_num);
            }  
            
        }

    }

    //DBからデータ取得する
    async Task<List<MyData>> FetchData(string url){
    
        using (HttpClient client = new HttpClient()){//HTTPリクエストを送信し、受信する
            HttpResponseMessage response = await client.GetAsync(url);//レスポンス結果

            if(response.IsSuccessStatusCode){//レスポンスが正常に取得できた時、データを取得する
                string responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<MyData>>(responseData);
            }
            else{//エラー処理
                Debug.LogError("Error: " + response.StatusCode);
                return null;
            }
        }
    }

    [Serializable]
    public class MyData{
    public int id;
    public string title;
    public string detailed_title;
    public string user;
    public string time;
    public int photo_num;
    public string content;
    public int height;
    public int width;
    public string tag;
        
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
                Debug.Log(current.photoNum_);
                
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
                    Debug.Log(current.photoNum_);
                    
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