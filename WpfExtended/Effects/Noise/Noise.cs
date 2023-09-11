using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace System.Windows.Media.Extensions.Effects
{
#if SILVERLIGHT 
    using UIPropertyMetadata = System.Windows.PropertyMetadata ;      
#endif
    public class Noise : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Noise), 0);
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(double), typeof(Noise), new UIPropertyMetadata(1d, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty StrengthProperty = DependencyProperty.Register("Strength", typeof(double), typeof(Noise), new UIPropertyMetadata(0.01d, PixelShaderConstantCallback(1)));

        private readonly static PixelShader pixelShader = new PixelShader();

        static Noise()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<Noise>();
        }

        public Noise()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(TimeProperty);
            UpdateShaderValue(StrengthProperty);
        }

        [System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public double Time
        {
            get { return (double)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        public double Strength
        {
            get { return (double)GetValue(StrengthProperty); }
            set { SetValue(StrengthProperty, value); }
        }
    }
}
