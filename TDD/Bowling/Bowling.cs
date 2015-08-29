using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Bowling
{

    public enum Lancer
    {
        Normal,
        Spare,
        Strike
    }

    public class Bowling
    {
        public int Frame { get { return framesFois2 / 2; } }
        public int Quilles { get; private set; } = 10;
        public int Score { get; private set; }
        public Lancer ResultatLancer { get { return lancers.Last(); } private set { lancers.Add(value); } }

        private int framesFois2 = 0;
        private IList<Lancer> lancers = new List<Lancer>();

        internal void Lance(int nombreDeQuillesTombees)
        {
            SetResultatLancer(nombreDeQuillesTombees);
            Quilles -= nombreDeQuillesTombees;
            ++framesFois2;
            if (ResultatLancer == Lancer.Strike)
            {
                ++framesFois2;
            }
            if (framesFois2 % 2 == 0)
            {
                Quilles = 10;
            }
            Score += nombreDeQuillesTombees;
            if( lancers.Count >= 2 && lancers[lancers.Count - 2] == Lancer.Spare)
            {
                Score += nombreDeQuillesTombees;
            }
            Score += nombreDeQuillesTombees * lancers.Reverse().Skip(1).Take(2).Count(x => x == Lancer.Strike);
        }

        private void SetResultatLancer(int nombreDeQuillesTombees)
        {
            if (nombreDeQuillesTombees == 10 && framesFois2 % 2 == 0)
            {
                ResultatLancer = Lancer.Strike;
            }
            else if(Quilles == nombreDeQuillesTombees)
            {
                ResultatLancer = Lancer.Spare;
            }
            else
            {
                ResultatLancer = Lancer.Normal;
            }
        }
    }

    [TestFixture]
    public class Test
    {

        [TestCase]
        public void AuPremierLancerOnEstALaFrame0()
        {
            var bowling = new Bowling();
            Assert.AreEqual(0, bowling.Frame);
        }

        [TestCase]
        public void AuPremierLancerLeScoreEstA0()
        {
            var bowling = new Bowling();
            Assert.AreEqual(0, bowling.Score);
        }


        [TestCase]
        public void PremierLancerPeutFaireTomberAuPlus10Quilles()
        {
            var bowling = new Bowling();
            Assert.AreEqual(10, bowling.Quilles);
        }

        [TestCase]
        public void ApresPremierLancerOnPeutFaireTomber10moinsXQuilles()
        {
            var bowling = new Bowling();
            bowling.Lance(6);
            Assert.AreEqual(4, bowling.Quilles);
        }

        [TestCase]
        public void LePremierLancerNonStrikeEstNormal()
        {
            var bowling = new Bowling();
            bowling.Lance(5);
            Assert.AreEqual(Lancer.Normal, bowling.ResultatLancer);
        }

        [TestCase]
        public void LeSecondLancerEstNormalSIlResteDesQuilles()
        {
            var bowling = new Bowling();
            bowling.Lance(4);
            bowling.Lance(5);
            Assert.AreEqual(Lancer.Normal, bowling.ResultatLancer);
        }

        [TestCase]
        public void LePremierLancerEstUnStrike()
        {
            var bowling = new Bowling();
            bowling.Lance(10);
            Assert.AreEqual(Lancer.Strike, bowling.ResultatLancer);
        }

        [TestCase]
        public void SiLes2LancerFontTomber10QuillesLeLancerEstUnSpare()
        {
            var bowling = new Bowling();
            bowling.Lance(6);
            bowling.Lance(4);
            Assert.AreEqual(Lancer.Spare, bowling.ResultatLancer);
        }

        [TestCase]
        public void ApresUnLancer0EtUnLancer10OnAUnSpare()
        {
            var bowling = new Bowling();
            bowling.Lance(0);
            bowling.Lance(10);
            Assert.AreEqual(Lancer.Spare, bowling.ResultatLancer);
        }

        [TestCase]
        public void Apres2LancerOnPasseALaFrameSuivante()
        {
            var bowling = new Bowling();
            var currentFrame = bowling.Frame;
            bowling.Lance(4);
            bowling.Lance(3);
            Assert.AreEqual(currentFrame + 1, bowling.Frame);
        }

        [TestCase]
        public void ApresUnStrikeOnPasseALaFrameSuivante()
        {
            var bowling = new Bowling();
            var currentFrame = bowling.Frame;
            bowling.Lance(10);
            Assert.AreEqual(currentFrame + 1, bowling.Frame);
        }

        [TestCase]
        public void ALaProchaineFrameOnDoitLancer10Quilles()
        {
            var bowling = new Bowling();
            bowling.Lance(5);
            bowling.Lance(4);
            Assert.AreEqual(10, bowling.Quilles);
        }

        [TestCase]
        public void ApresUnLancerOnAjouteLeNombreDeQuillesTombeesAuScore()
        {
            var bowling = new Bowling();
            var currentScore = bowling.Score;
            bowling.Lance(6);
            Assert.AreEqual(currentScore + 6, bowling.Score);
        }

        [TestCase]
        public void SiLeLancerDAvantEstUnSpareOnCompte2FoisLeLancer()
        {
            var bowling = new Bowling();
            bowling.Lance(4);
            bowling.Lance(6);
            var currentScore = bowling.Score;
            bowling.Lance(3);
            Assert.AreEqual(currentScore + 3 * 2, bowling.Score);
        }

        [TestCase]
        public void SiLeLancerDAvantEstUnStrikeOnCompte2FoisLeLancer()
        {
            var bowling = new Bowling();
            bowling.Lance(10);
            var currentScore = bowling.Score;
            bowling.Lance(7);
            Assert.AreEqual(currentScore + 7 * 2, bowling.Score);
        }

        [TestCase]
        public void SiLeLancerDAvantEstUnStrikeEtCeluiAvantUnStrikeOnCompte3FoisLeLancer()
        {
            var bowling = new Bowling();
            bowling.Lance(10);
            bowling.Lance(10);
            var currentScore = bowling.Score;
            bowling.Lance(6);
            Assert.AreEqual(currentScore + 6 * 3, bowling.Score);
        }

    }
}
