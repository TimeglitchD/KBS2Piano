using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

namespace PianoApp
{
    public class metronomeSound
    {
        private SoundPlayer metronome;
        private SoundPlayer metronomeBeat;
        private SoundPlayer empty;

        private Thread t;

        private int beats;
        private int elapsedBeats = 0;

        private bool countDown = false;
        private int countDownAmount;
        private int elapsedCountdown;

        public metronomeSound()
        {
            //playsync silent sound to prevent soundplayer delay
            empty = new SoundPlayer();
            empty.SoundLocation = System.AppDomain.CurrentDomain.BaseDirectory + @"/sounds/empty.wav";
            empty.PlaySync();

            //load actual sounds
            metronome = new SoundPlayer();
            metronome.SoundLocation = System.AppDomain.CurrentDomain.BaseDirectory + @"/sounds/metronome.wav";
            metronome.Load();

            //load beat sound
            metronomeBeat = new SoundPlayer();
            metronomeBeat.SoundLocation = System.AppDomain.CurrentDomain.BaseDirectory + @"/sounds/metronomeBeat.wav";
            metronomeBeat.Load();
        }

        //start metronome with specified amount of countdown measures
        public void startMetronome(int bpm, int beats, int countDownAmount)
        {
            this.countDown = true;
            this.countDownAmount = countDownAmount;
            elapsedCountdown = 0;
            startMetronome(bpm, beats);
        }

        //start metronome without countdown
        public void startMetronome(int bpm, int beats)
        {
            int interval = (int)( 1000.0 / (bpm / 60.0));
            this.beats = beats;
            t = new Thread(() => metronomeTimer(interval));
            t.Start();
        }

        //force stop metronome
        public void stopMetronome()
        {
            this.countDown = false;
            //abort because thread is only playing sound.
            t.Abort();
        }

        //event that fires on every beat
        private void timer_tick(object sender, EventArgs e)
        {
            if(countDown && elapsedBeats < beats * countDownAmount)
            {
                this.playMetronome(true);
                elapsedBeats++;
                return;
            } else if(countDown && elapsedBeats == beats * countDownAmount)
            {
                elapsedBeats = 0;
                countDown = false;
            }

            if (elapsedBeats == 0)
            {
                this.playMetronome(true);
                elapsedBeats++;
            }
            else if (elapsedBeats == beats -1)
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

        //function runs in new thread.
        private void metronomeTimer(int interval)
        {
            //Thread.CurrentThread.IsBackground = true;
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(this.timer_tick);
            timer.Interval = interval;
            timer.Start();
        }

        //sound play function
        private void playMetronome(bool isBeat)
        {
            if (isBeat)
            {
                metronomeBeat.Play();
            }
            else
            {
                metronome.Play();
            }
        }

        public bool getCountdown()
        {
            return countDown;
        }
    }
}
