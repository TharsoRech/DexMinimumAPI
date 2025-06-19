
using DexMinimumApi.Context;
using DexMinimumApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;

namespace DexMinimumApi.Repository;

public class DexRepository : IDexRepository
{
    IDapperContext _context;
    public DexRepository(IDapperContext dapperContext)
    {
        _context = dapperContext;
    }
    public async Task<bool> SaveDEXMeter(SaveDEXMeter saveDEXMeter)
    {
        int dexMeterId = await _context.ExecuteStoreProcedure(
            "SaveDEXMeter",
            saveDEXMeter
        );

        return dexMeterId > 0;
    }

    public async Task<bool> SaveDEXLaneMeter(SaveDEXLaneMeter saveDEXMeter)
    {
        int dexMeterId = await _context.ExecuteStoreProcedure(
            "SaveDEXMeter", saveDEXMeter
        );

        return dexMeterId > 0;
    }
}