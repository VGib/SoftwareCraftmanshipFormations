using System;

namespace ConsoleQuickSort
{
    static class Program
    {


        static void Main(string[] args)
        {

            int[] test_array = RandomArray(200);
            DoQuickSort(test_array);

            Console.WriteLine(string.Join(",", test_array));
            TestThatArrayIsSort(test_array);
        }

        static void TestThatArrayIsSort(int[] test_array)
        {
            for (int position = 0; position < test_array.Length - 1; position++)
            {
                if(test_array[position] > test_array[position + 1])
                {
                    throw new Exception($"The array is not sorted position {position} : {test_array[position]} > {position + 1} : {test_array[position + 1]}");
                }
            }
        }
        private static int[] RandomArray(int maxLength)
        {
            var test_array = new int[maxLength];

            for (int position = 0; position < maxLength; position++)
            {
                test_array[position] = rnd.Next(10_000);
            }

            return test_array;
        }

        private struct ArrayLimit
            {
            public int Bottom { get; set; }
            public int Top { get; set; }

            internal int Size()
            {
                return Top - Bottom + 1;
            }
        }

        static void DoQuickSort( int[] test_array)
        {
            DoQuickSort(new ArrayLimit { Bottom = 0, Top = test_array.Length - 1 }, test_array);
        }

        static void DoQuickSort(ArrayLimit array_limit, int[] test_array)
        {

            if(array_limit.Size() <= 1)
            {
                return;
            }

            if(array_limit.Size() == 2)
            {
                SortArrayOfLength2(array_limit, test_array);
                return;
            }

            var pivot = choosePivot(array_limit, test_array);
            (var left, var right) = partitionAndReturnleftPivotArrayAndRightPivotArray(array_limit, pivot, test_array);

            DoQuickSort(left, test_array);
            DoQuickSort(right, test_array);
     
        }

        private static void SortArrayOfLength2(ArrayLimit array_limit, int[] test_array)
        {
            if(test_array[array_limit.Bottom] > test_array[array_limit.Top])
            {
                Swap(array_limit.Bottom, array_limit.Top, test_array);
            }
         }

        private static Random rnd = new Random();

   

        private static (ArrayLimit left, ArrayLimit right) partitionAndReturnleftPivotArrayAndRightPivotArray(ArrayLimit array_limit,int pivot, int[] array)
        {
            var i = array_limit.Bottom;
            var j = array_limit.Top - 1;

            Swap(pivot, array_limit.Top, array);

            while(j >= i )
            {
                if(array[i] <= array[array_limit.Top])
                {
                    ++i;
                }
                else
                {
                    Swap(i, j, array);
                    --j;
                }
            }

            pivot = Math.Max(i, j);
            Swap(array_limit.Top, pivot, array);

            return (
                new ArrayLimit
                {
                    Bottom = array_limit.Bottom,
                    Top = pivot - 1
                },
                new ArrayLimit
                {
                    Bottom = pivot + 1,
                    Top = array_limit.Top
                }
                );

        }

        private static void Swap(int left, int right, int[] array)
        {
            var temp = array[left];
            array[left] = array[right];
            array[right] = temp;
        }

        private static int choosePivot(ArrayLimit array_limit, int[] test_array) => rnd.Next(array_limit.Bottom, array_limit.Top);
    }
}
