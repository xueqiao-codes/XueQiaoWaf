using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_foundation_lib.performance_monitor
{
    /// <summary>
    /// app 性能数据
    /// </summary>
    public class AppPerformanceData
    {
        // 单位:byte
        public float WorkingSetMemory { get; set; }

        // 单位:byte
        public float WorkingSetPrivateMemory { get; set; }

        // 单位:byte
        public float WorkingSetPeak { get; set; }

        // 单位: MByte
        public float AvailableMBytes { get; set; }

        // 当前进程的 cpu 使用率（在0-100之间）
        public float CurrentProcessCpuUsage { get; set; }

        // 整台电脑的 cpu 使用率（在0-100之间）
        public float TotalCpuUsage { get; set; }

        // 线程数
        public int ThreadCount { get; set; }
    }
}
