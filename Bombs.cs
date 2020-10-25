using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace Bombs
{
    class Program
    {
        static bool FillMatrix(int[,] matrix)
        {
            List<string> list = new List<string>();
            string text;
            int counter = 0;
            bool result = true;
            while(counter<matrix.GetLength(0))
            {
                text = Console.ReadLine();
                list = text.Split(' ').ToList();
                if(list.Count!=matrix.GetLength(1))
                {
                    result = false;
                    Console.WriteLine("Elements in the row should be equal to {0}",matrix.GetLength(0));
                    break;
                }
                else
                {
                    for (int i=0;i<list.Count;i++)
                    {
                        int.TryParse(list[i], out matrix[counter, i]);
                    }
                    counter++;
                }
            }
            return result;
        }
        static void PrintMatrix(int[,] matrix)
        {
            if(FillMatrix(matrix)==true)
            {
                for(int i=0;i<matrix.GetLength(0);i++)
                {
                    for(int j=0;j<matrix.GetLength(1);j++)
                    {
                        Console.Write("{0} ", matrix[i, j]);
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Matrix is not filled");
            }
        }
        static void GetCoordinates(Dictionary<int,List<int>> dict)
        {
            string text;
            text = Console.ReadLine();
            List<string>coordinates = text.Split(' ', ',').ToList();
            List<int> temp = new List<int>();
            for(int i=0;i<coordinates.Count;i+=2)
            {
                    int k = int.Parse(coordinates[i]);
                    int v = int.Parse(coordinates[i + 1]);
                    if(dict.ContainsKey(k)==true)
                    {
                        dict[k].Add(v);
                    }
                    else
                    {
                       temp.Add(v);
                       dict.Add(k, temp);
                       temp = new List<int>();
                    }
            }
        }
        static bool IsValidCoordinate(int[,] matrix,int row,int col)
        {
            if((row<matrix.GetLength(0) && row>=0) && (col>=0 && col<matrix.GetLength(1)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static void Attack(int[,] matrix,int row,int col,int explosionPower)
        {
            AttackNeighbours(matrix, row - 1, col, explosionPower);
            AttackNeighbours(matrix, row - 1, col-1, explosionPower);
            AttackNeighbours(matrix, row - 1, col+1, explosionPower);
            AttackNeighbours(matrix, row, col-1, explosionPower);
            AttackNeighbours(matrix, row, col+1, explosionPower);
            AttackNeighbours(matrix, row +1, col, explosionPower);
            AttackNeighbours(matrix, row + 1, col-1, explosionPower);
            AttackNeighbours(matrix, row + 1, col+1, explosionPower);
        }
        static void AttackNeighbours(int[,] matrix,int row,int col,int explosionPower)
        {
            if(IsValidCoordinate(matrix,row,col)==true)
            {
                if(matrix[row,col]>0)
                {
                    matrix[row, col] -= explosionPower;
                }
            }
        }
        static void SearchBombsAndExplode(int[,] matrix,Dictionary<int,List<int>> dict,ref int explosionPower)
        {
            foreach(KeyValuePair<int,List<int>> key in dict)
            {
                int i = key.Key;
                foreach(int j in key.Value)
                {
                    if(IsValidCoordinate(matrix,i,j)==true)
                    {
                        if(matrix[i,j]>0)
                        {
                            explosionPower = matrix[i, j];
                            matrix[i, j] = 0;
                            Attack(matrix, i, j, explosionPower);
                        }
                        else
                        {
                            Console.WriteLine("Cell at position [{0},{1}] is dead and cant be bomb", i, j);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid coordinates");
                    }
                }
            }
        }
        static void CountAndSumOfAliveCells(int[,] matrix,out int counter,out int sum)
        {
            sum = 0;
            counter = 0;
            for(int i=0;i<matrix.GetLength(0);i++)
            {
                for(int j=0;j<matrix.GetLength(1);j++)
                {
                    if(matrix[i,j]>0)
                    {
                        sum = sum + matrix[i, j];
                        counter++;
                    }
                }
            }
        }
        static void Main()
        {
            int n,counter,sum,explosionPower = 0;
            Console.WriteLine("Enter number of rows in the matrix:");
            n = int.Parse(Console.ReadLine());
            int[,] matrix = new int[n, n];
            PrintMatrix(matrix);
            Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
            GetCoordinates(dict);
            SearchBombsAndExplode(matrix, dict, ref explosionPower);
            CountAndSumOfAliveCells(matrix,out counter,out sum);
            Console.WriteLine("Number of alive cells is {0},sum of alive cells is {1}", counter, sum);
       }
    }
}
