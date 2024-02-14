using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTextData", menuName = "ScriptableObject/UI")]
public class TextDataScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class TextEntry
    {
        public string text;
        public Color color = Color.black;
        public TMP_FontAsset fontAsset;
        public float fontSize = 12f;
    }

    public TextEntry TextData;
}