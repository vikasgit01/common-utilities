using GameManagers;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GameUtills
{
    public class Timer
    {
        CancellationTokenSource cancellationTokenSource;
        bool runTimer;
        float CurrentTime = 0;

        public Timer() => cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Works with Timer constructor
        /// </summary>
        /// <param name="timeInms"> delay time in milli sec</param>
        /// <param name="onTimerComplete"> callback on delay complete</param>
        public async void StartTimer(int timeInms = 1000, Action onTimerComplete = null)
        {
            LogsManager.Print($"Start - Tier");

            StopTimer();
            CancellationTokenSource source = new CancellationTokenSource();
            cancellationTokenSource = source;

            Task task = Task.Delay(timeInms, source.Token);

            await task;

            onTimerComplete?.Invoke();
            LogsManager.Print($"End - Timer");
        }

        /// <summary>
        /// Coroutine To run timer and get progress
        /// </summary>
        /// <param name="timeinSec">timer value</param>
        /// <param name="onTimerComplete">action after timer complete</param>
        /// <param name="runningTimer">action when timer is running</param>
        /// <returns></returns>
        public IEnumerator StartTimerCoroutine(float timeinSec, Action onTimerComplete = null, Action<float> runningTimer = null)
        {
            StopTimer();

            runTimer = true;
            CurrentTime = 0;

            while (runTimer && CurrentTime < timeinSec)
            {
                CurrentTime += Time.deltaTime;
                runningTimer?.Invoke(CurrentTime / timeinSec);
                yield return null;
            }

            onTimerComplete?.Invoke();
        }

        /// <summary>
        /// To end The Current Running Timer 
        /// Recommended to user to when needed to stop timer.
        /// </summary>
        public void StopTimer()
        {
            cancellationTokenSource?.Cancel();

            runTimer = false;
            CurrentTime = 0;
        }


        #region NOT TO USED STILL STUFF TO DO
        /// <summary>
        /// Currently Not Working Recommended Not To use.
        /// </summary>
        /// <param name="timeinSec">time to run in float</param>
        /// <param name="onTimerComplete">callback on timer complete</param>
        public async void StartTimer(float timeinSec = 1f, Action onTimerComplete = null, Action<float> runningTimer = null, int id = 0)
        {
            //StopTimer();
            runTimer = true;
            CurrentTime = 0;
            //LogsManager.Print($"runTimer : {runTimer}, Condition : {CurrentTime < timeinSec}, timeinSec : {timeinSec}, CurrentTimer : {CurrentTime}");

            LogsManager.Print($"Condition : {runTimer}");
            while (runTimer && CurrentTime < timeinSec)
            {
                //LogsManager.Print($"CurrentTime : {CurrentTime}, timeinSec : {timeinSec}, deltaTime {Time.deltaTime}");
                CurrentTime += Time.deltaTime;
                LogsManager.Print($"CurrentTime {id}: {CurrentTime}");
                runningTimer?.Invoke(CurrentTime / timeinSec);
                LogsManager.Print($"Condition {id} : {runTimer}");
                await Task.Yield();
                LogsManager.Print($"Condition {id} : {runTimer}");
            }

            LogsManager.Print($"Condition : {runTimer}");
            LogsManager.Print($"Condition : {(CurrentTime < timeinSec)}");
            onTimerComplete?.Invoke();
        }
        #endregion
    }
}
