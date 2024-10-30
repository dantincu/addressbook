using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database.Migrations
{
    internal class RandomValueGenerator
    {
        private static readonly ReadOnlyCollection<int> primeNumbers;

        static RandomValueGenerator()
        {
            primeNumbers = new([
                2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97]);
        }

        public RandomValueGenerator(
            int addressIdx)
        {
            this.addressIdx = addressIdx;
            this.primeNumIdx = 0;
        }

        private int addressIdx;
        private int primeNumIdx;

        public char GetRandomChar(
            char minChar, char maxChar)
        {
            int min = minChar;
            int max = maxChar;
            int retInt = GetRandomInt(min, max);
            char retChar = (char)retInt;
            return retChar;
        }

        public int GetRandomInt(
            int min, int max)
        {
            long nextPrimeNumber = GetNextPrimNumber();
            long idx = nextPrimeNumber * addressIdx;
            int normIdx = (int)(idx % (max - min));
            int retVal = min + normIdx;
            return retVal;
        }

        public T GetRandomValue<T>(
            IList<T> valuesArr)
        {
            long nextPrimeNumber = GetNextPrimNumber();
            long idx = nextPrimeNumber * addressIdx;

            var value = GetRandomValueCore(
                valuesArr, idx);

            return value;
        }

        private long GetNextPrimNumber(
            ) => GetRandomValueCore(
                primeNumbers,
                ++primeNumIdx);

        private T GetRandomValueCore<T>(
            IList<T> list,
            long idx)
        {
            int normIdx = (int)(idx % list.Count);
            var value = list[normIdx];

            return value;
        }
    }
}
