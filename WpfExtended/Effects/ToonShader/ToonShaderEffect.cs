// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace WpfExtended.Effects
{
    public class ToonShaderEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ToonShaderEffect), 0);

        private static readonly PixelShader pixelShader;

        static ToonShaderEffect()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader("ToonShader/ToonShader.ps");

            // Just saying hardware only for now since our drop of sw doesn't have sin/cos in
            // it, and thus we can't validate against that.
#if !SILVERLIGHT 
            // TODO: 
            pixelShader.ShaderRenderMode = ShaderRenderMode.HardwareOnly;
#endif 
        }

        public ToonShaderEffect()
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
