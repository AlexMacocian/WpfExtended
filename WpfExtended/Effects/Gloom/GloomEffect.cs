// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using LiteWare.Wpf.ShaderEffects;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace WpfExtended.Effects
{
#if SILVERLIGHT 
    using UIPropertyMetadata = System.Windows.PropertyMetadata ;      
#endif
    public class GloomEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(GloomEffect), 0);
        public static readonly DependencyProperty GloomIntensityProperty = DependencyProperty.Register("GloomIntensity", typeof(double), typeof(GloomEffect), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty BaseIntensityProperty = DependencyProperty.Register("BaseIntensity", typeof(double), typeof(GloomEffect), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty GloomSaturationProperty = DependencyProperty.Register("GloomSaturation", typeof(double), typeof(GloomEffect), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty BaseSaturationProperty = DependencyProperty.Register("BaseSaturation", typeof(double), typeof(GloomEffect), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(3)));

        private readonly static PixelShader pixelShader = new PixelShader();

        static GloomEffect()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader("Gloom/Gloom.ps");
        }

        public GloomEffect()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(GloomIntensityProperty);
            UpdateShaderValue(BaseIntensityProperty);
            UpdateShaderValue(GloomSaturationProperty);
            UpdateShaderValue(BaseSaturationProperty);
        }

		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public double GloomIntensity
        {
            get { return (double)GetValue(GloomIntensityProperty); }
            set { SetValue(GloomIntensityProperty, value); }
        }
        public double BaseIntensity
        {
            get { return (double)GetValue(BaseIntensityProperty); }
            set { SetValue(BaseIntensityProperty, value); }
        }
        public double GloomSaturation
        {
            get { return (double)GetValue(GloomSaturationProperty); }
            set { SetValue(GloomSaturationProperty, value); }
        }
        public double BaseSaturation
        {
            get { return (double)GetValue(BaseSaturationProperty); }
            set { SetValue(BaseSaturationProperty, value); }
        }
    }
}
