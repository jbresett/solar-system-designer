using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;

public class UnityMenu : MonoBehaviour {

    // Build Source and Destination for overwriting default WebGL index file.
    private const string BUILD_SRC = "build.index.html";
    private const string BUILD_DEST = "Build/index.html";

    [MenuItem("SolarSystemDesigner/Build/Build", false, 0)]
    public static void Build()
    {
        buildProject(BuildOptions.None);
    }

    [MenuItem("SolarSystemDesigner/Build/Build && Run", false, 0)]
    public static void BuildAndRun()
    {
        buildProject(BuildOptions.AutoRunPlayer);
    }

    [MenuItem("SolarSystemDesigner/Test", false, 0)]
    public static void Test()
    {
    }   

    private static void buildProject(BuildOptions options)
    { 
        // Update Build
        int[] ver = VersionArray;
        ver[2]++;
        VersionArray = ver;

        // Set Options
        BuildPlayerOptions buildOptions = new BuildPlayerOptions();
        buildOptions.scenes = new[] { "Assets/Scenes/MainScene.unity" };
        buildOptions.locationPathName = "Build";
        buildOptions.target = BuildTarget.WebGL;
        buildOptions.options = options;

        // Run and View Results.
        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

        FileUtil.DeleteFileOrDirectory(BUILD_DEST);
        FileUtil.CopyFileOrDirectory(BUILD_SRC, BUILD_DEST);

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build " + report.summary.result + " @ " + (report.summary.totalSize / 1024) + "kb.");
        }
        else
        {
            Debug.Log("Build " + report.summary.result);
        }
    }

    [MenuItem("SolarSystemDesigner/Version/+Major", false, 0)]
    static void AddMajor()
    {
        int[] ver = VersionArray;
        ver[0]++;
        ver[1] = 0;
        VersionArray = ver;
    }

    [MenuItem("SolarSystemDesigner/Version/+Minor", false, 0)]
    static void AddMinor()
    {
        int[] ver = VersionArray;
        ver[1]++;
        VersionArray = ver;
    }

    [MenuItem("SolarSystemDesigner/Scene/Revert", false, 0)]
    static void RevertScene()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Notice", "You can't reset the main scene in play mode. Please stop the simulation before reverting.", "OK");
            return;
        }
        EditorSceneManager.OpenScene(EditorSceneManager.GetActiveScene().path);
    }

    /// <summary>
    /// Returns or sets the version in a {Major, Minor, Build} format.
    /// </summary>
    private static int[] VersionArray
    {
        // Converts version string to array of integers.
        get
        {
            string version = Config.Instance.Version;
            
            // Return 1.0.0 if version isn't set.
            if (version == "")
            {
                Debug.Log("Version not set. Default to '1.0.0'.");
                return new int[] { 1, 0, 0 };
            }

            // Split Version
            string[] verAry = version.Split('.');
            try
            {
                int[] result = new int[verAry.Length];
                for (int i = 0; i < 3; i++) result[i] = int.Parse(verAry[i]);
                return result;
            }

            // Invalid Code: Version should be in #.#.# format.
            catch (System.Exception)
            {   Debug.Log("Incorrect version '" + version + "'. Defaulted to '1.0.0.");
                return new int[] { 1, 0, 0 };
            }

        }
        // Converts array of integers to version.
        set
        {
            string ver = value[0].ToString();
            for (int i = 1; i < value.Length; i++) ver += "." + value[i];
            Sim.Config.Version = ver;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
