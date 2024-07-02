using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS2_02_LE_03_01.Model.WeatherModel
{
    public class WeatherData
    {
        public Coord coord { get; set; }
        public Weather[] weather { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public Sys sys { get; set; }
        public string name { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public int cod { get; set; }
    }
}
