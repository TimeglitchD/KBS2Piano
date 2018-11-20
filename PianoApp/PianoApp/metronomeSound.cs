using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

namespace PianoApp.Models
{
    class metronomeSound
    {
        private SoundPlayer metronome;
        private SoundPlayer metronomeBeat;

        private Thread t;
        private System.Timers.Timer timer;

        private int beats;
        private int elapsedBeats = 0;

        public metronomeSound()
        {
            metronome = new SoundPlayer();
            metronome.SoundLocation = System.AppDomain.CurrentDomain.BaseDirectory + @"/sounds/metronome.wav";
            metronome.Load();

            metronomeBeat = new SoundPlayer();
            metronomeBeat.SoundLocation = System.AppDomain.CurrentDomain.BaseDirectory + @"/sounds/metronomeBeat.wav";
            metronomeBeat.Load();
        }

        private void playMetronome(bool isBeat)
        {
            if(isBeat)
            {
                metronomeBeat.Play();
            } else
            {
                metronome.Play();
            }
        }

        public void startMetronome(int bpm, int beats)
        {
            int interval = 1000 / (bpm / 60);
            this.beats = beats - 1;
            t = new Thread(() => metronomeTimer(interval));
            t.Start();
        }

        public void stopMetronome()
        {
            timer.Stop();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            if (elapsedBeats == 0)
            {
                this.playMetronome(true);
                elapsedBeats++;
            }
            else if (elapsedBeats == beats)
            {
                this.playMetronome(false);
                elapsedBeats = 0;
            }
            else
            {
                this.playMetronome(false);
                elapsedBeats++;
            }
        }

        private void metronomeTimer(int interval)
        {
            //Thread.CurrentThread.IsBackground = true;
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(this.timer_tick);
            timer.Interval = interval;
            timer.Start();
        }
    }
}
