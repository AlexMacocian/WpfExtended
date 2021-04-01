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
    public class ContrastAdjust : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ContrastAdjust), 0);
        public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register("Brightness", typeof(double), typeof(ContrastAdjust), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty ContrastProperty = DependencyProperty.Register("Contrast", typeof(double), typeof(ContrastAdjust), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(1)));

        private readonly static PixelShader pixelShader = new PixelShader();

        static ContrastAdjust()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<ContrastAdjust>();
        }

        public ContrastAdjust()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(BrightnessProperty);
            UpdateShaderValue(ContrastProperty);
        }

		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public double Brightness
        {
            get { return (double)GetValue(BrightnessProperty); }
            set { SetValue(BrightnessProperty, value); }
        }
        public double Contrast
        {
            get { return (double)GetValue(ContrastProperty); }
            set { SetValue(ContrastProperty, value); }
        }
    }
}
