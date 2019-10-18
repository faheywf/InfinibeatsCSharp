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
