#include <immintrin.h>
#include <cstdint>
#include <stdio.h>

#if (defined WIN32 || defined _WIN32)
# define EXPORTS(returntype) __declspec(dllexport) returntype __cdecl
#else
# define EXPORTS(returntype) __attribute__((visibility("default"))) returntype
#endif

extern "C"
{
    // Copied from https://github.com/WojciechMula/toys/blob/master/is_sorted/avx2.simd-generic.cpp (c) WojciechMula
    EXPORTS(bool) is_sorted_avx2_generic(int32_t* a, size_t n)
    {
        size_t i = 0;
        if (n > 8) {
            for (/**/; i < n - 8; i += 8) {
                const __m256i curr = _mm256_loadu_si256(reinterpret_cast<const __m256i*>(a + i));
                const __m256i next = _mm256_loadu_si256(reinterpret_cast<const __m256i*>(a + i + 1));

                const __m256i mask = _mm256_cmpgt_epi32(curr, next);
                if (!_mm256_testz_si256(mask, mask)) {
                    return false;
                }
            }
        }

        for (/**/; i + 1 < n; i++) {
            if (a[i] > a[i + 1])
                return false;
        }

        return true;
    }
}
