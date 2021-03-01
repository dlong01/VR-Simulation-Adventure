using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateHeatMap : MonoBehaviour
{
    public GameObject simController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        List<int[]> coords = ReadLogger(hit.gameObject);
        var heatMap = DrawHeatMap(coords);

        string pngPath = Application.dataPath + "/logs/" + SceneManager.GetActiveScene().name + "MovementHeatMap-" + System.DateTime.Now.Ticks + ".png";
        SaveTextureAsPNG(heatMap, pngPath);
    }

    private List<int[]> ReadLogger(GameObject player)
    {
        int[] xy = new int[2];
        List<int[]> coords = new List<int[]>();

        string fname = player.GetComponent<Logger>().getPositionFileName();
        StreamReader f = new StreamReader(fname); 

        var line = f.ReadLine();
        line = f.ReadLine();

        while (line != null) {
            string[] values = line.Split(',');

            var xRaw = float.Parse(values[1]) + 2.5f * 100.0f;
            xy[0] = Mathf.FloorToInt(Mathf.Clamp(xRaw, 0, 500));

            var yRaw = float.Parse(values[3]) + 2.5f * 100.0f;
            xy[1] = Mathf.FloorToInt(Mathf.Clamp(yRaw, 0, 500));

            coords.Add(xy);
            line = f.ReadLine();
        }

        return coords;
    }

    private Texture2D DrawHeatMap(List<int[]> coords)
    {
        var heatMap = new Texture2D(500, 500, TextureFormat.RGB24, false);

        int tValues = coords.Count;
        int totalCol = 766;
        float inc = totalCol / tValues;

        foreach (int[] xy in coords)
        {
            Color oldColor = heatMap.GetPixel(xy[0], xy[1]);
            Color newColor = new Color(0, 0, 0);

            if (oldColor.g < 255 && oldColor.r == 0)
            {
                newColor.g = oldColor.g + inc;
            }
            if (oldColor.g == 255 && oldColor.r < 255)
            {
                newColor.r = oldColor.r + inc;
            }
            if (oldColor.g > 0 && oldColor.r == 255)
            {
                newColor.g = oldColor.g - inc;
            }

            Mathf.Clamp(newColor.r, 0, 255);
            Mathf.Clamp(newColor.g, 0, 255);

            heatMap.SetPixel(xy[0], xy[1], newColor);
        }

        heatMap.Apply();
        return heatMap;
    }

    public static void SaveTextureAsPNG(Texture2D texture, string fullPath)
    {
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, bytes);
    }
}
