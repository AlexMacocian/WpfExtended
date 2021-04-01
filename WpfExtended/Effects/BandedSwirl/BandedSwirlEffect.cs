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
#if SILVERLIGHT
    using UIPropertyMetadata = System.Windows.PropertyMetadata;    
#endif
    public class BandedSwirlEffect : ShaderEffect
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(BandedSwirlEffect), new UIPropertyMetadata(new Point(0.5, 0.5), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty SwirlStrengthProperty = DependencyProperty.Register("SwirlStrength", typeof(double), typeof(BandedSwirlEffect), new UIPropertyMetadata(0.5, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty DistanceThresholdProperty = DependencyProperty.Register("DistanceThreshold", typeof(double), typeof(BandedSwirlEffect), new UIPropertyMetadata(0.2, PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(BandedSwirlEffect), 0);

        private readonly static PixelShader pixelShader;

        static BandedSwirlEffect()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader("BandedSwirl/BandedSwirl.ps");
        }

        public BandedSwirlEffect()
        {
            this.PixelShader = pixelShader;

            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(SwirlStrengthProperty);
            UpdateShaderValue(DistanceThresholdProperty);
            UpdateShaderValue(InputProperty);
        }

        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }
        public double SwirlStrength
        {
            get { return (double)GetValue(SwirlStrengthProperty); }
            set { SetValue(SwirlStrengthProperty, value); }
        }
        public double DistanceThreshold
        {
            get { return (double)GetValue(DistanceThresholdProperty); }
            set { SetValue(DistanceThresholdProperty, value); }
        }
		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
    }
}