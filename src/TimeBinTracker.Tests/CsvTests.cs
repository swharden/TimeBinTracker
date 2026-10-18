namespace TimeBinTracker.Tests;

public class Tests
{
    [Test]
    public void Test_CSV_Parse()
    {
        CsvData csv = new(SampleData.CSV);
        TestContext.Out.WriteLine(csv);
        TestContext.Out.WriteLine();

        CsvDay updateDay1 = new("2026-10-14,1100,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,1000,0000,0000,0000,0000,0000,test-name");
        csv.Update(updateDay1);
        TestContext.Out.WriteLine(csv);
        TestContext.Out.WriteLine();

        CsvDay updateDay2 = new("2026-10-16,1100,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,1000,0000,0000,0000,0000,0000,test-name");
        csv.Update(updateDay2);
        TestContext.Out.WriteLine(csv);
        TestContext.Out.WriteLine();
    }
}
