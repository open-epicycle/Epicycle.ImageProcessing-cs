using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Epicycle.Commons;
using Epicycle.Math.Geometry;
using Epicycle.Graphics;

namespace Epicycle.ImageProcessing
{
    [TestFixture]
    public sealed class ConvolutionTest : AssertionHelper
    {
        private const double _tolerance = 1e-9;

        [Test]
        public void HoriztonalConvolution_is_correct()
        {
            var imageData = new float[,,]
            {
                { { 0,  1,  2}, { 0, -1, -2}, {-1,  0,  1}, { 1,  0,  1} },
                { { 1,  0,  0}, { 0,  1,  0}, { 0,  0,  1}, { 0,  0,  1} },
                { { 1,  0,  0}, { 0,  1,  0}, { 0,  0,  1}, { 0,  0,  1} },
                { { 0,  1,  2}, { 0, -1, -2}, {-1,  0,  1}, {-1,  0,  1} },
                { { 1,  0,  0}, { 0,  1,  0}, { 0,  0,  1}, { 0,  0,  1} }
            };

            var image = new ImageFloat<RgbImageType>(imageData);

            var kernel = new float[] { 1, 2, -2, -1 };

            var actualResult = image.HorizontalConvolution(kernel, leftOffset: 1);

            var expectedResult = new float[,,]
            {
                { { 1,  5,  9}, { 1, -1, -5}, {-5, -1, -3}, {-2,  0,  0} },
                { { 3, -2, -1}, { 1,  2, -3}, { 0,  1, -1}, { 0,  0,  0} },
                { { 3, -2, -1}, { 1,  2, -3}, { 0,  1, -1}, { 0,  0,  0} },
                { { 1,  5,  9}, { 3, -1, -5}, { 1, -1, -3}, { 0,  0,  0} },
                { { 3, -2, -1}, { 1,  2, -3}, { 0,  1, -1}, { 0,  0,  0} }
            };

            Expect(actualResult.Dimensions, Is.EqualTo(image.Dimensions));

            for (var i = 0; i < image.Dimensions.X; i++)
            {
                for (var j = 0; j < image.Dimensions.Y; j++)
                {
                    for (var c = 0; c < 3; c++)
                    {
                        Expect(actualResult[new Vector2i(i, j), c], Is.EqualTo(expectedResult[j, i, c]).Within(_tolerance), string.Format("[{0},{1},{2}]", i, j, c));
                    }
                }
            }
        }

        [Test]
        public void VerticalConvolution_is_correct()
        {
            var imageData = new float[,,]
            {
                { { 0,  1,  2}, { 1,  0,  0}, { 1,  0,  0}, { 0,  1,  2}, { 1,  0,  0} },
                { { 0, -1, -2}, { 0,  1,  0}, { 0,  1,  0}, { 0, -1, -2}, { 0,  1,  0} },
                { {-1,  0,  1}, { 0,  0,  1}, { 0,  0,  1}, {-1,  0,  1}, { 0,  0,  1} },
                { { 1,  0,  1}, { 0,  0,  1}, { 0,  0,  1}, {-1,  0,  1}, { 0,  0,  1} }
            };

            var image = new ImageFloat<RgbImageType>(imageData);

            var kernel = new float[] { 1, 2, -2, -1 };

            var actualResult = image.VerticalConvolution(kernel, topOffset: 1);

            var expectedResult = new float[,,]
            {
                { { 1,  5,  9}, { 3, -2, -1}, { 3, -2, -1}, { 1,  5,  9}, { 3, -2, -1} },
                { { 1, -1, -5}, { 1,  2, -3}, { 1,  2, -3}, { 3, -1, -5}, { 1,  2, -3} },
                { {-5, -1, -3}, { 0,  1, -1}, { 0,  1, -1}, { 1, -1, -3}, { 0,  1, -1} },
                { {-2,  0,  0}, { 0,  0,  0}, { 0,  0,  0}, { 0,  0,  0}, { 0,  0,  0} }
            };

            Expect(actualResult.Dimensions, Is.EqualTo(image.Dimensions));

            for (var i = 0; i < image.Dimensions.X; i++)
            {
                for (var j = 0; j < image.Dimensions.Y; j++)
                {
                    for (var c = 0; c < 3; c++)
                    {
                        Expect(actualResult[new Vector2i(i, j), c], Is.EqualTo(expectedResult[j, i, c]).Within(_tolerance), string.Format("[{0},{1},{2}]", i, j, c));
                    }
                }
            }
        }
    }
}
