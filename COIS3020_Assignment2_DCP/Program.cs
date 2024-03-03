using COIS3020_Assignment2_DCP;

public class Program
{
    public static void Main(string[] args)
    {
        Rope rope = new Rope("Hello, I want to use the sample code to split");

        rope.PrintRope();

        //int ropeLength = rope.Length(rope.Root);
        //Console.WriteLine($"Length of the rope is {ropeLength}");

        rope.Rebalance();

        rope.PrintRope();
    }
}