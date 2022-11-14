using BionicSearchLib;
namespace SearchConsole
{
    internal class Program
    {
        private static void Main()
        {
            var SearchEngine = new  BionicSearchEngine(null, null, 103);


            //
            // Prepare data
            //

            string fileName = "airports.txt";
            string path = Path.Combine(Environment.CurrentDirectory, fileName);

            var lines = File.ReadAllLines(path);


            //
            // Insert
            //

            var key = 0; // foreign key
            var documents = new List<Document>();
            foreach (var item in lines)
            {
                var doc = new Document(key, 0, item, "");
                documents.Add(doc);
                key++;
            }

            SearchEngine.Insert(documents.ToArray());


            //
            // Index
            //

            SearchEngine.IndexAsync();


            //
            // Check status if ready to search
            //

            var status = SearchEngine.Status;
            while (!status.SearchIsAllowed) // Check and print status while indexing
            {
                Console.WriteLine(status.IndexProgressPercent + "%");
                System.Threading.Thread.Sleep(5); // log every 5ms
            }


            //
            // Set up search query
            //

            var text = "frnkfrt amain"; // pattern to be searched for
            var algorithm = Algorithm.ProprietaryRelevancyRanking; // Default algorithm
            var numRecords = 5; // Records to be returned
            var timeOutLimit = 1000; // Timeout if cpu overload in milliseconds
            var rmDuplicates = true; // remove duplicates with same key
            var logPrefix = ""; // logger prefix per search

            var query = new SearchQuery(text, algorithm, numRecords, timeOutLimit, rmDuplicates, logPrefix);
            
            //
            // Search and retrieve records
            //
            

            var result = SearchEngine.Search(query);

            foreach (var record in result.SearchRecords)
            {
                    Console.WriteLine(record.DocumentTextToBeIndexed);
                    Console.WriteLine(record.MetricScore);
            }


        } // end main
    } // end class
} // end namespace