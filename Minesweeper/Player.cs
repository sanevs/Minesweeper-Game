using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Minesweeper
{
    public class Player
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public DateTime DateTime { get; set; }

        public Player(string name, int duration, DateTime dateTime)
        {
            Name = name;
            Duration = duration;
            DateTime = dateTime;
        }

        public static IList<Player> FromXML()
        {
            XElement node = XElement.Load("highScores.xml");
            IEnumerable<XElement> elements = node.Elements();
            IList<Player> players = new List<Player>();

            foreach (XElement element in elements)
                players.Add(
                    new Player(
                        (string)element.Element("score").Element("name"),
                        (int)element.Element("score").Element("duration"),
                        (DateTime)element.Element("score").Element("dateTime")
                    )
                );

            return players;
        }

        public static void ToXML(IList<Player> players)
        {
            XElement xml =
                new XElement("scores",
                    new XElement("difficulty",
                        new XAttribute("name", "Difficulty: easy"),
                        new XElement("score",
                            new XElement("name", players[0].Name),
                            new XElement("duration", players[0].Duration),
                            new XElement("dateTime", players[0].DateTime)
                        )
                    ),
                    new XElement("difficulty",
                        new XAttribute("name", "Difficulty: medium"),
                        new XElement("score",
                            new XElement("name", players[1].Name),
                            new XElement("duration", players[1].Duration),
                            new XElement("dateTime", players[1].DateTime)
                        )
                    ),
                    new XElement("difficulty",
                        new XAttribute("name", "Difficulty: hard"),
                        new XElement("score",
                            new XElement("name", players[2].Name),
                            new XElement("duration", players[2].Duration),
                            new XElement("dateTime", players[2].DateTime)
                        )
                    )
                );
            xml.Save("highScores.xml");
        }
    }
}
