using System.Numerics;

namespace SAE_PK;
using System.IO;

class Program
{
    public static void Main(string[] args)
    {
       List<int> msBehind = new();
       List<int> ticksBehind = new();
       Console.WriteLine("Dieses Programm wertet die log Datei eines Minecraft Servers aus und, \nspeichert die Dauer der Verzögerungen in einer neuen Datei ab.");
       Console.WriteLine("----------------------------------------------------------------------------------------");
       (msBehind, ticksBehind) = DatenVerarbeiten(DatenLesen());
       Console.WriteLine($"{msBehind[2]}" +" "+ $"{ticksBehind[3]}"); //temp
       // msBehind und ticksBehind sind die beiden Lists wo schon alles abgespeichert wurde
    }

    public static void DatenSpeichern()
    {
        
    }

    public static List<string> DatenLesen()
    {
        Console.WriteLine("Bitte gebe den Dateipfad der Logdatei an: ");
        string filePath = Console.ReadLine();
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Die Datei '{filePath}' wurde nicht gefunden");
        }
        List<string> logs = new();
        using StreamReader sr = new(filePath);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            logs.Add(line);
        }
        return logs;
    }

    public static (List<int>, List<int>) DatenVerarbeiten(List<string> logs) //Done
    {
        int msTimeWoMs = 0;
        string msTimeWMs = "";
        List<int> serveroverloadTimeTicks = new();
        List<int> serveroverloadTimeInMs = new();
        string[] splittedServeroverloadTime = new string[14]; //ms=10, tick=12
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
        logCounter = 0;
        while (logCounter < serveroverloadLog.Count)
        {
            splittedServeroverloadTime = serveroverloadLog[logCounter].Split(' ');    
            logCounter++;
            msTimeWMs = splittedServeroverloadTime[10];
            msTimeWoMs = int.Parse(msTimeWMs.Substring(0, msTimeWMs.Length - 2));
            serveroverloadTimeInMs.Add(msTimeWoMs);
            serveroverloadTimeTicks.Add(int.Parse(splittedServeroverloadTime[12]));
        }
        return (serveroverloadTimeInMs, serveroverloadTimeTicks);
    }
}