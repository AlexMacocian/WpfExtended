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
    public class Snowfall : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Snowfall), 0);
        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed", typeof(double), typeof(Snowfall), new UIPropertyMetadata(0.25, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty WindStrengthProperty =
            DependencyProperty.Register("WindStrength", typeof(double), typeof(Snowfall), new UIPropertyMetadata(0.2, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty FlakeSizeProperty =
            DependencyProperty.Register("FlakeSize", typeof(double), typeof(Snowfall), new UIPropertyMetadata(300.0, PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(double), typeof(Snowfall), new UIPropertyMetadata(0.0, PixelShaderConstantCallback(3)));
        public static readonly DependencyProperty ThresholdProperty =
            DependencyProperty.Register("Threshold", typeof(double), typeof(Snowfall), new UIPropertyMetadata(0.77, PixelShaderConstantCallback(4)));

        private readonly static PixelShader pixelShader = new();

        static Snowfall()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<Snowfall>();
        }

        public Snowfall()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(SpeedProperty);
            UpdateShaderValue(WindStrengthProperty);
            UpdateShaderValue(FlakeSizeProperty);
            UpdateShaderValue(TimeProperty);
            UpdateShaderValue(ThresholdProperty);
        }

		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }
        public double WindStrength
        {
            get { return (double)GetValue(WindStrengthProperty); }
            set { SetValue(WindStrengthProperty, value); }
        }
        public double FlakeSize
        {
            get { return (double)GetValue(FlakeSizeProperty); }
            set { SetValue(FlakeSizeProperty, value); }
        }
        public double Time
        {
            get { return (double)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        public double Threshold
        {
            get { return (double)GetValue(ThresholdProperty); }
            set { SetValue(ThresholdProperty, value); }
        }
    }
}
