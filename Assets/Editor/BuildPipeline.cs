using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;

using UnityEditor;
using System.IO;


public class ScriptBatch
{
  [MenuItem("Build/Build All Platform")]
  public static void BuildAllPlatform()
  {
    string path = "Deployment/AutomaticBuild/";
    string[] levels = new string[] { "Assets/Scenes/GameMainScene.unity" };

    // Build player.
    BuildPipeline.BuildPlayer(levels, path + "WindowsClient/3B-A-Card-Game.exe", BuildTarget.StandaloneWindows, BuildOptions.ShowBuiltPlayer);
    BuildPipeline.BuildPlayer(levels, path + "WindowsServer/3B-A-Card-Game-SERVER.exe", BuildTarget.StandaloneWindows, BuildOptions.EnableHeadlessMode);
    BuildPipeline.BuildPlayer(levels, path + "WebClient", BuildTarget.WebGL, BuildOptions.None);
  }
  
  [MenuItem("Build/Build and Run Windows Client Only")]
  public static void BuildWindowsClient()
  {
    string path = "Deployment/WindowsClient/";
    string[] levels = new string[] { "Assets/Scenes/GameMainScene.unity" };

    // Build player.
    BuildPipeline.BuildPlayer(levels, path + "3B-A-Card-Game.exe", BuildTarget.StandaloneWindows, BuildOptions.AutoRunPlayer);
  }
  
  [MenuItem("Build/Build and Run Windows Client And Server")]
  public static void BuildWindowsClientAndServer()
  {
    string path = "Deployment/WindowsClientAndServer/";
    string[] levels = new string[] { "Assets/Scenes/GameMainScene.unity" };

    // Build player.
    BuildPipeline.BuildPlayer(levels, path + "WindowsClient/3B-A-Card-Game.exe", BuildTarget.StandaloneWindows, BuildOptions.ShowBuiltPlayer);
    BuildPipeline.BuildPlayer(levels, path + "WindowsServer/3B-A-Card-Game-SERVER.exe", BuildTarget.StandaloneWindows, BuildOptions.EnableHeadlessMode);
  }
}