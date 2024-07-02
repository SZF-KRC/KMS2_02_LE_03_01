using System.Collections.Generic;

namespace KMS2_02_LE_03_01.Model.WallStreetModel
{

    public class NewsApiResponse
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }
        public List<Article> Articles { get; set; }
    }
}
