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
    public class Monochrome : ShaderEffect
    {
        public static readonly DependencyProperty FilterColorProperty = DependencyProperty.Register("FilterColor", typeof(Color), typeof(Monochrome), new UIPropertyMetadata(Colors.White, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Monochrome), 0);

        private readonly static PixelShader pixelShader;

        static Monochrome()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<Monochrome>();
        }
        public Monochrome()
        {
            this.PixelShader = pixelShader;

            UpdateShaderValue(FilterColorProperty);
            UpdateShaderValue(InputProperty);
        }

        public Color FilterColor
        {
            get { return (Color)GetValue(FilterColorProperty); }
            set { SetValue(FilterColorProperty, value); }
        }
		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
    }
}