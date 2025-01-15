using System;
using System.Text;


public class Question3
{
    public static void Main(string[] args)
    {

        try
        {
            Console.WriteLine("Input the number of row: ");
            int nRow = int.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("Input the number of column: ");
            int nColumn = int.Parse(Console.ReadLine() ?? "0");
            if (nColumn < 0 || nRow < 0)
            {
                Console.WriteLine("The number of row or column must be positive!");
            }
            string[][] board = new string[nRow][];
            Console.WriteLine($"Input the text seperated by space");
            for (int i = 0; i < nRow; i++)
            {
                Console.WriteLine($"In row {i + 1}: ");
                string[] row = (Console.ReadLine() ?? "").Split(" ");
                board[i] = row;
            }
            Console.WriteLine("Input the word needed to find: ");
            string target = Console.ReadLine() ?? "";
            bool[,] visited = new bool[nRow, nColumn];
            //visited[0, 0] = true;
            bool answer = DFS(board, nRow, nColumn, target, 0, 0, 0, visited);

            Console.WriteLine($"The answer is: {answer}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Please input the valid number!");
        }
    }

    public static bool DFS(string[][] board, int nRow, int nColumn, string target, int idx, int r, int c, bool[,] visited)
    {
        if (idx == target.Length)
        {
            return true;
        }

        if (r < 0 || r >= nRow || c < 0 || c >= nColumn || visited[r, c] || board[r][c].ToString() != target.Substring(idx, 1))
        {
            return false;
        }
        visited[r, c] = true;
        bool answer = DFS(board, nRow, nColumn, target, idx + 1, r + 1, c, visited) ||
            DFS(board, nRow, nColumn, target, idx + 1, r, c + 1, visited) ||
            DFS(board, nRow, nColumn, target, idx + 1, r - 1, c, visited) ||
            DFS(board, nRow, nColumn, target, idx + 1, r, c - 1, visited);

        visited[r, c] = false;
        return answer;


    }
}