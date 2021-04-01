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
    public class Bloom : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Bloom), 0);
        public static readonly DependencyProperty BloomIntensityProperty =
            DependencyProperty.Register("BloomIntensity", typeof(double), typeof(Bloom), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty BaseIntensityProperty =
            DependencyProperty.Register("BaseIntensity", typeof(double), typeof(Bloom), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty BloomSaturationProperty =
            DependencyProperty.Register("BloomSaturation", typeof(double), typeof(Bloom), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty BaseSaturationProperty =
            DependencyProperty.Register("BaseSaturation", typeof(double), typeof(Bloom), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(3)));

        private readonly static PixelShader pixelShader = new PixelShader();

        static Bloom()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<Bloom>();
        }

        public Bloom()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(BloomIntensityProperty);
            UpdateShaderValue(BaseIntensityProperty);
            UpdateShaderValue(BloomSaturationProperty);
            UpdateShaderValue(BaseSaturationProperty);
        }

		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public double BloomIntensity
        {
            get { return (double)GetValue(BloomIntensityProperty); }
            set { SetValue(BloomIntensityProperty, value); }
        }
        public double BaseIntensity
        {
            get { return (double)GetValue(BaseIntensityProperty); }
            set { SetValue(BaseIntensityProperty, value); }
        }
        public double BloomSaturation
        {
            get { return (double)GetValue(BloomSaturationProperty); }
            set { SetValue(BloomSaturationProperty, value); }
        }
        public double BaseSaturation
        {
            get { return (double)GetValue(BaseSaturationProperty); }
            set { SetValue(BaseSaturationProperty, value); }
        }
    }
}
