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
    public class MonochromeEffect : ShaderEffect
    {
        public static readonly DependencyProperty FilterColorProperty = DependencyProperty.Register("FilterColor", typeof(Color), typeof(MonochromeEffect), new UIPropertyMetadata(Colors.White, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(MonochromeEffect), 0);

        private readonly static PixelShader pixelShader;

        static MonochromeEffect()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader("MonoChrome/Monochrome.ps");
        }
        public MonochromeEffect()
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