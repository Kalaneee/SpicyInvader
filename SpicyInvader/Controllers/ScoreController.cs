using SpicyInvader.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SpicyInvader.Controllers
{
    public class ScoreController
    {
        private static string _fileName = "scores.xml";

        public List<Score> Scores { get; private set; }
        public List<Score> HighScores { get; private set; }

        public ScoreController()
          : this(new List<Score>())
        {

        }

        public ScoreController(List<Score> scores)
        {
            Scores = scores;
            UpdateHighscores();
        }

        public void Add(Score score)
        {
            Scores.Add(score);
            Scores = Scores.OrderByDescending(c => c.Value).ToList(); // Best scores first
            UpdateHighscores();
        }

        /// <summary>
        ///  Load XML file with score and create List of Scores
        /// </summary>
        /// <returns></returns>
        public static ScoreController Load()
        {
            // No file
            if (!File.Exists(_fileName))
                return new ScoreController();

            using (StreamReader reader = new StreamReader(new FileStream(_fileName, FileMode.Open)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
                List<Score> scores = (List<Score>)serializer.Deserialize(reader);
                return new ScoreController(scores);
            }
        }

        private void UpdateHighscores()
        {
            HighScores = Scores.Take(10).ToList();
        }

        /// <summary>
        ///  Save the List of Scores into the XML file
        /// </summary>
        /// <param name="scoreController"></param>
        public static void Save(ScoreController scoreController)
        {
            using (StreamWriter reader = new StreamWriter(new FileStream(_fileName, FileMode.Create)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
                serializer.Serialize(reader, scoreController.Scores);
            }
        }
    }
}
