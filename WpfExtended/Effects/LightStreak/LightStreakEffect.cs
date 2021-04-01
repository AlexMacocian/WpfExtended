﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using LiteWare.Wpf.ShaderEffects;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace WpfExtended.Effects
{
#if SILVERLIGHT 
    using UIPropertyMetadata = System.Windows.PropertyMetadata ;      
#endif
    public class LightStreakEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(LightStreakEffect), 0);
        public static readonly DependencyProperty BrightThresholdProperty = DependencyProperty.Register("BrightThreshold", typeof(double), typeof(LightStreakEffect), new UIPropertyMetadata(0.25, PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(double), typeof(LightStreakEffect), new UIPropertyMetadata(0.75, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty AttenuationProperty = DependencyProperty.Register("Attenuation", typeof(double), typeof(LightStreakEffect), new UIPropertyMetadata(1.0, PixelShaderConstantCallback(2)));

        private readonly static PixelShader pixelShader = new PixelShader();

        static LightStreakEffect()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader("LightStreak/LightStreak.ps");
        }

        public LightStreakEffect()
        {
            this.PixelShader = pixelShader;

            // Update each DependencyProperty that's registered with a shader register.  This
            // is needed to ensure the shader gets sent the proper default value.
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(BrightThresholdProperty);
            UpdateShaderValue(ScaleProperty);
            UpdateShaderValue(AttenuationProperty);
        }

		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }
        public double BrightThreshold
        {
            get { return (double)GetValue(BrightThresholdProperty); }
            set { SetValue(BrightThresholdProperty, value); }
        }
        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }
        public double Attenuation
        {
            get { return (double)GetValue(AttenuationProperty); }
            set { SetValue(AttenuationProperty, value); }
        }
    }
}
