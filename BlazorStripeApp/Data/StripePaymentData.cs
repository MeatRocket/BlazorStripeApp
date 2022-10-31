using Microsoft.Azure.Cosmos.Table;

namespace BlazorStripeApp.Data
{
    public class StripePaymentData
    {
        private CloudStorageAccount StorageClient { get; set; }
        private CloudTableClient TableClient { get; set; }
        private string UserTableName { get; set; }
        private static CloudTable StripeCustomersTables { get; set; }
        private string RawKey => "FailedPayment";
        public StripePaymentData(string connectionString, string usersTableName)
        {
            StorageClient = CloudStorageAccount.Parse(connectionString);
            TableClient = StorageClient.CreateCloudTableClient();
            StripeCustomersTables = TableClient.GetTableReference(usersTableName);
           
        }
       
        public async Task<ClientPayment> GetUserPayment()
        {
            TableQuery<ClientPayment> tableQuery = new();
            TableContinuationToken continuationToken = null;
            TableQuerySegment<ClientPayment> query;

            try
            {
                do
                {
                    query = await StripeCustomersTables.ExecuteQuerySegmentedAsync(
                        tableQuery, continuationToken).ConfigureAwait(false);

                    return query.Results.FirstOrDefault();

                    continuationToken = query.ContinuationToken;

                } while (continuationToken != null);
            }
            catch (Exception) { }

            return null;
        }

        public async Task DeleteClientPayment(string paymentId)
        {
            try
            {

                await StripeCustomersTables.ExecuteAsync(TableOperation.Delete(new TableEntity()
                {
                    PartitionKey = paymentId,
                    RowKey = RawKey,
                    ETag ="*"

                }));
                }
            catch (Exception) { }
        }

        public async Task AddClientPayment(ClientPayment client)
        {
            try
            {

                await StripeCustomersTables.ExecuteAsync(TableOperation.InsertOrReplace(new ClientPayment()
                {
                    PartitionKey = client.PaymentId,
                    RowKey = RawKey,
                    ClientName = client.ClientName,
                    PaymentAmount = client.PaymentAmount,
                    PaymentId = client.PaymentId

                }));
            }
            catch (Exception) { }
        }


    }
}
