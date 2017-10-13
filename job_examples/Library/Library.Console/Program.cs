namespace Library.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var job = new CustomJob();

            job.ExecuteJob();

            System.Console.ReadLine();
        }
    }
}
