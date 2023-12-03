public class PartNumberPosition
{
    public int Row { get; set; }

    public int Col { get; set; }
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

        var result = CalculateGearRatio(inputList);
        Console.WriteLine(result);
    }

    private static int CalculatePartNumbers(List<string> input)
    {
        var row = input.Count;
        var col = input[0].Length;
        var filteredPartNumbers = new List<int>();

        var partNumberArray = ExtractPartNumberArray(input, row, col);
        var adjacentToSymboolPartNumbers = GetAdjacentToSymboolPartNumbers(row, col, partNumberArray);

        foreach(var adjacentToSymboolPartNumber in adjacentToSymboolPartNumbers)
        {
            var isPartnumberFound = false;

            var leftMostPosition = new PartNumberPosition
            {
                Row = Math.Max(adjacentToSymboolPartNumber.leftMostPosition.Row - 1, 0),
                Col = Math.Max(adjacentToSymboolPartNumber.leftMostPosition.Col - 1, 0)
            };

            var rightMostPosition = new PartNumberPosition
            {
                Row = Math.Min(adjacentToSymboolPartNumber.rightMostPosition.Row + 1, row - 1),
                Col = Math.Min(adjacentToSymboolPartNumber.rightMostPosition.Col + 1, col - 1)
            };

            for(int i = leftMostPosition.Row; i <= rightMostPosition.Row; i++)
            {
                for(int j = leftMostPosition.Col; j <= rightMostPosition.Col; j++)
                {
                    if (!char.IsNumber(partNumberArray[i, j]) && partNumberArray[i, j] != '.')
                    {
                        filteredPartNumbers.Add(adjacentToSymboolPartNumber.partNumber);
                        isPartnumberFound = true;
                        break;
                    }
                }
                if(isPartnumberFound)
                {
                    break;
                }
            }
        }

        return filteredPartNumbers.Sum(x => x);
    }

    private static int CalculateGearRatio(List<string> input)
    {
        var row = input.Count;
        var col = input[0].Length;
        var gears = new Dictionary<(int row, int col), List<int>>();

        var partNumberArray = ExtractPartNumberArray(input, row, col);
        var adjacentToSymboolPartNumbers = GetAdjacentToSymboolPartNumbers(row, col, partNumberArray);

        foreach (var adjacentToSymboolPartNumber in adjacentToSymboolPartNumbers)
        {
            var isPartnumberFound = false;

            var leftMostPosition = new PartNumberPosition
            {
                Row = Math.Max(adjacentToSymboolPartNumber.leftMostPosition.Row - 1, 0),
                Col = Math.Max(adjacentToSymboolPartNumber.leftMostPosition.Col - 1, 0)
            };

            var rightMostPosition = new PartNumberPosition
            {
                Row = Math.Min(adjacentToSymboolPartNumber.rightMostPosition.Row + 1, row - 1),
                Col = Math.Min(adjacentToSymboolPartNumber.rightMostPosition.Col + 1, col - 1)
            };

            for (int i = leftMostPosition.Row; i <= rightMostPosition.Row; i++)
            {
                for (int j = leftMostPosition.Col; j <= rightMostPosition.Col; j++)
                {
                    if (partNumberArray[i, j] == '*')
                    {
                        if (!gears.Keys.Any(x => x == (i,j)))
                        {
                            gears.Add((i, j), new List<int> { adjacentToSymboolPartNumber.partNumber });
                        }
                        else
                        {
                            gears[(i, j)].Add((adjacentToSymboolPartNumber.partNumber));
                        }                        
                        isPartnumberFound = true;
                        break;
                    }
                }
                if (isPartnumberFound)
                {
                    break;
                }
            }
        }

        var result = 0;
        foreach(var gear in gears)
        {
            if(gear.Value.Count == 2)
            {
                result = result + gear.Value.Aggregate((x, y) => x * y);
            }
        }

        return result;
    }

    private static List<(int partNumber, PartNumberPosition leftMostPosition, PartNumberPosition rightMostPosition)> GetAdjacentToSymboolPartNumbers(int row, int col, char[,] partNumberArray)
    {
        List<(int partNumber, PartNumberPosition leftMostPosition, PartNumberPosition rightMostPosition)> adjacentToSymboolPartNumbers = new();
        var adjacentToSymboolPartNumber = 0;
        var leftMostPosition = new PartNumberPosition();
        var isLeftMostIndexAlreadyFound = false;
        var rightMostPosition = new PartNumberPosition();

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                if (char.IsNumber(partNumberArray[r, c]))
                {
                    if (!isLeftMostIndexAlreadyFound)
                    {
                        leftMostPosition = new PartNumberPosition
                        {
                            Row = r,
                            Col = c
                        };
                        isLeftMostIndexAlreadyFound = true;
                    }
                    rightMostPosition = new PartNumberPosition
                    {
                        Row = r,
                        Col = c
                    };
                    adjacentToSymboolPartNumber = adjacentToSymboolPartNumber * 10 + int.Parse(partNumberArray[r, c].ToString());
                }
                if (!char.IsNumber(partNumberArray[r, c]) && adjacentToSymboolPartNumber != 0)
                {
                    adjacentToSymboolPartNumbers.Add((adjacentToSymboolPartNumber, leftMostPosition, rightMostPosition));
                    isLeftMostIndexAlreadyFound = false;
                    adjacentToSymboolPartNumber = 0;
                    leftMostPosition = new PartNumberPosition();
                    rightMostPosition = new PartNumberPosition();
                    continue;
                }
            }
        }

        return adjacentToSymboolPartNumbers;
    }

    private static char[,] ExtractPartNumberArray(List<string> input, int row, int col)
    {
        
        char[,] partNumberArray = new char[row, col];

        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < col; c++)
            {
                partNumberArray[r, c] = input[r].ToCharArray()[c];
            }
        }

        return partNumberArray;
    }
}