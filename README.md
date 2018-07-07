# IntrinsicsPlayground

My toys to play with intrinsics in pure C# (see `System.Runtime.Intrinsics.Experimental`)

Environment:
``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
Frequency=3609374 Hz, Resolution=277.0564 ns, Timer=TSC
.NET Core SDK=2.1.301
  [Host]     : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
```


## **1. ArraySum**

Sum of elements in an array of floats 
```csharp
var sum = arrayOfFloats.Sum();
````

|            Method | TestArrayLength |               Mean |    Scaled |
|-------------------|-----------------|-------------------:|----------:|
|      **Sum_LINQ** |          **10** |      **45.629 ns** |  **4.81** |
|        Sum_Simple |              10 |           2.879 ns |      0.30 |
| Sum_LinqFasterLib |              10 |          15.100 ns |      1.59 |
|           Sum_Avx |              10 |           9.496 ns |      1.00 |
|                   |                 |                    |           |
|      **Sum_LINQ** |         **121** |     **439.021 ns** | **25.20** |
|        Sum_Simple |             121 |          78.111 ns |      4.48 |
| Sum_LinqFasterLib |             121 |          22.432 ns |      1.29 |
|           Sum_Avx |             121 |          17.423 ns |      1.00 |
|                   |                 |                    |           |
|      **Sum_LINQ** |       **32768** | **109,128.390 ns** | **31.87** |
|        Sum_Simple |           32768 |      27,345.722 ns |      7.99 |
| Sum_LinqFasterLib |           32768 |       3,432.010 ns |      1.00 |
|           Sum_Avx |           32768 |       3,423.826 ns |      1.00 |


## **2. ArrayEqual**

Are arrays of floats equal?
```csharp
var equal = arrayOfFloats1.SequenceEqual(arrayOfFloats2);
```

|                Method | TestArrayLength |               Mean |    Scaled |
|-----------------------|-----------------|-------------------:|----------:|
|   **ArrayEqual_LINQ** |          **10** |     **116.175 ns** | **30.22** |
|     ArrayEqual_Simple |              10 |           6.130 ns |      1.59 |
|       ArrayEqual_AVX2 |              10 |           3.845 ns |      1.00 |
|                       |                 |                    |           |
|   **ArrayEqual_LINQ** |         **121** |     **783.744 ns** | **50.82** |
|     ArrayEqual_Simple |             121 |          60.981 ns |      3.95 |
|       ArrayEqual_AVX2 |             121 |          15.425 ns |      1.00 |
|                       |                 |                    |           |
|   **ArrayEqual_LINQ** |       **32768** | **198,072.829 ns** | **52.85** |
|     ArrayEqual_Simple |           32768 |      13,773.698 ns |      3.67 |
|       ArrayEqual_AVX2 |           32768 |       3,748.025 ns |      1.00 |


## **3. ArrayMax**

Max element in an array of ints 
```csharp
var max = arrayOfInts.Max();
```

|            Method | TestArrayLength |               Mean |    Scaled |
|-------------------|-----------------|-------------------:|----------:|
|      **Max_LINQ** |          **10** |      **44.932 ns** |  **3.38** |
|        Max_Simple |              10 |           3.125 ns |      0.23 |
| Max_LinqFasterLib |              10 |          14.071 ns |      1.06 |
|           Max_Avx |              10 |          13.307 ns |      1.00 |
|                   |                 |                    |           |
|      **Max_LINQ** |         **121** |     **447.998 ns** | **23.34** |
|        Max_Simple |             121 |          42.731 ns |      2.23 |
| Max_LinqFasterLib |             121 |          19.230 ns |      1.00 |
|           Max_Avx |             121 |          19.219 ns |      1.00 |
|                   |                 |                    |           |
|      **Max_LINQ** |       **32768** | **116,163.414 ns** | **69.46** |
|        Max_Simple |           32768 |      10,300.011 ns |      6.16 |
| Max_LinqFasterLib |           32768 |       1,759.705 ns |      1.05 |
|           Max_Avx |           32768 |       1,672.383 ns |      1.00 |


## **4. ArrayIsSorted**

