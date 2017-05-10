using System;

using IKVM.Attributes;
using java.lang;

namespace edu.cmu.sphinx.trainer
{
	public class Stage : java.lang.Object
	{

		public override string toString()
		{
			return this.name;
		}

		
		protected internal Stage(string name)
		{
			this.name = name;
		}

		public virtual bool equals(Stage stage)
		{
			return stage != null && java.lang.String.instancehelper_equals(this.toString(), stage.toString());
		}

		public virtual bool equals(string stage)
		{
			return stage != null && java.lang.String.instancehelper_equals(this.toString(), stage);
		}
		static Stage()
		{
		}

		
		public static Stage _00_INITIALIZATION
		{
			get
			{
				return Stage.___00_INITIALIZATION;
			}
		}

		
		public static Stage _10_CI_TRAIN
		{
			get
			{
				return Stage.___10_CI_TRAIN;
			}
		}

		
		public static Stage _20_UNTIED_CD_TRAIN
		{
			get
			{
				return Stage.___20_UNTIED_CD_TRAIN;
			}
		}

		
		public static Stage _30_STATE_PRUNING
		{
			get
			{
				return Stage.___30_STATE_PRUNING;
			}
		}

		
		public static Stage _40_TIED_CD_TRAIN
		{
			
			get
			{
				return Stage.___40_TIED_CD_TRAIN;
			}
		}

		
		public static Stage _90_CP_MODEL
		{
			
			get
			{
				return Stage.___90_CP_MODEL;
			}
		}

		private string name;

		internal static Stage ___00_INITIALIZATION = new Stage("_00_INITIALIZATION");

		internal static Stage ___10_CI_TRAIN = new Stage("_10_CI_TRAIN");

		internal static Stage ___20_UNTIED_CD_TRAIN = new Stage("_20_UNTIED_CD_TRAIN");

		internal static Stage ___30_STATE_PRUNING = new Stage("_30_STATE_PRUNING");

		internal static Stage ___40_TIED_CD_TRAIN = new Stage("_40_TIED_CD_TRAIN");

		internal static Stage ___90_CP_MODEL = new Stage("_90_CP_MODEL");
	}
}
