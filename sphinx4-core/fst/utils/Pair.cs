﻿using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.fst.utils
{
	
	public class Pair : java.lang.Object
	{
		
		[LineNumberTable(new byte[]
		{
			159,
			178,
			104,
			103,
			103
		})]
		
		public Pair(object left, object right)
		{
			this.left = left;
			this.right = right;
		}

		
		public virtual object getLeft()
		{
			return this.left;
		}

		
		public virtual object getRight()
		{
			return this.right;
		}

		
		public virtual void setRight(object right)
		{
			this.right = right;
		}

		
		public virtual void setLeft(object left)
		{
			this.left = left;
		}

		[LineNumberTable(new byte[]
		{
			9,
			99,
			98,
			113,
			113
		})]
		
		public override int hashCode()
		{
			int num = 1;
			num = 31 * num + Object.instancehelper_hashCode(this.left);
			return 31 * num + Object.instancehelper_hashCode(this.right);
		}

		[LineNumberTable(new byte[]
		{
			18,
			100,
			98,
			99,
			130,
			103,
			115,
			98,
			115,
			98
		})]
		
		public override bool equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			Pair pair = (Pair)obj;
			return Object.instancehelper_equals(this.left, pair.left) && Object.instancehelper_equals(this.right, pair.right);
		}

		
		
		public override string toString()
		{
			return new StringBuilder().append("(").append(this.left).append(", ").append(this.right).append(")").toString();
		}

		
		private object left;

		
		private object right;
	}
}
