// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using LiteWare.Wpf.ShaderEffects;

namespace WpfExtended.Effects
{
    public class ColorKeyAlphaEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ColorKeyAlphaEffect), 0);

        private readonly static PixelShader pixelShader;

        static ColorKeyAlphaEffect()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader("ColorKeyAlpha/ColorKeyAlpha.ps");
        }

        public ColorKeyAlphaEffect()
        {
            this.PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
        }

		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
    }
}
