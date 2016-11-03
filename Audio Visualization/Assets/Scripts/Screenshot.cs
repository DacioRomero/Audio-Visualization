using UnityEngine;
using System.IO;

public class Screenshot : MonoBehaviour
{
    [SerializeField]
    private int superSize = 2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            string path = Path.GetDirectoryName(Application.dataPath) + "/Screenshots/";
            string fileName = System.DateTime.Now.ToFileTime() + ".png";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Debug.Log("Taking " + fileName + " at " + path);

            Application.CaptureScreenshot(path + fileName, superSize);
        }
    }
}
