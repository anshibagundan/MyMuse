using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using TMPro;
using Newtonsoft.Json;


public class LogIn : MonoBehaviour
{
    // Start is called before the first frame update

    //ボタンを押すとDBにusernameとpasswordが送信される。そこでpasswordが一致するかを判定して返す
    //認証が成功したら次のシーンに移る。

    public Button button;

    //Unity用のユーザ名とパスワード
    public TMP_InputField inputUserName;
    public TMP_InputField inputPassword;
    
    //DV送信用
    public MyData mydata = new MyData();//Post用にJSONを作成
    private static readonly HttpClient client = new HttpClient();//HTTPリクエストを送信し、HTTPレスポンスを受信するためのクラス
    
    public async void OnClick(){
        SceneManager.LoadScene("MyMuseum");

        //ユーザ名とパスワードをテキストフィールドから代入する。
        string userFromU = inputUserName.text;
        string passwordFromU = inputPassword.text;

        //DB上でパスワードが一致するかを判定しbooleanで返す
        string url = "https://vr-museum-6034ae04d19d.herokuapp.com/api/login/";
        bool match = await MatchPassword(url, userFromU, passwordFromU);//DBに送信する

        
        //入力されたパスワードとデータベースからのパスワードを比較する。
        if(match){
            //MyMuseumSceneに切り替え
            //MyMuseumSceneは美術館生成しているSceneである。
            SceneManager.LoadScene("MyMuseum");
        }
        else{
            UnityEngine.Debug.Log ("エラーが起きました。");
        }
        
        
    }

    

    //DBからデータ取得する
    async Task<bool> MatchPassword(string url, string userFromU, string passwordFromU){
        
        //JSON作成
        MyData mydata = new MyData
        {
            username = userFromU,
            password = passwordFromU
        };
        string myJson = JsonConvert.SerializeObject(mydata);
        StringContent content = new StringContent(myJson, Encoding.UTF8, "application/json");//HTTPリクエス用stringに変換
    
        using (HttpResponseMessage response = await client.PostAsync(url, content)){//HTTPリクエストを送信し、受信する
            if (response.IsSuccessStatusCode)//レスポンスが正常に取得できた時、データを取得する
            {
                string responseData = await response.Content.ReadAsStringAsync();
                return bool.Parse(responseData);  
            }
            else
            {
                Debug.LogError("Error: " + response.StatusCode);
                return false;
            }
        }
    }

    [Serializable]
    public class MyData{
        public string username;
        public string password;
    }
}
