using System.Collections.Generic;
using System.Linq;

using Epicycle.Commons;
using Epicycle.Commons.Unsafe;
using Epicycle.Math.Geometry;
using Epicycle.Graphics;

namespace Epicycle.ImageProcessing
{
    using System;

    public static class Convolution
    {
        public unsafe static IImageFloat<TType> HorizontalConvolution<TType>
            (this IReadOnlyImageFloat<TType> @this, float* kernel, int leftOffset, int rightOffset)
            where TType : IImageType, new()
        {
            ArgAssert.AtLeast(leftOffset, "leftOffset", 0);
            ArgAssert.AtLeast(rightOffset, "rightOffset", 0);

            var answer = new ImageFloat<TType>(@this.Dimensions);

            var channelsCount = Singleton<TType>.Instance.ChannelsCount;

            using (var pinInput = @this.Open())
            {
                var inPtr = pinInput.Ptr;

                using (var pinOutput = answer.Open())
                {
                    var outPtr = pinOutput.Ptr;

                    for (var j = 0; j < @this.Dimensions.Y; j++)
                    {
                        var inPixelPtr = inPtr;
                        var outPixelPtr = outPtr;

                        for (var i = 0; i < @this.Dimensions.X; i++)
                        {
                            int k = -leftOffset;
                            var kernelPtr = kernel;

                            for (; k < -i; k++)
                            {
                                for (var c = 0; c < channelsCount; c++)
                                {
                                    outPixelPtr[c] += *kernelPtr * inPtr[c];                                        
                                }

                                kernelPtr++;
                            }

                            var rightBound = Math.Min(@this.Dimensions.X - i - 1, rightOffset);
                            var windowPtr = inPixelPtr + @this.Step.X * k;

                            for (; k < rightBound; k++)
                            {
                                for (var c = 0; c < channelsCount; c++)
                                {
                                    outPixelPtr[c] += *kernelPtr * windowPtr[c];                                    
                                }

                                kernelPtr++;
                                windowPtr += @this.Step.X;
                            }

                            for (; k <= rightOffset; k++)
                            {
                                for (var c = 0; c < channelsCount; c++)
                                {
                                    outPixelPtr[c] += *kernelPtr * windowPtr[c];
                                }

                                kernelPtr++;
                            }

                            inPixelPtr += @this.Step.X;
                            outPixelPtr += answer.Step.X;    
                        }

                        inPtr += @this.Step.Y;
                        outPtr += answer.Step.Y;
                    }
                }
            }
            
            return answer;
        }

        public static unsafe IImageFloat<TType> HorizontalConvolution<TType>(this IReadOnlyImageFloat<TType> @this, float* kernel, int radius)
            where TType : IImageType, new()
        {
            return HorizontalConvolution(@this, kernel, leftOffset: radius, rightOffset: radius);
        }

        public static unsafe IImageFloat<TType> HorizontalConvolution<TType>(this IReadOnlyImageFloat<TType> @this, float[] kernel, int leftOffset)
            where TType : IImageType, new()
        {
            using (var pinKernel = new PinnedFloatBuffer(kernel))
            {
                return HorizontalConvolution(@this, pinKernel.Ptr, leftOffset, rightOffset: kernel.Length - leftOffset - 1);
            }            
        }

        public static unsafe IImageFloat<TType> HorizontalConvolution<TType>(this IReadOnlyImageFloat<TType> @this, float[] kernel)
            where TType : IImageType, new()
        {
            var leftOffset = kernel.Length / 2;
            var rightOffset = kernel.Length - leftOffset - 1;

            using (var pinKernel = new PinnedFloatBuffer(kernel))
            {
                return HorizontalConvolution(@this, pinKernel.Ptr, leftOffset, rightOffset);
            }
        }

