using System;
using System.Collections.Generic;
using System.Linq;

using Epicycle.Math;
using Epicycle.Graphics.Images;

namespace Epicycle.ImageProcessing
{
    public static class ImageDifferentialOperators
    {
        private static float[] TransverseSobelKernel(int kernelSize)
        {
            var n = kernelSize - 1;

            return Enumerable.Range(0, kernelSize).Select
                (k => (float)Combinatorics.QuickBinomial(n, k))
                .ToArray();
        }

        private static float[] CollinearSobelKernel(int kernelSize)
        {
            var answer = new float[kernelSize];

            var n = kernelSize - 2;

            answer[0] = -1;
            answer[kernelSize - 1] = +1;

            for (var k = 1; k < kernelSize - 1; k++)
            {
                answer[k] = Combinatorics.QuickBinomial(n, k - 1) - Combinatorics.QuickBinomial(n, k);
            }

            return answer;
        }

        public static IImageFloat<TType> SobelX<TType>(this IReadOnlyImageFloat<TType> @this, int kernelSize)
            where TType : IImageType, new()
        {
            return @this.
                HorizontalConvolution(CollinearSobelKernel(kernelSize)).
                VerticalConvolution(TransverseSobelKernel(kernelSize));
        }

        public static IImageFloat<TType> SobelY<TType>(this IReadOnlyImageFloat<TType> @this, int kernelSize)
            where TType : IImageType, new()
        {
            return @this.
                VerticalConvolution(CollinearSobelKernel(kernelSize)).
                HorizontalConvolution(TransverseSobelKernel(kernelSize));
        }

        public static void ComputeSecondDerivatives<TType>
            (this IReadOnlyImageFloat<TType> @this,
            out IImageFloat<TType> dXX, out IImageFloat<TType> dXY, out IImageFloat<TType> dYY,
            int sobelKernelSize)
            where TType : IImageType, new()
        {
            var dX = @this.SobelX(sobelKernelSize);
            var dY = @this.SobelY(sobelKernelSize);

            dXX = dX.SobelX(sobelKernelSize);
            dXY = dX.SobelY(sobelKernelSize);
            dYY = dY.SobelY(sobelKernelSize);
        }
    }
}
