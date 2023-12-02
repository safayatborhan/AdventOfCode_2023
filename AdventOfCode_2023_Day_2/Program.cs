
public class Kube
{
    public int Blue { get; set; }

    public int Green { get; set; }

    public int Red { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        string userInput;
        List<string> inputList = new List<string>();
        while ((userInput = Console.ReadLine()) != null && userInput != "")
        {
            inputList.Add(userInput);
        }

        //var result = CalculateSumOfPossibleCombinations(inputList, 12, 13, 14);
        var result = CalculateSumOfMinimumSetOfKubes(inputList);
        Console.WriteLine(result);
    }

    private static int CalculateSumOfPossibleCombinations(List<string> inputs, int redThreshold, int blueThreshold, int greenThreshold)
    {
        var gameDictionary = ExtractGameDictionary(inputs);
        var possibilities = new List<int>();

        foreach(var game in gameDictionary)
        {
            if(!game.Value.Any(g => g.Red > redThreshold || g.Blue > blueThreshold || g.Green > greenThreshold))
            {
                possibilities.Add(game.Key);
            }
        }

        var totalPossibleCount = possibilities.Sum(x => x);

        return totalPossibleCount;
    }

    private static int CalculateSumOfMinimumSetOfKubes(List<string> inputs)
    {
        var gameDictionary = ExtractGameDictionary(inputs);
        var result = 0;

        foreach (var game in gameDictionary)
        {
            var maxNumberOfRed = game.Value.Max(x => x.Red);
            var maxNumberOfGreen = game.Value.Max(x => x.Green);
            var maxNumberOfBlue = game.Value.Max(x => x.Blue);

            var power = maxNumberOfRed * maxNumberOfGreen * maxNumberOfBlue;
            result += power;
        }

        return result;
    }

    private static Dictionary<int, List<Kube>> ExtractGameDictionary(List<string> inputs)
    {
        var gameDictionary = new Dictionary<int, List<Kube>>();

        foreach (var input in inputs)
        {
            var splittedInput = input.Split(':');
            var gameId = int.Parse(splittedInput[0].Replace("Game", string.Empty).Trim());

            var loads = splittedInput[1].Split(';');
            var listOfKube = new List<Kube>();

            foreach (var load in loads)
            {
                var kubes = load.Split(",");
                foreach (var kube in kubes)
                {
                    if (kube.Contains("blue"))
                    {
                        var numberOfKube = int.Parse(kube.Replace("blue", string.Empty).Trim());
                        listOfKube.Add(new Kube
                        {
                            Blue = numberOfKube
                        });
                    }
                    if (kube.Contains("red"))
                    {
                        var numberOfKube = int.Parse(kube.Replace("red", string.Empty).Trim());
                        listOfKube.Add(new Kube
                        {
                            Red = numberOfKube
                        });
                    }
                    if (kube.Contains("green"))
                    {
                        var numberOfKube = int.Parse(kube.Replace("green", string.Empty).Trim());
                        listOfKube.Add(new Kube
                        {
                            Green = numberOfKube
                        });
                    }
                }                
            }

            gameDictionary.Add(gameId, listOfKube);
        }

        return gameDictionary;
    }
}