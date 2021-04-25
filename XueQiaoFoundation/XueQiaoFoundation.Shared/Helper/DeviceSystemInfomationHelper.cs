using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public static class DeviceSystemInfomationHelper
    {
        public static bool IsNetworkAvailable()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        public static DeviceSystemInformation DeviceSystemInformation()
        {
            var info = new DeviceSystemInformation();

            GetSystemInformation(out string manufacturer, out string deviceModel, 
                out int totalPhysicalMemoryInGB, out string osVersion, out bool Is64BitOS, out int processorCount);
            info.Manufacturer = manufacturer;
            info.DeviceModel = deviceModel;
            info.TotalPhysicalMemoryInGB = totalPhysicalMemoryInGB;
            info.OSVersion = osVersion;
            info.Is64BitOS = Is64BitOS;
            info.ProcessorCount = processorCount;

            GetCpuInfo(out string cpuInfo);
            info.CpuInfo = cpuInfo;

            GetVideoControllerInfo(out string videoCtrlInfo);
            info.VideoControllerInfo = videoCtrlInfo;

            return info;
        }

        private static void GetSystemInformation(out string _manufacturer, out string _deviceModel,
            out int _totalPhysicalMemoryInGB,
            out string _osVersion, out bool _Is64BitOS, 
            out int _processorCount)
        {
            System.Management.SelectQuery query = new System.Management.SelectQuery(@"Select * from Win32_ComputerSystem");
            string manufacturerStr = null;
            string modelStr = null;
            long? totalPhysicalMemory = null;
            using (System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(query))
            {
                foreach (System.Management.ManagementObject process in searcher.Get())
                {
                    process.Get();
                    manufacturerStr = process["Manufacturer"]?.ToString();
                    modelStr = process["Model"]?.ToString();
                    try
                    {
                        totalPhysicalMemory = System.Convert.ToInt64(process["TotalPhysicalMemory"]);
                    }
                    catch (Exception) { }
                }
            }

            _manufacturer = manufacturerStr;
            _deviceModel = modelStr;
            if (totalPhysicalMemory.HasValue)
            {
                _totalPhysicalMemoryInGB = (int)(totalPhysicalMemory.Value / (1024 * 1024 * 1024));
            }
            else { _totalPhysicalMemoryInGB = 0; }

            _osVersion = Environment.OSVersion.ToString();
            _Is64BitOS = Environment.Is64BitOperatingSystem;
            _processorCount = Environment.ProcessorCount;
        }

        private static void GetCpuInfo(out string _cpu)
        {
            System.Management.SelectQuery query = new System.Management.SelectQuery(@"Select * from Win32_Processor");
            string cpuStr = null;
            using (System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(query))
            {
                foreach (System.Management.ManagementObject process in searcher.Get())
                {
                    process.Get();
                    cpuStr = process["Name"]?.ToString();
                }
            }
            _cpu = cpuStr;
        }

        private static void GetVideoControllerInfo(out string _caption)
        {
            System.Management.SelectQuery query = new System.Management.SelectQuery(@"Select * from Win32_VideoController");
            string captionStr = null;
            using (System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(query))
            {
                foreach (System.Management.ManagementObject process in searcher.Get())
                {
                    process.Get();
                    captionStr = process["Name"]?.ToString();
                }
            }
            _caption = captionStr;
        }
    }
    
    public struct DeviceSystemInformation
    {
        public string Manufacturer { get; set; }
        public string DeviceModel { get; set; }
        public int TotalPhysicalMemoryInGB { get; set; }
        public string OSVersion { get; set; }
        public bool Is64BitOS { get; set; }
        public int ProcessorCount { get; set; }
        public string CpuInfo { get; set; }
        public string VideoControllerInfo { get; set; }

        public override string ToString()
        {
            return $"DeviceSystemInformation{{Manufacturer:{Manufacturer}, " +
                $"DeviceModel:{DeviceModel}, " +
                $"TotalPhysicalMemoryInGB:{TotalPhysicalMemoryInGB}, " +
                $"OSVersion:{OSVersion}, " +
                $"Is64BitOS:{Is64BitOS}, " +
                $"ProcessorCount:{ProcessorCount}, " +
                $"CpuInfo:{CpuInfo}, " +
                $"VideoControllerInfo:{VideoControllerInfo}}}";
        }
    }
}
