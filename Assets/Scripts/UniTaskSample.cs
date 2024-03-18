using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UniTaskSample : MonoBehaviour
{
    readonly string uri = "https://gamecodeschool.com/wp-content/uploads/2015/06/bob.png";
    public Image downLoadImage;
    public Button btnDownloadImage;

    // Start is called before the first frame update
    void Start()
    {
        btnDownloadImage.onClick.AddListener(UniTask.UnityAction(DownloadImage));
    }

    private async UniTaskVoid DownloadImage()
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
        {
            UnityWebRequest result = await webRequest.SendWebRequest().ToUniTask(Progress.Create<float>(p =>
            Debug.Log($"progress：{p * 100}")));
            Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);

            int totalSpriteCount = 5;
            int perSpriteWidth = texture.width / totalSpriteCount;
            Sprite[] sprites = new Sprite[totalSpriteCount];

            for (int i = 0; i < totalSpriteCount; i++)
            {
                sprites[i] = Sprite.Create(texture, new Rect(new Vector2(perSpriteWidth * i, 0), new Vector2(perSpriteWidth, texture.height)), new Vector2(0.5f, 0.5f));
            }

            await RunSpriteSheet(totalSpriteCount, sprites);
        }
    }

    private async UniTask RunSpriteSheet(int totalSpriteCount, Sprite[] sprites)
    {
        float perFreamTime = 0.1f;
        while (true)
        {
            for (int i = 0; i < totalSpriteCount; i++)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(perFreamTime));
                var sprite = sprites[i];
                downLoadImage.sprite = sprite;
            }
        }
    }
}
