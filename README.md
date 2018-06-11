# IntrinsicsPlayground

My toys to play with intrinsics in pure C# (see `System.Runtime.Intrinsics.Experimental`)

Environment:
``` ini

BenchmarkDotNet=v0.10.14, OS=macOS High Sierra 10.13.3 (17D47) [Darwin 17.4.0]
Intel Core i7-4980HQ CPU 2.80GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.300
  [Host]     : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
```

**1. ArraySum**

Sum of elements in an array of floats 
```csharp
var sum = arrayOfFloats.Sum();
````

|            Method | TestArrayLength |           Mean | Scaled |
|------------------ |---------------- |---------------:|-------:|
|          **Sum_LINQ** |              **10** |      **68.007 ns** |   **5.33** |
|        Sum_Simple |              10 |       4.926 ns |   0.39 |
| Sum_LinqFasterLib |              10 |      20.665 ns |   1.62 |
|           Sum_Avx |              10 |      12.757 ns |   1.00 |
|                   |                 |                |        |
|          **Sum_LINQ** |             **121** |     **646.813 ns** |  **31.92** |
|        Sum_Simple |             121 |      80.691 ns |   3.98 |
| Sum_LinqFasterLib |             121 |      32.656 ns |   1.61 |
|           Sum_Avx |             121 |      20.263 ns |   1.00 |
|                   |                 |                |        |
|          **Sum_LINQ** |           **32768** | **169,085.710 ns** |  **44.67** |
|        Sum_Simple |           32768 |  25,109.373 ns |   6.63 |
| Sum_LinqFasterLib |           32768 |   3,826.928 ns |   1.01 |
|           Sum_Avx |           32768 |   3,784.930 ns |   1.00 |

**2. ArrayEqual**

Are arrays of floats equal?
```csharp
var equal = arrayOfFloats1.SequenceEqual(arrayOfFloats2);
```

|            Method | TestArrayLength |           Mean | Scaled |
|------------------ |---------------- |---------------:|-------:|
|   **ArrayEqual_LINQ** |              **10** |     **233.546 ns** |  **38.74** |
| ArrayEqual_Simple |              10 |      10.013 ns |   1.66 |
|   ArrayEqual_AVX2 |              10 |       6.045 ns |   1.00 |
|                   |                 |                |        |
|   **ArrayEqual_LINQ** |             **121** |   **1,345.860 ns** |  **60.32** |
| ArrayEqual_Simple |             121 |     100.944 ns |   4.52 |
|   ArrayEqual_AVX2 |             121 |      22.387 ns |   1.00 |
|                   |                 |                |        |
|   **ArrayEqual_LINQ** |           **32768** | **334,854.912 ns** |  **50.67** |
| ArrayEqual_Simple |           32768 |  23,460.582 ns |   3.55 |
|   ArrayEqual_AVX2 |           32768 |   6,626.225 ns |   1.00 |


**3. ArrayMax**

Max element in an array of ints 
```csharp
var max = arrayOfInts.Max();
```

|            Method | TestArrayLength |           Mean | Scaled |
|------------------ |---------------- |---------------:|-------:|
|          **Max_LINQ** |              **10** |      **71.186 ns** |   **3.97** |
|        Max_Simple |              10 |       6.910 ns |   0.39 |
| Max_LinqFasterLib |              10 |      19.458 ns |   1.08 |
|           Max_Avx |              10 |      18.046 ns |   1.00 |
|                   |                 |                |        |
|          **Max_LINQ** |             **121** |     **679.652 ns** |  **28.59** |
|        Max_Simple |             121 |      64.353 ns |   2.71 |
| Max_LinqFasterLib |             121 |      28.253 ns |   1.19 |
|           Max_Avx |             121 |      23.884 ns |   1.00 |
|                   |                 |                |        |
|          **Max_LINQ** |           **32768** | **175,971.956 ns** |  **84.17** |
|        Max_Simple |           32768 |  14,003.368 ns |   6.70 |
| Max_LinqFasterLib |           32768 |   2,731.388 ns |   1.31 |
|           Max_Avx |           32768 |   2,096.625 ns |   1.00 |

**4. ArrayIsSorted**

Check if an array is sorted or not

TODO: results

**5. ArrayReverse**

Reverse all elements in an array

```csharp
Array.Reverse(arrayOfInts);
```

|      Method | TestArrayLength |         Mean | Scaled |
|------------ |---------------- |-------------:|-------:|
| **Reverse_BCL** |              **10** |     **77.82 ns** |   **0.94** |
| Reverse_SSE |              10 |     82.44 ns |   1.00 |
|             |                 |              |        |
| **Reverse_BCL** |             **121** |    **156.44 ns** |   **1.24** |
| Reverse_SSE |             121 |    125.95 ns |   1.00 |
|             |                 |              |        |
| **Reverse_BCL** |           **32768** | **27,924.33 ns** |   **1.34** |
| Reverse_SSE |           32768 | 20,774.09 ns |   1.00 |


**6. ArrayIndexOf**

Searche for the specified element and returns the index of its first occurrence
```csharp
int index = Array.IndexOf(arrayOfInts, element);
```

|       Method | TestArrayLength |         Mean | Scaled |
|------------- |---------------- |-------------:|-------:|
|  **IndexOf_BCL** |              **10** |    **11.242 ns** |   **1.61** |
| IndexOf_Avx2 |              10 |     7.059 ns |   1.00 |
|              |                 |              |        |
|  **IndexOf_BCL** |             **121** |    **51.018 ns** |   **3.98** |
| IndexOf_Avx2 |             121 |    12.928 ns |   1.00 |
|              |                 |              |        |
|  **IndexOf_BCL** |           **32768** | **8,361.998 ns** |   **4.16** |
| IndexOf_Avx2 |           32768 | 2,008.645 ns |   1.00 |

**7. MatrixSum**

Add two matrices (`Matrix4x4`)

```csharp
var matrix3 = matrix1 + matrix2;
```

|        Method |     Mean | Scaled |
|-------------- |---------:|-------:|
| MatrixSum_BCL | 29.85 us |   1.09 |
| MatrixSum_Avx | 27.36 us |   1.00 |

**8. MatrixNegate**

Negate a matrix (`Matrix4x4`)
```csharp
matrix = -matrix;
```

|           Method |     Mean | Scaled |
|----------------- |---------:|-------:|
| MatrixNegate_BCL | 37.31 us |   1.07 |
| MatrixNegate_Avx | 34.77 us |   1.00 |

**9. MatrixMultiply**

Multiply a matrix by a vector 
```csharp
matrix = matrix * vec;
```

|                      Method |     Mean | Scaled |
|---------------------------- |---------:|-------:|
| MatrixMultiply_ByVector_BCL | 37.67 us |   1.06 |
| MatrixMultiply_ByVector_Avx | 35.38 us |   1.00 |


# Misc

**1. Array.Sort vs DualPivotQuickSort**

Dual pivot quick sort is used in Java for primitive types (`Arrays.sort(int[])`).
![alt text](Screenshots/DualPivotQuickSort.png)
