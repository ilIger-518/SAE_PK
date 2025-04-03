using System.Numerics;

namespace SAE_PK;
using System.IO;

class Program
{
    public static void Main(string[] args)
    {
       Console.WriteLine("Dieses Programm wertet die log Datei eines Servers aus verarbeitet die relevanten Daten.");
       Console.WriteLine("----------------------------------------------------------------------------------------");
       DatenVerarbeiten(DatenLesen());
    }

    public static void DatenSpeichern()
    {
        
    }

    public static List<string> DatenLesen()
    {
        List<string> runningBehind = new();
        string filePath = ("/Users/m.switon/Desktop/testLog.txt");
        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            while ((line = sr.ReadLine()) != null) 
            {
                runningBehind.Add(line);
            }
        }

        return runningBehind;
    }

    public static List<string> DatenVerarbeiten(List<string> logs)
    {
        List<string> serveroverloadLog = new();
        int logCounter = 0;
        while (logCounter < logs.Count)
        {
            if (logs[logCounter].Contains("Is the server overloaded?"))
            {
                serveroverloadLog.Add(logs[logCounter]);
            }
            logCounter++;
        }
        
        
        return;
    }
}