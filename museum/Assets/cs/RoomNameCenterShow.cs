using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomNameCenterShow : MonoBehaviour
{
    private TextMeshPro nameText;
    private TMP_CharacterInfo[] charInfos;
    private int nameCount;
    public float wordWaitTime = 0.1f;
    public RoomNameLeftUpShow roomNameLeftUpShow;

    // Start is called before the first frame update
    void Start()
    {
        //カメラ設定
        Canvas canvas = this.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 1;
    }

    public void TextChange(string name)
    {
        nameText = this.GetComponent<TextMeshPro>();
        nameText.text = name;
        nameText.ForceMeshUpdate(true);
        nameCount = name.Length;
        charInfos = nameText.textInfo.characterInfo;
        for (var i = 0; i < charInfos.Length; i++) // 全ての文字を透明化
        {
            SetTextAlpha(i, 0);
        }
    }

    public IEnumerator GraduallyShow()
    {
        for (var i = 0; i < nameCount; i++)
        {
            if (char.IsWhiteSpace(charInfos[i].character)) continue;
            // 一文字ごとに0.1秒待機
            yield return new WaitForSeconds(0.1f);

            float alpha = 0.0f;
            float FadeSpeed = 5.0f;

            while (true)
            {
                // FixedUpdateのタイミングまで待つ
                yield return new WaitForFixedUpdate();

                // 一文字の不透明度を増加させていく
                float alphaDelta = FadeSpeed * Time.fixedDeltaTime;
                alpha = Mathf.Min(alpha + alphaDelta, 1.0f);
                SetTextAlpha(i, (byte)(255 * alpha));

                // 不透明度が1.0を超えたら次の文字に移る
                if (alpha >= 1.0f){
                    if(i == nameCount-1){
                        Invoke( "UnShow", 0.5f );
                    }
                    break;
                }
            }
        }
    }

    private IEnumerator GraduallyUnShow()
    {
		float alpha = 1.0f;
		var colortemp = nameText.color;
        float FadeSpeed = 5.0f;

		while (true)
		{
            yield return new WaitForFixedUpdate();

            float alphaDelta = FadeSpeed * Time.fixedDeltaTime;
			nameText.color = new Color(colortemp.r, colortemp.g, colortemp.b, alpha);
			alpha = Mathf.Min(alpha - alphaDelta, 0.5f);

			if (alpha <= 0.0f)
			{
                roomNameLeftUpShow.Show(nameText.text);
				break;
			}
		}
    }

    private void UnShow(){
        StartCoroutine(GraduallyUnShow());
    }

    // charIndexで指定した文字の透明度を変更
    private void SetTextAlpha(int charIndex, byte alpha)
    {
        TMP_TextInfo textInfo = nameText.textInfo;
        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

        TMP_MeshInfo meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];

        var rectVerticesNum = 4;
        for (var i = 0; i < rectVerticesNum; ++i)
        {
            meshInfo.colors32[charInfo.vertexIndex + i].a = alpha;
        }

        // 頂点カラーを変更したことを通知
        nameText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
