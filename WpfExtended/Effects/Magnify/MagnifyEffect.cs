// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Windows.Media.Effects;

namespace System.Windows.Media.Extensions.Effects
{
#if SILVERLIGHT
    using UIPropertyMetadata = System.Windows.PropertyMetadata ;   
    using Vector = System.Windows.Point ; 
#endif
    public class MagnifyEffect : ShaderEffect
    {
        public static readonly DependencyProperty RadiiProperty = DependencyProperty.Register("Radii", typeof(Size), typeof(MagnifyEffect), new UIPropertyMetadata(new Size(0.2, 0.2), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(MagnifyEffect), new UIPropertyMetadata(new Point(0.25, 0.25), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty ShrinkFactorProperty = DependencyProperty.Register("ShrinkFactor", typeof(double), typeof(MagnifyEffect), new UIPropertyMetadata(0.5, PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(MagnifyEffect), 0);

        private readonly static PixelShader pixelShader;
        private readonly MagnifyGeneralTransform generalTransform;

        static MagnifyEffect()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader("Magnify/Magnify.ps");
        }

        public MagnifyEffect()
        {
            this.PixelShader = pixelShader;

            UpdateShaderValue(RadiiProperty);
            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(ShrinkFactorProperty);
            UpdateShaderValue(InputProperty);

            this.generalTransform = new MagnifyGeneralTransform(this);
        }

        public Size Radii
        {
            get { return (Size)GetValue(RadiiProperty); }
            set { SetValue(RadiiProperty, value); }
        }
        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }
        public double ShrinkFactor
        {
            get { return (double)GetValue(ShrinkFactorProperty); }
            set { SetValue(ShrinkFactorProperty, value); }
        }
		[System.ComponentModel.BrowsableAttribute(false)]
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        protected override GeneralTransform EffectMapping
        {
            get
            {
                return this.generalTransform;
            }
        }

        private class MagnifyGeneralTransform : GeneralTransform
        {
            private readonly MagnifyEffect effect;
            private bool thisIsInverse;
            private MagnifyGeneralTransform inverseTransform;
            public MagnifyGeneralTransform(MagnifyEffect fx)
            {
                this.effect = fx;
            }
            public override GeneralTransform Inverse
            {
                get
                {
                    // Cache this since it can get called often
                    if (this.inverseTransform == null)
                    {
#if !SILVERLIGHT 
                        this.inverseTransform = (MagnifyGeneralTransform)this.Clone();
#else 
                        this.inverseTransform = new MagnifyGeneralTransform( this.effect) ;
#endif
                        this.inverseTransform.thisIsInverse = !this.thisIsInverse;
                    }

                    return this.inverseTransform;
                }
            }
            public override Rect TransformBounds(Rect rect)
            {
                Rect result;
                bool ok1 = this.TryTransform( new Point ( rect.Left, rect.Top ) , out Point tl);
                bool ok2 = this.TryTransform( new Point ( rect.Right, rect.Bottom ), out Point br);
                if (ok1 && ok2)
                {
                    result = new Rect(tl, br);
                }
                else
                {
                    result = Rect.Empty;
                }

                return result;
            }
            public override bool TryTransform(Point targetPoint, out Point result)
            {
                // In this particular case, the inverse transform is the same as the forward
                // transform.
                if (!this.PointIsInEllipse(targetPoint, this.effect.Center, this.effect.Radii))
                {
                    // If outside the ellipse, just the identity.
                    result = targetPoint;
                }
                else
                {
                    // If inside the ellipse, calculate that magnification/minification
                    Point center = this.effect.Center;
                    Point ray = new Point(targetPoint.X - center.X, targetPoint.Y - center.Y);

                    // Inverse maps a point from after the effect was applied to the point that it came from before the effect.
                    // Non-inverse maps where a point before the effect is applied goes after the effect is applied.
                    // The operation the shader itself performs should match up with the "inverse" operation here.
                    double scaleFactor = this.thisIsInverse ? this.effect.ShrinkFactor : 1.0 / this.effect.ShrinkFactor;

                    result = new Point(center.X + scaleFactor * ray.X, center.Y + scaleFactor * ray.Y);
                }

                return true;
            }

#if !SILVERLIGHT 
            protected override Freezable CreateInstanceCore()
            {
                return new MagnifyGeneralTransform(this.effect) { thisIsInverse = this.thisIsInverse };
            }
#endif 
            private bool PointIsInEllipse(Point pt, Point center, Size radii)
            {
                Point ray =  new Point ( pt.X - center.X , pt.Y - center.Y) ;
                double rayPctX = ray.X / radii.Width;
                double rayPctY = ray.Y / radii.Height;

                // Normally would take sqrt() for length, but since we're comparing 
                // to 1.0, it doesn't matter.
                double pctLength = rayPctX * rayPctX + rayPctY * rayPctY;

                return pctLength <= 1.0;
            }
        }
    }
}
