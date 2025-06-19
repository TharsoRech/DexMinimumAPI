namespace DexMinimumApi.Models
{
    public class SaveDEXMeter
    {
        public string Machine { get; set; }
        public DateTime DEXDateTime { get; set; }
        public string MachineSerialNumber { get; set; }
        public decimal ValueOfPaidVends { get; set; }
    }
}
