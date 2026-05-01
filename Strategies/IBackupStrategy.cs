using EasySave.Models;

namespace EasySave.Strategies;

/// <summary>
/// Defines a backup execution strategy.
/// </summary>
public interface IBackupStrategy
{
    // La méthode Execute prend un BackupJob en paramètre et exécute la logique de sauvegarde définie par la stratégie.
    // Chaque stratégie devra implémenter cette méthode pour réaliser le processus de sauvegarde spécifique à son type.
    // C'est ça l'avantage de l'inteface : elle définit un contrat que toutes les stratégies doivent respecter.
    void Execute(BackupJob job);
}