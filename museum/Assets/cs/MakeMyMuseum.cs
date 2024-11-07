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
using Unity.VisualScripting;

public class MakeMyMuseum : MonoBehaviour
{
    [SerializeField] private GameObject streetPrefab;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private List<GameObject> roomList = new List<GameObject>();
    [SerializeField] private GameObject exhibitPrefab;
    [SerializeField] private GameObject canvasTitle;

    private readonly Vector3 startPosition = new Vector3(0, 0, 50);
    private readonly Vector3 positionOffset = new Vector3(0, 0, 50);
    private readonly Vector3 exhibitStart = new Vector3(10, 10, -15);
    private readonly Vector3 exhibitOffset = new Vector3(0, 0, 10);
    public static int streetNum = 0;

    private Dictionary<string, GameObject> roomTypePrefabMap;
    private MuseumData museumData;

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

    private async void Start()
    {
        await InitializeMuseum();
        CreateMuseum();
    }

    private async Task InitializeMuseum()
    {
        InitializeRoomPrefabs();
        museumData = await FetchMuseumData();
    }

    private void InitializeRoomPrefabs()
    {
        roomTypePrefabMap = new Dictionary<string, GameObject>
        {
            { "通常部屋", roomList[0] },
            { "春の部屋", roomList[1] },
            { "夏の部屋", roomList[2] },
            { "秋の部屋", roomList[3] },
            { "冬の部屋", roomList[4] },
            { "廊下", streetPrefab }
        };
    }

    private async Task<MuseumData> FetchMuseumData()
    {
        string userFromU = LogIn.UserName;
        string url = "https://mymuse-79b7481a50f4.herokuapp.com/api/unity/" + "a"; // APIエンドポイントを適切に設定
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            Debug.Log("aaaaaa");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Debug.Log(json);
                return JsonConvert.DeserializeObject<MuseumData>(json);
            }
            Debug.Log($"Fetch museum data: {response.StatusCode}");
            return null;
        }
    }

    private void CreateMuseum()
    {
        foreach (var tag in museumData.tags)
        {
            if (tag.photos == null || tag.photos.Count == 0) continue;
            Debug.Log(tag);

            var position = startPosition + streetNum * positionOffset;
            var roomPrefab = GetRoomPrefab(tag.roomType);

            Debug.Log($"Room Prefab Name: {roomPrefab.name}");

            var room = InstantiateRoom(roomPrefab, position, tag.name);

            if (tag.roomType == "廊下")
            {
                if (tag.photos.Count > 4)
                {
                    CreateStreetExhibits(tag.photos, position, 0);

                    streetNum++;
                    position = startPosition + streetNum * positionOffset;
                    InstantiateRoom(roomPrefab, position, tag.name);
                    CreateStreetExhibits(tag.photos, position, 4);
                }
                else
                {
                    CreateStreetExhibits(tag.photos, position, 0);
                }

            }
            else
            {
                Debug.Log(tag.roomType);
                CreateRoomExhibits(tag.photos, position);
            }

            streetNum++;
        }
    }

    private GameObject GetRoomPrefab(string roomType)
    {
        return roomTypePrefabMap.TryGetValue(roomType, out var prefab) ? prefab : roomList[0];
    }

    private GameObject InstantiateRoom(GameObject prefab, Vector3 position, string roomName)
    {
        var room = Instantiate(prefab, position, Quaternion.identity);
        var roomNameDisplay = room.GetComponentInChildren<RoomNameCenterShow>();
        if (roomNameDisplay != null)
        {
            roomNameDisplay.TextChange(roomName);
        }
        return room;
    }

    private void CreateStreetExhibits(List<PhotoData> photos, Vector3 basePosition, int a)
    {
        for (int i = 0; i < Mathf.Min(photos.Count-a, 4); i++)
        {
            var photo = photos[a+i];
            var position = basePosition + exhibitStart + i * exhibitOffset;
            CreateExhibit(photo, position, Quaternion.Euler(0, 90, 180));
        }
    }

    private void CreateRoomExhibits(List<PhotoData> photos, Vector3 basePosition)
    {
        for (int i = 0; i < Mathf.Min(photos.Count, roomPhotoPos.Count); i++)
        {
            var photo = photos[i];
            var position = basePosition + roomPhotoPos[i];
            var rotation = Quaternion.Euler(roomPhotoRote[i]);
            CreateExhibit(photo, position, rotation);
        }
    }

    private async void CreateExhibit(PhotoData photo, Vector3 position, Quaternion rotation)
    {
        var exhibit = Instantiate(exhibitPrefab, position, rotation);
        var scale = CalculateExhibitScale(photo.width, photo.height);
        exhibit.transform.localScale = scale;

        await LoadExhibitTexture(exhibit, photo.Content);
        CreateExhibitTitle(photo, position, rotation, scale.y);
    }

    private async Task LoadExhibitTexture(GameObject exhibit, string imageUrl)
    {
        using (var www = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            var operation = www.SendWebRequest();

            // 非同期操作が完了するまで待機
            while (!operation.isDone)
            {
                await Task.Yield(); // 非同期で待機
            }

            if (www.result == UnityWebRequest.Result.Success)
            {
                var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                exhibit.GetComponent<Renderer>().material.mainTexture = texture;
            }
            else
            {
                Debug.LogError($"Failed to load texture: {www.error}");
            }
        }
    }

    private void CreateExhibitTitle(PhotoData photo, Vector3 position, Quaternion rotation, float height)
    {
        var titleHeight = height / 2 + 2;
        var titlePosition = position - new Vector3(0, titleHeight, 0);
        var titleInstance = Instantiate(canvasTitle, titlePosition, rotation);

        var button = titleInstance.GetComponentInChildren<Button>();
        button.GetComponentInChildren<TextMeshProUGUI>().text = photo.Title;

        var detailsPanel = titleInstance.transform.Find("DetailsPanel");
        SetupDetailsPanel(detailsPanel, photo);
    }

    private void SetupDetailsPanel(Transform detailsPanel, PhotoData photo)
    {
        detailsPanel.Find("TitleforDetail").GetComponent<TextMeshProUGUI>().text = $"『{photo.Title}』";
        detailsPanel.Find("Details").GetComponent<TextMeshProUGUI>().text = photo.DetailedTitle;
        detailsPanel.gameObject.SetActive(false);
    }

    private Vector3 CalculateExhibitScale(float width, float height)
    {
        var scaleFactor = width / (width + height) * 10;
        return new Vector3(scaleFactor, height / (width + height) * 10, 0.05f);
    }
}

[Serializable]
public class MuseumData
{
    public List<TagData> tags { get; set; }
}

[Serializable]
public class TagData
{
    public int ID { get; set; }
    public string name { get; set; }
    public string roomType { get; set; }
    public List<PhotoData> photos { get; set; }
}

[Serializable]
public class PhotoData
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string DetailedTitle { get; set; }
    public string Content { get; set; }
    public int height { get; set; }
    public int width { get; set; }
}