namespace ConsoleApp1
{
    internal static class Test
    {
        static public void DoStuff(int value)
        {
            value += 40;
            Console.WriteLine(value);

            // to see this place in the dump debuggin, open the dump in IDE
            Console.ReadLine();
        }
    }
}
