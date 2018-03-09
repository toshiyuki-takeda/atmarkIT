﻿/*
This software includes the work that is distributed in the Apache License 2.0 

ZXing.Net
Copyright © 2012-2017 Michael Jahn https://github.com/micjahn/ZXing.Net

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
/*
This software includes the work that is distributed in the MIT License

SkiaSharp
Copyright © 2016-2018 Xamarin Inc. https://github.com/mono/SkiaSharp

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
https://opensource.org/licenses/mit-license.php
*/


using SkiaSharp;
using System;
using System.Threading.Tasks;
using ZXing; // BarcodeReaderExtensionsの拡張メソッドを利用

// これは .NET Standard 1.3 規格の PCL です。
// ビルド出力の NuGet パッケージを、使いたい側のプロジェクトにインストールします。
// ※プロジェクトの参照にこのプロジェクトを直接追加したときは、このプロジェクトが依存している ZXing.NET 等のパッケージを手動で追加します

// 利用可能なプラットフォーム:
// .NET Framework 4.6
// .NET Core 1.0
// UWP 10.0.12040
// Xamarin.Android 7.0
// Xamarin.iOS 10.0

// このようにしておけば、利用する側のプロジェクトでは…
// ・NuGet パッケージを 1 つだけ入れる
// ・画像は、とにかくバイト配列にして渡せばいい
// …と単純化されるので、便利かもしれません。
// (ただし、プラットフォームによっては無駄な処理が増えるし、バイナリのサイズも大きくなる)

namespace StdLib
{
  public class ZXingResultSummary
  {
    public string Text { get; }
    public string Format { get; }

    private ZXingResultSummary()
    {
      // avoid instance
    }
    internal ZXingResultSummary(ZXing.Result result)
    {
      Text = result.Text;
      Format = result.BarcodeFormat.ToString();
    }
  }

  public class ZXingBarcodeReader
  {
    public static async Task<ZXingResultSummary> DecodeAsync(byte[] imageFileData)
    {
      if (imageFileData == null || imageFileData.Length == 0)
        return null;

      using (SKBitmap bitmap = SKBitmap.Decode(imageFileData))
      {
        if (bitmap == null)
          throw new NotSupportedException("Unsupported image format by SkiaSharp");

        var reader = new ZXing.BarcodeReader()
        {
          AutoRotate = true,
          Options = { TryHarder = true },
        };
        var result = await Task.Run(() => reader.Decode(bitmap));
        return (result != null) ? new ZXingResultSummary(result) : null;
      }
    }
  }
}