        public unsafe static IImageFloat<TType> VerticalConvolution<TType>
            (this IReadOnlyImageFloat<TType> @this, float* kernel, int topOffset, int bottomOffset)
            where TType : IImageType, new()
        {
            ArgAssert.AtLeast(topOffset, "topOffset", 0);
            ArgAssert.AtLeast(bottomOffset, "bottomOffset", 0);

            var answer = new ImageFloat<TType>(@this.Dimensions);

            var channelsCount = Singleton<TType>.Instance.ChannelsCount;

            using (var pinInput = @this.Open())
            {
                var inPtr = pinInput.Ptr;

                using (var pinOutput = answer.Open())
                {
                    var outPtr = pinOutput.Ptr;

                    for (var j = 0; j < @this.Dimensions.X; j++)
                    {
                        var inColPtr = inPtr;
                        var outColPtr = outPtr;

                        for (var i = 0; i < @this.Dimensions.Y; i++)
                        {
                            int k = -topOffset;
                            var kernelPtr = kernel;

                            for (; k < -i; k++)
                            {
                                for (var c = 0; c < channelsCount; c++)
                                {
                                    outColPtr[c] += *kernelPtr * inPtr[c];
                                }

                                kernelPtr++;
                            }

                            var rightBound = Math.Min(@this.Dimensions.Y - i - 1, bottomOffset);
                            var windowPtr = inColPtr + @this.Step.Y * k;

                            for (; k < rightBound; k++)
                            {
                                for (var c = 0; c < channelsCount; c++)
                                {
                                    outColPtr[c] += *kernelPtr * windowPtr[c];
                                }

                                kernelPtr++;
                                windowPtr += @this.Step.Y;
                            }

                            for (; k <= bottomOffset; k++)
                            {
                                for (var c = 0; c < channelsCount; c++)
                                {
                                    outColPtr[c] += *kernelPtr * windowPtr[c];
                                }

                                kernelPtr++;
                            }

                            inColPtr += @this.Step.Y;
                            outColPtr += answer.Step.Y;
                        }

                        inPtr += @this.Step.X;
                        outPtr += answer.Step.X;
                    }
                }
            }

            return answer;
        }

        public static unsafe IImageFloat<TType> VerticalConvolution<TType>(this IReadOnlyImageFloat<TType> @this, float* kernel, int radius)
            where TType : IImageType, new()
        {
            return VerticalConvolution(@this, kernel, topOffset: radius, bottomOffset: radius);
        }

        public static unsafe IImageFloat<TType> VerticalConvolution<TType>(this IReadOnlyImageFloat<TType> @this, float[] kernel, int topOffset)
            where TType : IImageType, new()
        {
            using (var pinKernel = new PinnedFloatBuffer(kernel))
            {
                return VerticalConvolution(@this, pinKernel.Ptr, topOffset, bottomOffset: kernel.Length - topOffset - 1);
            }
        }

        public static unsafe IImageFloat<TType> VerticalConvolution<TType>(this IReadOnlyImageFloat<TType> @this, float[] kernel)
            where TType : IImageType, new()
        {
            var topOffset = kernel.Length / 2;
            var bottomOffset = kernel.Length - topOffset - 1;

            using (var pinKernel = new PinnedFloatBuffer(kernel))
            {
                return VerticalConvolution(@this, pinKernel.Ptr, topOffset, bottomOffset);
            }
        }

        public static IImageFloat<TType> GaussianBlur<TType>(this IReadOnlyImageFloat<TType> @this, Vector2 stdev, Vector2i kernelSize)
            where TType : IImageType, new()
        {
            var xGaussian = ComputeGaussian(stdev.X, kernelSize.X);
            var yGaussian = ComputeGaussian(stdev.Y, kernelSize.Y);

            return @this.HorizontalConvolution(xGaussian).VerticalConvolution(yGaussian);
        }

        private static float[] ComputeGaussian(double stdev, int size)
        {
            var answer = new float[size];

            int i = 0;

            var center = (size - 1) / 2f;

            for (; i <= size / 2; i++)
            {
                answer[i] = (float)Math.Exp(-BasicMath.Sqr((i - center) / stdev));
            }

            for (; i < size; i++)
            {
                answer[i] = answer[size - i - 1];
            }
            
            return answer;
        }
    }
}
