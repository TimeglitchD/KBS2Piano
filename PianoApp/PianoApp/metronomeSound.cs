using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using NAudio.Wave;
using System.IO;
using System.Windows;

namespace PianoApp
{
    public class metronomeSound
    {
        private DirectSoundOut outputDeviceMetronome;
        private DirectSoundOut outputDeviceMetronomeBeat;

        private AudioFileReader metronomeSoundFile;
        private AudioFileReader metronomeBeatSoundFile;

        private Thread t;

        private System.Timers.Timer timer;

        //values for keeping track of current position
        private int beats;
        public int elapsedBeats = 0;

        private bool countDown = false;
        private bool countDownOnly = false;
        private int countDownAmount;
        private int elapsedCountdown;

        public event EventHandler countdownFinished;
        public event EventHandler countDownTickElapsed;


        public metronomeSound()
        {
            //get soundfiles relative to exe file
            metronomeSoundFile = new AudioFileReader(System.AppDomain.CurrentDomain.BaseDirectory + @"/sounds/metronome.wav");
            metronomeBeatSoundFile = new AudioFileReader(System.AppDomain.CurrentDomain.BaseDirectory + @"/sounds/metronomeBeat.wav");

            //emptyy guid so sounds play through default audio device
            Guid guid = Guid.Empty;
            outputDeviceMetronomeBeat = new DirectSoundOut(guid, 50);
            outputDeviceMetronome = new DirectSoundOut(guid, 50);

            outputDeviceMetronome.Init(metronomeSoundFile);
            outputDeviceMetronomeBeat.Init(metronomeBeatSoundFile);

            primeSounds();
        }

        //start metronome with specified amount of countdown measures. Raise event after countdown is done to start guide
        public void startMetronome(float bpm, int beats, int countDownAmount)
        {
            this.countDown = true;
            this.countDownAmount = countDownAmount;
            elapsedCountdown = 0;
            startMetronome(bpm, beats);
        }

        //start metronome without countdown
        public void startMetronome(float bpm, int beats)
        {
            int interval = (int)(1000.0 / (bpm / 60.0));
            this.beats = beats;
            t = new Thread(() => metronomeTimer(interval));
            t.Start();
        }

        //start metronome with countdown only. Raise event after countdown is done.
        public void startMetronomeCountDownOnly(float bpm, int beats, int countDownAmount)
        {
            countDownOnly = true;
            startMetronome(bpm, beats, countDownAmount);
        }

        //stop metronome
        public bool stopMetronome()
        {
            try
            {
                this.countDown = false;
                timer.Stop();
                resetValues();
                return true;
            } catch(Exception)
            {
                return false;
            }
        }

        //function runs in new thread.
        private void metronomeTimer(int interval)
        {
            //Thread.CurrentThread.IsBackground = true;
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(this.timer_tick);
            timer.Interval = interval;
            timer.Start();
        }

        //sound play function
        private void playMetronome(bool isBeat)
        {
            if (isBeat)
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    metronomeBeatSoundFile.CurrentTime = System.TimeSpan.Zero;
                    outputDeviceMetronomeBeat.Play();
                }).Start();

            }
            else
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    metronomeSoundFile.CurrentTime = System.TimeSpan.Zero;
                    outputDeviceMetronome.Play();
                }).Start();
            }
        }

        //raise event in new thread so timer doesn't get delayed
        private void eventNewThread(Action<EventArgs> method)
        {
            new Thread(() =>
            {
                method(EventArgs.Empty);
            }).Start();
        }

        //countdownfinished event
        private void onCountdownFinished(EventArgs e)
        {
            if (countdownFinished != null)
            {
                countdownFinished(this, e);
            }
        }

        private void onCountDownTickElapsed(EventArgs e)
        {
            if(countDownTickElapsed != null)
            {
                countDownTickElapsed(this, e);
            }
        }

        //resets all values so metronome functions as expected when started after using stopmetronome.
        private void resetValues()
        {
            beats = 0;
            elapsedBeats = 0;
            countDown = false;
            countDownOnly = false;
            countDownAmount = 0;
            elapsedCountdown = 0;
        }

        //Silently plays the sounds so there's no unexpected delay when starting the countdown
        private void primeSounds()
        {
            metronomeSoundFile.Volume = 0.0f;
            metronomeBeatSoundFile.Volume = 0.0f;
            Thread.Sleep(500);
            outputDeviceMetronomeBeat.Play();
            outputDeviceMetronome.Play();
            Thread.Sleep(1000);
            metronomeSoundFile.Volume = 1.0f;
            metronomeBeatSoundFile.Volume = 1.0f;
        }

        //event handler on every beat
        private void timer_tick(object sender, EventArgs e)
        {
            if (countDownOnly)
            {
                if (countDown && elapsedBeats < beats * countDownAmount)
                {
                    this.playMetronome(true);
                    elapsedBeats++;
                    eventNewThread(onCountDownTickElapsed);
                    return;
                }
                else
                {
                    stopMetronome();
                    eventNewThread(onCountDownTickElapsed);
                    eventNewThread(onCountdownFinished);
                }
            }

            else
            {
                if (countDown && elapsedBeats < beats * countDownAmount)
                {
                    this.playMetronome(true);
                    elapsedBeats++;
                    eventNewThread(onCountDownTickElapsed);
                    return;
                }
                else if (countDown && elapsedBeats == beats * countDownAmount)
                {
                    elapsedBeats = 0;
                    countDown = false;
                    eventNewThread(onCountDownTickElapsed);
                    eventNewThread(onCountdownFinished);
                }

                if (elapsedBeats == 0)
                {
                    this.playMetronome(true);
                    elapsedBeats++;
                }
                else if (elapsedBeats == beats - 1)
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
        }

        //test get
        public bool getCountdown()
        {
            return countDown;
        }
    }
}
