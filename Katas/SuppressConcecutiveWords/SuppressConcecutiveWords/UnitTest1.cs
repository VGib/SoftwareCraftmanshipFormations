using NUnit.Framework;
using System.Collections.Generic;

namespace SuppressConcecutiveWords
{

    public static class SuppressDuplicate
    {
        public static string DoSuppression(string input)
        {
            return string.Join(" ", DoSuppressionIterator(input));
        }

        private static IEnumerable<string> DoSuppressionIterator(string input)
        {
            var lastWord = string.Empty;

            for (int index = -1; index < input.Length;)
            {
                (var nextWord, var incrementIndex) = GetNextWord(input, index);
                index += incrementIndex;

                if (string.IsNullOrWhiteSpace(nextWord))
                {
                    continue;
                }

                nextWord = nextWord.ToLower();

                if (nextWord == lastWord)
                {
                    continue;
                }

                lastWord = nextWord;
                yield return lastWord;

            }
        }

 

        private static (string nextWord, int incrementIndex) GetNextWord(string input, int index)
        {
            var position = index + 1;
            for (; position < input.Length; position++)
            {
                if(input[position] == ' ')
                {
                    break;
                }
            }

            if(position == index + 1)
            {
                return ( position >= input.Length ? " " :  input.Substring(index  + 1, 1)  , 1);
            }

            var incrementIndex = position - index - 1;


            return (input.Substring(index + 1, incrementIndex), incrementIndex);
        }
    }

    public class Tests
    {

        [Test]
        public void ShouldSuppressFollowingDuplicateWords()
        {
            DoTest("chat voiture voiture pigeon oiseau oiseau mésange", "chat voiture pigeon oiseau mésange");
        }

        private static void DoTest(string input, string expected)
        {
            var result = SuppressDuplicate.DoSuppression(input);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldDealWithDuplicatesInBegining()
        {
            DoTest("chat chat chat voiture maison", "chat voiture maison");
        }


        [Test]
        public void ShouldDealWithMultiplesFollowingDuplicatesWords()
        {
            DoTest("chat voiture voiture voiture voiture voiture maison", "chat voiture maison");
        }

        [Test]
        public void WhenWordIsNotDuplicateFollowingTheWordShouldNotBeDuplicated()
        {
            DoTest("chat voiture voiture voiture maison voiture", "chat voiture maison voiture");
        }

        [Test]
        public void ShouldDealWithDuplicatesAtEnd()
        {
            DoTest("chat voiture maison maison maison", "chat voiture maison");
        }

        [Test]
        public void ShouldDealWithOneCharacterWord()
        {
            DoTest("chat a  a  a voiture", "chat a voiture");
        }


        [Test]
        public void ShouldDealWithMultiplesSpaces()
        {
            DoTest("   chat     pigeon      pigeon     pigeon pigeon voiture       maison", "chat pigeon voiture maison");
        }

        [Test]
        public void ShouldbeCaseInsensitve()
        {
            DoTest("   chat     pigeon      Pigeon     pIGeon pigeon voiture       maison", "chat pigeon voiture maison");
        }



    }
}