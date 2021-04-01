// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Windows.Media.Effects;

namespace System.Windows.Media.Extensions.Effects
{
    public class InvertColor : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(InvertColor), 0);

        private readonly static PixelShader pixelShader;

        static InvertColor()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<InvertColor>();
        }

        public InvertColor()
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
