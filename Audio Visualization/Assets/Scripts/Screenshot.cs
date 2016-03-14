using UnityEngine;

public class Screenshot : MonoBehaviour
{
    [SerializeField]
    private int superSize = 2;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            string path = System.IO.Path.GetDirectoryName(Application.dataPath) + "/Screenshots/";
            string fileName = System.DateTime.Now.ToFileTime() + ".png";
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            Debug.Log("Taking " + fileName + " at " + path);
            Application.CaptureScreenshot(path + fileName, superSize);
        }
    }
}
