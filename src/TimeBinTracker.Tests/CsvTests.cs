namespace TimeBinTracker.Tests;

public class Tests
{
    [Test]
    public void Test_CSV_Parse()
    {
        CsvData csv = new(SampleData.CSV);
        TestContext.Out.WriteLine(csv);
    }
}
