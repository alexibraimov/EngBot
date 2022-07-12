using EngBotApp.Helpers;
using EngBotApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Files
{
    public static class WordCollection
    {
        static WordCollection() 
        {
            All = new List<Word>();
            Parser();
        }

        public static IList<Word> All { get; }

        private static void Parser()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Resources//allwords.csv");

            using(var sr = new StreamReader(path)) 
            {
                var line = sr.ReadLine();
                while (line!=null)
                {
                    var cell = line.Split(';');

                    All.Add(new Word(int.Parse(cell[0]), cell[1], cell[2]));

                    line = sr.ReadLine();
                }
            }
        }
    }
}