Check if an array is sorted or not
```csharp
bool IsSorted(int[] array)
{
    if (array.Length < 2)
        return true;

    for (int i = 0; i < array.Length - 1; i++)
    {
        if (array[i] > array[i + 1])
            return false;
    }
    return true;
}
```
|                    Method | TestArrayLength |              Mean |   Scaled |
|---------------------------|-----------------|------------------:|---------:|
|       **IsSorted_Simple** |          **10** |      **6.309 ns** | **2.51** |
| IsSorted_Simple_Optimized |              10 |          4.784 ns |     1.90 |
|            IsSorted_Sse41 |              10 |          2.691 ns |     1.07 |
|             IsSorted_Avx2 |              10 |          2.513 ns |     1.00 |
|                           |                 |                   |          |
|       **IsSorted_Simple** |         **121** |     **80.036 ns** | **6.38** |
| IsSorted_Simple_Optimized |             121 |         54.508 ns |     4.35 |
|            IsSorted_Sse41 |             121 |         22.927 ns |     1.83 |
|             IsSorted_Avx2 |             121 |         12.539 ns |     1.00 |
|                           |                 |                   |          |
|       **IsSorted_Simple** |       **32768** | **19,928.318 ns** | **5.31** |
| IsSorted_Simple_Optimized |           32768 |     13,705.714 ns |     3.65 |
|            IsSorted_Sse41 |           32768 |      6,190.854 ns |     1.65 |
|             IsSorted_Avx2 |           32768 |      3,755.430 ns |     1.00 |


## **5. ArrayReverse**

Reverse all elements in an array

```csharp
Array.Reverse(arrayOfInts);
```

|          Method | TestArrayLength |             Mean |   Scaled |
|-----------------|-----------------|-----------------:|---------:|
| **Reverse_BCL** |          **10** |     **46.72 ns** | **0.96** |
|     Reverse_SSE |              10 |         48.70 ns |     1.00 |
|                 |                 |                  |          |
| **Reverse_BCL** |         **121** |    **102.90 ns** | **1.27** |
|     Reverse_SSE |             121 |         80.82 ns |     1.00 |
|                 |                 |                  |          |
| **Reverse_BCL** |       **32768** | **18,083.44 ns** | **1.56** |
|     Reverse_SSE |           32768 |     11,577.33 ns |     1.00 |


## **6. ArrayIndexOf**

Searche for the specified element and returns the index of its first occurrence
```csharp
int index = Array.IndexOf(arrayOfInts, element);
```

|          Method | TestArrayLength |             Mean |   Scaled |
|-----------------|---------------- |-----------------:|---------:|
| **IndexOf_BCL** |          **10** |     **7.701 ns** | **1.37** |
|    IndexOf_Avx2 |              10 |         5.620 ns |     1.00 |
|                 |                 |                  |          |
| **IndexOf_BCL** |         **121** |    **31.721 ns** | **3.59** |
|    IndexOf_Avx2 |             121 |         8.825 ns |     1.00 |
|                 |                 |                  |          |
| **IndexOf_BCL** |       **32768** | **6,961.574 ns** | **3.72** |
|    IndexOf_Avx2 |           32768 |     1,870.329 ns |     1.00 |


## **7. MatrixSum**

Add two matrices (`Matrix4x4`)

```csharp
var matrix3 = matrix1 + matrix2;
```

|        Method |     Mean | Scaled |
|-------------- |---------:|-------:|
| MatrixSum_BCL | 25.81 us |   1.30 |
| MatrixSum_Avx | 19.89 us |   1.00 |


## **8. MatrixNegate**

Negate a matrix (`Matrix4x4`)
```csharp
matrix = -matrix;
```

|           Method |     Mean | Scaled |
|----------------- |---------:|-------:|
| MatrixNegate_BCL | 34.31 us |   1.38 |
| MatrixNegate_Avx | 24.87 us |   1.00 |


## 9. MatrixMultiply 

Multiply a matrix by a vector 
```csharp
matrix = matrix * vec;
```

|                      Method |     Mean | Scaled |
|---------------------------- |---------:|-------:|
| MatrixMultiply_ByVector_BCL | 28.01 us |   1.04 |
| MatrixMultiply_ByVector_Avx | 26.83 us |   1.00 |


# Misc

## **1. Array.Sort vs DualPivotQuickSort**

Dual pivot quick sort is used in Java for primitive types (`Arrays.sort(int[])`).
![alt text](Screenshots/DualPivotQuickSort.png)
