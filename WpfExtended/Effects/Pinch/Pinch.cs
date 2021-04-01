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
    public class Pinch : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Pinch), 0);
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX", typeof(double), typeof(Pinch), new UIPropertyMetadata(0.5, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY", typeof(double), typeof(Pinch), new UIPropertyMetadata(0.5, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(Pinch), new UIPropertyMetadata(0.25, PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register("Amount", typeof(double), typeof(Pinch), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(3)));

        private readonly static PixelShader pixelShader = new PixelShader();

        static Pinch()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<Pinch>();
        }
        public Pinch()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(CenterXProperty);
            UpdateShaderValue(CenterYProperty);
            UpdateShaderValue(RadiusProperty);
            UpdateShaderValue(AmountProperty);
        }

		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }
        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        public double Amount
        {
            get { return (double)GetValue(AmountProperty); }
            set { SetValue(AmountProperty, value); }
        }
    }
}
