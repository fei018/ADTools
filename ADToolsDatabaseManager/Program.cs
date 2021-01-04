using System;


namespace ADToolsDatabaseManager
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ADToolsDatabase db = new ADToolsDatabase();
                db.CodeFirstCreateDb();

                Console.WriteLine("Done.....");

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
