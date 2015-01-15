// [[[[INFO>
// Copyright 2015 Epicycle (http://epicycle.org, https://github.com/open-epicycle)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// For more information check https://github.com/open-epicycle/Epicycle.ImageProcessing-cs
// ]]]]

using Epicycle.Graphics.Images;
using Epicycle.Math;
using System.Linq;

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
