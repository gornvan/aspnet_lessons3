namespace ConsoleApp1
{
    internal static class Test
    {
        static public void DoStuff(int value)
        {
            value += 42;
            Console.WriteLine(value);

            // to see this place in the dump debuggin, open the dump in IDE
            Console.Write("Waiting for ReadLine...");
            Console.ReadLine();
        }
    }
}
