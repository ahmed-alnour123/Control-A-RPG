using System.IO;
using UnityEngine;

public class CameraImageSave : MonoBehaviour {
    private void Update() {
        if (Input.GetKeyDown(KeyCode.S))
            Save_camera_image();
    }

    public void Save_camera_image() {

        var path = "Assets/images/Image_" + Random.value * 2000 + ".png";
        Debug.Log(path);
        var cam = GetComponent<Camera>();

        // set render target to target texture
        var currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();

        // encode to PNG
        byte[] _bytes = image.EncodeToPNG();

        // save file
        System.IO.File.WriteAllBytes(path, _bytes);

        // set render texture back to default
        RenderTexture.active = currentRT;
    }
}

