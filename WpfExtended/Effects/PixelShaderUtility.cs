﻿// MIT License
//
// Copyright (c) 2020 Hisham Maudarbocus
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.IO;
using System.Windows.Media.Effects;

namespace System.Windows.Media.Extensions.Effects
{
    internal static class PixelShaderUtility
    {
        public static readonly string AssemblyShortName;

        static PixelShaderUtility()
        {
            AssemblyShortName = typeof(PixelShaderUtility)
                .Assembly
                .GetName()
                .Name;
        }

        public static PixelShader LoadPixelShader<T>()
            where T : ShaderEffect
        {
            var pixelShaderName = typeof(T).Name;
            var resourceName = $"{AssemblyShortName}.Effects.{pixelShaderName}.{pixelShaderName}.ps";
            var pixelShader = new PixelShader();
            pixelShader.SetStreamSource(GetShaderStream(resourceName));
            return pixelShader;
        }

        internal static Stream GetShaderStream(string name)
        {
            return typeof(PixelShaderUtility).Assembly.GetManifestResourceStream(name) ?? throw new InvalidOperationException($"Could not find shader resource for {name}");
        }
    }
}