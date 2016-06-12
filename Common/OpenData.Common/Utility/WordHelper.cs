using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OpenData.Utility
{
    public class Singularizer
    {
        private static readonly IList<string> Unpluralizables =
            new List<string>
                {
                    "equipment",
                    "information",
                    "rice",
                    "money",
                    "species",
                    "series",
                    "fish",
                    "sheep",
                    "deer"
                };

        private static readonly IDictionary<string, string> Singularizations =
            new Dictionary<string, string>
                {
                    // Start with the rarest cases, and move to the most common
                    {"people", "person"},
                    {"oxen", "ox"},
                    {"children", "child"},
                    {"feet", "foot"},
                    {"teeth", "tooth"},
                    {"geese", "goose"},
                    // And now the more standard rules.
                    {"(.*)ives?", "$1ife"},
                    {"(.*)ves?", "$1f"},
                    // ie, wolf, wife
                    {"(.*)men$", "$1man"},
                    {"(.+[aeiou])ys$", "$1y"},
                    {"(.+[^aeiou])ies$", "$1y"},
                    {"(.+)zes$", "$1"},
                    {"([m|l])ice$", "$1ouse"},
                    {"matrices", @"matrix"},
                    {"indices", @"index"},
                    {"(.*)ices", @"$1ex"},
                    // ie, Matrix, Index
                    {"(octop|vir)i$", "$1us"},
                    {"(.+(s|x|sh|ch))es$", @"$1"},
                    {"(.+)s", @"$1"}
                };

        public static string Singularize(string word)
        {
            if (Unpluralizables.Contains(word.ToLowerInvariant()))
            {
                return word;
            }

            foreach (var singularization in Singularizations)
            {
                if (Regex.IsMatch(word, singularization.Key))
                {
                    return Regex.Replace(word, singularization.Key, singularization.Value);
                }
            }

            return word;
        }

        public static bool IsPlural(string word)
        {
            if (Unpluralizables.Contains(word.ToLowerInvariant()))
            {
                return true;
            }

            foreach (var singularization in Singularizations)
            {
                if (Regex.IsMatch(word, singularization.Key))
                {
                    return true;
                }
            }

            return false;
        }
    }

	#region Test for Singularizer

	/*
using System.Collections.Generic;
using NUnit.Framework;

namespace Pingularizer
{
    [TestFixture]
    public class SingularizerTests
    {
        [Test]
        public void StandardSingularizationTests()
        {
            Dictionary<string, string> dictionary = GetTestDictionary();

            foreach (var singular in dictionary.Keys)
            {
                var plural = dictionary[singular];
                Assert.AreEqual(singular, Singularizer.Singularize(plural));
            }
        }

        [Test]
        public void IrregularSingularizationTests()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("person", "people");
            dictionary.Add("child", "children");
            dictionary.Add("ox", "oxen");

            foreach (var singular in dictionary.Keys)
            {
                var plural = dictionary[singular];
                Assert.AreEqual(singular, Singularizer.Singularize(plural));
            }
        }

        [Test]
        public void NonSingularizationPluralizationTests()
        {
            var nonPluralizingWords = new List<string> { "equipment", "information", "rice", "money", "species", "series", "fish", "sheep", "deer" };

            foreach (var word in nonPluralizingWords)
            {
                Assert.AreEqual(word, Singularizer.Singularize(word));
            }
        }

        private Dictionary<string, string> GetTestDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("sausage", "sausages"); // Most words - Just add an 's'
            dictionary.Add("status", "statuses"); // Words that end in 's' - Add 'es'
            dictionary.Add("ax", "axes"); // Words that end in 'x' - Add 'es'
            dictionary.Add("octopus", "octopi"); // Some Words that end in 'us' - Replace 'us' with 'i'
            dictionary.Add("virus", "viri"); // Some Words that end in 'us' - Replace 'us' with 'i'
            dictionary.Add("crush", "crushes"); // Words that end in 'sh' - Add 'es'
            dictionary.Add("crutch", "crutches"); // Words that end in 'ch' - Add 'es'
            dictionary.Add("matrix", "matrices"); // Words that end in 'ix' - Replace with 'ices'
            dictionary.Add("index", "indices"); // Words that end in 'ex' - Replace with 'ices'
            dictionary.Add("mouse", "mice"); // Some Words that end in 'ouse' - Replace with 'ice'
            dictionary.Add("quiz", "quizzes"); // Words that end in 'z' - Add 'zes'
            dictionary.Add("mailman", "mailmen"); // Words that end in 'man' - Replace with 'men'
            dictionary.Add("man", "men"); // Words that end in 'man' - Replace with 'men'
            dictionary.Add("wolf", "wolves"); // Words that end in 'f' - Replace with 'ves'
            dictionary.Add("wife", "wives"); // Words that end in 'fe' - Replace with 'ves'
            dictionary.Add("day", "days"); // Words that end in '[vowel]y' - Replace with 'ys'
            dictionary.Add("sky", "skies"); // Words that end in '[consonant]y' - Replace with 'ies'
            return dictionary;
        }
    }
}
	*/

	#endregion

	public class Pluralizer
	{
		private static readonly IList<string> Unpluralizables = new List<string> { "equipment", "information", "rice", "money", "species", "series", "fish", "sheep", "deer" };
		private static readonly IDictionary<string, string> Pluralizations = new Dictionary<string, string>
        {
            // Start with the rarest cases, and move to the most common
            { "person", "people" },
            { "ox", "oxen" },
            { "child", "children" },
            { "foot", "feet" },
            { "tooth", "teeth" },
            { "goose", "geese" },
            // And now the more standard rules.
            { "(.*)fe?", "$1ves" },         // ie, wolf, wife
            { "(.*)man$", "$1men" },
            { "(.+[aeiou]y)$", "$1s" },
            { "(.+[^aeiou])y$", "$1ies" },
            { "(.+z)$", "$1zes" },
            { "([m|l])ouse$", "$1ice" },
            { "(.+)(e|i)x$", @"$1ices"},    // ie, Matrix, Index
            { "(octop|vir)us$", "$1i"},
            { "(.+(s|x|sh|ch))$", @"$1es"},
            { "(.+)", @"$1s" }
        };

		public static string Pluralize( int count, string singular )
		{
			if( count == 1 )
				return singular;

			if( Unpluralizables.Contains( singular ) )
				return singular;

			var plural = "";

			foreach( var pluralization in Pluralizations )
			{
				if( Regex.IsMatch( singular, pluralization.Key ) )
				{
					plural = Regex.Replace( singular, pluralization.Key, pluralization.Value );
					break;
				}
			}

			return plural;
		}
	}

	#region Test for Pluralizer
	/*
	 namespace AutomatedTests
{
    [TestFixture]
    public class FormattingTests
    {
        [Test]
        public void StandardPluralizationTests()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("sausage", "sausages");  // Most words - Just add an 's'
            dictionary.Add("status", "statuses");   // Words that end in 's' - Add 'es'
            dictionary.Add("ax", "axes");           // Words that end in 'x' - Add 'es'
            dictionary.Add("octopus", "octopi");    // Some Words that end in 'us' - Replace 'us' with 'i'
            dictionary.Add("virus", "viri");        // Some Words that end in 'us' - Replace 'us' with 'i'
            dictionary.Add("crush", "crushes");     // Words that end in 'sh' - Add 'es'
            dictionary.Add("crutch", "crutches");   // Words that end in 'ch' - Add 'es'
            dictionary.Add("matrix", "matrices");   // Words that end in 'ix' - Replace with 'ices'
            dictionary.Add("index", "indices");     // Words that end in 'ex' - Replace with 'ices'
            dictionary.Add("mouse", "mice");        // Some Words that end in 'ouse' - Replace with 'ice'
            dictionary.Add("quiz", "quizzes");      // Words that end in 'z' - Add 'zes'
            dictionary.Add("mailman", "mailmen");   // Words that end in 'man' - Replace with 'men'
            dictionary.Add("man", "men");           // Words that end in 'man' - Replace with 'men'
            dictionary.Add("wolf", "wolves");       // Words that end in 'f' - Replace with 'ves'
            dictionary.Add("wife", "wives");        // Words that end in 'fe' - Replace with 'ves'
            dictionary.Add("day", "days");          // Words that end in '[vowel]y' - Replace with 'ys'
            dictionary.Add("sky", "skies");         // Words that end in '[consonant]y' - Replace with 'ies'

            foreach (var singular in dictionary.Keys)
            {
                var plural = dictionary[singular];

                Assert.AreEqual(plural, Formatting.Pluralize(2, singular));
                Assert.AreEqual(singular, Formatting.Pluralize(1, singular));
            }
        }

        [Test]
        public void IrregularPluralizationTests()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("person", "people");
            dictionary.Add("child", "children");
            dictionary.Add("ox", "oxen");

            foreach (var singular in dictionary.Keys)
            {
                var plural = dictionary[singular];

                Assert.AreEqual(plural, Formatting.Pluralize(2, singular));
                Assert.AreEqual(singular, Formatting.Pluralize(1, singular));
            }
        }

        [Test]
        public void NonPluralizingPluralizationTests()
        {
            var nonPluralizingWords = new List<string> { "equipment", "information", "rice", "money", "species", "series", "fish", "sheep", "deer" };

            foreach (var word in nonPluralizingWords)
            {
                Assert.AreEqual(word, Formatting.Pluralize(2, word));
                Assert.AreEqual(word, Formatting.Pluralize(1, word));
            }
        }
    }
}
	 */
	#endregion

}