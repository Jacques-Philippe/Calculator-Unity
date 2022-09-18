using UnityEngine;

using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace Calculator.AppManagement.Editor
{

    /// <summary>
    /// This script exposes two functions to build the project, as well as its addressables
    /// </summary>
    public class BuildScripts : MonoBehaviour
    {
        /// <summary>
        /// The list of scenes to be included in the Unity project build
        /// </summary>
        public static readonly string[] Scenes = { "Assets/Calculator/Scenes/Main.unity", };

        /// <summary>
        /// The output directory for the build.
        /// </summary>
        /// <remarks>
        /// Make sure to update the Azure Pipeline configuration
        /// if you change this.
        /// </remarks>
        private const string OUT_DIRECTORY = "Builds";

        /// <summary>
        /// The standard build options for both the client and server builds.
        /// </summary>
        private const BuildOptions DEFAULT_BUILD_OPTIONS = BuildOptions.StrictMode;

        [MenuItem("CICD/Build Addressables")]
        /// <summary>
        /// Build Addressable (required for localization package)
        /// </summary>
        /// <remarks>
        /// This function is invoked as part of the main Build function
        /// </remarks>
        private static void sBuildAddressables()
        {
            // Build addressables - needed for localization package
            Debug.Log("Start building player content (Addressables)");
            string assetPath = AddressableAssetSettingsDefaultObject.DefaultAssetPath;
            AddressableAssetProfileSettings profileSettings = AssetDatabase
                .LoadAssetAtPath<AddressableAssetSettings>(assetPath)
                .profileSettings;
            string profileId = profileSettings.GetProfileId("Default");
            AddressableAssetSettingsDefaultObject.Settings.activeProfileId = profileId;

            AddressableAssetSettings.BuildPlayerContent();
        }

        [MenuItem("CICD/Build Project (Dev)")]
        /// <summary>
        /// Build the HoloLens app for dev from the Unity Editor
        /// </summary>
        public static bool sBuildDev()
        {
            sBuildAddressables();

            var options = new BuildPlayerOptions
            {
                scenes = Scenes,
                locationPathName = OUT_DIRECTORY,
                targetGroup = BuildTargetGroup.WSA,
                target = BuildTarget.WSAPlayer,
                options = DEFAULT_BUILD_OPTIONS
            };

            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WSA, "DEV_BUILD");

            var success = sBuild(options);
            Debug.Log($"{(success ? "Build successful" : "Build unsuccessful")}");
            return success;
        }

        /// <summary>
        /// Build the HoloLens app for dev from the CICD pipeline
        /// </summary>
        public static void sBuildDevCICD()
        {
            var success = sBuildDev();

            // We need to explicitly kill the application when done
            // otherwise the Unity Build task in Azure Pipelines
            // will hang
            EditorApplication.Exit(success ? 0 : 1);
        }

        ///// <summary>
        ///// Build the client (Hololens) app for production.
        ///// </summary>
        //public static void sBuildProd()
        //{
        //    sBuildAddressables();

        //    var options = new BuildPlayerOptions
        //    {
        //        scenes = Scenes,
        //        locationPathName = OUT_DIRECTORY,
        //        targetGroup = BuildTargetGroup.WSA,
        //        target = BuildTarget.WSAPlayer,
        //        options = DEFAULT_BUILD_OPTIONS
        //    };

        //    PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WSA, "PROD_BUILD");

        //    var success = sBuild(options);

        //    // We need to explicitly kill the application when done
        //    // otherwise the Unity Build task in Azure Pipelines
        //    // will hang
        //    EditorApplication.Exit(success ? 0 : 1);
        //}

        ///// <summary>
        ///// Build the client (Hololens) app for staging.
        ///// </summary>
        //public static void sBuildStaging()
        //{
        //    sBuildAddressables();

        //    var options = new BuildPlayerOptions
        //    {
        //        scenes = Scenes,
        //        locationPathName = OUT_DIRECTORY,
        //        targetGroup = BuildTargetGroup.WSA,
        //        target = BuildTarget.WSAPlayer,
        //        options = DEFAULT_BUILD_OPTIONS
        //    };

        //    PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WSA, "STAGING_BUILD");

        //    var success = sBuild(options);

        //    // We need to explicitly kill the application when done
        //    // otherwise the Unity Build task in Azure Pipelines
        //    // will hang
        //    EditorApplication.Exit(success ? 0 : 1);
        //}

        ///// <summary>
        ///// Build the client (Hololens) app for uat.
        ///// </summary>
        //public static void sBuildUAT()
        //{
        //    sBuildAddressables();

        //    var options = new BuildPlayerOptions
        //    {
        //        scenes = Scenes,
        //        locationPathName = OUT_DIRECTORY,
        //        targetGroup = BuildTargetGroup.WSA,
        //        target = BuildTarget.WSAPlayer,
        //        options = DEFAULT_BUILD_OPTIONS
        //    };

        //    PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WSA, "UAT_BUILD");

        //    var success = sBuild(options);

        //    // We need to explicitly kill the application when done
        //    // otherwise the Unity Build task in Azure Pipelines
        //    // will hang
        //    EditorApplication.Exit(success ? 0 : 1);
        //}

        /// <summary>
        /// Build the Unity project with the specified options. Outputs a Visual Studio
        /// UWP project to the specified directory
        /// </summary>
        /// <remarks>
        /// Based on the example code in https://docs.unity3d.com/ScriptReference/BuildPipeline.BuildPlayer.html
        /// </remarks>
        /// <param name="options">The options for the build</param>
        private static bool sBuild(BuildPlayerOptions options)
        {
            Debug.Log(
                $"Building {options.locationPathName} for target {options.target} with options {options.options}"
            );

            BuildReport report = BuildPipeline.BuildPlayer(options);
            BuildSummary summary = report.summary;

            bool success = false;

            switch (summary.result)
            {
                case BuildResult.Succeeded:
                    success = true;
                    Debug.Log(
                        $"Build succeeded - {options.locationPathName} for target {options.target} with options {options.options}"
                    );
                    break;
                case BuildResult.Unknown: // fall-through to failed case
                case BuildResult.Failed:
                    Debug.Log(
                        $"Build failed - {options.locationPathName} for target {options.target} with options {options.options}"
                    );
                    break;
                case BuildResult.Cancelled:
                    Debug.Log("Build cancelled");
                    break;
            }

            return success;
        }
    }
}
