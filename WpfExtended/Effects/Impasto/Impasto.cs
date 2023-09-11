using System.Windows.Media.Effects;

namespace System.Windows.Media.Extensions.Effects
{
#if SILVERLIGHT
    using UIPropertyMetadata = System.Windows.PropertyMetadata ;      
#endif
    public class Impasto : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Impasto), 0);
        public static readonly DependencyProperty TexelWidthProperty = DependencyProperty.Register("TexelWidth", typeof(double), typeof(Impasto), new UIPropertyMetadata(512d, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty TexelHeightProperty = DependencyProperty.Register("TexelHeight", typeof(double), typeof(Impasto), new UIPropertyMetadata(512d, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty StrengthProperty = DependencyProperty.Register("Strength", typeof(double), typeof(Impasto), new UIPropertyMetadata(15d, PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty DepthProperty = DependencyProperty.Register("Depth", typeof(double), typeof(Impasto), new UIPropertyMetadata(100d, PixelShaderConstantCallback(3)));

        private readonly static PixelShader pixelShader;

        static Impasto()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<Impasto>();
        }
        public Impasto()
        {
            this.PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(TexelWidthProperty);
            UpdateShaderValue(TexelHeightProperty);
            UpdateShaderValue(StrengthProperty);
            UpdateShaderValue(DepthProperty);
        }

		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public double TexelWidth
        {
            get { return (double)GetValue(TexelWidthProperty); }
            set { SetValue(TexelWidthProperty, value); }
        }

        public double TexelHeight
        {
            get { return (double)GetValue(TexelHeightProperty); }
            set { SetValue(TexelHeightProperty, value); }
        }

        public double Strength
        {
            get { return (double)GetValue(StrengthProperty); }
            set { SetValue(StrengthProperty, value); }
        }

        public double Depth
        {
            get { return (double)GetValue(DepthProperty); }
            set { SetValue(DepthProperty, value); }
        }
    }
}