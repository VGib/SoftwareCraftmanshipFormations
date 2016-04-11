using System;
using Xunit;

namespace Tennis
{
    public class TennisTest
    {
        public TennisJeux Jeux { get; private set; }

        public TennisTest ()
        {
            Jeux = new TennisJeux();
        }

        [Fact]
        public void AuDebutLeScoreDoitEtreDe0()
        {
            Assert.Equal("0", Jeux.Joueur1Score);
            Assert.Equal("0", Jeux.Joueur2Score);
        }

        [Fact]
        public void ApresUnPointDuJoueur1LeScoreDoitEtreDe15()
        {
            Jeux.MarqueJoueur1();
            Assert.Equal("15", Jeux.Joueur1Score);
        }

        [Fact]
        public void ApresDeuxPointsDuJoueur1LeScoreDoitEtreDe30()
        {
            Jeux.MarqueJoueur1();
            Jeux.MarqueJoueur1();
            Assert.Equal("30", Jeux.Joueur1Score);
        }

        [Fact]
        public void ApresTroisPointsDuJoueur1LeScoreDoitEtreDe40()
        {
            Jeux.MarqueJoueur1();
            Jeux.MarqueJoueur1();
            Jeux.MarqueJoueur1();
            Assert.Equal("40", Jeux.Joueur1Score);
        }

        [Fact]
        public void LorsqueLeJoueur1MarqueEtQueLeJoueur2NestPasA40LeJoueur1Gagne()
        {
            Jeux.MarqueJoueur1();
            Jeux.MarqueJoueur1();
            Jeux.MarqueJoueur1();
            Jeux.MarqueJoueur1();
            Assert.Equal(TennisJeux.Vainqueur, Jeux.Joueur1Score);
        }

        [Fact]
        public void ApresUnPointDuJoueur2LeScoreDoitEtreDe15()
        {
            Jeux.MarqueJoueur2();
            Assert.Equal("15", Jeux.Joueur2Score);
        }

        [Fact]
        public void ApresDeuxPointsDuJoueur2LeScoreDoitEtreDe30()
        {
            Jeux.MarqueJoueur2();
            Jeux.MarqueJoueur2();
            Assert.Equal("30", Jeux.Joueur2Score);
        }

        [Fact]
        public void ApresTroisPointsDuJoueur2LeScoreDoitEtreDe40()
        {
            Jeux.MarqueJoueur2();
            Jeux.MarqueJoueur2();
            Jeux.MarqueJoueur2();
            Assert.Equal("40", Jeux.Joueur2Score);
        }

        [Fact]
        public void LorsqueLeJoueur2MarqueEtQueLeJoueur2NestPasA40LeJoueur2Gagne()
        {
            Jeux.MarqueJoueur2();
            Jeux.MarqueJoueur2();
            Jeux.MarqueJoueur2();
            Jeux.MarqueJoueur2();
            Assert.Equal(TennisJeux.Vainqueur, Jeux.Joueur2Score);
        }

        [Fact]
        public void SiUnJoueurEstA40EtQueLeDeuxiemeJoueurArriveA40LeScoreEstEgalite()
        {
            ScoreEgalite();
            Assert.Equal(TennisJeux.Egalite, Jeux.Joueur1Score);
            Assert.Equal(TennisJeux.Egalite, Jeux.Joueur2Score);
        }

        [Fact]
        public void EnCasDEgaliteLeJoueurQuiMarqueAAvantageEtCeluiQuiPerdLePointADesavantage()
        {
            ScoreEgalite();
            Jeux.MarqueJoueur1();
            Assert.Equal(TennisJeux.Avantage, Jeux.Joueur1Score);
            Assert.Equal(TennisJeux.Desavantage, Jeux.Joueur2Score);
        }

        [Fact]
        public void AuCasOuUnJoueurEnDesavantageMarqueLesDeuxJoueursRedeviennentEgalite()
        {
            ScoreEgalite();
            Jeux.MarqueJoueur1();
            Jeux.MarqueJoueur2();
            Assert.Equal(TennisJeux.Egalite, Jeux.Joueur1Score);
            Assert.Equal(TennisJeux.Egalite, Jeux.Joueur2Score);
        }

        [Fact]
        public void AuCasOuUnJoueurEnAvantageMarqueIlEstVainquer()
        {
            ScoreEgalite();
            Jeux.MarqueJoueur1();
            Jeux.MarqueJoueur1();
            Assert.Equal(TennisJeux.Vainqueur, Jeux.Joueur1Score);
        }

        [Fact]
        private void ScoreEgalite()
        {
            Jeux.MarqueJoueur1();
            Jeux.MarqueJoueur1();
            Jeux.MarqueJoueur1();
            Jeux.MarqueJoueur2();
            Jeux.MarqueJoueur2();
            Jeux.MarqueJoueur2();
        }
    }

    public class TennisJeux
    {
        public const string Vainqueur = "Vainqueur";
        public const string Egalite = "Egalite";
        public const string Avantage = "Avantage";
        public const string Desavantage = "Desavantage";

        private readonly string[] tableauDeScore = { "0", "15", "30","40" };

        public string Joueur1Score { get { return joueur1.Score; } } 
        public string Joueur2Score { get { return joueur2.Score; } }

        private readonly Joueur joueur1 = new Joueur();
        private readonly Joueur joueur2 = new Joueur();

        private  class Joueur
        {
            public string Score { get; set; } = "0";
        }


        private void Marque (Joueur joueur, Joueur adversaire)
        {
            if (joueur.Score == Egalite)
            {
                joueur.Score = Avantage;
                adversaire.Score = Desavantage;
            }
            else if (joueur.Score == Avantage)
            {
                joueur.Score = Vainqueur;
            }
            else if(joueur.Score == Desavantage)
            {
                joueur.Score = Egalite;
                adversaire.Score = Egalite;
            }
            else if (adversaire.Score == "40" && joueur.Score == "30")
            {
                joueur.Score = Egalite;
                adversaire.Score = Egalite;
            }
            else if (joueur.Score == "40")
            {
                joueur.Score = Vainqueur;
            }
            else
            {
                AvanceScoreDansTableau(joueur);
            }
        }

        private void AvanceScoreDansTableau(Joueur joueur)
        {
            var positionDansLeTableauDeScore = Array.IndexOf(tableauDeScore, joueur.Score);
            joueur.Score = tableauDeScore[positionDansLeTableauDeScore + 1];
        }

        public  void MarqueJoueur1()
        {
            Marque(joueur1, joueur2);
        }

        public void MarqueJoueur2()
        {
            Marque(joueur2, joueur1);
        }
    }
}
