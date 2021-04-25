using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.BusinessResources.Helpers
{
    /// <summary>
    /// 雪橇声音
    /// </summary>
    public class XQSound
    {
        private readonly SoundPlayer soundPlayer;

        public XQSound(Stream stream)
        {
            soundPlayer = new SoundPlayer(stream);
        }

        public XQSound(string soundLocation)
        {
            soundPlayer = new SoundPlayer(soundLocation);
        }

        public void SetStream(Stream stream)
        {
            soundPlayer.Stop();
            soundPlayer.Stream = stream;
        }

        public void SetSoundLocation(string soundLocation)
        {
            soundPlayer.Stop();
            soundPlayer.SoundLocation = soundLocation;
        }

        public void Play()
        {
            soundPlayer.Stop();
            soundPlayer.Play();
        }

        public void Stop()
        {
            soundPlayer.Stop();
        }
    }
}
