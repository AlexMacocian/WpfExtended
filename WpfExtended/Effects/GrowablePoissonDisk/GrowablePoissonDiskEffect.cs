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
    public class GrowablePoissonDiskEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(GrowablePoissonDiskEffect), 0);
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(GrowablePoissonDiskEffect), new UIPropertyMetadata(0.5, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(GrowablePoissonDiskEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(double), typeof(GrowablePoissonDiskEffect), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(2)));

        private readonly static PixelShader pixelShader = new PixelShader();

        static GrowablePoissonDiskEffect()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader("GrowablePoissonDisk/GrowablePoissonDisk.ps");
        }

        public GrowablePoissonDiskEffect()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(RadiusProperty);
            UpdateShaderValue(WidthProperty);
            UpdateShaderValue(HeightProperty);
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
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
    }
}
