namespace DexMinimumApi.Services;

public interface IDexService
{
    Task SaveDexFile(string content, string machineName);
}