using EasySave.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Mocks
{
    public class MockLogService
    {
        public void CreateLog(LogEntry entry)
        {
            Console.WriteLine(
                $"[MOCK LOG]\n" +
                $"  Time       : {entry.Timestamp}\n" +
                $"  Backup     : {entry.BackupName}\n" +
                $"  Source     : {entry.SourcePath}\n" +
                $"  Target     : {entry.TargetPath}\n" +
                $"  Size (B)   : {entry.FileSize}\n" +
                $"  Duration   : {entry.TransferTime} ms\n"
            );
        }
    }
}
