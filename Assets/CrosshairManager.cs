using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField] private Texture2D crosshairTexture;
    private Vector2 crosshairHotspot;
    // Start is called before the first frame update
    void Start()
    {
        crosshairHotspot = new Vector2(crosshairTexture.width / 2, crosshairTexture.height / 2);
        Cursor.SetCursor(crosshairTexture, crosshairHotspot, CursorMode.Auto); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
