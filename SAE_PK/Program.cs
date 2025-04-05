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
       (msBehind, ticksBehind) = VerzögerungenAuslesen(DatenLesen());
       Console.WriteLine($"{msBehind[0]}" +" "+ $"{ticksBehind[0]}"); //temp
       // msBehind und ticksBehind sind die beiden Lists wo schon alles abgespeichert wurde
       Console.ReadKey();
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

        if (!logs[9].Contains("INFO]:") && !logs[9].Contains("WARN]:")) //Not working yet
        {
            Console.WriteLine("Die von dir angegebene Datei scheint nicht die korrekte log Datei zu sein.\nBitte überprüfe die Datei nochmal oder fahre trotzdem fort.\nFortfahren? [J]/[N(Press any button)]: ");
            if (Console.ReadLine() == "j" || Console.ReadLine() == "J")
            {
                return logs;
            }
            else
            {
                Console.WriteLine("Programm frühzeitig vom user beendet da log Datei inkorrekt war");
                Environment.Exit(1);
                return logs; //Otherwise method won't work
            }
        }
        else
        {
            return logs;
        }
    }

    public static (List<int>, List<int>) VerzögerungenAuslesen(List<string> logs) //Done
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

    public static (int[], int[]) KeineAhnungDigga(List<int> msBehind, List<int> ticksBehind)
    {
        msBehind.Sort();
        ticksBehind.Sort();
        int highestTick = ticksBehind[ticksBehind.Count - 1];
        int lowestTick = ticksBehind[0];
        int highestMs = msBehind[msBehind.Count - 1];
        int lowestMs = msBehind[0];
        int tickSum = 0;
        int msSum = 0;
        int msAverage = 0;
        int tickAverage = 0;

        for (int i = 0; i < ticksBehind.Count; i++)
        {
            tickSum += ticksBehind[i];
        }

        for (int i = 0; i < msBehind.Count; i++)
        {
            msSum += msBehind[i];
        }
        
        msAverage = msSum / msBehind.Count;
        tickAverage = tickSum / ticksBehind.Count;
        return;
    }
}