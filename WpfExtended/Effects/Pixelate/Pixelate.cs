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
    public class Pixelate : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Pixelate), 0);
        public static readonly DependencyProperty HorizontalPixelCountsProperty = DependencyProperty.Register("HorizontalPixelCounts", typeof(double), typeof(Pixelate), new UIPropertyMetadata(80.0, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty VerticalPixelCountsProperty = DependencyProperty.Register("VerticalPixelCounts", typeof(double), typeof(Pixelate), new UIPropertyMetadata(80.0, PixelShaderConstantCallback(1)));

        private readonly static PixelShader pixelShader = new PixelShader();

        static Pixelate()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<Pixelate>();
        }

        public Pixelate()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(HorizontalPixelCountsProperty);
            UpdateShaderValue(VerticalPixelCountsProperty);
        }

		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public double HorizontalPixelCounts
        {
            get { return (double)GetValue(HorizontalPixelCountsProperty); }
            set { SetValue(HorizontalPixelCountsProperty, value); }
        }
        public double VerticalPixelCounts
        {
            get { return (double)GetValue(VerticalPixelCountsProperty); }
            set { SetValue(VerticalPixelCountsProperty, value); }
        }
    }
}
