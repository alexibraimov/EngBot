using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Models
{
    public class Word
    {
        public Word(int id, string en, string ru)
        {
            Id = id;
            En = en;
            Ru = ru;
        }

        public int Id { get; }

        public string En { get; }

        public string Ru { get; }
    }
}
