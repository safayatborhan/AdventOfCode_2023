
public class Replacement
{
    public string Value { get; set; }
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

        ExtractWithStringAndNumericValue(inputList, out List<int> calibrationValueArray);

        var result = calibrationValueArray.Sum(x => x);
        Console.WriteLine(result);
    }

    private static void ExtractWithNumericValueOnly(List<string> inputList, out List<int> calibrationValueArray)
    {
        calibrationValueArray = new List<int>();

        foreach (string input in inputList)
        {
            ExtractCalibrationValue(calibrationValueArray, input);
        }
    }

    private static void ExtractWithStringAndNumericValue(List<string> inputList, out List<int> calibrationValueArray)
    {
        var numberList = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
            { "1", 1 },
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 },
        };

        calibrationValueArray = new List<int>();

        foreach (string input in inputList)
        {
            var formattedInput = input;

            string slicedInput = string.Empty;
            Replacement replacement = new Replacement();
            for(int i = 0; i < input.Length; i++)
            {
                slicedInput += input[i];
                if (numberList.Keys.Any(IsContaining(slicedInput, replacement))) // Passing a class for reference type XD
                {
                    if (!char.IsNumber(input[i])) // If this is a number, then there is no overlapping
                    {
                        formattedInput = formattedInput.Replace(replacement.Value, numberList[replacement.Value].ToString() + input[i].ToString());
                    }
                    else // Overlapping may need as this is a character
                    {
                        formattedInput = formattedInput.Replace(replacement.Value, numberList[replacement.Value].ToString());
                    }
                    //slicedInput = string.Empty;
                    slicedInput = input[i].ToString();
                }
            }

            ExtractCalibrationValue(calibrationValueArray, formattedInput);
        }
    }    

    private static Func<string, bool> IsContaining(string slicedInput, Replacement toBeReplaced)
    {
        return (string input) => 
        {
            if(slicedInput.Contains(input, StringComparison.OrdinalIgnoreCase))
            {
                toBeReplaced.Value = input;
                return true;
            }

            return false;
        };
    }

    private static void ExtractCalibrationValue(List<int> calibrationValueArray, string input)
    {
        var inputSize = input.Length;
        List<int> calibrationDocumentArray = new List<int>();

        var leftIndex = 0;
        var rightIndex = inputSize - 1;
        for (; leftIndex < inputSize; leftIndex++)
        {
            if (char.IsNumber(input[leftIndex]))
            {
                calibrationDocumentArray.Add(input[leftIndex] - 48);
                break;
            }
        }

        for (; rightIndex >= 0; rightIndex--)
        {
            if (char.IsNumber(input[rightIndex]))
            {
                calibrationDocumentArray.Add(input[rightIndex] - 48);
                break;
            }
        }

        var calibrationValue = string.Join("", calibrationDocumentArray);
        calibrationValueArray.Add(int.Parse(calibrationValue));
    }
}