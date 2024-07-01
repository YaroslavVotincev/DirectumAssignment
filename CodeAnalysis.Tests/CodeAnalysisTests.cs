namespace CodeAnalysis.Tests
{
    public class CodeAnalysisTests
    {
        private readonly KeyValuePair<int, string>[] testArray = [

            new (5, "e"),
            new (3, "c"),
            new (1, "a"),
            new (4, "d"),
            new (2, "b"),
            new (7, "g"),
            new (6, "f"),
            new (10, "j"),
            new (8, "h"),
            new (9, "i"),
            new (12, "l"),
            new (11, "k"),
            new (14, "n"),
            new (13, "m"),
            new (17, "q"),
            new (16, "p"),
            new (15, "o"),
            new (19, "s"),
            new (18, "r"),
            new (21, "u"),
            new (20, "t"),
            new (22, "v"),
            new (23, "w"),
            new (24, "x"),
            new (26, "z"),
        ];
        private readonly KeyValuePair<int, string> newElement = new(25, "y");
        private readonly KeyValuePair<int, string>[] targetArray = [
            new (1, "a"),
            new (2, "b"),
            new (3, "c"),
            new (4, "d"),
            new (5, "e"),
            new (6, "f"),
            new (7, "g"),
            new (8, "h"),
            new (9, "i"),
            new (10, "j"),
            new (11, "k"),
            new (12, "l"),
            new (13, "m"),
            new (14, "n"),
            new (15, "o"),
            new (16, "p"),
            new (17, "q"),
            new (18, "r"),
            new (19, "s"),
            new (20, "t"),
            new (21, "u"),
            new (22, "v"),
            new (23, "w"),
            new (24, "x"),
            new (25, "y"),
            new (26, "z"),
            ];

        [Fact]
        public void TestFunc1()
        {
            var arr = new KeyValuePair<int, string>[testArray.Length];
            testArray.CopyTo(arr, 0);
            CodeAnalysis.Func1(ref arr, newElement.Key, newElement.Value);
            Assert.Equal(targetArray.Length, arr.Length);
            for (int i = 0; i < targetArray.Length; i++)
            {
                Assert.Equal(targetArray[i].Key, arr[i].Key);
                Assert.Equal(targetArray[i].Value, arr[i].Value);
            }
        }
        [Fact]
        public void TestFunc1Lists()
        {
            var arr = new List<KeyValuePair<int, string>>(testArray);
            CodeAnalysis.Func1Lists(ref arr, newElement.Key, newElement.Value);
            Assert.Equal(targetArray.Length, arr.Count);
            for (int i = 0; i < targetArray.Length; i++)
            {
                Assert.Equal(targetArray[i].Key, arr[i].Key);
                Assert.Equal(targetArray[i].Value, arr[i].Value);
            }
        }
        [Fact]
        public void TestFunc1ListsWithNull()
        {
            List<KeyValuePair<int, string>> arr = null;
            CodeAnalysis.Func1Lists(ref arr, newElement.Key, newElement.Value);
            Assert.Single(arr);
            Assert.Equal(newElement.Key, arr[0].Key);
            Assert.Equal(newElement.Value, arr[0].Value);
        }
        [Fact]
        public void TestFunc1Linq()
        {
            var arr = new KeyValuePair<int, string>[testArray.Length];
            testArray.CopyTo(arr, 0);
            CodeAnalysis.Func1Linq(ref arr, newElement.Key, newElement.Value);
            Assert.Equal(targetArray.Length, arr.Length);
            for (int i = 0; i < targetArray.Length; i++)
            {
                Assert.Equal(targetArray[i].Key, arr[i].Key);
                Assert.Equal(targetArray[i].Value, arr[i].Value);
            }
        }
        [Fact]
        public void TestFunc1LinqWithNull()
        {
            KeyValuePair<int, string>[] arr = null;
            CodeAnalysis.Func1Linq(ref arr, newElement.Key, newElement.Value);
            Assert.Single(arr);
            Assert.Equal(newElement.Key, arr[0].Key);
            Assert.Equal(newElement.Value, arr[0].Value);
        }
        [Fact]
        public void TestFunc1Insertion()
        {
            var arr = new KeyValuePair<int, string>[testArray.Length];
            testArray.CopyTo(arr, 0);
            CodeAnalysis.Func1Insertion(ref arr, newElement.Key, newElement.Value);
            Assert.Equal(targetArray.Length, arr.Length);
            for (int i = 0; i < targetArray.Length; i++)
            {
                Assert.Equal(targetArray[i].Key, arr[i].Key);
                Assert.Equal(targetArray[i].Value, arr[i].Value);
            }
        }
        [Fact]
        public void TestFunc1InsertionWithNull()
        {
            KeyValuePair<int, string>[] arr = null;
            CodeAnalysis.Func1Insertion(ref arr, newElement.Key, newElement.Value);
            Assert.Single(arr);
            Assert.Equal(newElement.Key, arr[0].Key);
            Assert.Equal(newElement.Value, arr[0].Value);
        }
        [Fact]
        public void TestFunc1QuickSort()
        {
            var arr = new KeyValuePair<int, string>[testArray.Length];
            testArray.CopyTo(arr, 0);
            CodeAnalysis.Func1QuickSort(ref arr, newElement.Key, newElement.Value);
            Assert.Equal(targetArray.Length, arr.Length);
            for (int i = 0; i < targetArray.Length; i++)
            {
                Assert.Equal(targetArray[i].Key, arr[i].Key);
                Assert.Equal(targetArray[i].Value, arr[i].Value);
            }
        }
        [Fact]
        public void TestFunc1QuickSortWithNull()
        {
            KeyValuePair<int, string>[] arr = null;
            CodeAnalysis.Func1QuickSort(ref arr, newElement.Key, newElement.Value);
            Assert.Single(arr);
            Assert.Equal(newElement.Key, arr[0].Key);
            Assert.Equal(newElement.Value, arr[0].Value);
        }
    }
}