using System.ComponentModel;
using System.Threading.Channels;
using Xunit.Abstractions;

namespace CaseApplication.IntegrationTests.SpeedTests
{
    public class OpeningCasesSpeedTest
    {
        private readonly ITestOutputHelper _output;
        private static readonly Random random = new();

        public OpeningCasesSpeedTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void OpeningSecondMaketTest()
        {
            List<int> lossChance = new()
            {
                2, 1, 3, 99994
            };

            List<int> CountDrop = new(lossChance.Count);

            foreach (decimal i in lossChance)
                CountDrop.Add(0);

            for (int i = 0; i < 1000; i++)
            {
                CountDrop[RandomizerSecond(lossChance)]++;
            }

            string outputDrop = "";
            foreach (int i in CountDrop)
            {
                outputDrop += $"{i} ";
            }
            _output.WriteLine(outputDrop);
        }

        private static int RandomizerSecond(List<int> lossChance)
        {
            List<List<int>> partsCaseChance = new();
            int startParts = 0;
            int lengthPart;
            int maxRandomValue;
            int randomNumber;
            int winIndex = 0;

            for (int i = 0; i < lossChance.Count; i++)
            {
                lengthPart = lossChance[i];
                partsCaseChance.Add(new List<int>() { startParts, startParts+lengthPart-1 });
                startParts += lengthPart;
            }
            maxRandomValue = partsCaseChance[^1][1];
            randomNumber = random.Next(0, maxRandomValue+1);

            for(int i = 0; i < partsCaseChance.Count; i++)
            {
                List<int> part = partsCaseChance[i];
                if (part[0] <= randomNumber && part[1] >= randomNumber)
                {
                    winIndex = i;
                }
            }

            return winIndex;
        }
    }
}
