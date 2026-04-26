using EasySave.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Mocks
{
    public class MockStateService
    {
        public void Update(State state)
        {
            Console.WriteLine(
                $"[MOCK STATE]\n" +
                $"  Backup     : {state.BackupName}\n" +
                $"  Status     : {state.Status}\n" +
                $"  Timestamp  : {state.Timestamp}\n" +
                $"  Progress   : {state.TotalFiles - state.RemainingFiles}/{state.TotalFiles}\n" +
                $"  Remaining  : {state.RemainingFiles} files\n" +
                $"  Size left  : {state.RemainingSize} bytes\n" +
                $"  Current    : {state.CurrentSourceFile}\n"
            );
        }
    }
}
