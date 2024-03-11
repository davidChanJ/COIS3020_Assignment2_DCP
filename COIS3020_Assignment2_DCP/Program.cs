using COIS3020_Assignment2_DCP;
using Microsoft.VisualBasic;

public class Program
{
    public static void Main(string[] args)
    {
        Rope rope = new Rope("abcdefghijklmnopqrstuvwxyz");

        Console.WriteLine("ToString of the rope: " + rope.ToString());
        Console.WriteLine();

        Console.WriteLine("String after inserting test test test in index 14:");
        rope.Insert("test test test ", 14);
        Console.WriteLine(rope.ToString());
        Console.WriteLine();
        Console.WriteLine("Substring:");
        Console.WriteLine(rope.Substring(1, 13));
    }
}