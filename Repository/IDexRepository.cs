using DexMinimumApi.Models;

namespace DexMinimumApi.Repository;

public interface IDexRepository
{
    Task<bool> SaveDEXMeter(SaveDEXMeter saveDEXMeter);

    Task<bool> SaveDEXLaneMeter(SaveDEXLaneMeter saveDEXMeter);
}