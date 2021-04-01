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
    public class ToneMapping : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ToneMapping), 0);
        public static readonly DependencyProperty ExposureProperty = DependencyProperty.Register("Exposure", typeof(double), typeof(ToneMapping), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty DefogProperty = DependencyProperty.Register("Defog", typeof(double), typeof(ToneMapping), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty GammaProperty = DependencyProperty.Register("Gamma", typeof(double), typeof(ToneMapping), new UIPropertyMetadata(0.454545, PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty FogColorProperty = DependencyProperty.Register("FogColor", typeof(Color), typeof(ToneMapping), new UIPropertyMetadata(Colors.White, PixelShaderConstantCallback(3)));
        public static readonly DependencyProperty VignetteRadiusProperty = DependencyProperty.Register("VignetteRadius", typeof(double), typeof(ToneMapping), new UIPropertyMetadata(0.35, PixelShaderConstantCallback(4)));
        public static readonly DependencyProperty VignetteCenterProperty = DependencyProperty.Register("VignetteCenter", typeof(Point), typeof(ToneMapping), new UIPropertyMetadata( new Point(0.5, 0.5), PixelShaderConstantCallback(5)));
        public static readonly DependencyProperty BlueShiftProperty = DependencyProperty.Register("BlueShift", typeof(double), typeof(ToneMapping), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(6)));

        private readonly static PixelShader pixelShader;

        static ToneMapping()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<ToneMapping>();
        }

        public ToneMapping()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ExposureProperty);
            UpdateShaderValue(DefogProperty);
            UpdateShaderValue(GammaProperty);
            UpdateShaderValue(FogColorProperty);
            UpdateShaderValue(VignetteRadiusProperty);
            UpdateShaderValue(VignetteCenterProperty);            
            UpdateShaderValue(BlueShiftProperty);
        }

		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public double Exposure
        {
            get { return (double)GetValue(ExposureProperty); }
            set { SetValue(ExposureProperty, value); }
        }
        public double Defog
        {
            get { return (double)GetValue(DefogProperty); }
            set { SetValue(DefogProperty, value); }
        }
        public double Gamma
        {
            get { return (double)GetValue(GammaProperty); }
            set { SetValue(GammaProperty, value); }
        }
        public Color FogColor
        {
            get { return (Color)GetValue(FogColorProperty); }
            set { SetValue(FogColorProperty, value); }
        }
        public double VignetteRadius
        {
            get { return (double)GetValue(VignetteRadiusProperty); }
            set { SetValue(VignetteRadiusProperty, value); }
        }
        public Point VignetteCenter
        {
            get { return (Point)GetValue(VignetteCenterProperty); }
            set { SetValue(VignetteCenterProperty, value); }
        }
        public double BlueShift
        {
            get { return (double)GetValue(BlueShiftProperty); }
            set { SetValue(BlueShiftProperty, value); }
        }
    }
}
