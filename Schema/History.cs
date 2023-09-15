using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAFFLE.Schema
{
    public class HistoryEntity
    {
        public string Time { get; set; }
        public float Rate { get; set; }
        public float Price { get; set; }
        public int WinnerNumber { get; set; }
        public float WinnerPrice { get; set; }
        public float AdminPrice { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
