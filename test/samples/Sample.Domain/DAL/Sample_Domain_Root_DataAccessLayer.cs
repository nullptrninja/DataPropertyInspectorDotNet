using System;
using DataInspector.DataAccess.DAL;
using Sample.Domain;

namespace Sample.Domain.DAL {
	public class Sample_Domain_Root_DataAccessLayer : BaseDirectPropertyCallDAL<Root> {
		public Sample_Domain_Root_DataAccessLayer() : base() {
			callChainDispatchMap.Add("rootnodeid", RootNodeId);
			callChainDispatchMap.Add("glorpchild.isenabled", GlorpChild_IsEnabled);
			callChainDispatchMap.Add("glorpchild.valuef", GlorpChild_ValueF);
			callChainDispatchMap.Add("glorpchild.name", GlorpChild_Name);
			callChainArrayDispatchMap.Add("derpchild.alltheflerbs[].subids[]", DerpChild_AllTheFlerbs_SubIds);
			callChainArrayDispatchMap.Add("derpchild.alltheflerbs[].flerbid", DerpChild_AllTheFlerbs_FlerbId);
			callChainDispatchMap.Add("derpchild.otherglorp.isenabled", DerpChild_OtherGlorp_IsEnabled);
			callChainDispatchMap.Add("derpchild.otherglorp.valuef", DerpChild_OtherGlorp_ValueF);
			callChainDispatchMap.Add("derpchild.otherglorp.name", DerpChild_OtherGlorp_Name);
			callChainDispatchMap.Add("derpchild.id", DerpChild_Id);
			callChainDispatchMap.Add("derpchild.uuid", DerpChild_Uuid);
		}
		private object RootNodeId(Root inputObject) {
			return inputObject.RootNodeId;
		}
		private object GlorpChild_IsEnabled(Root inputObject) {
			return inputObject.GlorpChild.IsEnabled;
		}
		private object GlorpChild_ValueF(Root inputObject) {
			return inputObject.GlorpChild.ValueF;
		}
		private object GlorpChild_Name(Root inputObject) {
			return inputObject.GlorpChild.Name;
		}
		private object DerpChild_AllTheFlerbs_SubIds(Root inputObject, int[] indicies) {
			return inputObject.DerpChild.AllTheFlerbs[indicies[0]].SubIds[indicies[1]];
		}
		private object DerpChild_AllTheFlerbs_FlerbId(Root inputObject, int[] indicies) {
			return inputObject.DerpChild.AllTheFlerbs[indicies[0]].FlerbId;
		}
		private object DerpChild_OtherGlorp_IsEnabled(Root inputObject) {
			return inputObject.DerpChild.OtherGlorp.IsEnabled;
		}
		private object DerpChild_OtherGlorp_ValueF(Root inputObject) {
			return inputObject.DerpChild.OtherGlorp.ValueF;
		}
		private object DerpChild_OtherGlorp_Name(Root inputObject) {
			return inputObject.DerpChild.OtherGlorp.Name;
		}
		private object DerpChild_Id(Root inputObject) {
			return inputObject.DerpChild.Id;
		}
		private object DerpChild_Uuid(Root inputObject) {
			return inputObject.DerpChild.Uuid;
		}

	}
}

