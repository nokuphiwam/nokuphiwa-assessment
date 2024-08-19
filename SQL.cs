string sql = "SELECT TOP 1000000 * FROM received WHERE status = 1 ORDER BY re_ref";

// List of SQL nodes to query
IEnumerable<IConfigurationSection> sqlNodes = Program.Configuration
    .GetSection("ConnectionStrings")
    .GetSection("SqlNodes")
    .GetChildren();

// Merged results set
List<received> results = new List<received>();

// Parallel.ForEach with MaxDegreeOfParallelism to control concurrent queries
ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 64 };
Parallel.ForEach(sqlNodes, parallelOptions, node =>
{
    received[] result = DBQuery<received>.Query(node.Value, sql);
    lock (results) // Synchronize access to shared list
    {
        results.AddRange(result);
    }
});

// Bulk insert instead of individual inserts
using (SqlConnection connection = new SqlConnection(ConnectionString))
{
    connection.Open();
    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
    {
        bulkCopy.DestinationTableName = "received_total";
        bulkCopy.WriteToServer(results);
    }
}
