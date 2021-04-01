// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Windows.Media.Effects;

namespace System.Windows.Media.Extensions.Effects
{
    public class ToonShader : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ToonShader), 0);

        private static readonly PixelShader pixelShader;

        static ToonShader()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<ToonShader>();

            // Just saying hardware only for now since our drop of sw doesn't have sin/cos in
            // it, and thus we can't validate against that.
#if !SILVERLIGHT 
            // TODO: 
            pixelShader.ShaderRenderMode = ShaderRenderMode.HardwareOnly;
#endif 
        }

        public ToonShader()
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
