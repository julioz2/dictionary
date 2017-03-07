using System;
using System.Collections.Generic;
using System.Linq;

namespace EnglishExercise
{
    class Program : GoogleApi
    {
        private static bool _isTheAnswearCorrect;
        private static bool _isAnswearCorrect;
        private static int _numberOfChoices = 3;
        private static int _fisrtWordIndex;
        private static int _secondWordIndex = 1;
        private static int _questionCount;
        private static int _rightQuestionCount;
        private static int _questionLimit = 10;

        static void Main()
        {
            IList<IList<Object>> values = GetGoogleData();
            SelectLanguage();
            SetQuestionLimit();
            ChooseTheExercise(values);
        }

        // first the last entries

        private static void SetQuestionLimit()
        {
            Console.WriteLine("Select the number of questions:");
            Console.WriteLine("1. 5");
            Console.WriteLine("2. 10");
            Console.WriteLine("3. 20");
            Console.WriteLine("3. 40");

            int numOfQuestions = Convert.ToInt32(Console.ReadLine());

            switch (numOfQuestions)
            {
                case 1:
                    _questionLimit = 5;
                    break;
                case 2:
                    _questionLimit = 10;
                    break;
                case 3:
                    _questionLimit = 20;
                    break;
                case 4:
                    _questionLimit = 40;
                    break;
                default:
                    Console.WriteLine("Select a right number");
                    Console.WriteLine("");
                    SetQuestionLimit();
                    break;
            }
        }

        private static void SelectLanguage()
        {
            Console.WriteLine("Select the language:");
            Console.WriteLine("1. From English to Italian");
            Console.WriteLine("2. From Italian to English");

            int exerciseLanguage = Convert.ToInt32(Console.ReadLine());

            if (exerciseLanguage == 1)
            {
                _fisrtWordIndex = 0;
                _secondWordIndex = 1;
            }
            else if (exerciseLanguage == 2)
            {
                _fisrtWordIndex = 1;
                _secondWordIndex = 0;
            }
            else
            {
                Console.WriteLine("Select a right number");
                Console.WriteLine("");
                SelectLanguage();
            }
        }

        private static void ChooseTheExercise(IList<IList<object>> values)
        {
            Console.WriteLine("Choose your exercise:");
            Console.WriteLine("1. Exact match word");
            Console.WriteLine("2. Multiple choise word");

            int exerciseNumber = Convert.ToInt32(Console.ReadLine());

            if (exerciseNumber == 1)
                RandomWord(values);
            else if (exerciseNumber == 2)
                MultipleChoice(values);
            else
            {
                Console.WriteLine("Select a right number");
                Console.WriteLine("");
                ChooseTheExercise(values);
            }
        }

        private static void MultipleChoice(IList<IList<object>> values)
        {
            var random = new Random();
            int randomIndex = random.Next(values.Count);

            if (values[randomIndex].Count > 0)
                Console.WriteLine("What does '" + values[randomIndex][_fisrtWordIndex] + "' mean?");
            else
                MultipleChoice(values);

            // provide 4 answear
            var rightAnswear = values[randomIndex][_secondWordIndex].ToString();

            // display answears
            DisplayMultipleAnswears(values, rightAnswear, _numberOfChoices, randomIndex);
        }

        private static void DisplayMultipleAnswears(IList<IList<object>> values, string rightAnswear, int answearLength, int rightIndex)
        {
            var randomRightAnswear = new Random();
            int randomRightAnswearIndex = randomRightAnswear.Next(answearLength);
            int rightNumber = 0;

            Random rand = new Random();
            List<int> randomWrongAnswearList = new List<int>();

            while (randomWrongAnswearList.Count <= answearLength + 3)
            {
                int num;
                num = rand.Next(values.Count);
                if (num != rightIndex)
                    randomWrongAnswearList.Add(num);
            }

            for (var i = 0; i <= answearLength; i++)
            {
                var indexRandom = randomWrongAnswearList[i];

                if (i == randomRightAnswearIndex)
                {
                    rightNumber = i + 1;
                    Console.WriteLine(i + 1 + ". " + rightAnswear);
                }
                else
                {
                    Console.WriteLine(i + 1 + ". " + values[indexRandom][_secondWordIndex]);
                }
            }

            int answearNumber = Convert.ToInt32(Console.ReadLine());

            if (rightNumber == answearNumber)
            {
                _rightQuestionCount++;
                Console.WriteLine("YES");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("NO");
                Console.WriteLine("");
            }

            _questionCount++;

            if (_questionCount < _questionLimit)
                MultipleChoice(values);
            else
                GetResults();
        }

        private static void RandomWord(IList<IList<object>> values)
        {
            var random = new Random();
            int randomIndex = random.Next(values.Count);

            if (values[randomIndex].Count > 0)
                Console.WriteLine("What does '" + values[randomIndex][_fisrtWordIndex] + "' mean?");
            else
                RandomWord(values);

            string answear = Console.ReadLine();

            _isAnswearCorrect = CheckTheAnswear(answear, values[randomIndex][_secondWordIndex].ToString());

            if (answear != null && _isAnswearCorrect)
            {
                _rightQuestionCount++;
                Console.WriteLine("Yes");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("NO");
                Console.WriteLine("Right answear: " + values[randomIndex][_secondWordIndex]);
                Console.WriteLine("");
            }

            GetTheDefinition(values[randomIndex][_fisrtWordIndex].ToString());

            _questionCount++;

            if (_questionCount < _questionLimit)
                RandomWord(values);
            else
                GetResults();
        }

        private static void GetTheDefinition(string word)
        {
            Console.WriteLine("Do you want the oxford definition?");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");

            int numOfQuestions = Convert.ToInt32(Console.ReadLine());

            switch (numOfQuestions)
            {
                case 1:
                    GetWordDefinition Oxford = new GetWordDefinition();
                    Oxford.OxfordRequest(word);
                    break;
                case 2:
                    break;
                default:
                    Console.WriteLine("Select a right number");
                    Console.WriteLine("");
                    GetTheDefinition(word);
                    break;
            }
        }

        //GetWordDefinition Oxford = new GetWordDefinition();
        //Oxford.OxfordRequest();

        private static void GetResults()
        {
            Console.WriteLine($"Your score is: {_rightQuestionCount} correct answers on {_questionLimit}");
            Console.ReadLine();
        }

        private static bool CheckTheAnswear(string answear, string wordMeaning)
        {
            _isTheAnswearCorrect = false;

            if (wordMeaning.Contains(",")) // synonyms check
            {
                List<string> meanings = wordMeaning.Split(',').ToList();

                foreach (var meaning in meanings)
                {
                    var newMeaning = meaning;

                    if (meaning.Substring(0, 1) == " ")
                        newMeaning = meaning.Remove(0, 1);

                    if (string.Equals(answear, newMeaning, StringComparison.CurrentCultureIgnoreCase))
                        _isTheAnswearCorrect = true;
                }
            }
            else if (string.Equals(answear, wordMeaning, StringComparison.CurrentCultureIgnoreCase))
                _isTheAnswearCorrect = true;

            return _isTheAnswearCorrect;
        }
    }
}

/*
 * 
 * 
 
 * 
 * */