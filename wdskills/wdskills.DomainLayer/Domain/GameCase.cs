using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wdskills.DomainLayer.Domain
{
    public class GameCase
    {
        public string GameCaseName { get; set; } = null!;
        public Dictionary<GameItem, decimal> GameCaseContent { get; set; } = null!;
        public decimal GameCaseCost { get; set; }
        public decimal GameCaseSetPrecentageRevenue { get; set; } = 0.1M;
    }
}
