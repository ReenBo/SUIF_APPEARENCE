using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SUIF.Editor.Build
{
    public static class WebGLBuilder
    {
        private const string DefaultOutputPath = "Builds/WebGL";

        [MenuItem("SUIF/Build/WebGL (Release)", priority = 100)]
        public static void BuildRelease()
        {
            PerformBuild(BuildOptions.None);
        }

        [MenuItem("SUIF/Build/WebGL (Development)", priority = 101)]
        public static void BuildDevelopment()
        {
            PerformBuild(BuildOptions.Development | BuildOptions.AllowDebugging);
        }

        public static void BuildFromCommandLine()
        {
            bool isDevelopment = HasCommandLineArgument("-developmentBuild");
            BuildOptions options = isDevelopment
                ? (BuildOptions.Development | BuildOptions.AllowDebugging)
                : BuildOptions.None;

            bool success = PerformBuild(options);

            if (Application.isBatchMode)
            {
                EditorApplication.Exit(success ? 0 : 1);
            }
        }

        public static bool PerformBuild(BuildOptions buildOptions)
        {
            string outputPath = ResolveOutputPath();

            EnsureTargetPlatform();
            ConfigureWebGLPlayerSettings(buildOptions);

            if (!BuildAddressablesContent())
            {
                Debug.LogError("[WebGLBuilder] Addressables build failed. Aborting player build.");
                return false;
            }

            string[] scenes = GetBuildScenePaths();
            if (scenes.Length == 0)
            {
                Debug.LogError("[WebGLBuilder] No enabled scenes found in EditorBuildSettings.");
                return false;
            }

            BuildPlayerOptions playerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = outputPath,
                target = BuildTarget.WebGL,
                options = buildOptions
            };

            BuildReport report = BuildPipeline.BuildPlayer(playerOptions);
            bool success = report.summary.result == BuildResult.Succeeded;

            if (success)
            {
                Debug.Log($"[WebGLBuilder] WebGL build succeeded at: {outputPath} ({report.summary.totalSize / (1024 * 1024)} MB)");
                GenerateWebDeploymentArtifacts(outputPath);
            }
            else
            {
                Debug.LogError($"[WebGLBuilder] WebGL build failed with result: {report.summary.result}. Errors: {report.summary.totalErrors}");
            }

            return success;
        }

        private static bool BuildAddressablesContent()
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("[WebGLBuilder] AddressableAssetSettings not found in project.");
                return false;
            }

            Debug.Log("[WebGLBuilder] Building Addressables player content...");
            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);

            if (!string.IsNullOrEmpty(result.Error))
            {
                Debug.LogError($"[WebGLBuilder] Addressables build error: {result.Error}");
                return false;
            }

            Debug.Log("[WebGLBuilder] Addressables content successfully built.");
            return true;
        }

        private static void EnsureTargetPlatform()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
            {
                Debug.Log("[WebGLBuilder] Switching active build target to WebGL...");
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            }
        }

        private static void ConfigureWebGLPlayerSettings(BuildOptions options)
        {
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
            PlayerSettings.WebGL.decompressionFallback = true;
            
            bool isDev = (options & BuildOptions.Development) != 0;
            PlayerSettings.WebGL.exceptionSupport = isDev 
                ? WebGLExceptionSupport.FullWithStacktrace 
                : WebGLExceptionSupport.FullWithoutStacktrace;
        }

        private static string[] GetBuildScenePaths()
        {
            return EditorBuildSettings.scenes
                .Where(scene => scene.enabled && !string.IsNullOrEmpty(scene.path))
                .Select(scene => scene.path)
                .ToArray();
        }

        private static string ResolveOutputPath()
        {
            string customPath = GetCommandLineArgumentValue("-outputFolder");
            if (!string.IsNullOrEmpty(customPath))
            {
                return customPath;
            }

            return DefaultOutputPath;
        }

        private static bool HasCommandLineArgument(string argumentName)
        {
            return Environment.GetCommandLineArgs().Any(arg => string.Equals(arg, argumentName, StringComparison.OrdinalIgnoreCase));
        }

        private static string GetCommandLineArgumentValue(string argumentName)
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (string.Equals(args[i], argumentName, StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }

            return null;
        }

        private static void GenerateWebDeploymentArtifacts(string outputDirectory)
        {
            GenerateCloudflareHeaders(outputDirectory);
            GenerateNoJekyllMarker(outputDirectory);
        }

        private static void GenerateCloudflareHeaders(string outputDirectory)
        {
            string headersFilePath = Path.Combine(outputDirectory, "_headers");
            string headersContent = @"/*
  Cross-Origin-Opener-Policy: same-origin
  Cross-Origin-Embedder-Policy: require-corp
  Access-Control-Allow-Origin: *

/*.wasm.br
  Content-Type: application/wasm
  Content-Encoding: br

/*.wasm.gz
  Content-Type: application/wasm
  Content-Encoding: gzip

/*.js.br
  Content-Type: application/javascript
  Content-Encoding: br

/*.js.gz
  Content-Type: application/javascript
  Content-Encoding: gzip

/*.data.br
  Content-Type: application/octet-stream
  Content-Encoding: br

/*.data.gz
  Content-Type: application/octet-stream
  Content-Encoding: gzip

/*.symbols.json.br
  Content-Type: application/octet-stream
  Content-Encoding: br
";

            File.WriteAllText(headersFilePath, headersContent);
            Debug.Log($"[WebGLBuilder] Generated Cloudflare _headers file at: {headersFilePath}");
        }

        private static void GenerateNoJekyllMarker(string outputDirectory)
        {
            string noJekyllPath = Path.Combine(outputDirectory, ".nojekyll");
            if (!File.Exists(noJekyllPath))
            {
                File.WriteAllText(noJekyllPath, string.Empty);
            }
        }
    }
}

