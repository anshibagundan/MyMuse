using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//画像データを保存する場所

public class LinkedPhoto{
    public string title_{get; set;}
    public string detailedTitle_{get; set;}
    public string time_{get; set;}
    public GameObject picture_{get; set;}
    public float height_{get; set;}
    public float width_{get; set;}
    public string tag_{get; set;}
    public int photoNum_{get; set;}

    public LinkedPhoto PreviousPhoto {get; set;}
    public LinkedPhoto NextPhoto {get; set;}
    

    public LinkedPhoto(string title, string detailedTitle, string time, GameObject picture, float height, float width, string tag, int photoNum){
        title_ = title;
        detailedTitle_ = detailedTitle;
        time_ = time;
        picture_ = picture;
        height_ = height;
        width_ = width;
        tag_ = tag;
        photoNum_ = photoNum;

        PreviousPhoto = null;
        NextPhoto = null;
    }

    //pictureの配置
    public void SetUp(Vector3 position, Quaternion rot){
        picture_.transform.position = position;
        picture_.transform.rotation = rot;
        picture_.SetActive(true);//表示可能にする
    }
}

public class LinkedList{
    private LinkedPhoto first_;
    private LinkedPhoto last_;

    public LinkedPhoto Head {get{return first_;}}
    public LinkedPhoto Tail {get{return last_;}}

    public LinkedList(){
        first_ = null;
        last_ = null;
    }

    //最初のLinkedPhotoを引き渡す
    public LinkedPhoto First(){
       
        return first_;
    }
   
    //追加する
    public void Append(string title, string detailedTitle, string time, GameObject picture, float height, float width, string tag, int photoNum){
        LinkedPhoto photo = new LinkedPhoto(title, detailedTitle, time, picture, height, width, tag, photoNum);
        if(first_ == null){
            first_ = photo;
            last_ = photo;
        }   
        else{
            photo.PreviousPhoto = last_;
            last_.NextPhoto = photo;
            last_ = photo;
        }
    }

    public void SorR(List<string> list){
        LinkedPhoto current = first_;
        while(current != null){
            list.Add(current.tag_);
            current = current.NextPhoto;
        }
    }

}
