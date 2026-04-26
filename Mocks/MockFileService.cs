using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Mocks
{
    public class MockFileService
    {
        public void CopyFile(string source, string target)
        {
            //Console.WriteLine($"[MOCK COPY] {source} -> {target}");

            // Désolé, Clément. J'ai dû mettre une fonction de copie de base, réelle. 
            File.Copy(source, target, true);
        }
    }
}
