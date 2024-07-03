using System;
using System.Collections;
using System.Transactions;
using TMPro;
using UnityEngine;

public class IsEnterRoom : MonoBehaviour
{
    private string roomName;
    private TextMeshPro nameText;
    private TMP_CharacterInfo[] charInfos;

    private bool isUnShow = false;

    void Start()
    {
        // 制御設定
        nameText = this.GetComponent<TextMeshPro>();
        roomName = "Sports Day";
        nameText.text = roomName;

        // 初期設定
        nameText.ForceMeshUpdate(true);
        charInfos = nameText.textInfo.characterInfo;
        for (var i = 0; i < charInfos.Length; i++) // 全ての文字を透明化
        {
            SetTextAlpha(i, 0);
        }

        // コルーチンを開始
        StartCoroutine(GraduallyShow());
    }

    private IEnumerator GraduallyShow()
    {
        for (var i = 0; i < charInfos.Length; i++)
        {
            if (char.IsWhiteSpace(charInfos[i].character)) continue;
            // 一文字ごとに0.1秒待機
            yield return new WaitForSeconds(0.1f);

            float alpha = 0.0f;
            float FadeSpeed = 3.0f;

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
                    if(i == charInfos.Length-1){
                        StopCoroutine(GraduallyShow());
                        Invoke( "UnShow", 2.0f );
                    }
                    break;
                }
            }
        }
    }

    private IEnumerator GraduallyUnShow()
    {
        for (var i = charInfos.Length-1; i >= 0; i--)
        {
            if (char.IsWhiteSpace(charInfos[i].character)) continue;
            // 一文字ごとに0.1秒待機
            yield return new WaitForSeconds(0.1f);

            float alpha = 1.0f;
            float FadeSpeed = 3.0f;

            while (true)
            {
                // FixedUpdateのタイミングまで待つ
                yield return new WaitForFixedUpdate();

                float alphaDelta = FadeSpeed * Time.fixedDeltaTime;
                alpha = Mathf.Min(alpha - alphaDelta, 1.0f);
                SetTextAlpha(i, (byte)(255 * alpha));

                if (alpha <= 0.0f){
                    if(i == charInfos.Length-1){
                        StopCoroutine(GraduallyUnShow());
                    }
                    break;
                }
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
