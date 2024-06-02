using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;
using TMPro;
using DesignPatterns;

namespace GameUtills
{
    public class GameStatsManager : Singleton<GameStatsManager>
    {
        [SerializeField] private GameStatsScriptable _scriptable;
        [SerializeField] private TMP_Text _stats;
        [SerializeField] private GameStatsFpsCounter _fpsCounter;

        ProfilerRecorder systemMemoryRecorder;
        ProfilerRecorder gcMemoryRecorder;
        ProfilerRecorder mainThreadTimeRecorder;
        ProfilerRecorder drawCallsCountRecorder;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        static double GetRecorderFrameAverage(ProfilerRecorder recorder)
        {
            var samplesCount = recorder.Capacity;
            if (samplesCount == 0)
                return 0;

            double r = 0;
            var samples = new List<ProfilerRecorderSample>(samplesCount);
            recorder.CopyTo(samples);

            for (var i = 0; i < samples.Count; ++i)
                r += samples[i].Value;

            r /= samplesCount;

            return r;
        }

        void OnEnable()
        {
            systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
            gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
            mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
            drawCallsCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
        }

        void OnDisable()
        {
            systemMemoryRecorder.Dispose();
            gcMemoryRecorder.Dispose();
            mainThreadTimeRecorder.Dispose();
            drawCallsCountRecorder.Dispose();
        }

        void Update()
        {
            var sb = new StringBuilder(500);

            if (_scriptable)
            {
                if (_scriptable.GetFrameRate() && _fpsCounter)
                    sb.AppendLine($"{_fpsCounter.OnUpdate()} FPS");

                if (_scriptable.GetDrawCalls())
                    sb.AppendLine($"Draw Calls: {drawCallsCountRecorder.LastValue}");

                if (_scriptable.GetFrameTime())
                    sb.AppendLine($"CPU : {GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f):F1} ms");

                if (_scriptable.GetGCMemory())
                    sb.AppendLine($"GC Memory: {gcMemoryRecorder.LastValue / (1024 * 1024)} MB");

                if (_scriptable.GetSystemMemory())
                    sb.AppendLine($"System Memory: {systemMemoryRecorder.LastValue / (1024 * 1024)} MB");
            }

            _stats.text = sb.ToString();
        }

    }
}
