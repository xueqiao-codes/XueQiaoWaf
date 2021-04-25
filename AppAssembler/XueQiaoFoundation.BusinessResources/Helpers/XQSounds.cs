using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Helpers
{
    /// <summary>
    /// 雪橇声音管理器
    /// </summary>
    public static class XQSoundManager
    {
        private static object soundsLock = new object();
        private static HashSet<XQSound> sounds = new HashSet<XQSound>();
        
        /// <summary>
        /// 播放某个声音
        /// </summary>
        /// <param name="sound"></param>
        public static void PlaySound(XQSound sound)
        {
            StopAllSounds();
            if (sound == null) return;
            lock (soundsLock)
            {
                sounds.Add(sound);
                sound.Play();
            }
        }

        /// <summary>
        /// 停止播放某个声音
        /// </summary>
        /// <param name="sound"></param>
        public static void StopSound(XQSound sound)
        {
            if (sound == null) return;
            lock (soundsLock)
            {
                sounds.Remove(sound);
                sound.Stop();
            }
        }

        /// <summary>
        /// 停止播放所有声音
        /// </summary>
        private static void StopAllSounds()
        {
            lock (soundsLock)
            {
                foreach (var sound in sounds.ToArray())
                {
                    sounds.Remove(sound);
                    sound.Stop();
                }
            }
        }

        private static XQSound _orderOccurException;

        /// <summary>
        /// 订单异常提醒声音
        /// </summary>
        public static XQSound OrderOccurException
        {
            get
            {
                if (_orderOccurException == null)
                {
                    var a = System.Reflection.Assembly.GetExecutingAssembly();
                    var fileName = a.GetManifestResourceNames().FirstOrDefault(i => i.EndsWith(".Sounds.Error.wav"));
                    Stream s = a.GetManifestResourceStream(fileName);
                    _orderOccurException = new XQSound(s);
                }
                return _orderOccurException;
            }
        }

        private static XQSound _orderStateAmbiguous;

        /// <summary>
        /// 订单状态不明确提醒声音
        /// </summary>
        public static XQSound OrderStateAmbiguous
        {
            get
            {
                if (_orderStateAmbiguous == null)
                {
                    var a = System.Reflection.Assembly.GetExecutingAssembly();
                    var fileName = a.GetManifestResourceNames().FirstOrDefault(i => i.EndsWith(".Sounds.Error.wav"));
                    Stream s = a.GetManifestResourceStream(fileName);
                    _orderStateAmbiguous = new XQSound(s);
                }
                return _orderStateAmbiguous;
            }
        }
        
        private static XQSound _orderTriggered;

        /// <summary>
        /// 订单已触发提醒声音
        /// </summary>
        public static XQSound OrderTriggered
        {
            get
            {
                if (_orderTriggered == null)
                {
                    var a = System.Reflection.Assembly.GetExecutingAssembly();
                    var fileName = a.GetManifestResourceNames().FirstOrDefault(i => i.EndsWith(".Sounds.Trade.wav"));
                    Stream s = a.GetManifestResourceStream(fileName);
                    _orderTriggered = new XQSound(s);
                }
                return _orderTriggered;
            }
        }

        private static XQSound _orderTraded;

        /// <summary>
        /// 订单已成交提醒声音
        /// </summary>
        public static XQSound OrderTraded
        {
            get
            {
                if (_orderTraded == null)
                {
                    var a = System.Reflection.Assembly.GetExecutingAssembly();
                    var fileName = a.GetManifestResourceNames().FirstOrDefault(i => i.EndsWith(".Sounds.Trade2.wav"));
                    Stream s = a.GetManifestResourceStream(fileName);
                    _orderTraded = new XQSound(s);
                }
                return _orderTraded;
            }
        }

        private static XQSound _composeLameTraded;

        /// <summary>
        /// 雪橇组合瘸腿提醒声音
        /// </summary>
        public static XQSound ComposeLameTraded
        {
            get
            {
                if (_composeLameTraded == null)
                {
                    var a = System.Reflection.Assembly.GetExecutingAssembly();
                    var fileName = a.GetManifestResourceNames().FirstOrDefault(i => i.EndsWith(".Sounds.Error.wav"));
                    Stream s = a.GetManifestResourceStream(fileName);
                    _composeLameTraded = new XQSound(s);
                }
                return _composeLameTraded;
            }
        }


        private static string ManifestResource_SoundsDir
        {
            get
            {
                var a = System.Reflection.Assembly.GetExecutingAssembly();
                return $"{a.GetName().Name}.Sounds";
            }
        }
    }
}
