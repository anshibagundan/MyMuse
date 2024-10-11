# プロダクト名 VR美術館 MyMuse
<!-- プロダクト名に変更してください -->
サポーターズハッカソン2024vol.5　優秀賞👑 <br>
技育博2024/07 サイバーエージェント賞👑

<!-- プロダクト名・イメージ画像を差し変えてください -->


## チーム名
白旗🏳️

## メンバー
<dl>
            <dt>さとこ</dt>
            <dd>Unity側でのAPI接続やプレゼン作成，ディレクター</dd>
            <dt>ちょり</dt>
            <dd>VRの操作感や物の配置方法などのUnityの処理全般，季節に応じたオブジェクトのモデリング</dd>
            <dt>ぞの</dt>
            <dd>APIの作成，Webページの作成，Unityでのログイン処理の作成</dd>
            <dt>Meg</dt>
            <dd>美術館や額縁などモデリングや季節に応じた壁紙の作成</dd>
</dl>

### リンク
- [さとこ](https://github.com/stk1201)
- [ちょり](https://x.com/bearl_develop)
- [ぞの](https://github.com/zono0013)
- [Meg](https://github.com/MegKuma)



## 背景・課題・解決されること
　私たちは，よく友達の誕生日会を開くことがあります．しかし，いつもプレゼントをあげて，ケーキを食べるだけの簡易なものになっています．
そこで，感動するものを作りたく，でもムービーやアルバムを1から作るのは大変ということで，
**VR美術館**を作りました．<br>
他のVR美術館では，作成するのに多額の費用や講義を受ける手間がありました．
この"My Muse"では，写真を追加するだけで作成することができます．

## 構成要素
<dl>
            <dt> Webページ側</dt>
            <dd>
                        アカウント作成して，webに写真を追加していく。</br>
                        写真には，写真タイトル，撮影時間，詳細，タグを記入して追加します。</br>
                        タグとは，写真の表示場所を設定するカテゴライズです</br>
                        タグには6種類あり，廊下，普通の部屋，春夏秋冬の部屋があります．</br>
                        廊下には日常的な生活の写真，普通の部屋には季節に関係ないイベントの写真（753や誕生日など）</br>
                        春夏秋冬の部屋には，その季節にあった写真を飾ります。</br>
            </dd>
            <dt>Unity</dt>
            <dd>
                        Webで作成したアカウントでログインすると，VR美術館が広がっています
            </dd>
</dl>

## プロダクト説明
MyMuseとは，写真をwebページに上げることで，それがVR上で額縁に入り美術館に並ぶものです．<br>
もし，追加したい写真が同じイベントの複数の写真だった場合，それらに対して同じタグをつけることができる．
それにより，VR上で部屋が出来上がりその部屋では同じタグをつけた写真が並ぶ．


VRの操作様子：
https://youtu.be/I3AjpXDCTGw

webページ：https://vr-museum-6034ae04d19d.herokuapp.com/login 

## 注力したポイント
ユーザにとって簡単に操作できるというコンセプトなのでクリック操作のみで写真や情報を追加できるようにし，VRでは説明が書いてある看板の設置をしました．<br>
また内装や額縁のデザインにもこだわり，よりユーザに感動を届けるようにしました．
加えてVR酔い対策として，HMDの向いている方向を正面として現実世界と変わりなく進むことができるようにしました．

## 使用技術
- Unity
- Django
- PostgreSQL
- Blender
            


