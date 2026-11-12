
#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 * Copyright 2009-2024 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
	internal class EffectReader : ContentTypeReader<Effect>
	{
		protected internal override Effect Read(
			ContentReader input,
			Effect existingInstance
		)
		{
			int length = input.ReadInt32();

			Console.WriteLine(
				"[FNA EffectReader] Asset: " + input.AssetName
			);

			Console.WriteLine(
				"[FNA EffectReader] Declared length: " + length
			);

			if (length < 4 || length > 64 * 1024 * 1024)
			{
				throw new InvalidDataException(
					"Invalid effect bytecode length: " + length
				);
			}

			byte[] effectCode = input.ReadBytes(length);

			if (effectCode.Length != length)
			{
				throw new EndOfStreamException(
					"Effect '" + input.AssetName +
					"' is truncated. Expected " + length +
					" bytes, received " + effectCode.Length
				);
			}

			string header = BitConverter.ToString(
				effectCode,
				0,
				Math.Min(32, effectCode.Length)
			);

			uint magicLE = BitConverter.ToUInt32(effectCode, 0);

			Console.WriteLine(
				"[FNA EffectReader] Actual length: " +
				effectCode.Length
			);

			Console.WriteLine(
				"[FNA EffectReader] Header: " + header
			);

			Console.WriteLine(
				"[FNA EffectReader] Magic LE: 0x" +
				magicLE.ToString("X8")
			);

			Console.WriteLine(
				"[FNA EffectReader] Expected magic: 0xFEFF0901"
			);

			Effect effect;

			try
			{
				effect = new Effect(
					input.ContentManager.GetGraphicsDevice(),
					effectCode
				);
			}
			catch (Exception ex)
			{
				Console.WriteLine(
					"[FNA EffectReader] FAILED: " +
					input.AssetName
				);

				Console.WriteLine(ex.ToString());
				throw;
			}

			effect.Name = input.AssetName;

			Console.WriteLine(
				"[FNA EffectReader] SUCCESS: " +
				input.AssetName
			);

			return effect;
		}
	}
}
