using System;

namespace Tennis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var jeux = new TennisJeux();
            while(true)
            {
                if(jeux.Joueur1Score == TennisJeux.Vainqueur)
                {
                    Console.WriteLine("Joueur 1 a gagne");
                    break;
                }
                if (jeux.Joueur2Score == TennisJeux.Vainqueur)
                {
                    Console.WriteLine("Joueur 2 a gagne");
                    break;
                }
                Console.WriteLine("Score {0} - {1}", jeux.Joueur1Score, jeux.Joueur2Score);
                Console.WriteLine("Qui a marqué Joueur 1 (1) ou 2 (2)?");
                var joueur = Console.ReadLine();
                Console.WriteLine();
                if (joueur == "1") jeux.MarqueJoueur1();
                if (joueur == "2") jeux.MarqueJoueur2();
            }
        }
    }
}
