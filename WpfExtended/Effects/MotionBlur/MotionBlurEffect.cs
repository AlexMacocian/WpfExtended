// MIT License
//
// Copyright (c) 2020 Hisham Maudarbocus
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Windows.Media.Effects;

namespace System.Windows.Media.Extensions.Effects
{
    public class MotionBlurEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(nameof(Input), typeof(MotionBlurEffect), 0);
        public static readonly DependencyProperty BlurAngleProperty =
            DependencyProperty.Register(nameof(BlurAngle), typeof(double), typeof(MotionBlurEffect), new UIPropertyMetadata(0d, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty BlurMagnitudeProperty =
            DependencyProperty.Register(nameof(BlurMagnitude), typeof(double), typeof(MotionBlurEffect), new UIPropertyMetadata(10d, PixelShaderConstantCallback(1)));

        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }
        public double BlurAngle
        {
            get => (double)GetValue(BlurAngleProperty);
            set => SetValue(BlurAngleProperty, value);
        }
        public double BlurMagnitude
        {
            get => (double)GetValue(BlurMagnitudeProperty);
            set => SetValue(BlurMagnitudeProperty, value);
        }

        public MotionBlurEffect()
        {
            PixelShader = PixelShaderUtility.LoadPixelShader("MotionBlur/MotionBlurEffect.ps");

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(BlurAngleProperty);
            UpdateShaderValue(BlurMagnitudeProperty);
        }
    }
}