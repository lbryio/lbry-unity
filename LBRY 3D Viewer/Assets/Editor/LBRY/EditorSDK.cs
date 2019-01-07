using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class LBRY
{
    [MenuItem("LBRY/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "LBRY/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory)) {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.WebGL);
    }

    [MenuItem("LBRY/Build Project")]
    public static void BuildWebGL()
    {
        // Switch to WebGL build.
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);

        PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm;
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
        PlayerSettings.WebGL.debugSymbols = false;
		PlayerSettings.WebGL.dataCaching = false;
        PlayerSettings.stripEngineCode = false;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        List<string> scenes = new List<string>();
        foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
          if(scene.enabled) {
            scenes.Add(scene.path);
          }
        }

        buildPlayerOptions.scenes = scenes.ToArray();
        buildPlayerOptions.locationPathName = "LBRY/Build";
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.UncompressedAssetBundle;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded) {
          UnityEngine.Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        } else if (summary.result == BuildResult.Failed) {
          UnityEngine.Debug.Log("Build failed");
        }
    }

    [MenuItem("LBRY/Package Build for LBRY")]
    public static void PackageLBRY()
    {
      /*
       * WARNING: These paths will look weird on Windows
       * ******** BUT THEY WORK, TAKE CAUTION EDITING!
       */
      var assetPath = Application.dataPath;
      UnityEngine.Debug.Log("assetPath: " + assetPath);

      var unityEditorLbryPath = assetPath + "/Editor/LBRY/";
      UnityEngine.Debug.Log("unityEditorLbryPath: " + unityEditorLbryPath);

      var projectRoot = Path.Combine(assetPath, "../");
      UnityEngine.Debug.Log("projectRoot: " + projectRoot);

      var targetBuildRoot = projectRoot + "/LBRY/";
      UnityEngine.Debug.Log("targetBuildRoot: " + targetBuildRoot);

      var packageScript = "\"" + Path.Combine(Path.GetDirectoryName(unityEditorLbryPath), "package.js") + "\"";
      UnityEngine.Debug.Log("packageScript: " + packageScript);

      var lbryFormatDir = Path.Combine(Path.GetDirectoryName(unityEditorLbryPath), "lbry-format~/");

      // Copy `lbry-format` to `lbry-format~` (Unity special folder)
      if(!Directory.Exists(lbryFormatDir)) {
        var srcLbryFormatDir = Path.Combine(Path.GetDirectoryName(unityEditorLbryPath), "lbry-format/");
        UnityEngine.Debug.LogWarning("Creating `lbry-format~` (Unity ignored)");
        FileUtil.CopyFileOrDirectory(srcLbryFormatDir, lbryFormatDir);
      }

      // Install `lbry-format` dependencies
      var lbryFormatModulesDir = Path.Combine(Path.GetDirectoryName(unityEditorLbryPath), "lbry-format~/node_modules/");
      if(!Directory.Exists(lbryFormatModulesDir)) {
        UnityEngine.Debug.LogWarning("'lbry-format' modules missing, running `npm i`");
        UnityEngine.Debug.Log(Path.GetDirectoryName(lbryFormatDir));
        ExecProcessShell(Path.GetDirectoryName(lbryFormatDir), "npm", "i");
      }

      ExecProcess(Path.GetDirectoryName(targetBuildRoot), "node", packageScript);
    }

    public static void ExecProcess(string directory, string name, string args)
    {
        Process p = new Process();

        p.StartInfo.FileName = name;
        p.StartInfo.Arguments = args;


        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.ErrorDialog = false;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.EnableRaisingEvents = true;
        p.OutputDataReceived += process_OutputDataReceived;
        p.ErrorDataReceived += process_ErrorDataReceived;
        p.Exited += process_Exited;

        //p.StartInfo.FileName = Path.GetFileName(fullPath);
        p.StartInfo.WorkingDirectory = directory;

        p.Start();

        void process_Exited(object sender, System.EventArgs e)
        {
            // do something when process terminates;
            //UnityEngine.Debug.Log("Exit");
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            UnityEngine.Debug.Log("process_OutputDataReceived");
            string s = e.Data;
            UnityEngine.Debug.Log(s);
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            UnityEngine.Debug.Log("process_ErrorDataReceived");
            string s = e.Data;
            UnityEngine.Debug.Log(s);
        }

        string log = p.StandardOutput.ReadToEnd();
        string errorLog = p.StandardError.ReadToEnd();

        if (log != "") {
          UnityEngine.Debug.Log(log);
        }
        if (errorLog != "") {
          UnityEngine.Debug.Log(errorLog);
        }

        p.WaitForExit();
        p.Close();
    }

    public static void ExecProcessShell(string directory, string name, string args)
    {
        Process p = new Process();

        p.StartInfo.FileName = name;
        p.StartInfo.Arguments = args;


        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.ErrorDialog = false;
        p.StartInfo.UseShellExecute = true;
        p.EnableRaisingEvents = true;
        p.Exited += process_Exited;

        //p.StartInfo.FileName = Path.GetFileName(fullPath);
        p.StartInfo.WorkingDirectory = directory;

        p.Start();

        void process_Exited(object sender, System.EventArgs e)
        {
            // do something when process terminates;
            //UnityEngine.Debug.Log("Exit");
        }

        p.WaitForExit();
        p.Close();
    }
}
