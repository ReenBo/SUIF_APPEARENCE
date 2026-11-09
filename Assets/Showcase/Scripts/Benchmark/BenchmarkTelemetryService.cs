using System;
using global::R3;
using Unity.Profiling;
using UnityEngine;
using VContainer.Unity;

namespace SUIF.Showcase.Benchmark
{
    public sealed class BenchmarkTelemetryService : ITickable, IDisposable
    {
        private const int MaxSampleCapacity = 15000;
        private const float SmoothingInterval = 0.5f;

        private readonly ReactiveProperty<float> _currentFps = new(60f);
        private readonly ReactiveProperty<long> _drawCalls = new(0);
        private readonly ReactiveProperty<long> _batches = new(0);
        private readonly ReactiveProperty<float> _memoryMb = new(0f);
        private readonly ReactiveProperty<bool> _isRecording = new(false);

        private readonly float[] _frameSamples = new float[MaxSampleCapacity];
        private int _sampleCount;
        private float _recordingStartTime;

        private float _timeAccumulator;
        private int _framesAccumulator;

        private ProfilerRecorder _drawCallsRecorder;
        private ProfilerRecorder _batchesRecorder;
        private ProfilerRecorder _totalMemoryRecorder;
        private ProfilerRecorder _gcMemoryRecorder;

        public ReadOnlyReactiveProperty<float> CurrentFps => _currentFps;
        public ReadOnlyReactiveProperty<long> DrawCalls => _drawCalls;
        public ReadOnlyReactiveProperty<long> Batches => _batches;
        public ReadOnlyReactiveProperty<float> MemoryMb => _memoryMb;
        public ReadOnlyReactiveProperty<bool> IsRecording => _isRecording;

        public BenchmarkTelemetryService()
        {
            InitializeRecorders();
        }

        private void InitializeRecorders()
        {
            _drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
            _batchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
            _totalMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
            _gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocated In Frame");
        }

        public void Tick()
        {
            float dt = Time.unscaledDeltaTime;
            if (dt <= 0.00001f)
            {
                return;
            }

            _timeAccumulator += dt;
            _framesAccumulator++;

            if (_timeAccumulator >= SmoothingInterval)
            {
                _currentFps.Value = _framesAccumulator / _timeAccumulator;
                _timeAccumulator = 0f;
                _framesAccumulator = 0;

                UpdateSystemMetrics();
            }

            if (_isRecording.Value && _sampleCount < MaxSampleCapacity)
            {
                _frameSamples[_sampleCount++] = dt;
            }
        }

        private void UpdateSystemMetrics()
        {
            if (_drawCallsRecorder.Valid)
            {
                _drawCalls.Value = _drawCallsRecorder.LastValue;
            }

            if (_batchesRecorder.Valid)
            {
                _batches.Value = _batchesRecorder.LastValue;
            }

            if (_totalMemoryRecorder.Valid)
            {
                _memoryMb.Value = _totalMemoryRecorder.LastValue / (1024f * 1024f);
            }
            else
            {
                _memoryMb.Value = GC.GetTotalMemory(false) / (1024f * 1024f);
            }
        }

        public void StartRecording()
        {
            _sampleCount = 0;
            _recordingStartTime = Time.unscaledTime;
            _isRecording.Value = true;
        }

        public BenchmarkMetrics StopRecording()
        {
            _isRecording.Value = false;
            float totalDuration = Mathf.Max(Time.unscaledTime - _recordingStartTime, 0.001f);

            var metrics = new BenchmarkMetrics
            {
                totalFrames = _sampleCount,
                durationSeconds = totalDuration,
                currentDrawCalls = _drawCalls.Value,
                currentBatches = _batches.Value,
                totalUsedMemoryMb = _memoryMb.Value,
                gcAllocatedMemoryMb = _gcMemoryRecorder.Valid ? _gcMemoryRecorder.LastValue / (1024f * 1024f) : 0f
            };

            if (_sampleCount == 0)
            {
                metrics.status = "NO_DATA";
                return metrics;
            }

            float totalFrameTime = 0f;
            float minFrameTime = float.MaxValue;
            float maxFrameTime = 0f;

            for (int i = 0; i < _sampleCount; i++)
            {
                float frame = _frameSamples[i];
                totalFrameTime += frame;

                if (frame < minFrameTime) minFrameTime = frame;
                if (frame > maxFrameTime) maxFrameTime = frame;
            }

            Array.Sort(_frameSamples, 0, _sampleCount);

            int worstOnePercentIndex = Mathf.Clamp((int)(_sampleCount * 0.99f), 0, _sampleCount - 1);
            float onePercentWorstFrameTime = _frameSamples[worstOnePercentIndex];

            metrics.averageFrameTimeMs = (totalFrameTime / _sampleCount) * 1000f;
            metrics.maxFrameTimeMs = maxFrameTime * 1000f;
            metrics.averageFps = _sampleCount / totalDuration;
            metrics.minFps = maxFrameTime > 0.0001f ? 1f / maxFrameTime : 0f;
            metrics.maxFps = minFrameTime > 0.0001f ? 1f / minFrameTime : 0f;
            metrics.onePercentLowFps = onePercentWorstFrameTime > 0.0001f ? 1f / onePercentWorstFrameTime : 0f;
            metrics.status = "COMPLETED";

            return metrics;
        }

        public void Dispose()
        {
            _drawCallsRecorder.Dispose();
            _batchesRecorder.Dispose();
            _totalMemoryRecorder.Dispose();
            _gcMemoryRecorder.Dispose();

            _currentFps.Dispose();
            _drawCalls.Dispose();
            _batches.Dispose();
            _memoryMb.Dispose();
            _isRecording.Dispose();
        }
    }
}

