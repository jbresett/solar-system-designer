using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class UnityMenu : MonoBehaviour {

    [MenuItem("SolarSystemDesigner/Build/Build")]
    public static void Build()
    {
        buildProject(false);
    }

    [MenuItem("SolarSystemDesigner/Build/Build && Run")]
    public static void BuildAndRun()
    {
        buildProject(true);
    }

    private static void buildProject(bool run)
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
        buildOptions.options = (run ? BuildOptions.AutoRunPlayer : BuildOptions.None);

        // Run and View Results.
        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
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
        ver[2] = 0;
        VersionArray = ver;
    }

    [MenuItem("SolarSystemDesigner/Version/+Minor", false, 0)]
    static void AddMinor()
    {
        int[] ver = VersionArray;
        ver[1]++;
        ver[2] = 0;
        VersionArray = ver;
    }

    /// <summary>
    /// Returns or sets the version in a {Major, Minor, Build} format.
    /// </summary>
    private static int[] VersionArray
    {
        // Converts version string to array of integers.
        get
        {
            string version = FindObjectOfType<Sim>().version;
            
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
            FindObjectOfType<Sim>().version = ver;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
