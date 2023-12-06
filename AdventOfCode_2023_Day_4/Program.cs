
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

        var result = CalculateTotalScratchCards(inputList);
        Console.WriteLine(result);
    }

    private static int CalculateTotalWinning(List<string> inputs)
    {
        var result = new List<int>();

        foreach (var input in inputs)
        {
            var nummbers = GenerateWinningAndOwnNumberList(input);
            var trackWinningNumber = 0;
            var isFirstWin = true;

            foreach (var ownNumber in nummbers.ownNumbers)
            {
                if (nummbers.winningNumbers.Any(w => w == ownNumber))
                {
                    if (isFirstWin)
                    {
                        trackWinningNumber = 1;
                        isFirstWin = false;
                    }
                    else
                    {
                        trackWinningNumber *= 2;
                    }
                }
            }

            result.Add(trackWinningNumber);
        }

        return result.Sum(x => x);
    }

    private static int CalculateTotalScratchCards(List<string> inputs)
    {
        var bufferedInputs = new List<string>(inputs);

        var result = ProcessScratchCards(inputs, bufferedInputs);

        return result;
    }

    private static int ProcessScratchCards(List<string> inputs, List<string> bufferedInputs)
    {
        int index = 0;
        var cardsDictionary = new Dictionary<string, List<(List<int> winningNumbers, List<int> ownNumbers)>>();

        foreach (var input in inputs)
        {
            var winningAndOwnNumbers = GenerateWinningAndOwnNumberList(input);
            cardsDictionary.Add(winningAndOwnNumbers.key, new List<(List<int> winningNumbers, List<int> ownNumbers)> { (winningAndOwnNumbers.winningNumbers, winningAndOwnNumbers.ownNumbers) });
        }

        foreach (var key in cardsDictionary)
        {
            foreach(var value in key.Value)
            {
                var itemIndexToInsert = index;
                foreach (var ownNumber in value.ownNumbers)
                {
                    if (value.winningNumbers.Any(w => w == ownNumber))
                    {
                        var itemToInsert = ++itemIndexToInsert;
                        if (bufferedInputs.ElementAtOrDefault(itemToInsert) != null)
                        {
                            var winningAndOwnNumbers = GenerateWinningAndOwnNumberList(bufferedInputs[itemToInsert]);
                            cardsDictionary[winningAndOwnNumbers.key].Add((winningAndOwnNumbers.winningNumbers, winningAndOwnNumbers.ownNumbers));
                        }
                    }
                }                
            }
            index++;
        }

        var result = cardsDictionary.Values.ToList().Sum(v =>
        {
            return v.Select(w => w).Count();
        });

        return result;
    }

    private static (string key, List<int> winningNumbers, List<int> ownNumbers) GenerateWinningAndOwnNumberList(string input)
    {
        var splittedInput = input.Split("|");
        var key = splittedInput[0].Split(":")[0];
        var winningNumbersInString = splittedInput[0].Split(":")[1];
        var numbersIHaveInString = splittedInput[1] + " ";
        var winningNumbers = new List<int>();
        var ownNumbers = new List<int>();
        var tempWinningNumber = 0;
        var tempOwnNumber = 0;

        GenerateWinningAndOwnNumbers(winningNumbersInString, numbersIHaveInString, winningNumbers, ownNumbers, ref tempWinningNumber, ref tempOwnNumber);

        return (key, winningNumbers, ownNumbers);
    }

    private static void GenerateWinningAndOwnNumbers(string winningNumbersInString, string numbersIHaveInString, List<int> winningNumbers, List<int> ownNumbers, ref int tempWinningNumber, ref int tempOwnNumber)
    {
        foreach (var winningNumberChar in winningNumbersInString)
        {
            if (char.IsDigit(winningNumberChar))
            {
                tempWinningNumber = (tempWinningNumber * 10) + int.Parse(winningNumberChar.ToString());
            }
            if (!char.IsDigit(winningNumberChar) && tempWinningNumber != 0)
            {
                winningNumbers.Add(tempWinningNumber);
                tempWinningNumber = 0;
            }
        }

        foreach (var ownNumber in numbersIHaveInString)
        {
            if (char.IsDigit(ownNumber))
            {
                tempOwnNumber = (tempOwnNumber * 10) + int.Parse(ownNumber.ToString());
            }
            if (!char.IsDigit(ownNumber) && tempOwnNumber != 0)
            {
                ownNumbers.Add(tempOwnNumber);
                tempOwnNumber = 0;
            }
        }
    }
}