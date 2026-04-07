using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.IO;

public class BuildManager : MonoBehaviour
{
    private const string BUILD_OUTPUT_DIRECTORY = "./Builds/";
    private const string EXE_NAME = "Protobot-Rebuilt.exe";

    [MenuItem("Build/Build Windows EXE")]
    public static void BuildWindowsEXE()
    {
        Debug.Log("Starting Windows EXE build...");

        // Ensure build directory exists
        if (!Directory.Exists(BUILD_OUTPUT_DIRECTORY))
            Directory.CreateDirectory(BUILD_OUTPUT_DIRECTORY);

        // Get all scenes in build settings
        string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorSceneManager.GetActiveScene().path);

        if (scenes.Length == 0)
        {
            EditorUtility.DisplayDialog("Build Failed", "No scenes found in Build Settings. Add scenes to File > Build Settings.", "OK");
            return;
        }

        string buildPath = Path.Combine(BUILD_OUTPUT_DIRECTORY, EXE_NAME);

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build succeeded! Output: {Path.GetFullPath(buildPath)}");
            EditorUtility.DisplayDialog("Build Successful", 
                $"EXE created at:\n{Path.GetFullPath(buildPath)}", "OK");
            
            // Open build folder
            EditorUtility.RevealInFinder(Path.GetFullPath(BUILD_OUTPUT_DIRECTORY));
        }
        else
        {
            Debug.LogError("Build failed!");
            EditorUtility.DisplayDialog("Build Failed", "See Console for details.", "OK");
        }
    }

    [MenuItem("Build/Build Windows EXE (Development)")]
    public static void BuildWindowsEXEDevelopment()
    {
        Debug.Log("Starting Development Windows EXE build...");

        if (!Directory.Exists(BUILD_OUTPUT_DIRECTORY))
            Directory.CreateDirectory(BUILD_OUTPUT_DIRECTORY);

        string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorSceneManager.GetActiveScene().path);

        if (scenes.Length == 0)
        {
            EditorUtility.DisplayDialog("Build Failed", "No scenes found in Build Settings.", "OK");
            return;
        }

        string buildPath = Path.Combine(BUILD_OUTPUT_DIRECTORY, "Protobot-Rebuilt-Dev.exe");

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
        {
            scenes = scenes,
            locationPathName = buildPath,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.Development | BuildOptions.AllowDebugging
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Development build succeeded! Output: {Path.GetFullPath(buildPath)}");
            EditorUtility.DisplayDialog("Build Successful", 
                $"Development EXE created at:\n{Path.GetFullPath(buildPath)}", "OK");
            
            EditorUtility.RevealInFinder(Path.GetFullPath(BUILD_OUTPUT_DIRECTORY));
        }
        else
        {
            Debug.LogError("Build failed!");
        }
    }

    [MenuItem("Build/Open Build Folder")]
    public static void OpenBuildFolder()
    {
        string fullPath = Path.GetFullPath(BUILD_OUTPUT_DIRECTORY);
        if (Directory.Exists(fullPath))
            EditorUtility.RevealInFinder(fullPath);
        else
            EditorUtility.DisplayDialog("Folder Not Found", $"Build folder not found at:\n{fullPath}", "OK");
    }
}
