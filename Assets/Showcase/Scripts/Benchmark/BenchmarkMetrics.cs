using System;

namespace SUIF.Showcase.Benchmark
{
    [Serializable]
    public sealed class BenchmarkMetrics
    {
        public float averageFps;
        public float minFps;
        public float maxFps;
        public float onePercentLowFps;
        public float averageFrameTimeMs;
        public float maxFrameTimeMs;
        public int totalFrames;
        public long currentDrawCalls;
        public long currentBatches;
        public float totalUsedMemoryMb;
        public float gcAllocatedMemoryMb;
        public float durationSeconds;
        public string status;
    }
}

