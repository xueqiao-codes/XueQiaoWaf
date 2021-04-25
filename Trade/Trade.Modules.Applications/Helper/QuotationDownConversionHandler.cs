using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace XueQiaoWaf.Trade.Modules.Applications.Helper
{
    /// <summary>
    /// 已到时间去处理某项行情
    /// </summary>
    /// <typeparam name="TTargetKey">行情标的 key 类型</typeparam>
    /// <typeparam name="TQuotModel">行情信息 model 类型</typeparam>
    /// <param name="handler">处理器</param>
    /// <param name="time2HandleQuots">要处理的最新行情</param>
    public delegate void Time2HandleQuotationUIItemAction<TTargetKey, TQuotModel>(QuotationDownConversionHandler<TTargetKey, TQuotModel> handler, Dictionary<TTargetKey, TQuotModel> time2HandleQuots);

    /// <summary>
    /// 行情降频处理器基类。提供了降频处理的定时功能
    /// </summary>
    /// <typeparam name="TTargetKey">行情标的 key 类型</typeparam>
    /// <typeparam name="TQuotModel">行情信息 model 类型</typeparam>
    public class QuotationDownConversionHandler<TTargetKey, TQuotModel> : IDisposable
    {
        private readonly double handleInterval;

        private readonly Dictionary<TTargetKey, TQuotModel> lastQuotDict = new Dictionary<TTargetKey, TQuotModel>();
        private readonly object lastQuotDictLock = new object();

        private System.Timers.Timer handleTimer;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="quotationHandleInterval">行情处理时间间隔</param>
        public QuotationDownConversionHandler(double quotationHandleInterval = 500)
        {
            if (quotationHandleInterval <= 0) throw new ArgumentException("quotationInterval must > 0");
            this.handleInterval = quotationHandleInterval;
        }

        ~QuotationDownConversionHandler()
        {
            Dispose(false);
        }

        /// <summary>
        /// 已到时间去处理行情的处理方法。
        /// </summary>
        public Time2HandleQuotationUIItemAction<TTargetKey, TQuotModel> Time2HandleQuotation;

        /// <summary>
        /// 启动处理器
        /// </summary>
        public void Start()
        {
            if (isDisposed == true) return;
            DisposeHandleTimer();
            handleTimer = new System.Timers.Timer(handleInterval);
            handleTimer.Elapsed += __OnHandleTimerElapsed;
            handleTimer.Start();
        }

        /// <summary>
        /// 停止处理器
        /// </summary>
        public void Stop()
        {
            if (isDisposed == true) return;
            ClearLastQuotDict();
            DisposeHandleTimer();
        }
        
        /// <summary>
        /// 更新某个标的的最新行情
        /// </summary>
        /// <param name="key">行情标的</param>
        /// <param name="lastQuotation">标的的最新行情</param>
        public void UpdateLastQuotation(TTargetKey key, TQuotModel lastQuotation)
        {
            if (isDisposed == true) return;
            lock (lastQuotDictLock)
            {
                lastQuotDict[key] = lastQuotation;
            }
        }

        private Dictionary<TTargetKey, TQuotModel> TryClearLastQuotDict()
        {
            Dictionary<TTargetKey, TQuotModel> rmDict = null;
            lock (lastQuotDictLock)
            {
                rmDict = lastQuotDict.ToDictionary(entry => entry.Key, entry => entry.Value);
                lastQuotDict.Clear();
            }
            return rmDict;
        }

        private void ClearLastQuotDict()
        {
            lock (lastQuotDictLock)
            {
                lastQuotDict.Clear();
            }
        }

        private void DisposeHandleTimer()
        {
            if (handleTimer != null)
            {
                handleTimer.Stop();
                handleTimer.Elapsed -= __OnHandleTimerElapsed;
                handleTimer.Dispose();
                handleTimer = null;
            }
        }
        
        private void __OnHandleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var waitHandleDict = TryClearLastQuotDict();
            if (waitHandleDict?.Count > 0)
            {
                Time2HandleQuotation?.Invoke(this, waitHandleDict);
            }
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool isDisposed = false;
        private void Dispose(bool isDisposing)
        {
            if (isDisposed) return;
            if (isDisposing)
            {
                DisposeHandleTimer();
            }
        }
        #endregion
    }
}
