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
    public class Ripple : ShaderEffect
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(Ripple), new UIPropertyMetadata(new Point(0.5, 0.5), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty AmplitudeProperty = DependencyProperty.Register("Amplitude", typeof(double), typeof(Ripple), new UIPropertyMetadata(0.1, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty FrequencyProperty = DependencyProperty.Register("Frequency", typeof(double), typeof(Ripple), new UIPropertyMetadata(50.0, PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty PhaseProperty = DependencyProperty.Register("Phase", typeof(double), typeof(Ripple), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(3)));
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Ripple), 0);

        private static readonly PixelShader pixelShader;

        static Ripple()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<Ripple>();
        }

        public Ripple()
        {
            this.PixelShader = pixelShader;

            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(AmplitudeProperty);
            UpdateShaderValue(PhaseProperty);
            UpdateShaderValue(FrequencyProperty);
            UpdateShaderValue(InputProperty);
        }

        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }
        public double Amplitude
        {
            get { return (double)GetValue(AmplitudeProperty); }
            set { SetValue(AmplitudeProperty, value); }
        }
        public double Frequency
        {
            get { return (double)GetValue(FrequencyProperty); }
            set { SetValue(FrequencyProperty, value); }
        }
        public double Phase
        {
            get { return (double)GetValue(PhaseProperty); }
            set { SetValue(PhaseProperty, value); }
        }
		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
    }
}
