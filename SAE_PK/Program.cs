namespace SAE_PK;
using System.IO;

class Program
{
    public static void Main(string[] args)
    {
       Console.WriteLine("Dieses Programm wertet die log Datei eines Servers aus verarbeitet die relevanten Daten.");
       Console.WriteLine("----------------------------------------------------------------------------------------");
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

    public static void DatenVerarbeiten()
    {
        List<string> logs = DatenLesen();
    }
}