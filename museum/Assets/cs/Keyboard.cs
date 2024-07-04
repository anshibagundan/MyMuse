using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keyboard : MonoBehaviour
{
    // InputField の読み込み
    [SerializeField] private TMP_InputField inputField;

    // 保存先
    private string Output;

    public bool isOpen = false;

    // キーボードの宣言
    private TouchScreenKeyboard _overlayKeyboard;

    // キーボードの変更時のみ動く
    private void Update()
    {
        if (_overlayKeyboard.text == "") return;

        if (isOpen)
        {
            inputField.text = _overlayKeyboard.text;
            Output = inputField.text;
        }
    }

    public void SetKeyboard()
    {
        if (!isOpen)
        {
            try
            {
                string initialText = Output ?? string.Empty;
                _overlayKeyboard = TouchScreenKeyboard.Open(initialText, TouchScreenKeyboardType.Default);
                isOpen = _overlayKeyboard != null;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"キーボードの開始中にエラーが発生しました: {e.Message}");
            }
        }
    }
}

