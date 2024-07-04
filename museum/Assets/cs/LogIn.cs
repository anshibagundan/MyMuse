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
    public Button button;
    public TMP_InputField inputUserName;
    public TMP_InputField inputPassword;

    private static readonly HttpClient client = new HttpClient();

    public async void OnClick()
    {
        // ユーザ名とパスワードをテキストフィールドから取得
        string userFromU = inputUserName.text;
        string passwordFromU = inputPassword.text;

        // DB上でパスワードが一致するかを判定しJSONレスポンスを処理する
        string url = "http://127.0.0.1:8000/unity/login/";
        bool match = await MatchPassword(url, userFromU, passwordFromU);

        // 入力されたパスワードとデータベースのパスワードを比較
        if (match)
        {
            // 認証が成功したらシーンを切り替える
            SceneManager.LoadScene("MyMuseum");
        }
        else
        {
            UnityEngine.Debug.Log("エラーが起きました。");
        }
    }

    async Task<bool> MatchPassword(string url, string userFromU, string passwordFromU)
    {
        // JSON作成
        MyData mydata = new MyData
        {
            username = userFromU,
            password = passwordFromU
        };
        string myJson = JsonConvert.SerializeObject(mydata);
        StringContent content = new StringContent(myJson, Encoding.UTF8, "application/json");

        using (HttpResponseMessage response = await client.PostAsync(url, content))
        {
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResponseData>(responseData);
                return result.status == "ok";
            }
            else
            {
                Debug.LogError("Error: " + response.StatusCode);
                return false;
            }
        }
    }

    [Serializable]
    public class MyData
    {
        public string username;
        public string password;
    }

    [Serializable]
    public class ResponseData
    {
        public string status;
    }
}
