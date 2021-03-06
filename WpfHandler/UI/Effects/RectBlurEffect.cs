﻿//Copyright 2019 Volodymyr Podshyvalov
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Reflection;

namespace WpfHandler.UI.Effects
{
    /// <summary>
    /// Effect that allow to blur elements under the rect.
    /// </summary>
    public class RectBlurEffect : ShaderEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();
        private static readonly PropertyInfo propertyInfo;

        /// <summary>
        /// Bind the Input property from the shader to the code behind.
        /// </summary>
        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(RectBlurEffect), 0);

        /// <summary>
        /// Bind the UpLeftCornerProperty property from the shader to the code behind.
        /// </summary>
        public static readonly DependencyProperty UpLeftCornerProperty =
            DependencyProperty.Register("UpLeftCorner", typeof(Point), typeof(RectBlurEffect),
                new UIPropertyMetadata(new Point(0, 0), PixelShaderConstantCallback(0)));

        /// <summary>
        /// Bind the LowRightCornerProperty property from the shader to the code behind.
        /// </summary>
        public static readonly DependencyProperty LowRightCornerProperty =
            DependencyProperty.Register("LowRightCorner", typeof(Point), typeof(RectBlurEffect),
                new UIPropertyMetadata(new Point(1, 1), PixelShaderConstantCallback(1)));

        /// <summary>
        /// Bind the FrameworkElementProperty property from the shader to the code behind.
        /// </summary>
        public static readonly DependencyProperty FrameworkElementProperty =
            DependencyProperty.Register("FrameworkElement", typeof(FrameworkElement), typeof(RectBlurEffect),
            new PropertyMetadata(null, OnFrameworkElementPropertyChanged));

        /// <summary>
        /// Getting the shader executable source.
        /// </summary>
        static RectBlurEffect()
        {
            Uri shaderUri = MakePackUri("Shaders/RectBlurEffect.ps");
            pixelShader.UriSource = shaderUri;
            propertyInfo = typeof(RectBlurEffect).GetProperty("InheritanceContext",
                BindingFlags.Instance | BindingFlags.NonPublic);
        }

        /// <summary>
        /// MakePackUri is a utility method for computing a pack uri
        /// for the given resource. 
        /// </summary>
        /// <param name="relativeFile">Path to the file.</param>
        /// <returns>URI to the file.</returns>
        public static Uri MakePackUri(string relativeFile)
        {
            Assembly a = typeof(RectBlurEffect).Assembly;

            // Extract the short name.
            string assemblyShortName = a.ToString().Split(',')[0];

            string uriString = "pack://application:,,,/" +
                assemblyShortName +
                ";component/" +
                relativeFile;

            return new Uri(uriString);
        }

        /// <summary>
        /// Instiniate the shader effect.
        /// </summary>
        public RectBlurEffect()
        {
            PixelShader = pixelShader;
            UpdateShaderValue(InputProperty);
            UpdateShaderValue(UpLeftCornerProperty);
            UpdateShaderValue(LowRightCornerProperty);
        }

        /// <summary>
        /// Defines input brush.
        /// </summary>
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        /// <summary>
        /// Defines up left corner of the rect.
        /// </summary>
        public Point UpLeftCorner
        {
            get { return (Point)GetValue(UpLeftCornerProperty); }
            set { SetValue(UpLeftCornerProperty, value); }
        }

        /// <summary>
        /// Defines low right corner of the rect.
        /// </summary>
        public Point LowRightCorner
        {
            get { return (Point)GetValue(LowRightCornerProperty); }
            set { SetValue(LowRightCornerProperty, value); }
        }

        /// <summary>
        /// Defines binded framwork element.
        /// </summary>
        public FrameworkElement FrameworkElement
        {
            get { return (FrameworkElement)GetValue(FrameworkElementProperty); }
            set { SetValue(FrameworkElementProperty, value); }
        }

        private FrameworkElement GetInheritanceContext()
        {
            return propertyInfo.GetValue(this, null) as FrameworkElement;
        }

        private void UpdateEffect(object sender, EventArgs args)
        {
            Rect underRectangle;
            Rect overRectangle;
            Rect intersect;

            FrameworkElement under = GetInheritanceContext();
            FrameworkElement over = this.FrameworkElement;

            Point origin = under.PointToScreen(new Point(0, 0));
            underRectangle = new Rect(origin.X, origin.Y, under.ActualWidth, under.ActualHeight);

            origin = over.PointToScreen(new Point(0, 0));
            overRectangle = new Rect(origin.X, origin.Y, over.ActualWidth, over.ActualHeight);

            intersect = Rect.Intersect(overRectangle, underRectangle);

            if (intersect.IsEmpty)
            {
                UpLeftCorner = new Point(0, 0);
                LowRightCorner = new Point(0, 0);
            }
            else
            {
                origin = new Point(intersect.X, intersect.Y);
                origin = under.PointFromScreen(origin);

                UpLeftCorner = new Point(origin.X / under.ActualWidth,
                    origin.Y / under.ActualHeight);
                LowRightCorner = new Point(UpLeftCorner.X + (intersect.Width / under.ActualWidth),
                    UpLeftCorner.Y + (intersect.Height / under.ActualHeight));
            }

        }

        private static void OnFrameworkElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            RectBlurEffect rectBlurEffect = (RectBlurEffect)d;

            if (args.OldValue is FrameworkElement frameworkElement)
            {
                frameworkElement.LayoutUpdated -= rectBlurEffect.UpdateEffect;
            }

            frameworkElement = args.NewValue as FrameworkElement;

            if (frameworkElement != null)
            {
                frameworkElement.LayoutUpdated += rectBlurEffect.UpdateEffect;
            }
        }
    }
}