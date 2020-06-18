using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace InhumaneCards {
	static class Utils {

		public const float FLOAT_PI = (float)(Math.PI);

		public static Rectangle Times(this Rectangle self, float factor) {
			return new Rectangle(self.Location.Times(factor), self.Size.Times(factor));
		}

		public static Point Times(this Point self, float factor) {
			return new Point((int)(self.X * factor), (int)(self.Y * factor));
		}

		public static byte B(this int self) {
			return (byte)self;
		}

	}
}
