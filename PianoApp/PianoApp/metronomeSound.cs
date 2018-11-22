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

namespace PianoApp.Models
{
    class metronomeSound
    {
        private DirectSoundOut outputDeviceMetronome;
        private DirectSoundOut outputDeviceMetronomeBeat;

        private AudioFileReader metronomeSoundFile;
        private AudioFileReader metronomeBeatSoundFile;

        private Thread t;

        private System.Timers.Timer timer;

        private int beats;
        private int elapsedBeats = 0;

        private bool countDown = false;
        private bool countDownOnly = false;
        private int countDownAmount;
        private int elapsedCountdown;

        public event EventHandler countdownFinished;


        public metronomeSound()
        {
            metronomeSoundFile = new AudioFileReader(System.AppDomain.CurrentDomain.BaseDirectory + @"/sounds/metronome.wav");
            metronomeBeatSoundFile = new AudioFileReader(System.AppDomain.CurrentDomain.BaseDirectory + @"/sounds/metronomeBeat.wav");

            Guid guid = Guid.Empty;
            outputDeviceMetronomeBeat = new DirectSoundOut(guid, 50);
            outputDeviceMetronome = new DirectSoundOut(guid, 50);
            
            outputDeviceMetronome.Init(metronomeSoundFile);
            outputDeviceMetronomeBeat.Init(metronomeBeatSoundFile);

            primeSounds();
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

        public void startMetronomeCountDownOnly(int bpm, int beats, int countDownAmount)
        {
            countDownOnly = true;
            startMetronome(bpm, beats, countDownAmount);
        }

        //force stop metronome
        public void stopMetronome()
        {
            this.countDown = false;
            timer.Stop();
            resetValues();
        }

        //event that fires on every beat
        private void timer_tick(object sender, EventArgs e)
        {
            if (countDownOnly)
            {
                if (countDown && elapsedBeats < beats * countDownAmount)
                {
                    this.playMetronome(true);
                    elapsedBeats++;
                    return;
                }
                else
                {
                    stopMetronome();
                    countdownFinishedNewThread();
                }
            }

            else
            {
                if (countDown && elapsedBeats < beats * countDownAmount)
                {
                    this.playMetronome(true);
                    elapsedBeats++;
                    return;
                }
                else if (countDown && elapsedBeats == beats * countDownAmount)
                {
                    elapsedBeats = 0;
                    countDown = false;
                    countdownFinishedNewThread();
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

        private void onCountdownFinished(EventArgs e)
        {
            if(countdownFinished != null)
            {
                countdownFinished(this, e);
            }
        }

        private void countdownFinishedNewThread()
        {
            new Thread(() =>
            {
                onCountdownFinished(EventArgs.Empty);
            }).Start();
        }

        private void resetValues()
        {
            beats = 0;
            elapsedBeats = 0;
            countDown = false;
            countDownOnly = false;
            countDownAmount = 0;
            elapsedCountdown = 0;
        }

        private void primeSounds()
        {
            metronomeSoundFile.Volume = 0.0f;
            metronomeBeatSoundFile.Volume = 0.0f;
            outputDeviceMetronomeBeat.Play();
            outputDeviceMetronome.Play();
            Thread.Sleep(1000);
            metronomeSoundFile.Volume = 1.0f;
            metronomeBeatSoundFile.Volume = 1.0f;
        }
    }
}
