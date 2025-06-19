using DexMinimumApi.Models;
using DexMinimumApi.Repository;
using System.Globalization;
using System.Reflection.PortableExecutable;

namespace DexMinimumApi.Services;

public class DexService:IDexService
{
    IDexRepository _dexRepository;
    public DexService(IDexRepository dapperContext)
    {
        _dexRepository = dapperContext;
    }
    public async Task SaveDexFile(string content, string machineName)
    {
        var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        SaveDEXMeter dexMeter = new();
        dexMeter.Machine = machineName;
        List<SaveDEXLaneMeter> laneMeters = new();

        foreach (var line in lines)
        {
            var parts = line.Split('*');
            var prefix = parts[0];

            switch (prefix)
            {
                case "ID1":
                    dexMeter.MachineSerialNumber = parts[1];
                    break;

                case "ID5":
                    string dateStr = parts[1];
                    string timeStr = parts[2];
                    if (DateTime.TryParseExact(dateStr + timeStr, "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                    {
                        dexMeter.DEXDateTime = dt;
                    }
                    break;

                case "VA1":
                    if (decimal.TryParse(parts[1], out decimal va1))
                    {
                        dexMeter.ValueOfPaidVends = va1;
                    }
                    break;
                case "PA1":
                    var lane = new SaveDEXLaneMeter();
                    lane.ProductIdentifier = parts[1];

                    if (decimal.TryParse(parts[2], out decimal price))
                    {
                        lane.Price = price;
                    }
                    laneMeters.Add(lane);
                    break;
                case "PA2":
                     var lastlane = laneMeters.LastOrDefault();
                    if(lastlane != null)
                    {
                        if (int.TryParse(parts[1], out int numberOfVends))
                        {
                            lastlane.NumberOfVends = numberOfVends;
                        }
                        if (decimal.TryParse(parts[2], out decimal paidSales))
                        {
                            lastlane.ValueOfPaidSales = paidSales;
                        }
                    }
                    break;
                default:
                    break;

            }

        }
        var saveDexMeterSuccefully = await _dexRepository.SaveDEXMeter(dexMeter);

        if (!saveDexMeterSuccefully)
        {
            throw new Exception("Dex Meter not saved sucefully");
        }

        foreach (var lane in laneMeters)
        {
            var saveDexMeterLanelSuccefully = await _dexRepository.SaveDEXLaneMeter(lane);

            if (!saveDexMeterSuccefully)
            {
                throw new Exception("Dex Meter Lane not saved sucefully");
            }
        }

    }

}