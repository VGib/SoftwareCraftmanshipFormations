using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling
{
    class Program
    {
        static void Main(string[] args)
        {
            var bowling = new Bowling();
            while (!bowling.PartieFinie)
            {
                try
                {
                    Console.WriteLine("Frame: {0}", bowling.Frame + 1);
                    Console.WriteLine("Score: {0}", bowling.Score);
                    Console.WriteLine("Lancer: {0}", bowling.LancerDansLaFrame);
                    Console.WriteLine("Quilles:{0}", bowling.Quilles);

                    Console.Write("Nombre de quilles tombees:");
                    bowling.Lance(int.Parse(Console.ReadLine()));
                    Console.WriteLine();
                }
                catch (FormatException)
                {
                    Console.WriteLine("FormatIncorrect");
                }
            }
            Console.WriteLine("Score Final:{0}", bowling.Score);
        }
    }
}
