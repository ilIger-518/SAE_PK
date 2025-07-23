using System.Numerics; //idc1
using Microsoft.VisualBasic;

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
        int[] tempParameter = VerzögerungenVerarbeiten(msBehind, ticksBehind);
        DatenSpeichern(tempParameter);
        Console.ReadKey();
    }

    public static void DatenSpeichern(int[] everyLatency)
    {
        string dateAndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //Speichert das aktuelle Datum in dateAndTime ab
        // everyLatency = highestTick, lowestTick, highestMs, lowestMs, tickSum, msSum, msAverage, tickAverage 
        string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{dateAndTime} serverLatencies.txt"); //Sucht den Pfad zum Desktop, kombiniert ihn mit dem Dateinamen, dem aktuellen Datum und speichert ihn in "desktopPath" ab
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
            outputPath = Path.Combine(outputPath, $"{dateAndTime} serverLatencies.txt"); //Fügt das aktuelle Datum zum Dateinamen hinzu
        }
        else
        {
            outputPath = desktopPath;
        }
		

        using (StreamWriter sw = new StreamWriter(outputPath)) //Erstellt die Datei mit den Ergebnissen und speichert sie alle in der Datei ab
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

        if (File.Exists(outputPath)) //Prüft ob die Datei erfolgreich erstellt wurde
        {
            Console.WriteLine("Die Datei wurde erfolgreich erstellt.");
        }
        else
        {
            Console.WriteLine("Ein Fehler ist aufgetreten.");
        }
    }

    public static List<string> DatenLesen()
    {
        Console.WriteLine("Bitte gebe den Dateipfad der Logdatei an: ");
        string filePath = Console.ReadLine();
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Die Datei '{filePath}' wurde nicht gefunden"); //Wirft den Fehlercode "FileNotFoundException" wenn die Datei nicht existiert
        }
        List<string> logs = new();
        using StreamReader sr = new(filePath);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            logs.Add(line);
        }

        for (int i = 0; i < logs.Count; i++) {
            if (!logs[i].Contains("INFO]") && !logs[i].Contains("WARN]") && !logs[i].Contains("[Server thread/")) //Prüft ob es sich um eine Minecraft Log Datei handelt anhand von bestimmten Merkmalen
            {
                Console.WriteLine(
                    "Die von dir angegebene Datei scheint nicht die korrekte log Datei zu sein.\nBitte überprüfe die Datei nochmal oder fahre trotzdem fort.\nFortfahren? [J]/[N]: ");
                if (Console.ReadLine() == "j" || Console.ReadLine() == "J")
                {
                    return logs;
                }
                else
                {
                    Console.WriteLine("Programm frühzeitig vom user beendet da log Datei inkorrekt war");
                    Environment.Exit(1); //Beendet das Programm mit dem Fehlercode 1
                    return logs; 
                }
            }
            else
            {
                return logs;
            }
        }
                return logs;
    }

    public static (List<int>, List<int>) VerzögerungenAuslesen(List<string> logs)
    {
        int msTimeWoMs = 0;
        string msTimeWMs = "";
        List<int> serveroverloadTimeTicks = new();
        List<int> serveroverloadTimeInMs = new();
        string[] splittedServeroverloadTime = new string[14]; //ms=12, tick=14
        List<string> serveroverloadLog = new();
        int logCounter = 0;
        while (logCounter < logs.Count) //Relevanten Zeilen ausfiltern
        {
            if (logs[logCounter].Contains("Is the server overloaded?"))
            {
                serveroverloadLog.Add(logs[logCounter]);
            }
            logCounter++;
        }
        logCounter = 0;
        while (logCounter < serveroverloadLog.Count) //Verzögerungen aus Log Zeile rausfiltern
        {
            splittedServeroverloadTime = serveroverloadLog[logCounter].Split(' ');    
            logCounter++;
            msTimeWMs = splittedServeroverloadTime[12];
            msTimeWoMs = int.Parse(msTimeWMs.Substring(0, msTimeWMs.Length - 2));
            serveroverloadTimeInMs.Add(msTimeWoMs);
            serveroverloadTimeTicks.Add(int.Parse(splittedServeroverloadTime[14]));
        }
        return (serveroverloadTimeInMs, serveroverloadTimeTicks);
    }

    public static int[] VerzögerungenVerarbeiten(List<int> msBehind, List<int> ticksBehind)
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
