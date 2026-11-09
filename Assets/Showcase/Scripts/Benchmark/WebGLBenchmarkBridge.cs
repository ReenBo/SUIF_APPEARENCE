using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SUIF.Showcase.Benchmark
{
    public static class WebGLBenchmarkBridge
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SUIF_EmitBenchmarkComplete(string jsonStrPtr);

        [DllImport("__Internal")]
        private static extern string SUIF_GetUrlSearchParam(string paramNamePtr);
#endif

        public static void ReportComplete(string jsonReport)
        {
            if (string.IsNullOrEmpty(jsonReport))
            {
                return;
            }

#if UNITY_WEBGL && !UNITY_EDITOR
            try
            {
                SUIF_EmitBenchmarkComplete(jsonReport);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[WebGLBenchmarkBridge] Failed to emit benchmark result: {ex.Message}");
            }
#else
            Debug.Log($"[WebGLBenchmarkBridge] Benchmark Complete Report:\n{jsonReport}");
#endif
        }

        public static string GetQueryParameter(string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                return null;
            }

#if UNITY_WEBGL && !UNITY_EDITOR
            try
            {
                return SUIF_GetUrlSearchParam(parameterName);
            }
            catch
            {
                return null;
            }
#else
            string[] args = Environment.GetCommandLineArgs();
            string prefix = $"-{parameterName}=";
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return args[i].Substring(prefix.Length);
                }

                if (string.Equals(args[i], $"-{parameterName}", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    return args[i + 1];
                }
            }

            return null;
#endif
        }
    }
}

