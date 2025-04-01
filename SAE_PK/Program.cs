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

    public static void DatenLesen()
    {
        List<string> runningBehind = new();
        StreamReader sr = new StreamReader("C:\\Users\\{Username}\\Desktop\\serverLog.log");
        
        
    }

    public static void DatenVerarbeiten()
    {
        
    }
}