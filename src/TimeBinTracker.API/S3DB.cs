using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace TimeBinTracker.API;

public class S3DB
{
    private const string ObjectKey = "time-bin-data.csv";
    private const string BucketName = "time-bin-tracker";
    private const string S3Region = "us-east-2";
    private static readonly AmazonS3Client S3Client = new(RegionEndpoint.GetBySystemName(S3Region));

    public S3DB()
    {
    }

    public async Task<CsvData> ReadData(CancellationToken cancellationToken = default)
    {
        try
        {
            using GetObjectResponse response = await S3Client.GetObjectAsync(BucketName, ObjectKey, cancellationToken);
            using var reader = new StreamReader(response.ResponseStream);
            string csv = await reader.ReadToEndAsync(cancellationToken);
            return new CsvData(csv);
        }
        catch (AmazonS3Exception ex) when (ex.ErrorCode == "NoSuchKey")
        {
            await Initialize(cancellationToken);
            return new CsvData(string.Empty);
        }
    }

    private static async Task Initialize(CancellationToken cancellationToken)
    {
        await S3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = BucketName,
            Key = ObjectKey,
            ContentBody = string.Empty,
            ContentType = "text/csv",
            IfNoneMatch = "*"
        }, cancellationToken);
    }

    public Task WriteData(CsvData csvData, CancellationToken cancellationToken = default)
    {
        return S3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = BucketName,
            Key = ObjectKey,
            ContentBody = csvData.ToString(),
            ContentType = "text/csv"
        }, cancellationToken);
    }
}
