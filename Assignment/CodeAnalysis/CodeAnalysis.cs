namespace CodeAnalysis
{
    public class CodeAnalysis
    {
        public static void Func1(ref KeyValuePair<int, string>[] a, int key, string value)
        {
            Array.Resize(ref a, a.Length + 1);

            var keyValuePair = new KeyValuePair<int, string>(key, value);
            a[a.Length - 1] = keyValuePair;

            for (int i = 0; i < a.Length; i++)
            {
                for (int j = a.Length - 1; j > 0; j--)
                {
                    if (a[j - 1].Key > a[j].Key)
                    {
                        KeyValuePair<int, string> x;
                        x = a[j - 1];
                        a[j - 1] = a[j];
                        a[j] = x;
                    }
                }
            }
        }
        public static void Func1Lists(ref List<KeyValuePair<int, string>> list, int key, string value)
        {
            if (list == null)
            {
                list = new List<KeyValuePair<int, string>>([new KeyValuePair<int, string>(key, value)]);
                return;
            }
            list.Add(new KeyValuePair<int, string>(key, value));
            list.Sort((x, y) => x.Key.CompareTo(y.Key));
        }
        public static void Func1Linq(ref KeyValuePair<int, string>[] a, int key, string value)
        {
            if (a == null)
            {
                a = [new KeyValuePair<int, string>(key, value)];
                return;
            }
            a = a.Append(new KeyValuePair<int, string>(key, value)).OrderBy(x => x.Key).ToArray();
        }
        public static void Func1Insertion(ref KeyValuePair<int, string>[] a, int key, string value)
        {
            if (a == null)
            {
                a = [new KeyValuePair<int, string>(key, value)];
                return;
            }

            Array.Resize(ref a, a.Length + 1);
            a[^1] = new KeyValuePair<int, string>(key, value);

            for (int i = 1; i < a.Length; i++)
            {
                var current = a[i];

                int j = i - 1;
                for (; j >= 0 && a[j].Key > current.Key; j--)
                    a[j + 1] = a[j];

                a[j + 1] = current;
            }
        }
        public static void Func1QuickSort(ref KeyValuePair<int, string>[] a, int key, string value)
        {
            if (a == null)
            {
                a = [new KeyValuePair<int, string>(key, value)];
                return;
            }

            Array.Resize(ref a, a.Length + 1);
            a[^1] = new KeyValuePair<int, string>(key, value);

            QuickSort(ref a, 0, a.Length - 1);
        }

        public static void QuickSort(ref KeyValuePair<int, string>[] array, int leftIndex, int rightIndex)
        {
            var i = leftIndex;
            var j = rightIndex;
            var pivot = array[leftIndex + (rightIndex - leftIndex) / 2];

            while (i <= j)
            {
                while (array[i].Key < pivot.Key)
                    i++;

                while (array[j].Key > pivot.Key)
                    j--;

                if (i <= j)
                {
                    Swap(ref array[i], ref array[j]);
                    i++;
                    j--;
                }
            }

            if (leftIndex < j)
                QuickSort(ref array, leftIndex, j);

            if (i < rightIndex)
                QuickSort(ref array, i, rightIndex);
        }
        private static void Swap(ref KeyValuePair<int, string> a, ref KeyValuePair<int, string> b)
        {
            (b, a) = (a, b);
        }

    }

}
