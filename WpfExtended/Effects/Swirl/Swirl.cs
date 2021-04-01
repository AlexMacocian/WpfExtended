// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System.Windows.Media.Effects;

namespace System.Windows.Media.Extensions.Effects
{
#if SILVERLIGHT
    using UIPropertyMetadata = System.Windows.PropertyMetadata ;   
    using Vector =  System.Windows.Point ; 
#endif
    public class Swirl : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Swirl), 0);
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(Swirl), new UIPropertyMetadata(new Point(0.5, 0.5), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty SwirlStrengthProperty = DependencyProperty.Register("SwirlStrength", typeof(double), typeof(Swirl), new UIPropertyMetadata(0.5, PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty AngleFrequencyProperty = DependencyProperty.Register("AngleFrequency", typeof(Vector), typeof(Swirl), new UIPropertyMetadata(new Vector(1, 1), PixelShaderConstantCallback(2)));

        private readonly static PixelShader pixelShader;
        private readonly SwirlGeneralTransform generalTransform;

        static Swirl()
        {
            pixelShader = PixelShaderUtility.LoadPixelShader<Swirl>();
        }

        public Swirl()
        {
            this.PixelShader = pixelShader;

            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(SwirlStrengthProperty);
            UpdateShaderValue(AngleFrequencyProperty);
            UpdateShaderValue(InputProperty);

            this.generalTransform = new SwirlGeneralTransform(this);
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
        public Vector AngleFrequency
        {
            get { return (Vector)GetValue(AngleFrequencyProperty); }
            set { SetValue(AngleFrequencyProperty, value); }
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
        private class SwirlGeneralTransform : GeneralTransform
        {
            private readonly Swirl theEffect;
            private bool thisIsInverse;
            private SwirlGeneralTransform inverseTransform;
            public SwirlGeneralTransform(Swirl eff)
            {
                this.theEffect = eff;
            }
            public override GeneralTransform Inverse
            {
                get
                {
                    // Cache this since it can get called often
                    if (this.inverseTransform == null)
                    {
#if !SILVERLIGHT 
                        this.inverseTransform = (SwirlGeneralTransform)this.Clone();
#else 
                        this.inverseTransform = new SwirlGeneralTransform (this.theEffect);
#endif
                        this.inverseTransform.thisIsInverse = !this.thisIsInverse;
                    }

                    return this.inverseTransform;
                }
            }
            public override Rect TransformBounds(Rect rect)
            {
                if (this.TryTransform(new Point(rect.Left, rect.Top), out Point tl) &&
                    this.TryTransform(new Point(rect.Right, rect.Top), out Point tr) &&
                    this.TryTransform(new Point(rect.Left, rect.Bottom), out Point bl) &&
                    this.TryTransform(new Point(rect.Right, rect.Bottom), out Point br))
                {
                    double maxX = Math.Max(tl.X, Math.Max(tr.X, Math.Max(bl.X, br.X)));
                    double minX = Math.Min(tl.X, Math.Min(tr.X, Math.Min(bl.X, br.X)));

                    double maxY = Math.Max(tl.Y, Math.Max(tr.Y, Math.Max(bl.Y, br.Y)));
                    double minY = Math.Min(tl.Y, Math.Min(tr.Y, Math.Min(bl.Y, br.Y)));

                    return new Rect(minX, minY, maxX - minX, maxY - minY);
                }
                else
                {
                    return Rect.Empty;
                }
            }
            public override bool TryTransform(Point targetPoint, out Point result)
            {
                // Exactly follows what the HLSL shader itself does.
                Point dir = new Point(targetPoint.X - this.theEffect.Center.X, targetPoint.Y - this.theEffect.Center.Y);
                double l = Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y);
                dir.X /= l;
                dir.Y /= l;


                double angle = Math.Atan2(dir.Y, dir.X);

                double inverseFactor = this.thisIsInverse ? 1 : -1;
                double newAngle = angle + inverseFactor * this.theEffect.SwirlStrength * l;

                Point angleFrequency = new Point(this.theEffect.AngleFrequency.X, this.theEffect.AngleFrequency.Y); 
                double xamt = Math.Cos(angleFrequency.X * newAngle) * l;
                double yamt = Math.Sin(angleFrequency.Y * newAngle) * l;

                result = new Point ( this.theEffect.Center.X + xamt, 
                                     this.theEffect.Center.Y + yamt);

                return true;
            }
#if !SILVERLIGHT             
            protected override Freezable CreateInstanceCore()
            {
                return new SwirlGeneralTransform(this.theEffect) { thisIsInverse = this.thisIsInverse };
            }
#endif 
        }
    }
}
