using EasySave.Models;
using EasySave.Strategies;

namespace EasySave.Factories;

/// <summary>
/// Factory to create backup strategies.
/// </summary>
public class BackupStrategyFactory
{
    public IBackupStrategy Create(BackupType type)
    {
        // J'ai utilisé un switch modern de C#.
        // Il veut simplement dire que selon le type de backup demandé, on retourne une instance de la stratégie correspondante.
        // Si le type n'est pas reconnu, on lance une exception.
        return type switch
        {
            BackupType.Full => new FullBackupStrategy(),
            BackupType.Differential => new DifferentialBackupStrategy(),
            _ => throw new ArgumentException("Unknown backup type")
        };
    }
}