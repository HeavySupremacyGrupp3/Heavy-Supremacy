﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEditor;

public class BuildWithNodeTexts : Editor
{
    [MenuItem("Build/Build With Nodes Directory Copied")]
    public static void BuildGame()
    {
        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", Application.dataPath + "/Builds", "");
        if (path != null && path != "")
        {
            string[] levels = new string[] { "Assets/Scenes/StartScene.unity", "Assets/Scenes/HUBScene.unity", "Assets/Scenes/PracticeScene.unity", "Assets/Scenes/WorkScene.unity" };

            string name = PlayerSettings.productName + "_v" + PlayerSettings.bundleVersion;

            // Build player.
            BuildPipeline.BuildPlayer(levels, path + "/" + name + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);

            // Copy a file from the project folder to the build folder, alongside the built game.
            FileUtil.CopyFileOrDirectory("Assets/Nodes", path + "/" + name + "_Data" + "/Nodes");

            path = path.Replace(@"/", @"\");   // explorer doesn't like front slashes
            Process.Start("explorer.exe", "/select," + path);
        }
    }
}