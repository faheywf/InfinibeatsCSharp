using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfinibeatsCSharp
{
    public class Track
    {
        private readonly Dictionary<string, double> frequencies = new Dictionary<string, double>()
            {
                { "A", 220.0 },
                { "B", 246.94 },
                { "C", 261.63 },
                { "C#", 277.18 },
                { "D", 293.66 },
                { "D#", 311.13 },
                { "E", 329.63 },
                { "F", 174.61 },
                { "F#", 369.99 },
                { "G", 392.00 },
                { "G#", 207.65 },
                { "REST", 0.00 }
        };

        private readonly Dictionary<string, WaveOutEvent> wouts = new Dictionary<string, WaveOutEvent>();

        private string[][] progression;
        private string[] rhythm;
        private readonly double duration;

        private bool playing = true;

        public Track(string[][] progression, string[] rhythm, double duration, double octiveMultiplier)
        {
            this.progression = progression;
            this.rhythm = rhythm;
            this.duration = duration;

            foreach (string note in frequencies.Keys)
            {
                SignalGenerator gen = new SignalGenerator()
                {
                    Gain = note != "REST" ? 0.1 : 0.0,
                    Frequency = frequencies[note] * octiveMultiplier,
                    Type = SignalGeneratorType.Sin
                };
                WaveOutEvent wo = new WaveOutEvent();
                wo.Init(gen);
                wouts[note] = wo;
            }            
        }

        public void Play()
        {
            playing = true;

            int currentChord = 0;
            int currentNote = 0;
            int currentBeat = 0;
            string noteName = "REST";

            Random random = new Random();

            while (playing)
            {
                if (currentBeat == rhythm.Length)
                {
                    currentBeat = 0;
                }

                if (rhythm[currentBeat++] == "x")
                {
                    if (currentNote == progression[currentChord].Length)
                    {
                        if (currentChord == progression.Length - 1)
                        {
                            currentChord = 0;
                        }
                        else
                        {
                            currentChord++;
                        }
                        currentNote = random.Next(0, progression[currentChord].Length);
                    }
                    noteName = progression[currentChord][currentNote];
                    wouts[noteName].Play();
                    //Console.WriteLine(noteName);
                    double halfPeriod = 1 / frequencies[noteName] / 2;
                    double d = duration;
                    if(d > halfPeriod)
                    {
                        d -= duration % halfPeriod;
                    }
                    else
                    {
                        d = halfPeriod;
                    }
                    Thread.Sleep((int)(d * 1000));
                    int prevNote = currentNote;
                    currentNote = random.Next(0, progression[currentChord].Length + 1);
                    if (prevNote != currentNote)
                    {
                        wouts[noteName].Pause();
                    }
                }
                else
                {
                    wouts[noteName].Pause();
                    Thread.Sleep((int)(duration * 1000));
                }
            }
        }

        public void Stop()
        {
            playing = false;
        }
    }
}
