namespace IntrinsicsPlayground.Misc.Sorting
{
    public static class ClassicQuickSort
    {
        public static void Sort(int[] inputArray) => Sort(inputArray, 0, inputArray.Length - 1);

        private static void Sort(int[] array, int left, int right)
        {
            var pivot = array[(left + right) / 2];
            int originalLeft = left;
            int originalRight = right;
            do
            {
                while (array[left] < pivot)
                    left++;

                while (pivot < array[right])
                    right--;

                if (left <= right)
                {
                    var temp = array[left];
                    array[left] = array[right];
                    array[right] = temp;
                    left++;
                    right--;
                }
            }
            while (left <= right);

            if (originalLeft < right)
                Sort(array, originalLeft, right);
            if (left < originalRight)
                Sort(array, left, originalRight);
        }
    }
}
