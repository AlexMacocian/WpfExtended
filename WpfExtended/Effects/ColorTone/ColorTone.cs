﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Windows.Media.Effects;

namespace System.Windows.Media.Extensions.Effects
{
#if SILVERLIGHT 
    using UIPropertyMetadata = System.Windows.PropertyMetadata ;      
#endif
    public class ColorTone : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ColorTone), 0);
        public static readonly DependencyProperty DesaturationProperty =
            DependencyProperty.Register("Desaturation", typeof(double), typeof(ColorTone), new UIPropertyMetadata(0.5, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty TonedProperty =
            DependencyProperty.Register("Toned", typeof(double), typeof(ColorTone), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty LightColorProperty =
            DependencyProperty.Register("LightColor", typeof(Color), typeof(ColorTone), new UIPropertyMetadata(Color.FromArgb(255, 255, 229, 128), PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty DarkColorProperty =
            DependencyProperty.Register("DarkColor", typeof(Color), typeof(ColorTone), new UIPropertyMetadata(Color.FromArgb(255, 51, 128, 0), PixelShaderConstantCallback(3)));

        private readonly static PixelShader pixelShader = new PixelShader();

        static ColorTone()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<ColorTone>();
        }

        public ColorTone()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(DesaturationProperty);
            UpdateShaderValue(TonedProperty);
            UpdateShaderValue(LightColorProperty);
            UpdateShaderValue(DarkColorProperty);
        }

		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public double Desaturation
        {
            get { return (double)GetValue(DesaturationProperty); }
            set { SetValue(DesaturationProperty, value); }
        }
        public double Toned
        {
            get { return (double)GetValue(TonedProperty); }
            set { SetValue(TonedProperty, value); }
        }
        public Color LightColor
        {
            get { return (Color)GetValue(LightColorProperty); }
            set { SetValue(LightColorProperty, value); }
        }
        public Color DarkColor
        {
            get { return (Color)GetValue(DarkColorProperty); }
            set { SetValue(DarkColorProperty, value); }
        }
    }
}
