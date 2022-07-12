using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Helpers
{
    public class RandomHelper
    {
        static RandomHelper()
        {
            Random = new Random();
        }
        public readonly static Random Random;
    }
}
