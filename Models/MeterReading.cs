public class MeterReading
{
    public int Id { get; set; }
    public string MeterName { get; set; } = "";
    public float Power { get; set; }
    public float Voltage { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
