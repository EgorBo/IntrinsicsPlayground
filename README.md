# IntrinsicsPlayground

My toys to play with intrinsics in pure C# (see `System.Runtime.Intrinsics.Experimental`)

**1. ArraySum**

Sum of elements in an array of floats 
`var sum = arrayOfFloats.Sum();`

![alt text](Screenshots/ArraySum.png)

**2. ArrayEqual**

Are arrays of floats equal?
`var equal = arrayOfFloats1.SequenceEqual(arrayOfFloats2);`

![alt text](Screenshots/ArrayEqual.png)

**3. ArrayMax**

Max element in an array of ints 
`var max = arrayOfInts.Max();`

![alt text](Screenshots/ArrayMax.png)

**4. ArrayIsSorted**

Check if an array is sorted or not

![alt text](Screenshots/ArrayIsSorted.png)

**5. ArrayReverse**

Reverse all elements in an array
`Array.Reverse(arrayOfInts);`

![alt text](Screenshots/ArrayReverse.png)


**6. MatrixSum**

Add two matrices (`Matrix4x4`)
`var matrix3 = matrix1 + matrix2;`
![alt text](Screenshots/MatrixSum.png)


**7. MatrixNegate**

Negate a matrix (`Matrix4x4`)
`matrix = -matrix;`
![alt text](Screenshots/MatrixNegate.png)


**8. MatrixMultiply**

Multiply a matrix by a vector 
`matrix = matrix * vec;`
![alt text](Screenshots/MatrixMultiply1.png)

# Misc

**1. Array.Sort vs DualPivotQuickSort**

Dual pivot quick sort is used in Java for primitive types (`Arrays.sort(int[])`).
![alt text](Screenshots/DualPivotQuickSort.png)
