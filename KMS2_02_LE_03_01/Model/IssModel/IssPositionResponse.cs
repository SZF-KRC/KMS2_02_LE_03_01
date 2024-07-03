namespace KMS2_02_LE_03_01.Model.IssModel
{
    public class IssPositionResponse
    {
        public string message { get; set; }
        public long timestamp { get; set; }
        public IssPosition iss_position { get; set; }
    }
}
