using lib.xqclient_base.logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace business_foundation_lib.performance_monitor
{
    public delegate void CurrentProcessPerformanceDataChanged(AppPerformanceData performanceData);

    /// <summary>
    /// 当前进程的性能检测器
    /// </summary>
    public class CurrentProcessPerformanceMonitor
    {
        private readonly TaskFactory monitorTaskFactory = new TaskFactory();
        private CancellationTokenSource monitorCLTS;
        private readonly object monitorCLTSLock = new object();
        
        /// <summary>
        /// 性能数据变化事件
        /// </summary>
        public event CurrentProcessPerformanceDataChanged PerformanceDataChanged;

        /// <summary>
        /// 开始检测
        /// </summary>
        public void Start()
        {
            Stop();
            PerformanceMonitoring(AcquireMonitorCLT());
        }

        /// <summary>
        /// 停止检测
        /// </summary>
        public void Stop()
        {
            CancelMonitor();
        }


        private CancellationToken AcquireMonitorCLT()
        {
            CancellationToken clt = CancellationToken.None;
            lock (monitorCLTSLock)
            {
                if (monitorCLTS == null || monitorCLTS.IsCancellationRequested)
                {
                    monitorCLTS = new CancellationTokenSource();
                }
                clt = monitorCLTS.Token;
            }
            return clt;
        }

        private void CancelMonitor()
        {
            lock (monitorCLTSLock)
            {
                if (monitorCLTS != null)
                {
                    monitorCLTS.Cancel();
                    monitorCLTS.Dispose();
                    monitorCLTS = null;
                }
            }
        }

        private void PerformanceMonitoring(CancellationToken clt)
        {
            monitorTaskFactory.StartNew(() =>
            {
                if (clt.IsCancellationRequested) return;

                Process reqProcess = Process.GetCurrentProcess();
                if (reqProcess == null) return;

                var processName = reqProcess.ProcessName;
                PerformanceCounter totalCpuUsageCounter = null,
                    processCpuUsageCounter = null,
                    workingSetMemoryCounter = null,
                    workingSetPeakMemoryCounter = null,
                    workingSetPrivateMemoryCounter = null,
                    availableRamMBytesCounter = null,
                    threadCountCounter = null;

                try
                {
                    totalCpuUsageCounter = new PerformanceCounter("Processor",
                    "% Processor Time", "_Total");
                    processCpuUsageCounter = new PerformanceCounter("Process",
                        "% Processor Time", processName);
                    workingSetMemoryCounter = new PerformanceCounter("Process",
                        "Working Set", processName);
                    workingSetPeakMemoryCounter = new PerformanceCounter("Process",
                        "Working Set Peak", processName);
                    workingSetPrivateMemoryCounter = new PerformanceCounter("Process",
                        "Working Set - Private", processName);
                    availableRamMBytesCounter = new PerformanceCounter("Memory",
                        "Available MBytes", true);
                    threadCountCounter = new PerformanceCounter("Process",
                        "Thread Count", processName);

                    // how much GC total use 
                    GC.GetTotalMemory(true);

                    while (!clt.IsCancellationRequested) //infinite loop
                    {
                        // Refresh the current process property values.
                        reqProcess.Refresh();

                        var performanceData = new AppPerformanceData();
                        performanceData.WorkingSetMemory = workingSetMemoryCounter?.NextValue() ?? -1;
                        performanceData.WorkingSetPrivateMemory = workingSetPrivateMemoryCounter?.NextValue() ?? -1;
                        performanceData.WorkingSetPeak = workingSetPeakMemoryCounter?.NextValue() ?? -1;
                        performanceData.AvailableMBytes = availableRamMBytesCounter?.NextValue() ?? -1;
                        performanceData.TotalCpuUsage = totalCpuUsageCounter?.NextValue() ?? -1;

                        var processCpuUsage = processCpuUsageCounter?.NextValue();
                        if (processCpuUsage != null)
                            processCpuUsage = processCpuUsage / System.Environment.ProcessorCount;
                        performanceData.CurrentProcessCpuUsage = processCpuUsage ?? -1;

                        performanceData.ThreadCount = (int)(threadCountCounter?.NextValue() ?? -1);

                        if (clt.IsCancellationRequested) return;

                        // Publish AppPerformanceUpdateEvent
                        PerformanceDataChanged?.Invoke(performanceData);

                        // give some time to accumulate data
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    AppLog.Error($"Performance Monitoring Statistics Exception: {ex.Message}");
                }
                finally
                {
                    totalCpuUsageCounter?.Dispose();
                    processCpuUsageCounter?.Dispose();
                    workingSetMemoryCounter?.Dispose();
                    workingSetPrivateMemoryCounter?.Dispose();
                    workingSetPeakMemoryCounter?.Dispose();
                    availableRamMBytesCounter?.Dispose();
                    threadCountCounter?.Dispose();
                }
            });
        }
    }
}
