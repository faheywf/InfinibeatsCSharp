using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Threading;

namespace InfinibeatsCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, double> noteFrequencies = new Dictionary<string, double>()
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
            Dictionary<string, SignalGenerator> generators = new Dictionary<string, SignalGenerator>();
            foreach (string note in noteFrequencies.Keys)
            {
                SignalGenerator gen = new SignalGenerator()
                {
                    Gain = note != "REST" ? 0.2 : 0.0,
                    Frequency = noteFrequencies[note] * 0.5,
                    Type = SignalGeneratorType.Sin
                };
                generators[note] = gen;
            }
            Dictionary<string, WaveOutEvent> wouts = new Dictionary<string, WaveOutEvent>();
            foreach (string note in generators.Keys)
            {
                WaveOutEvent wo = new WaveOutEvent();
                wo.Init(generators[note]);
                wouts[note] = wo;
            }

            string[] C = { "C", "E", "G" };
            string[] am = { "A", "C", "E" };
            string[] F = { "F", "A", "C" };
            string[] G = { "G", "B", "D" };
            string[][] progression = { C, am, F, G };
            string[] rhythm = { "x", ".", "x", "x", "x", ".", "x" };
            string[] rhythm2 = { "x", "." };
            string[] rhythm3 = { "x", ".", ".", "x", ".", ".", "x", "." };

            Track t = new Track(progression, rhythm, .25, 1.0);
            Track t2 = new Track(progression, rhythm2, 0.5, 1.0);
            Track t3 = new Track(progression, rhythm3, 0.75, .5);
            Thread thread = new Thread(t.Play);
            Thread thread2 = new Thread(t2.Play);
            Thread thread3 = new Thread(t3.Play);
            thread.Start();
            thread2.Start();
            thread3.Start();

            Console.ReadLine();
            t.Stop();
            t2.Stop();
            t3.Stop();
        }
    }
}
