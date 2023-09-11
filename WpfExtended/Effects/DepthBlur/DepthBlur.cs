using System.Windows.Media.Effects;

namespace System.Windows.Media.Extensions.Effects
{
#if SILVERLIGHT 
    using UIPropertyMetadata = System.Windows.PropertyMetadata ;      
#endif
    /// <summary>
    /// Combines Embossed effect with a noise effect to distort an image based on the depth of the image contents
    /// </summary>
    public class DepthBlur : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(DepthBlur), 0, SamplingMode.Bilinear);
        public static readonly DependencyProperty FocusDepthProperty = DependencyProperty.Register("FocusDepth", typeof(double), typeof(DepthBlur), new UIPropertyMetadata(0.5, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty FocusRangeProperty = DependencyProperty.Register("FocusStrength", typeof(double), typeof(DepthBlur), new UIPropertyMetadata(0.2, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty TextureWidthProperty = DependencyProperty.Register("TextureWidth", typeof(double), typeof(DepthBlur), new UIPropertyMetadata(512d, PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty TextureHeightProperty = DependencyProperty.Register("TextureHeight", typeof(double), typeof(DepthBlur), new UIPropertyMetadata(512d, PixelShaderConstantCallback(3)));
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(DepthBlur), new UIPropertyMetadata(0d, PixelShaderConstantCallback(4)));
        public static readonly DependencyProperty StrengthProperty = DependencyProperty.Register("Strength", typeof(double), typeof(DepthBlur), new UIPropertyMetadata(1d, PixelShaderConstantCallback(5)));

        private readonly static PixelShader pixelShader = new PixelShader();

        static DepthBlur()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<DepthBlur>();
        }

        public DepthBlur()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(FocusDepthProperty);
            UpdateShaderValue(FocusRangeProperty);
            UpdateShaderValue(TextureWidthProperty);
            UpdateShaderValue(TextureHeightProperty);
            UpdateShaderValue(AngleProperty);
            UpdateShaderValue(StrengthProperty);
        }

        [System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public double FocusDepth
        {
            get { return (double)GetValue(FocusDepthProperty); }
            set { SetValue(FocusDepthProperty, value); }
        }
        public double FocusRange
        {
            get { return (double)GetValue(FocusRangeProperty); }
            set { SetValue(FocusRangeProperty, value); }
        }
        public double TextureWidth
        {
            get { return (double)GetValue(TextureWidthProperty); }
            set { SetValue(TextureWidthProperty, value); }
        }
        public double TextureHeight
        {
            get { return (double)GetValue(TextureHeightProperty); }
            set { SetValue(TextureHeightProperty, value); }
        }
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }
        public double Strength
        {
            get { return (double)GetValue(StrengthProperty); }
            set { SetValue(StrengthProperty, value); }
        }
    }
}
