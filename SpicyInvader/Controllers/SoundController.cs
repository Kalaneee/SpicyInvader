using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace SpicyInvader.Controllers
{
    public class SoundController
    {
        private bool _isLooping;
        private bool _isBeingPlayed;
        private readonly string _fileName;

        public SoundController(string fileName)
        {
            _isLooping = false;
            _isBeingPlayed = false;
            this._fileName = "Resources\\" + fileName;
        }

        private void PlayWorker()
        {
            StringBuilder sb = new StringBuilder();
            int result = mciSendString("open \"" + _fileName + "\" type waveaudio  alias " + this._fileName, sb, 0, IntPtr.Zero);
            mciSendString("play " + this._fileName, sb, 0, IntPtr.Zero);
            _isBeingPlayed = true;
            sb = null;
            sb = new StringBuilder();
            mciSendString("status " + this._fileName + " length", sb, 255, IntPtr.Zero);
            if(sb.Length == 0)
            {
                return;
            }
            int length = Convert.ToInt32(sb.ToString());
            int pos = 0;

            while (_isBeingPlayed)
            {
                sb = null;
                sb = new StringBuilder();
                mciSendString("status " + this._fileName + " position", sb, 255, IntPtr.Zero);
                pos = Convert.ToInt32(sb.ToString());
                if (pos >= length)
                {
                    if (!_isLooping)
                    {
                        _isBeingPlayed = false;
                        break;
                    }
                    else
                    {
                        mciSendString("play " + this._fileName + " from 0", sb, 0, IntPtr.Zero);
                    }
                }
            }
            mciSendString("stop " + this._fileName, sb, 0, IntPtr.Zero);
            mciSendString("close " + this._fileName, sb, 0, IntPtr.Zero);
        }

        public void Play(bool Looping)
        {
            if (!Game.Sound)
            {
                return;
            }

            try
            {
                if (_isBeingPlayed)
                {
                    return;
                }
                if (!File.Exists(_fileName))
                {
                    _isBeingPlayed = true;
                    return;
                }
                this._isLooping = Looping;
                ThreadStart ts = new ThreadStart(PlayWorker);
                Thread WorkerThread = new Thread(ts);
                WorkerThread.Name = _fileName;
                WorkerThread.SetApartmentState(ApartmentState.STA);
                WorkerThread.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in sound play: " + ex.Message);
            }
        }

        public void StopPlaying()
        {
            _isBeingPlayed = false;
        }

        // Sound API
        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);
    }
}