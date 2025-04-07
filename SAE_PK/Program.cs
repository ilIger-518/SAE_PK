using System.Numerics;

namespace SAE_PK;
using System.IO;

class Program
{
    public static void Main(string[] args)
    {
       List<int> msBehind = new();
       List<int> ticksBehind = new();
       Console.WriteLine("Dieses Programm wertet die log Datei eines Minecraft Servers💀 aus und, \nspeichert die Dauer der Verzögerungen in einer neuen Datei ab.");
       Console.WriteLine("----------------------------------------------------------------------------------------");
       (msBehind, ticksBehind) = VerzögerungenAuslesen(DatenLesen());
       Console.WriteLine($"{msBehind[0]}" +" "+ $"{ticksBehind[0]}"); //temp
       int[] tempParameter = KeineAhnungDigga(msBehind, ticksBehind);
       DatenSpeichern(tempParameter);
       Console.ReadKey();
    }

    public static void DatenSpeichern(int[] everyLatency)
    {
        // everyLatency = highestTick, lowestTick, highestMs, lowestMs, tickSum, msSum, msAverage, tickAverage 
        string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "serverLatencies.txt");
        int userInput;
        while (true)
        {
            Console.WriteLine("Wo möchtest du die Datei abspeichern?\nBenutzerdefiniert [1]\nDesktop [2]");
            string input = Console.ReadLine();
            if (int.TryParse(input, out userInput) && (userInput == 1 || userInput == 2 ))
            {
                break;
            }

            Console.WriteLine("Ungültige Eingabe. Bitte gib 1 oder 2 ein");
        }

        string outputPath;
            
        if (userInput == 1)
        {
            Console.Write("Speicherpfad der Datei: ");
            outputPath = Convert.ToString(Console.ReadLine());
        }
        else
        {
            outputPath = desktopPath;
        }


        using (StreamWriter sw = new StreamWriter(outputPath)) // geht 
        {


            sw.WriteLine("Höchste verzögerung in ticks: " + everyLatency[0]);
            sw.WriteLine("Niedrigste verzögerung in ticks: " + everyLatency[1]);
            sw.WriteLine("Höchste verzögerung in Millisekunden: " + everyLatency[2]);
            sw.WriteLine("Niedrigste verzögerung in Millisekunden: " + everyLatency[3]);
            sw.WriteLine("Gesamtanzahl an verzögerungen in ticks: " + everyLatency[4]);
            sw.WriteLine("Gesamtanzahl an verzögerung in Millisekunden: " + everyLatency[5]);
            sw.WriteLine("Durchschnittliche verzögerung in Millisekunden: " + everyLatency[6]);
            sw.WriteLine("Durchschnittliche verzögerung in ticks: " + everyLatency[7]);
        }
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

        if (!logs[9].Contains("INFO]:") && !logs[9].Contains("WARN]:")) 
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

    public static int[] KeineAhnungDigga(List<int> msBehind, List<int> ticksBehind)  //method namen ändern
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
        int[] everyLatency;

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

        everyLatency = new int[8]
            { highestTick, lowestTick, highestMs, lowestMs, tickSum, msSum, msAverage, tickAverage };
        return(everyLatency);
    }
}