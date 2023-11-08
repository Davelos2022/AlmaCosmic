using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileManager
{
    public static string _pathToVideoGymnastics = Application.streamingAssetsPath + "/" + "GymnasticsData" + "/" + "Video" + "/";
    public const string _juniorGroupGymnastics = "JuniorGroup";
    public const string _seniorGroupGymnastics = "SeniorGroup";

    public static string _pathToVideoZvukoreche = Application.streamingAssetsPath + "/" + "ZvukorecheData" + "/" + "Video" + "/";

    public static string[] GetCountVideoFiles(string path, string[] androidFileName = null)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            var videosPaths = new List<string>();
            var androidPath = path;

            foreach (var file in androidFileName)
            {
                videosPaths.Add(Path.Combine(androidPath, file.TrimEnd() + ".mp4"));
            }

            return videosPaths.ToArray();
        }
        else
        {
            string[] videoFiles = Directory.GetFiles(path, "*.mp4",
                SearchOption.AllDirectories);

            return videoFiles;
        }
    }
}
