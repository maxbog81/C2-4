using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

//Богатов Максим


namespace lesson4
{
    class Program
    {
        //2.	Дана коллекция List<T>.Требуется подсчитать, сколько раз каждый элемент встречается в данной коллекции:
        //a.для целых чисел;
        //b.  * для обобщенной коллекции;
        //c.  ** используя Linq.
        private static Dictionary<T,int> GetUniques<T>(ICollection<T> list)
        {
            // Для отслеживания элементов используйте словарь 
            Dictionary<T, int> found = new Dictionary<T, int>();
            // Этот алгоритм сохраняет оригинальный порядок элементов 
            foreach (T val in list)
            {
                if (found.ContainsKey(val))
                    found[val] += 1; 
                else
                    found.Add(val, 1);

            }
            return found;
        }


        static void Main(string[] args)
        {
            var numbers = new List<int>() { 9, 9, 9, 5, 5, 51, 51, 27, 99, 99 };
            numbers.Sort();

            var dic = new Dictionary<int, int>();
            foreach (var n in numbers)
            {                
                if (dic.ContainsKey(n))
                    dic[n] += 1;
                else
                    dic.Add(n, 1);
            }
            WriteLine("Для целых чисел:");
            foreach (var x in dic.Keys)
                WriteLine("Число {0} встречается {1} раз(а).", x, dic[x]);

            var dicT = GetUniques(numbers);

            WriteLine("\nДля обобщенной коллекции:");

            foreach (var x in dicT.Keys)
                WriteLine("{0} встречается {1} раз(а).", x, dicT[x]);

            WriteLine("\nиспользуя Linq:");
            foreach (int val in numbers.Distinct())
            {
                WriteLine(val + " - " + numbers.Where(x => x == val).Count() + " раз(а)");
            }

            //var a = new[] { 11, 11, 23, 23, 23, 23, 23, 44, 88, 88 };
            //var g = a.GroupBy(i => i);
            //WriteLine("count: " + g.Count());
            //foreach (var k in g)
            //    WriteLine(k.Key + " (" + k.Count() + ")");

            WriteLine("\n");

            //3.	* Дан фрагмент программы:
            //а.Свернуть обращение к OrderBy с использованием лямбда-выражения =>.
            //b. * Развернуть обращение к OrderBy с использованием делегата.
            Dictionary<string, int> dict = new Dictionary<string, int>()
              {
                {"four",4 },
                {"two",2 },
                { "one",1 },
                {"three",3 },
              };
            //var d = dict.OrderBy(delegate (KeyValuePair<string, int> pair) { return pair.Value; });
            var d = dict.OrderBy(pair => pair.Value);
            foreach (var pair in d)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }


            ReadKey();
        }
    }
}
