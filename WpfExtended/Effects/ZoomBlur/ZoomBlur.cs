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
    public class ZoomBlur : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(nameof(Input), typeof(ZoomBlur), 0);
        public static readonly DependencyProperty BlurCenterProperty =
            DependencyProperty.Register(nameof(BlurCenter), typeof(Point), typeof(ZoomBlur), new UIPropertyMetadata(new Point(0.5D, 0.5D), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty BlurMagnitudeProperty =
            DependencyProperty.Register(nameof(BlurMagnitude), typeof(double), typeof(ZoomBlur), new UIPropertyMetadata(5D, PixelShaderConstantCallback(1)));

        public Brush Input
        {
            get => (Brush)GetValue(InputProperty);
            set => SetValue(InputProperty, value);
        }
        public Point BlurCenter
        {
            get => (Point)GetValue(BlurCenterProperty);
            set => SetValue(BlurCenterProperty, value);
        }
        public double BlurMagnitude
        {
            get => (double)GetValue(BlurMagnitudeProperty);
            set => SetValue(BlurMagnitudeProperty, value);
        }

        public ZoomBlur()
        {
            PixelShader = PixelShaderUtility.LoadPixelShader<ZoomBlur>();

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(BlurCenterProperty);
            UpdateShaderValue(BlurMagnitudeProperty);
        }
    }
}