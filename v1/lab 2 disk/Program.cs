using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab_2_disk
{
    class Graf
    {
        int[][] matr; // булева матрица графа
        int[] T_plus; // прямое транзитивное замыкание
        int[] T_minus; // обратное транзитивное замыкание 
        int num; // стартовая точка       

        // конструтор, arg1 - булева матрица, arg2 - стартовая точка
        public Graf(int[][] arg1, int arg2)
        {
            num = arg2;
            Array.Resize(ref matr, arg1.Length);
            Array.Resize(ref T_plus, arg1.Length);
            Array.Resize(ref T_minus, arg1.Length);
            for (int i = 0; i < arg1.Length; i++)
            {
                Array.Resize(ref matr[i], arg1[i].Length);
                T_plus[i] = -1;
                T_minus[i] = -1;
                for (int j = 0; j < arg1[i].Length; j++)
                    matr[i][j] = arg1[i][j];
            }
            T_plus[num] = 0;
            T_minus[num] = 0;
        }

        // нахождение прямого транзитивного замыкания, k-путь        
        public void T_plus_Find(int k)
        {
            bool writer = false; // признак записи
            for (int i = 0; i < T_plus.Length; i++)
            {
                if (T_plus[i] == k)
                {
                    for (int j = 0; j < T_plus.Length; j++)
                        // если вершина связана и не помечена в замыкании
                        if ((matr[i][j] == 1) && (T_plus[j] == -1))
                        {
                            T_plus[j] = k + 1;
                            writer = true;
                        }
                }
            }
            k++; // увеличение пути
            // рекурсивный вызов, если была запись
            if (writer) T_plus_Find(k);
        }

        // нахождение обратного транзитивного замыкания, k - путь
        public void T_minus_Find(int k)
        {
            bool writer = false; // признак записи
            for (int j = 0; j < T_minus.Length; j++)
            {
                if (T_minus[j] == k)
                {
                    for (int i = 0; i < T_minus.Length; i++)
                        // если вершина связана и не помечена в замыкании
                        if ((matr[i][j] == 1) && (T_minus[i] == -1))
                        {
                            T_minus[i] = k + 1;
                            writer = true;
                        }
                }
            }
            k++; // увеличение пути
            // рекурсивный вызов, если была запись
            if (writer) T_minus_Find(k);
        }

        //нахождение связаных вершин и создание нового графа без этих вершин
        //conect - массив, содержащий связанные вершины
        public bool NewGraf_Find(out string[] conect)
        {
            conect = new string[0];
            for (int i = 0; i < T_plus.Length; i++)
            {
                // если вершины в обоих замыканиях
                if ((T_plus[i] != -1) && (T_minus[i] != -1))
                {
                    Array.Resize(ref conect, conect.Length + 1);
                    // запись имени вершины
                    conect[conect.Length - 1] = "X" + (i + 1).ToString();
                    // помечание столбца и строки вершины
                    for (int j = 0; j < matr.Length; j++)
                    {
                        matr[i][j] = -1;
                        matr[j][i] = -1;
                    }
                }
            }
            // очищение массивов замыканий
            for (int i = 0; i < matr.Length; i++)
            {
                T_plus[i] = -1;
                T_minus[i] = -1;
            }

            for (int i = 0; i < matr.Length; i++)
                for (int j = 0; j < matr.Length; j++)
                    // если есть хоть одна не помеченая вершина
                    if (matr[i][j] != -1)
                    {
                        num = i; // новая стартовая позиция
                        T_plus[num] = 0;
                        T_minus[num] = 0;
                        return true;
                    }
            return false;
        }
    }    
    class Program
    {
        static void Main(string[] args)
        {
            
            int[][] matr_graf = new int[0][];
            //matr_graf[0] = new int[] {1, 0, 1, 0, 0, 0, 0};
            //matr_graf[1] = new int[] {1, 0, 0, 0, 0, 0, 0};
            //matr_graf[2] = new int[] {0, 1, 0, 0, 0, 0, 0};
            //matr_graf[3] = new int[] {0, 0, 1, 0, 0, 0, 1};
            //matr_graf[4] = new int[] {0, 0, 0, 0, 1, 1, 0};
            //matr_graf[5] = new int[] {0, 0, 0, 0, 0, 0, 0};
            //matr_graf[6] = new int[] {0, 0, 0, 1, 0, 1, 0};

            int[] g = new int[0]; //{1 3 1 2 3 7 5 6 4 6};
            int[] p = new int[0]; //{2 3 4 6 8 8 10};
            do
            {
                Console.WriteLine("<=====Ввод графа в формате MFO=====>");
                do
                {
                    Console.WriteLine("Введите массив G:");
                }
                while (Array_InPut(Console.ReadLine(), out g));
                do
                {
                    Console.WriteLine("Введите массив P:");
                }
                while (Array_InPut(Console.ReadLine(), out p));
            }
            while( MFO_Read(g, p, out matr_graf) ) ;
            for (int i = 0; i < matr_graf.Length; i++)
            {
                Console.WriteLine("");
                for (int j = 0; j < matr_graf[i].Length; j++)
                    Console.Write(matr_graf[i][j] + " ");
            }
            Console.WriteLine("");

            int start = 0;
            if (start >= matr_graf.Length)
                start = 0;
            string[][] points = new string[0][];
            
            bool z = false;
            Graf obj = new Graf(matr_graf, start);
            do
            {
                obj.T_plus_Find(0);
                obj.T_minus_Find(0);
                Array.Resize(ref points, points.Length + 1);
                z = obj.NewGraf_Find(out points[points.Length - 1]);

            }
            while (z);

            Console.Write("Максимально-связаные вершины: " + '\n');
            for (int i = 0; i < points.Length; i++)
            {
                for (int j = 0; j < points[i].Length; j++)
                    Console.Write(points[i][j] + " ");
                Console.WriteLine("");
            }

            Console.ReadLine();
        }

        // считывание графа формата MFO
        static bool MFO_Read(int[] G, int[] P, out int[][] graf)
        {
            graf = new int[P.Length][];
            for (int i = 0; i < G.Length; i++ )
                if ((G[i] < 1) || (G[i] > P.Length))
                {
                    Console.WriteLine("Массив G не корректен!");
                    return true;
                }
            for (int i = 0; i < P.Length; i++)
            {
                if ((P[i] < 0) || (P[i] > G.Length))
                {
                    Console.WriteLine("Массив P не корректен!");
                    return true;
                }
                if ( (i != 0) && (P[i-1] > P[i]) )
                {
                    Console.WriteLine("Массив Р не корректен! (Элементы массива должны быть возрастающими)");
                    return true;
                }
            }

            if (P.Length > 1)
                if (P[0] > P[1])
                {
                    Console.WriteLine("Массив Р не корректен! (Элементы массива должны быть возрастающими)");
                    return true;
                }

            
            for (int i = 0; i < P.Length; i++)
            {
                graf[i] = new int[P.Length];
                for (int j = 0; j < graf[i][j]; j++)
                    graf[i][j] = 0;
            }
            int start = 0;
            for (int i = 0; i < P.Length; i++)
            {
                int t = -1;
                for (int j = start; j < P[i]; j++)
                {
                    graf[i][G[j]-1] = 1;
                    t = P[i];
                }
                if (t != -1) start = t;
            }

            return false;
        }

        static bool Array_InPut(string str, out int[] arr)
        {
            str = str.Trim();
            int index = str.IndexOf("  ");
            while (index != -1)
            {
                str = str.Remove(index, 1);
                index = str.IndexOf("  ");
            }
            string[] nums = str.Split(new char[] {' '});
            arr = new int[nums.Length];
            try
            {
                for (int i = 0; i < nums.Length; i++)
                    arr[i] = int.Parse(nums[i]);
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка ввода! Попробуйте еще раз!");
                return true;
            }
            return false;
        
        }
    }
}
