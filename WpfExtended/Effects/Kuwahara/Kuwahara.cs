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
    public class Kuwahara : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Kuwahara), 0);
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(Kuwahara), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty TexelWidthProperty = DependencyProperty.Register("TexelWidth", typeof(double), typeof(Kuwahara), new UIPropertyMetadata(100.0, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty TexelHeightProperty = DependencyProperty.Register("TexelHeight", typeof(double), typeof(Kuwahara), new UIPropertyMetadata(100.0, PixelShaderConstantCallback(2)));

        private readonly static PixelShader pixelShader = new();

        static Kuwahara()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<Kuwahara>();
        }

        public Kuwahara()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(RadiusProperty);
            UpdateShaderValue(TexelWidthProperty);
            UpdateShaderValue(TexelHeightProperty);
        }

        [System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        public double TexelWidth
        {
            get { return (double)GetValue(TexelWidthProperty); }
            set { SetValue(TexelWidthProperty, value); }
        }
        public double TexelHeight
        {
            get { return (double)GetValue(TexelHeightProperty); }
            set { SetValue(TexelHeightProperty, value); }
        }
    }
}
