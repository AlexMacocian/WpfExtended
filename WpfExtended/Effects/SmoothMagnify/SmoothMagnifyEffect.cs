// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Windows.Media.Effects;

namespace System.Windows.Media.Extensions.Effects
{
#if SILVERLIGHT
    using UIPropertyMetadata = System.Windows.PropertyMetadata ;      
#endif
    public class SmoothMagnifyEffect : ShaderEffect
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(SmoothMagnifyEffect), new UIPropertyMetadata(new Point(0.5, 0.5), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(double), typeof(SmoothMagnifyEffect), new UIPropertyMetadata(.2, PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty MagnificationProperty = DependencyProperty.Register("Magnification", typeof(double), typeof(SmoothMagnifyEffect), new UIPropertyMetadata(2.0, PixelShaderConstantCallback(3)));
        public static readonly DependencyProperty OuterRadiusProperty = DependencyProperty.Register("OuterRadius", typeof(double), typeof(SmoothMagnifyEffect), new UIPropertyMetadata(.27, PixelShaderConstantCallback(4)));
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(SmoothMagnifyEffect), 0);

        private readonly static PixelShader pixelShader;

        static SmoothMagnifyEffect()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader("SmoothMagnify/SmoothMagnify.ps");
        }

        public SmoothMagnifyEffect()
        {
            this.PixelShader = pixelShader;

            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(InnerRadiusProperty);
            UpdateShaderValue(OuterRadiusProperty);
            UpdateShaderValue(MagnificationProperty);
            UpdateShaderValue(InputProperty);
        }

        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }
        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }
        public double Magnification
        {
            get { return (double)GetValue(MagnificationProperty); }
            set { SetValue(MagnificationProperty, value); }
        }
        public double OuterRadius
        {
            get { return (double)GetValue(OuterRadiusProperty); }
            set { SetValue(OuterRadiusProperty, value); }
        }
		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
    }
}
