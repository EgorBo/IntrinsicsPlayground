namespace IntrinsicsPlayground.Misc.Sorting
{
    public static class HeapSort
    {
        public static void Sort(int[] array)
        {
            int heapSize = array.Length;
            for (int p = (heapSize - 1) / 2; p >= 0; p--)
                MaxHeapify(array, heapSize, p);

            for (int i = array.Length - 1; i > 0; i--)
            {
                int temp = array[i];
                array[i] = array[0];
                array[0] = temp;

                heapSize--;
                MaxHeapify(array, heapSize, 0);
            }
        }

        private static void MaxHeapify(int[] input, int heapSize, int index)
        {
            int right = (index + 1) * 2;
            int left = right - 1;
            int largest;

            if (left < heapSize && input[left] > input[index])
                largest = left;
            else
                largest = index;

            if (right < heapSize && input[right] > input[largest])
                largest = right;

            if (largest != index)
            {
                int temp = input[index];
                input[index] = input[largest];
                input[largest] = temp;

                MaxHeapify(input, heapSize, largest);
            }
        }
    }
}
