using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System;
using GameManagers;

namespace GameUtilities
{
    public class Utility : MonoBehaviour
    {

        public static TextMesh CreateTextMeshAtWorldPosition(string text, Vector3 position, int size, Transform parent = null, TextAlignment alignment = TextAlignment.Left, TextAnchor anchor = TextAnchor.UpperLeft, Color? color = null, int defaultSize = 5000)
        {
            Color Colour = Color.white;

            if (color != null) Colour = (Color)color;

            GameObject go = new GameObject(text, typeof(TextMesh));
            Transform transform = go.transform;
            transform.localPosition = position;
            if (parent != null) transform.SetParent(parent, false);

            TextMesh textMesh = go.GetComponent<TextMesh>();
            textMesh.text = text;
            textMesh.fontSize = size;
            textMesh.alignment = alignment;
            textMesh.anchor = anchor;
            textMesh.color = Colour;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = defaultSize;
            return textMesh;
        }

        public static TMP_Text CreateTextAtWorldPosition(string text, Vector3 position, TMP_FontAsset font, int fontSize = 1, int size = 1, HorizontalAlignmentOptions horizontalAlignment = HorizontalAlignmentOptions.Center, VerticalAlignmentOptions verticalAlignmentOptions = VerticalAlignmentOptions.Middle, Transform parent = null)
        {
            GameObject goParent = new GameObject(text, typeof(Canvas));
            goParent.AddComponent<CanvasScaler>();
            goParent.AddComponent<GraphicRaycaster>();

            if (parent != null) goParent.transform.parent = parent;
            goParent.GetComponent<Canvas>().worldCamera = Camera.main;
            goParent.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            goParent.transform.localPosition = position;

            GameObject go = new GameObject(text, typeof(TextMeshProUGUI));
            go.transform.parent = goParent.transform;
            go.transform.localPosition = Vector3.zero;
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);

            TMP_Text goText = go.GetComponent<TMP_Text>();

            goText.text = text;
            goText.font = font;
            goText.fontSize = fontSize;
            goText.horizontalAlignment = horizontalAlignment;
            goText.verticalAlignment = verticalAlignmentOptions;

            return goText;
        }

        public static GameObject GetGameObjectFromResources(string name)
        {
            return Resources.Load<GameObject>(name);
        }

        public static T GenericDeserializer<T>(string data) => JsonConvert.DeserializeObject<T>(data);

        public static string GenerateName(int len)
        {
            LogsManager.Print($"Start Generating");

            System.Random r = new System.Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2;
            while (b < len)
            {
                LogsManager.Print($"Generating Name ...");
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            LogsManager.Print($"#GN Generate Name : {Name}");

            return Name;

        }

        public static Color GetColorFromHexCode(string hexCode)
        {
            if (ColorUtility.TryParseHtmlString(hexCode, out Color color))
            {
                LogsManager.Print($"GetColorFromHexCode - String : {hexCode}, color : {color}");
                return color;
            }

            return Color.white;
        }

        public static string GetHexCodeFromColor(Color color)
        {
            string col = "ffffff";

            if (color != null)
            {
                col = ColorUtility.ToHtmlStringRGBA(color);
            }

            return "#" + col;
        }

        public static string GetVersion() => Application.version;
    } 

}